using UnityEngine;
using TMPro;
using UnityEngine.UI;
//using Oculus.Interaction;
//using UnityEngine.Rendering.UI;
//using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
//using UnityEngine.UIElements;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.Rendering.Universal;

public class MachineShredder : MonoBehaviour
{
    public WheelStatus currWheelStatus;

    public UnityEvent finishedShreddingEvent;

    public bool initShredding = false;
    public int _breakAtThisValue;
    public float force;

    [SerializeField] private XRSocketInteractor wheelPreview;

    [SerializeField] private GameObject fakeWheelModel;

    [Header("VR REFERENCES")]
    public GameObject lever;
    public GameObject wheel;
    [SerializeField] private float valueToComplete;

    [Header("LOCATION SPAWNER PREFABS")]
    [SerializeField] private Transform particleSpawnLocation;
    public VrMachineItemCollider shredderItemCollider;
    [SerializeField] private GameObject fixWheelCollider; // enables this when wheel breaks

    [Header("MACHINE SETTINGS")]
    public float secretHealth;
    public float maxHealth;
    [SerializeField] private float fuelDecrease;
    [SerializeField] private float decreaseMultiplier;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float distToStop;
    [SerializeField] private Transform spawnPoint;

    [Header("MACHINE WOLRD UI")]
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Image progressBar;
    public TMP_Text shredderFuelText;

    [Header("FEEDBACK")]
    [SerializeField] private FeedbackEventData e_interactShredder;
    [SerializeField] private FeedbackEventData e_shredderFinish;

    private Bounds _spawnPointBound;
    private float _chargeValue;

    private XRKnob _wheelPlug;
    private WheelManager _wheelManager;
    private RefillFuelManager _refillManager;
    private XRLever _leverPlug;
    private bool _save;

    private XRVelocityRayGrab _grabber;

    [SerializeField] private GameObject _attachedWheel;

    [Header("FUEL POINTER SETTINGS")]
    [SerializeField] private GameObject fuelPointer;
    [SerializeField] private MeshRenderer fuelPointerRenderer;
    [SerializeField] private Material greenGlow;
    [SerializeField] private Material yellowGlow;
    [SerializeField] private Material redGlow;

    [SerializeField] private Vector3 OneHunderedPercentagePosition;
    [SerializeField] private Quaternion OneHunderedPercentageRotation;

    [SerializeField] private Vector3 SeventyFivePercentagePosition;
    [SerializeField] private Quaternion SeventyFivePercentageRotation;

    [SerializeField] private Vector3 FiftyPercentagePosition;
    [SerializeField] private Quaternion FiftyPercentageRotation;

    [SerializeField] private Vector3 TwentyFivePercentagePosition;
    [SerializeField] private Quaternion TwentyFivePercentageRotation;

    [SerializeField] private Vector3 ZeroPercentagePosition;
    [SerializeField] private Quaternion ZeroPercentageRotation;
    private enum fuelPercetnageType
    {
        Zero,
        TwentyFive,
        Fifty,
        SeventyFive,
        OneHundered
    }
    fuelPercetnageType currentFuelPercentageType;

    public UnityEvent enableWheelEvent;

