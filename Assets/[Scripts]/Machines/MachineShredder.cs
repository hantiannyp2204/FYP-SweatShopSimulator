using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Oculus.Interaction;
using UnityEngine.Rendering.UI;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UIElements;
using UnityEngine.XR.Content.Interaction;


public class MachineShredder : MonoBehaviour
{
    // unused variables // keep just in case

//   {
//    [SerializeField] private Transform spawnLocation;
//  }

    public WheelStatus currWheelStatus;

    public UnityEvent finishedShreddingEvent;

    public bool initShredding = false;
    public int _breakAtThisValue;
    public float force;

    [SerializeField] private XRSocketInteractor wheelPreview;

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

    // Start is called before the first frame update
    void Start()
    {
        if (finishedShreddingEvent == null)
        {
            finishedShreddingEvent = new UnityEvent();
        }

        //currWheelStatus = WheelStatus.WORKING;

        //wheel.SetActive(false);
        _attachedWheel = wheel;

        _wheelPlug = wheel.GetComponentInChildren<XRKnob>();

        _wheelPlug.enabled = false;

        if (_wheelPlug == null)
        {
            return;
        }

        _wheelPlug.value = 0;

        _wheelPlug.ValueChangeShredder.AddListener(ValueChangeCheck);

        _wheelManager = _wheelPlug.GetComponent<WheelManager>();

        _wheelManager.canStartShredding.AddListener(CanShred);

        _wheelManager.SetWheelCurrState(WheelStatus.WORKING);

        _grabber = _wheelManager.GetComponent<XRVelocityRayGrab>();

        if (_grabber != null)
        {
            _grabber.enabled = false; // disable ability to grab
        }

        _leverPlug = lever.GetComponentInChildren<XRLever>();
            
        _spawnPointBound = spawnPoint.GetComponent<Collider>().bounds;

        maxHealth = GetNewHealth();
        secretHealth = maxHealth;

        _refillManager = GetComponentInChildren<RefillFuelManager>();

        SetUpWheelProbability();

        fixWheelCollider.GetComponent<Collider>().enabled = false;

        wheelPreview.enabled = false;
    }

    public void InitWheelVariables()
    {
        _wheelPlug = wheel.GetComponentInChildren<XRKnob>();

        //_wheelPlug.enabled = false;

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
                wheelPreview.enabled = true;
                SetGameobjectLayer(wheel, 0);
                //currWheelStatus = WheelStatus.BROKEN;
                _wheelPlug.GetComponent<Rigidbody>().useGravity = true;
                _wheelPlug.GetComponent<Rigidbody>().isKinematic = false;
                _wheelPlug.GetComponent<Rigidbody>().AddForce(Vector3.up * force, ForceMode.Impulse);

                SetWheelStatus(false);
                ResetWheelValue();

                fixWheelCollider.GetComponent<Collider>().enabled = true; // enable collider after breaking
                _wheelManager.transform.SetParent(null);
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
            shredderFuelText.text = "Fuel: " + fuelPercentage.ToString("0") + "%";
            shredderFuelText.color = Color.white;
        }

        HandleFinishProcess();
        HandleShreddingProcess();
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
            Debug.Log("not shredding");
        }
    }
    void HandleFinishProcess()
    {
        if (_wheelPlug.value >= valueToComplete)
        {
            _leverPlug.SetHandleAngle(_leverPlug.maxAngle); // reset lever

            wheel.SetActive(false);
            _wheelPlug.value = 0;

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
                    a.GetPrefab().GetComponent<Rigidbody>().isKinematic = false;
                    a.GetPrefab().GetComponent<Rigidbody>().useGravity = true;

                    Instantiate(a.GetPrefab(), _spawnPointBound.center + new Vector3(x, 0f, z), Quaternion.identity);
                    //if (a.mat != null) // check if nothing has been 
                    //{
                    //    a.GetPrefab().GetComponent<Item>().SetMaterial(a.mat);
                    //}
                }
            }
            //finishedShreddingEvent.Invoke();
            //clear the list
            shredderItemCollider.ClearProductList();
        }
        else
        {
            Debug.Log("h");
        }
    }
    void UpdateProgressBar()
    {
        progressText.text = _wheelPlug.value.ToString("0.0") + "%";
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
        _wheelPlug.enabled = status;
    }

    public void SetAttachedWheel(GameObject newWheel)
    {
        newWheel.transform.SetParent(wheel.transform);
        InitWheel(newWheel);
    }

    public void InitWheel(GameObject toInit)
    {
        _attachedWheel = toInit;
        InitWheelVariables();
        //var grab = toInit.GetComponent<XRVelocityRayGrab>();
        //if (grab != null)
        //{
        //    grab.enabled = false;
        //}
    }
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
    
