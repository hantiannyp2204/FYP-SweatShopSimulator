using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RobotDisplayOrder : MonoBehaviour
{
    public UnityEvent EnableDisplay;

    [SerializeField] private GameObject correctItemTick;
    [SerializeField] private RequestBox requestBox;
    [SerializeField] private GameObject displayOrder;
    private Image _displayOrder;
    private Image _greenTick;

    private bool _coroutineStatus = false;
    // Start is called before the first frame update
    void Start()
    {
       // displayOrder.SetActive(false);
        _displayOrder = displayOrder.GetComponent<Image>();
        displayOrder.SetActive(false);
        _greenTick = correctItemTick.GetComponent<Image>();
        correctItemTick.SetActive(false);

        if (EnableDisplay == null)
        {
            EnableDisplay = new UnityEvent();
        }

        EnableDisplay.AddListener(FirstTimePressStart);
    }


    void FirstTimePressStart()
    {
        if (!displayOrder.activeSelf)
        {
            displayOrder.SetActive(true); // set true only if it is disabled at the start
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRequestedItemImage();

        if (requestBox.GetCustomerTable().GetEnteredProductVerdict()) // if player successfully puts in product then we spawn an overlay
        {
            StartCoroutine(CorrectItemCoroutine());
            UpdateRequestedItemImage();
            requestBox.GetCustomerTable().SetProductVerdict(false);
        }
    }


    void UpdateRequestedItemImage()
    {
        if (requestBox.GetRequestedItemData() != null)
        {
            _displayOrder.sprite = requestBox.GetRequestedItemData().imageToDisplay;
        }
        else
        {
            Debug.Log("There is nothing to rq");
        }
    }
    IEnumerator CorrectItemCoroutine()
    {
        _displayOrder.color = Color.green;
        yield return new WaitForSeconds(1f);
        _displayOrder.color = Color.white;
    }
}
