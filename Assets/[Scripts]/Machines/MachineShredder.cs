using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Oculus.Interaction;
using UnityEngine.Rendering.UI;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UIElements;

public class MachineShredder : MonoBehaviour
{
    [SerializeField] private GameObject spamButton;
    [SerializeField] private GameObject fuelButton;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Transform particleSpawnLocation;
    public VrMachineItemCollider shredderItemCollider;

    [Header("KEYBOARD PLAYER")]
    [HideInInspector] public float secretHealth;
/*    [HideInInspector]*/ 
    public float maxHealth;

    [SerializeField] private Scrollbar progressBar;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject afterInteract;

    [Header("Debug")]
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text distFromPlayerText;
    [SerializeField] private TMP_Text productToShredText;
    [SerializeField] private TMP_Text lockedInProductText;
    public TMP_Text shredderFuelText;

    [Header("Shredder Machine Settings")]
    [SerializeField] private float decreaseMultiplier;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float distToStop;
    [SerializeField] private Transform spawnPoint;

    [Header("Sound Effects / Feedback")]
    [SerializeField] private FeedbackEventData e_interactShredder;
    [SerializeField] private FeedbackEventData e_shredderFinish;

    private bool _initShredding = false;
    private float _chargeValue;

    private Bounds _spawnPointBound;
    private RefillFuelManager _refillManager;

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
        //_initShredding = true;
        //lock the items, so they can no longer be picked up
        foreach(Item itemToShred in shredderItemCollider.GetProductList())
        {
            XRBaseInteractable baseInteractable = itemToShred.GetComponent<XRBaseInteractable>();
            baseInteractable.enabled= false;
            itemToShred.GetComponent<Rigidbody>().isKinematic = true;
        }
        GameObject spam = Instantiate(spamButton, spawnLocation.transform.position, Quaternion.identity);
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
            if (!_initShredding)
            {
                _initShredding = true;
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
        return Random.Range(2,10);
    }

    // Start is called before the first frame update
    void Start()
    {
        _spawnPointBound = spawnPoint.GetComponent<Collider>().bounds;

        maxHealth = GetNewHealth();
        secretHealth = maxHealth;

        _refillManager = GetComponentInChildren<RefillFuelManager>();

        secretHealth = 0;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("FUEL LEFT: " + secretHealth);

        if (_initShredding || _refillManager.activateRefill)
        {
            shredderFuelText.text = "Fuel: " + (int) secretHealth;
            //e_interactShredder?.InvokeEvent(transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity, transform);
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


        if (_initShredding)
        {
            if (IsOutOfFuel())
            {
                if (!fuelButton.gameObject.activeSelf) // make button visible 
                {
                    fuelButton.gameObject.SetActive(true);
                }
                secretHealth = 0;
                _initShredding = false;
                return;
            }
            else
            {
                secretHealth -= 1 * Time.deltaTime;
                _chargeValue -= decreaseMultiplier * Time.deltaTime;
                if (_chargeValue < 0)
                {
                    _chargeValue = 0;
                }    
                UpdateProgressBar();

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

        if (_chargeValue >= 1)
        {
            spamButton.gameObject.SetActive(false);
            _chargeValue = 0;
            progressText.text = "Shreddinator Process Completed";

            maxHealth = GetNewHealth();
            secretHealth = maxHealth;

            e_shredderFinish?.InvokeEvent(particleSpawnLocation.position, Quaternion.identity, transform);

            _initShredding = false;

            //check who is in the list
            foreach(Item itemsToDelete in shredderItemCollider.GetProductList())
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
    }

    void UpdateProgressBar()
    {
        //progressBar.size = _chargeValue;
        progressText.text = (_chargeValue * 100).ToString("0.0") + "%";
    }

    void IncreaseProgress()
    {
        if (!_initShredding)
        {
            return;
    
        }
        _chargeValue += chargeSpeed * Time.deltaTime;

        //if (_chargeValue > 1)
        //{
        //    _chargeValue = 1;
        //}
    }
}
    
