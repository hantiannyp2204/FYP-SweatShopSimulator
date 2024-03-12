using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MachineShredder : MonoBehaviour, Iinteractable
{
    [SerializeField] private Scrollbar progressBar;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private TMP_Text timerText;

    [Header("Shredder Machine Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float timeToFinish;

    private float _timer = 0;
    private Item _productToShred;
    private bool _initShredding = false;

    private float chargeValue;
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
        if (player.playerInventory.GetCurrentItem() == null)
        {
            ResetText("Nothing to Shred!");
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
        _timer = 0;
        timerText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        {
            //if (_initShredding)
            //{
            //    _timer += Time.deltaTime;

            //    if (_timer >= timeToFinish)
            //    {
            //        ResetText("Shredding completed");
            //        _timer = 0;

            //        _initShredding = false; // Finish Shredding
            //        Destroy(_productToShred.gameObject);

            //        foreach (ItemData a in _productToShred.Data.productContainable)
            //        {
            //            Instantiate(a.GetPrefab(), spawnPoint.transform.position, Quaternion.identity);
            //        }
            //    }
            //    else
            //    {
            //        timerText.text = "Time to finish: " + _timer;
            //    }
            //}
        }
        if (_initShredding)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IncreaseProgress();
                UpdateProgressBar();
            }
        }

        if (chargeValue >= 1)
        {
            _initShredding = false;
            chargeValue = 0;
            foreach (ItemData a in _productToShred.Data.productContainable)
            {
                Instantiate(a.GetPrefab(), spawnPoint.transform.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("Shredding");
        }
    }

    void ResetText(string toReset)
    {
        timerText.text = toReset;
    }

    void UpdateProgressBar()
    {
        progressBar.size = chargeValue;
        progressText.text = (chargeValue * 100).ToString("0.0") + "%";
    }

    void IncreaseProgress()
    {
        chargeValue += chargeSpeed * Time.deltaTime;

        if (chargeValue > 1)
        {
            chargeValue = 1;
        }
    }
}