using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MachineShredder : MonoBehaviour, Iinteractable
{
    [SerializeField] private Scrollbar progressBar;

    [SerializeField] private GameObject player;

    [Header("Debug")]
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text distFromPlayerText;

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

    private Bounds spawnPointBound;
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

    public void Interact(GameManager player)
    {
        e_interactShredder?.InvokeEvent(transform.position, Quaternion.identity, transform);

        if (player.playerInventory.GetCurrentItem() == null)
        {
            return;
        }

        _initShredding = true;
        Item product = player.playerInventory.GetCurrentItem();
        if (product == null) return;
        _productToShred = product; // store product in seperate variable for safety purpose

        if (_productToShred.Data.productContainable != null)
        {
            foreach  (ItemData a in _productToShred.Data.productContainable)
            {
                Debug.Log("This product contains: " + a.itemName);
            }
        }
    }
   
    // Start is called before the first frame update
    void Start()
    {
         spawnPointBound = spawnPoint.GetComponent<Collider>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (_initShredding)
        {
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
  
        if (_chargeValue >= 1)
        {
            e_shredderFinish?.InvokeEvent(transform.position, Quaternion.identity, transform);

            _initShredding = false;
            _chargeValue = 0;

            progressText.text = "Shreddinator Process Completed";
            foreach (ItemData a in _productToShred.Data.productContainable)
            {
                float x = Random.Range(-spawnPointBound.extents.x, spawnPointBound.extents.x);
                float z = Random.Range(-spawnPointBound.extents.z, spawnPointBound.extents.z);

                /*GameObject contents = */
                //Instantiate(a.GetPrefab(), spawnPoint.transform.position, Quaternion.identity);
                Instantiate(a.GetPrefab(), spawnPointBound.center + new Vector3(x, 0f, z), Quaternion.identity);
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





















//{
//    if (_initShredding)
//    {
//        _timer += Time.deltaTime;

//        if (_timer >= timeToFinish)
//        {
//            ResetText("Shredding completed");
//            _timer = 0;

//            _initShredding = false; // Finish Shredding
//            Destroy(_productToShred.gameObject);

//            foreach (ItemData a in _productToShred.Data.productContainable)
//            {
//                Instantiate(a.GetPrefab(), spawnPoint.transform.position, Quaternion.identity);
//            }
//        }
//        else
//        {
//            timerText.text = "Time to finish: " + _timer;
//        }
//    }
//}