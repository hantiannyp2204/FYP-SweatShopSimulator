using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RequestBox : MonoBehaviour
{
    //Request box

    bool boxOpened = false;
    ItemData requestedItem;

    private float _timer;
    private float _pointsToReward;
    private float _tracker;


    //Request box hitbox
    public GameObject insertedItem;
    XRBaseInteractable interactable;
    private XRBaseInteractor interactorUsingThis;

    //hitbox skin
    [SerializeField] GameObject openedBox;
    [SerializeField] GameObject closedBox;
    private void Start()
    {
        OpenBox();
    }
    private void CloseBox()
    {
        openedBox.SetActive(false);
        closedBox.SetActive(true);
    }
    private void OpenBox()
    {
        openedBox.SetActive(true);
        closedBox.SetActive(false);
    }
    public void ResetBox()
    {
        CloseBox();
        if (insertedItem != null)
        {
            Destroy(insertedItem);
        }
        insertedItem = null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Item collisionItemComponent = collision.gameObject.GetComponent<Item>();
        // Make sure there's no inserted item already, and the collided object is an Item
        if (insertedItem != null || collisionItemComponent == null) return;

        // Attempt to get the XRBaseInteractable component of the collided item
        interactable = collision.gameObject.GetComponent<XRBaseInteractable>();

        // If the item is interactable and not currently being held
        if (interactable != null && !interactable.isSelected)
        {
            insertedItem = collision.gameObject;
            // Teleport the item onto the box    
            //set the parent to this
            insertedItem.transform.SetParent(transform, false);
            insertedItem.transform.localPosition = Vector3.zero;
            insertedItem.transform.localRotation = collisionItemComponent.Data.GetRequestBoxRotationOffset();

            insertedItem.GetComponent<Rigidbody>().isKinematic = true;


            // Temporarily disable the XRBaseInteractable to prevent picking up
            interactable.enabled = false;
        }
    }

    public void SetInsertedItem(GameObject item) // when robot reaches table insert the item
    {
        OpenBox();
        Item collisionItemComponent = item.gameObject.GetComponent<Item>();
        // Make sure there's no inserted item already, and the collided object is an Item
        if (insertedItem != null || collisionItemComponent == null)
        {
            return;
        }

        // Attempt to get the XRBaseInteractable component of the collided item
        interactable = item.gameObject.GetComponent<XRBaseInteractable>();

        // If the item is interactable and not currently being held
        if (interactable != null && !interactable.isSelected)
        {
            insertedItem = item;
            // Teleport the item onto the box    
            //set the parent to this
            insertedItem.transform.SetParent(transform, false);
            insertedItem.transform.localPosition = Vector3.zero;
            insertedItem.transform.localRotation = collisionItemComponent.Data.GetRequestBoxRotationOffset();

            insertedItem.GetComponent<Rigidbody>().isKinematic = true;


            // Temporarily disable the XRBaseInteractable to prevent picking up
            interactable.enabled = false;
        }
    }

    protected void OnEnable()
    {
        // Assuming this component is also an XRBaseInteractable (based on the previous implementation),
        // subscribe to the select entered event.
        var interactableComponent = GetComponent<XRBaseInteractable>();
        if (interactableComponent != null)
        {
            interactableComponent.selectEntered.AddListener(PickUpItemFromBox);
        }
    }

    protected void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        var interactableComponent = GetComponent<XRBaseInteractable>();
        if (interactableComponent != null)
        {
            interactableComponent.selectEntered.RemoveListener(PickUpItemFromBox);
        }
    }

    private void PickUpItemFromBox(SelectEnterEventArgs args)
    {
        if (insertedItem == null) return;
        interactorUsingThis = args.interactor;

        // Re-enable the interactable component on the item
        interactable.enabled = true;
        insertedItem.transform.SetParent(null);
        // Force the interactor to pick up the current item
        insertedItem.AddComponent<GeneratorGeneratedItem>().SetHandInteractorAndAnimator(interactorUsingThis);
        insertedItem.GetComponent<Rigidbody>().isKinematic = false;
        args.interactor.StartManualInteraction(insertedItem.GetComponent<IXRSelectInteractable>());

        insertedItem = null;
    }


    private void Update()
    {
        if (boxOpened)
        {

            // Start timer when receive requests
            _timer += Time.deltaTime;
            Debug.Log(_timer);
            _tracker += Time.deltaTime;

            if (_tracker >= 5)
            {
                _pointsToReward -= 10;
                if (_pointsToReward <= 0)
                {
                    _pointsToReward = 0;
                    return;
                }
                _tracker = 0;
            }
        }
    }
    public float GetTimer() => _timer;
    public int ShowScoreResult() => (int)_pointsToReward;
    public void ResetPointTracker()
    {
        _tracker = 0;
        _timer = 0;
        _pointsToReward = 0;
        boxOpened = false;
    }
    public void SetRequestedItem(ItemData newRequestedItem)
    {
        requestedItem = newRequestedItem;
        boxOpened = true;
        _pointsToReward = newRequestedItem.GetScoreGiven();
    }

    public ItemData GetRequestedItem() => requestedItem;
    public ItemData GetInsertedItemData() => insertedItem.GetComponent<Item>().Data;
    public GameObject GetInsertedItem() => insertedItem;
}