    public void SetValueToComplete(float newValue)
    {
        valueToComplete = newValue;
    }
    public bool AlreadyFull()
    {
        return secretHealth >= maxHealth;
    }
    public void FillFuel()
    {
        if (!_refillManager.activateRefill)
        {
            _refillManager.activateRefill = true;
        }
    }
    public void RunActive()
    {
        //lock the items, so they can no longer be picked up
        foreach(Item itemToShred in shredderItemCollider.GetProductList())
        {
            XRBaseInteractable baseInteractable = itemToShred.GetComponent<XRBaseInteractable>();
            baseInteractable.enabled= false;
            itemToShred.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void RunDeactive()
    {
        Debug.Log("Deactivate");
    }

    public void RunSpamButton()
    {
        if (_chargeValue >= 1 || IsOutOfFuel()) return;
        else
        {
            if (!initShredding)
            {
                initShredding = true;
            }

            IncreaseProgress();
            UpdateProgressBar();
        }
    }

    public bool IsOutOfFuel()
    {
        return secretHealth <= 0 ? true : false;
    }

    public void CanShred()
    {
        initShredding = true;
    }

    public void SetUpWheelProbability()
    {
        //_wheelManager.e_wheelturning?.InvokeEvent(_wheelManager.transform.position, Quaternion.identity, transform);
        _save = _wheelManager.chance.TryLuck();
        if (_save)
        {
            _breakAtThisValue = GetRandomValueToBreak();
        }
        else
        {
            _breakAtThisValue = 0;
        }
    }

    public void SetBreakValue(int value)
    {
        _breakAtThisValue = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        progressBar.fillAmount = 0;
        if (enableWheelEvent == null) // show wheel when fixed event
        {
            enableWheelEvent = new UnityEvent();
        }
        enableWheelEvent.AddListener(EnableWheel); 


        if (finishedShreddingEvent == null)
        {
            finishedShreddingEvent = new UnityEvent();
        }

        _wheelPlug = wheel.GetComponentInChildren<XRKnob>(); // get knob component

        _wheelPlug.enabled = false; // disable knob at start

        _attachedWheel = _wheelPlug.gameObject; // set the attached object to the knob's gameobject

        if (_wheelPlug == null) // attached object cannot be null
        {
            return;
        }

        _wheelPlug.value = 0; // set wheel value to 0 at start always

        _wheelPlug.ValueChangeShredder.AddListener(ValueChangeCheck); // every time wheel is turn call event

        _wheelManager = _wheelPlug.gameObject.GetComponent<WheelManager>(); // get wheel manager from wheel gameobject

        _wheelManager.canStartShredding.AddListener(CanShred); // 

        _wheelManager.SetWheelCurrState(WheelStatus.WORKING);

        _wheelManager.gameObject.GetComponent<Rigidbody>().isKinematic = true; // set wheel to not use gravity and kinematic
        _wheelManager.gameObject.GetComponent<Rigidbody>().useGravity = false;

        _grabber = _wheelManager.GetComponent<XRVelocityRayGrab>(); 

        if (_grabber != null)
        {
            _grabber.enabled = false; // disable ability to grab at the start because wheel is attached
        }

        _leverPlug = lever.GetComponentInChildren<XRLever>(); 
            
        _spawnPointBound = spawnPoint.GetComponent<Collider>().bounds;

        maxHealth = GetNewHealth();
        secretHealth = maxHealth;

        _refillManager = GetComponentInChildren<RefillFuelManager>();

        SetUpWheelProbability();

        fixWheelCollider.GetComponent<Collider>().enabled = false; // disable when wheel is not broken yet

        wheelPreview.enabled = false; // disable socket and enable when wheel breaks
    }


    public void EnableWheel()
    {
        _wheelManager.gameObject.SetActive(true); // set the wheel gameobject to be visible
        if (!_wheelPlug.enabled)
        {
            SetWheelStatus(true);
        }
    }

    public void InitWheelVariables()
    {
        _wheelPlug = wheel.GetComponentInChildren<XRKnob>();

        if (_wheelPlug == null)
        {
            return;
        }

        _wheelPlug.value = 0;

        _wheelPlug.ValueChangeShredder.AddListener(ValueChangeCheck);

        _wheelManager = _wheelPlug.GetComponent<WheelManager>();

        _wheelManager.canStartShredding.AddListener(CanShred);

        _wheelManager.SetWheelCurrState(WheelStatus.WORKING);
    }

    public void ValueChangeCheck()
    {
        //e_interactShredder?.InvokeEvent(particleSpawnLocation.position, Quaternion.identity, transform);
        _wheelManager.e_wheelturning?.InvokeEvent(transform.position, Quaternion.identity, transform);
        if (_save)
        {
            if (_wheelPlug.value >= _breakAtThisValue)
            {
                _wheelManager.SetWheelCurrState(WheelStatus.BROKEN);

                _attachedWheel = null;
                //wheelPreview.enabled = true;
                //_wheelPlug.GetComponentInChildren<Rigidbody>().useGravity = true;
                //_wheelPlug.GetComponentInChildren<Rigidbody>().isKinematic = false;
                //_wheelPlug.GetComponentInChildren<Rigidbody>().AddForce(Vector3.up * force, ForceMode.Impulse);
                //SetGameobjectLayer(wheel, 0); // 

                GameObject fakeWheel = Instantiate(fakeWheelModel.gameObject, _wheelManager.transform.position, Quaternion.identity);
                fakeWheel.GetComponent<Rigidbody>().useGravity = true;
                fakeWheel.GetComponent<Rigidbody>().isKinematic = false;
                fakeWheel.GetComponent<Rigidbody>().AddForce(Vector3.up * force, ForceMode.Impulse);

                SetWheelStatus(false);
                ResetWheelValue();

                fixWheelCollider.GetComponent<Collider>().enabled = true; // enable collider after breaking

                _wheelManager.gameObject.SetActive(false); // set wheel to invisible

                _save = false;
            }
        }
        if (IsOutOfFuel())
        {
            return;
        }
    }

    private void SetGameobjectLayer(GameObject toSet, int layer)
    {
        toSet.layer = layer;
        foreach (Transform child in toSet.transform)
        {
            child.gameObject.layer = layer;

            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetGameobjectLayer(child.gameObject, layer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currWheelStatus = _wheelManager.GetWheelCurrState();
        float fuelPercentage = (secretHealth / maxHealth) * 100f;
        //Debug.Log("percent is: " + fuelPercentage);

        if (AlreadyFull())
        {
            shredderFuelText.color = Color.green;
        }

        if (IsOutOfFuel())
        {
            shredderFuelText.text = "Fuel: 0%";
            shredderFuelText.color = Color.red; // indicate that shredder has no fuel
        }
        else
        {
            //update fuel pointer posision
            shredderFuelText.text = "Fuel: " + fuelPercentage.ToString("0") + "%";
            shredderFuelText.color = Color.white;
        }
        HandleFuelPointerUpdate(fuelPercentage);
        HandleFinishProcess();
        HandleShreddingProcess();
    }
    void HandleFuelPointerUpdate(float currentFuelPercentage)
    {
        if (currentFuelPercentage <= 0)
        {
            UpdateFuelPointer(fuelPercetnageType.Zero);
        }
        else if (currentFuelPercentage > 0 && currentFuelPercentage <= 25)
        {
            UpdateFuelPointer(fuelPercetnageType.TwentyFive);
        }
        else if (currentFuelPercentage > 25 && currentFuelPercentage <= 50)
        {
            UpdateFuelPointer(fuelPercetnageType.Fifty);
        }
        else if (currentFuelPercentage > 50 && currentFuelPercentage <= 75)
        {
            UpdateFuelPointer(fuelPercetnageType.SeventyFive);
        }
        else
        {
            UpdateFuelPointer(fuelPercetnageType.OneHundered);
        }
    }
    void UpdateFuelPointer(fuelPercetnageType newFuelPercentageType)
    {
        if (currentFuelPercentageType != newFuelPercentageType)
        {
            currentFuelPercentageType = newFuelPercentageType;
            fuelPointerRenderer.material = null;
            switch (newFuelPercentageType)
            {
                case fuelPercetnageType.Zero:
                    fuelPointerRenderer.material = redGlow;
                    fuelPointer.transform.localPosition = ZeroPercentagePosition;
                    fuelPointer.transform.localRotation = ZeroPercentageRotation;
                    break;
                case fuelPercetnageType.TwentyFive:
                    fuelPointerRenderer.material = yellowGlow;
                    fuelPointer.transform.localPosition = TwentyFivePercentagePosition;
                    fuelPointer.transform.localRotation = TwentyFivePercentageRotation;
                    break;
                case fuelPercetnageType.Fifty:
                    fuelPointerRenderer.material = yellowGlow;
                    fuelPointer.transform.localPosition = FiftyPercentagePosition;
                    fuelPointer.transform.localRotation = FiftyPercentageRotation;
                    break;
                case fuelPercetnageType.SeventyFive:
                    fuelPointerRenderer.material = greenGlow;
                    fuelPointer.transform.localPosition = SeventyFivePercentagePosition;
                    fuelPointer.transform.localRotation = SeventyFivePercentageRotation;
                    break;
                case fuelPercetnageType.OneHundered:
                    fuelPointerRenderer.material = greenGlow;
                    fuelPointer.transform.localPosition = OneHunderedPercentagePosition;
                    fuelPointer.transform.localRotation = OneHunderedPercentageRotation;
                    break;
            }
        }
    }
    void HandleShreddingProcess()
    {
        if (initShredding)
        {
            if (IsOutOfFuel())
            {
                SetWheelStatus(false);
                secretHealth = 0;
                initShredding = false;
                return;
            }
            else
            {
                _refillManager.activateRefill = false;

                if (_wheelPlug.value > 0.1f)
                {
                    secretHealth -= fuelDecrease * Time.deltaTime;
                }

                _wheelPlug.value -= decreaseMultiplier * Time.deltaTime;
                if (_wheelPlug.value < 0) // Check if player spins wheel other way round
                {
                    _wheelPlug.value = 0;
                }

                UpdateProgressBar();
            }
        }
        else
        {
            //Debug.Log("not shredding");
        }
    }
    void HandleFinishProcess()
    {
        if (_wheelPlug.value >= valueToComplete)
        {
            finishedShreddingEvent.Invoke();

            ResetProgressBar();

            _leverPlug.SetHandleAngle(_leverPlug.maxAngle); // reset lever

            _wheelPlug.value = 0;
            _wheelPlug.enabled = false; 

            progressText.text = "Shreddinator Process Completed";

            maxHealth = GetNewHealth();
            secretHealth = maxHealth;

            e_shredderFinish?.InvokeEvent(particleSpawnLocation.position, Quaternion.identity, transform);

            initShredding = false;

            //check who is in the list
            foreach (Item itemsToDelete in shredderItemCollider.GetProductList())
            {
                ItemData deletedItemData = itemsToDelete.Data;
                //delete the product
                Destroy(itemsToDelete.gameObject);
                //spawn the raw materials
                foreach (ItemData a in deletedItemData.productContainable)
                {
                    float x = Random.Range(-_spawnPointBound.extents.x, _spawnPointBound.extents.x);
                    float z = Random.Range(-_spawnPointBound.extents.z, _spawnPointBound.extents.z);
                    //a.GetPrefab().GetComponent<Rigidbody>().isKinematic = false;
                    //a.GetPrefab().GetComponent<Rigidbody>().useGravity = true;

                    //Instantiate(a.GetPrefab(), _spawnPointBound.center + new Vector3(x, 0f, z), Quaternion.identity);

                    Instantiate(a.GetPrefab(), spawnPoint.transform.GetComponent<Collider>().bounds.center, Quaternion.identity);
                }
            }
            //clear the list
            shredderItemCollider.ClearProductList(); 
        }
    }
    void UpdateProgressBar()
    {
        float fillAmount = _wheelPlug.value / valueToComplete; // Calculate fill amount based on current value and total value

        // Calculate the next milestone (next multiple of 0.1)
        float nextMilestone = Mathf.Ceil(fillAmount * 10) / 10;

        // Adjust fill amount to the next milestone
        progressBar.fillAmount = Mathf.Clamp(nextMilestone, 0f, 1f);

        // Update progress text
        progressText.text = (progressBar.fillAmount * 100).ToString("0.0") + "%";
    }

   
    void ResetProgressBar()
    {
        progressBar.fillAmount = 0;
    }

    void IncreaseProgress()
    {
        if (!initShredding)
        {
            return;
        }
        _chargeValue += chargeSpeed * Time.deltaTime;
    }

    public void ResetWheelValue()
    {
        _wheelPlug.value = 0;
    }


    // SETTERS // 
    public void SetWheelValue(float value)
    {
        _wheelPlug.value = value; 
    }

    public void SetWheelStatus(bool status)
    {
        _wheelPlug.enabled = status; // set ability to spin wheel here
    }

    public void SetWheelCurrState(WheelStatus stat)
    {
        _wheelManager.SetWheelCurrState(stat);
    }

    //public void SetAttachedWheel(GameObject newWheel)
    //{
    //    newWheel.transform.SetParent(wheel.transform);
    //    InitWheel(newWheel);
    //}

    //public void InitWheel(GameObject toInit)
    //{
    //    _attachedWheel = toInit;
    //    InitWheelVariables();
    //}
    // GETTERS
    public float GetWheelValue()
    {
        return _wheelPlug.value;
    }

    public GameObject GetAttachedWheel()
    {
        return _attachedWheel;
    }
    public WheelManager GetWheelHandler()
    {
        return _wheelManager;
    }
    public int GetRandomValueToBreak()
    {
        return Random.Range(3, (int)valueToComplete);
    }
    public float GetNewHealth()
    {
        return Random.Range(150, 200);
    }
}