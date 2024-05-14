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

    private bool gameStarted = false;
    ItemData requestedItem;

    private float _pointsToReward;
    private float _tracker;


    //Request box hitbox
    public GameObject insertedItem;
    XRBaseInteractable interactable;
    private XRBaseInteractor interactorUsingThis;
    [SerializeField] XRBaseInteractable boxInteractable;

    //hitbox skin
    [SerializeField] GameObject openedBox;
    [SerializeField] GameObject closedBox;

    [Header("FEEDBACK")]
    [SerializeField] private FeedbackEventData e_boxOpen;
    [SerializeField] private FeedbackEventData e_boxClose;

    private void Start()
    {
        OpenBox();
    }
    private void CloseBox()
    {
        boxInteractable.enabled = false;
        openedBox.SetActive(false);
        closedBox.SetActive(true);
        e_boxClose?.InvokeEvent(transform.position, Quaternion.identity, transform);

    }
    private void OpenBox()
    {
        if (boxInteractable != null) // null check, joshua
        {
            boxInteractable.enabled = false;
        }
        openedBox.SetActive(true);
        closedBox.SetActive(false);
        e_boxOpen?.InvokeEvent(transform.position, Quaternion.identity, transform);
    }
    //to run at when sending order
    public void SendRequestOver()
    {
        CloseBox();
        if (insertedItem != null)
        {
            Destroy(insertedItem);
        }
        insertedItem = null;
        requestedItem = null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        SendItemIntoRequestBox(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        SendItemIntoRequestBox(collision);
    }
    private void SendItemIntoRequestBox(Collision collision)
    {
        Item collisionItemComponent = collision.gameObject.GetComponent<Item>();
        // Make sure there's no inserted item already, and the collided object is an Item
        if (insertedItem != null || collisionItemComponent == null) return;

        // Attempt to get the XRBaseInteractable component of the collided item
        interactable = collision.gameObject.GetComponent<XRBaseInteractable>();

        // If the item is interactable and not currently being held
        if (interactable != null && !interactable.isSelected)
        {
            boxInteractable.enabled = true;
            insertedItem = collision.gameObject;
            // Teleport the item onto the box    
            //set the parent to this
            insertedItem.transform.SetParent(transform, false);
            insertedItem.transform.localPosition = collisionItemComponent.Data.GetRequestBoxPositionOffset();
            insertedItem.transform.localRotation = collisionItemComponent.Data.GetRequestBoxRotationOffset();

            insertedItem.GetComponent<Rigidbody>().isKinematic = true;


            // Temporarily disable the XRBaseInteractable to prevent picking up
            interactable.enabled = false;
        }
    }
    public void SetInsertedItem(GameObject item) // when robot reaches table insert the item
    {

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
            interactableComponent.selectEntered.RemoveAllListeners();
            interactableComponent.selectEntered.AddListener(PickUpItemFromBox);
        }
    }

    protected void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        var interactableComponent = GetComponent<XRBaseInteractable>();
        if (interactableComponent != null)
        {
            interactableComponent.selectEntered.RemoveAllListeners();
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
        boxInteractable.enabled = false;
    }


    private void Update()
    {
        if (gameStarted && requestedItem != null)
        {
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
    public int ShowScoreResult() => (int)_pointsToReward;
    public void Init()
    {
        gameStarted = false;
        ResetPointTracker();
    }
    public void StartGame()
    {
        if(gameStarted)
        {
            return; 
        }
        else
        {
            gameStarted = true;
        }
    }
    public void ResetPointTracker()
    {
        _tracker = 0;
     
        _pointsToReward = 0;
 
    }
    public void SetRequestedItem(ItemData newRequestedItem)
    {
        OpenBox();
        requestedItem = newRequestedItem;
        _pointsToReward = newRequestedItem.GetScoreGiven();
    }

    public ItemData GetRequestedItem() => requestedItem;
    public ItemData GetInsertedItemData() => insertedItem.GetComponent<Item>().Data;
    public GameObject GetInsertedItem() => insertedItem;
}
