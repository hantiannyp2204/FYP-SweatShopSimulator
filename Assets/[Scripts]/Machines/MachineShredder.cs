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
  /*  [HideInInspector]*/ public bool initShredding = false;
    public bool isWheeling = false;

    [Header("VR REFERENCES")]
    public GameObject lever;
    public GameObject wheel;
    [SerializeField] private float valueToComplete;

    [Header("BUTTON REFERENCES")]
    [SerializeField] private GameObject spamButton;
    [SerializeField] private GameObject fuelButton;

    [Header("LOCATION SPAWNER PREFABS")]
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Transform particleSpawnLocation;
    public VrMachineItemCollider shredderItemCollider;

    [Header("KEYBOARD PLAYER")]
     public float secretHealth;
    [SerializeField] private Scrollbar progressBar;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject afterInteract;
    public float maxHealth;

    [Header("Debug")]
    //[SerializeField] private TMP_Text distFromPlayerText;
    //[SerializeField] private TMP_Text productToShredText;
    //[SerializeField] private TMP_Text lockedInProductText;
    [SerializeField] private TMP_Text progressText;
    public TMP_Text shredderFuelText;

    [Header("Shredder Machine Settings")]
    [SerializeField] private float decreaseMultiplier;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float distToStop;
    [SerializeField] private Transform spawnPoint;

    [Header("Sound Effects / Feedback")]
    [SerializeField] private FeedbackEventData e_interactShredder;
    [SerializeField] private FeedbackEventData e_shredderFinish;

    private float _chargeValue;

    private Bounds _spawnPointBound;
    private RefillFuelManager _refillManager;

    private XRKnob _wheelPlug;
    private WheelManager _wheelManager;


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

    public float GetNewHealth()
    {
        return Random.Range(2, 10);
    }

    public void CanShred()
    {
        initShredding = true;
    }

    public void SetWheelStatus(bool status)
    {
        _wheelPlug.enabled = status;
    }

    // Start is called before the first frame update
    void Start()
    {
        wheel.SetActive(false);

        _wheelPlug = wheel.GetComponentInChildren<XRKnob>();

        _wheelPlug.value = 0;

        _wheelPlug.ValueChangeShredder.AddListener(ValueChangeCheck);

        _wheelManager = _wheelPlug.GetComponent<WheelManager>();

        _wheelManager.canStartShredding.AddListener(CanShred);
            
        _spawnPointBound = spawnPoint.GetComponent<Collider>().bounds;

        maxHealth = GetNewHealth();
        secretHealth = maxHealth;

        _refillManager = GetComponentInChildren<RefillFuelManager>();
    }

    public void ValueChangeCheck()
    {
        isWheeling = true;
        //if (IsOutOfFuel())
        //{
        //    return;
        //}
    }
    // Update is called once per frame
    void Update()
    {
        shredderFuelText.text = "Fuel: " + (int)secretHealth;
        UpdateProgressBar();

        if (initShredding || _refillManager.activateRefill)
        {
            e_interactShredder?.InvokeEvent(particleSpawnLocation.position,  Quaternion.identity, transform);
        }

        if (IsOutOfFuel())
        {
            shredderFuelText.text = "NO FUEL!";
        }
        else
        {
            Debug.Log("Holding nothing");
        }

        if (_wheelPlug.value >= valueToComplete)
        {
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
                    Instantiate(a.GetPrefab(), _spawnPointBound.center + new Vector3(x, 0f, z), Quaternion.identity);
                }
            }
            //clear the list
            shredderItemCollider.ClearProductList();    
        }

        if (initShredding)
        {
            if (IsOutOfFuel())
            {
                if (!fuelButton.gameObject.activeSelf) // make button visible 
                {
                    fuelButton.gameObject.SetActive(true);
                }

                secretHealth = 0;
                initShredding = false;
                return;
            }
            else if (isWheeling)
            {
                _refillManager.activateRefill = false;

                if (_wheelPlug.value > 0.0001f)
                {
                    secretHealth -= 1 * Time.deltaTime;
                }

                _wheelPlug.value -= decreaseMultiplier * Time.deltaTime; 
                if (_wheelPlug.value < 0) // Check if player spins wheel other way round
                {
                    _wheelPlug.value = 0;
                }

                UpdateProgressBar();
                {
                    //distFromPlayerText.text = "DFM: " + Vector3.Distance(player.transform.position, transform.position);
                    //// Check if shredding and player goes ou
                    //if (Vector3.Distance(transform.position, player.transform.position) >= distToStop && _initShredding) 
                    //{
                    //    _chargeValue = 0;
                    //    UpdateProgressBar();

                    //    _initShredding = false;
                    //}
                }
            }
        }
        {
            //if (_chargeValue >= 1)
            //{
            //    spamButton.gameObject.SetActive(false);
            //    _chargeValue = 0;
            //    progressText.text = "Shreddinator Process Completed";

            //    maxHealth = GetNewHealth();
            //    secretHealth = maxHealth;

            //    e_shredderFinish?.InvokeEvent(particleSpawnLocation.position, Quaternion.identity, transform);

            //    _initShredding = false;

            //    //check who is in the list
            //    foreach(Item itemsToDelete in shredderItemCollider.GetProductList())
            //    {
            //        ItemData deletedItemData = itemsToDelete.Data;
            //        //delete the product
            //        Destroy(itemsToDelete.gameObject);
            //        //spawn the raw materials
            //        foreach (ItemData a in deletedItemData.productContainable)
            //        {
            //            float x = Random.Range(-_spawnPointBound.extents.x, _spawnPointBound.extents.x);
            //            float z = Random.Range(-_spawnPointBound.extents.z, _spawnPointBound.extents.z);
            //            Instantiate(a.GetPrefab(), _spawnPointBound.center + new Vector3(x, 0f, z), Quaternion.identity);
            //        }
            //    }
            //    //clear the list
            //    shredderItemCollider.ClearProductList();
            //}
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
}
    
