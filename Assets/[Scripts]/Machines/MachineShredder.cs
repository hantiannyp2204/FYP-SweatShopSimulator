using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Oculus.Interaction;
using UnityEngine.Rendering.UI;
using Unity.VisualScripting;

public class MachineShredder : MonoBehaviour, Iinteractable
{
    [SerializeField] private GameObject spamButton;
    [SerializeField] private Transform spawnLocation;

    [Header("KEYBOARD PLAYER")]
    [HideInInspector] public float secretHealth;
    [HideInInspector] public float maxHealth;

    [SerializeField] private Scrollbar progressBar;

    [SerializeField] private GameObject player;
    [SerializeField] private KeyboardGameManager gameManager;
    [SerializeField] private GameObject afterInteract;

    [Header("Debug")]
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text distFromPlayerText;
    [SerializeField] private TMP_Text shredderFuelText;
    [SerializeField] private TMP_Text productToShredText;
    [SerializeField] private TMP_Text lockedInProductText;

    [Header("Shredder Machine Settings")]
    [SerializeField] private float decreaseMultiplier;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float distToStop;
    [SerializeField] private Transform spawnPoint;

    [Header("Sound Effects / Feedback")]
    [SerializeField] private FeedbackEventData e_interactShredder;
    [SerializeField] private FeedbackEventData e_shredderFinish;


    private Item _productToShred;
    private bool _initShredding = false;

    private float _chargeValue;

    private Bounds _spawnPointBound;
    private RefillFuelManager _refillManager;

    private Item _itemToSave;

    public void RunActive()
    {
        GameObject spam = Instantiate(spamButton, spawnLocation.transform.position, Quaternion.identity);
            

        _initShredding = true;
        Debug.Log("Running");
    }

    public void RunDeactive()
    {
        Debug.Log("Deactivate");
    }

    public void RunSpamButton()
    {
        Debug.Break();
        IncreaseProgress();
        UpdateProgressBar();
    }
    public bool IsOutOfFuel()
    {
        return secretHealth <= 0 ? true : false;
    }
    public float GetNewHealth()
    {
        return Random.Range(2,10);
    }
    public bool CanInteract()
    {
        return true;
    }

    public float GetInteractingLast()
    {
        throw new System.NotImplementedException();
    }

    public string GetInteractName()
    {
        return "Shredder";
    }

    public void Interact(KeyboardGameManager player)
    {
        if (_itemToSave == null)
        {
            productToShredText.text = "Press F to Lock Product While holding item";
            return;
        }

        if (player.playerInventory.GetCurrentItem() == null || _refillManager.activateRefill) // Check if in the middle of fuellling
        {
            return;
        }

        Item product = player.playerInventory.GetCurrentItem();
        // All other items will have their containable with nothing
        if (product == null || product.Data.productContainable.Count == 0) // Product needs to contain smth
        {
            return; // Check if item is product or just another itS
        }

        if (product != _itemToSave)
        {
            productToShredText.text = "NOT LOCKED IN PRODUCT";
            return;
        }
        else
        {
            //player.playerInventory.RemoveAtCurrentSlot();
            product.transform.position = afterInteract.transform.position;

            //e_interactShredder?.InvokeEvent(transform.position, Quaternion.Euler(-90, 0, 0), transform);

            e_interactShredder?.InvokeEvent(transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity, transform);

            _initShredding = true;
            productToShredText.text = "Product To Shred: " + product.Data.name;
            _productToShred = product; // store product in seperate variable for safety purpose

            //product.gameObject.transform.position = afterInteract.transform.position;

            if (_productToShred.Data.productContainable != null)
            {
                foreach  (ItemData a in _productToShred.Data.productContainable)
                {
                    Debug.Log("This product contains: " + a.itemName);
                }
            }
        }
    }
   
    // Start is called before the first frame update
    void Start()
    {
        _spawnPointBound = spawnPoint.GetComponent<Collider>().bounds;

        maxHealth = GetNewHealth();
        secretHealth = maxHealth;

        _refillManager = GetComponentInChildren<RefillFuelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        shredderFuelText.text = "Fuel: " + (int) secretHealth;
      
        if (IsOutOfFuel())
        {
            shredderFuelText.text = "NO FUEL!";
        }

        if (gameManager.playerInventory.GetCurrentItem() != null)
        {
            if (Input.GetKeyDown(KeyCode.F)) // check if item is a product
            {
                if (gameManager.playerInventory.GetCurrentItem().Data.productContainable != null)
                {
                    if (gameManager.playerInventory.GetCurrentItem() != _itemToSave && _itemToSave != null)
                    {
                        return;
                    }
                    else
                    {
                        _itemToSave = gameManager.playerInventory.GetCurrentItem();
                        lockedInProductText.text = "Locked In : " + _itemToSave.gameObject.name;
                    }
                }
                else
                {
                    Debug.Log("lalla");
                }
            }
        }
        else
        {
            Debug.Log("Holding nothing");
        }

        
        if (_initShredding)
        {
            if (IsOutOfFuel())
            {   
                secretHealth = 0;
                _initShredding = false;
                return;
            }
            else
            {
                secretHealth -= 2 * Time.deltaTime;

                _chargeValue -= decreaseMultiplier * Time.deltaTime;
                _chargeValue = Mathf.Clamp(_chargeValue, 0, 100);
                UpdateProgressBar();

                distFromPlayerText.text = "DFM: " + Vector3.Distance(player.transform.position, transform.position);
                // Check if shredding and player goes ou
                if (Vector3.Distance(transform.position, player.transform.position) >= distToStop && _initShredding) 
                {
                    _chargeValue = 0;
                    UpdateProgressBar();

                    _initShredding = false;
                }

                progressText.text = (_chargeValue * 100).ToString("0.0") + "%";

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    IncreaseProgress();
                    UpdateProgressBar();
                }
            }
        }


        if (_chargeValue >= 1)
        {
            _itemToSave = null;

            progressText.text = "Shreddinator Process Completed";

            maxHealth = GetNewHealth();
            secretHealth = maxHealth;

            e_shredderFinish?.InvokeEvent(transform.position + new Vector3(0, 1f, 0), Quaternion.identity, transform);

            _initShredding = false;
            _chargeValue = 0;

            foreach (ItemData a in _productToShred.Data.productContainable)
            {
                float x = Random.Range(-_spawnPointBound.extents.x, _spawnPointBound.extents.x);
                float z = Random.Range(-_spawnPointBound.extents.z, _spawnPointBound.extents.z);

                Instantiate(a.GetPrefab(), _spawnPointBound.center + new Vector3(x, 0f, z), Quaternion.identity);
            }
        }
    }

    void UpdateProgressBar()
    {
        //progressBar.size = _chargeValue;
        progressText.text = (_chargeValue * 100).ToString("0.0") + "%";
    }

    void IncreaseProgress()
    {
        _chargeValue += chargeSpeed * Time.deltaTime;

        if (_chargeValue > 1)
        {
            _chargeValue = 1;
        }
    }
}
    
