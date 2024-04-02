using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour, Iinteractable
{
    [SerializeField] private ItemData data;
    public ItemData Data => data;
    [SerializeField] private ITEM_STATE itemState;
    // Feedback
    [Header("FEEDBACK")]
    [SerializeField] private FeedbackEventData e_pickUp;
    [SerializeField] private FeedbackEventData e_drop;

    [SerializeField] private LayerMask groundLayer;

    List<Collider> ignoredColliderList = new();
    public string GetInteractName() => "Interact with: " + data.GetName();


    public bool CanInteract()
    {
        return !CompareState(ITEM_STATE.PICKED_UP);
    }
    public float GetInteractingLast() => 0f;

    public void ChangeState(ITEM_STATE newState)
    {
        itemState = newState;
        switch (itemState)
        {
            case ITEM_STATE.NOT_PICKED_UP:
                e_drop?.InvokeEvent(transform.position, Quaternion.identity, transform);
                Debug.Log("Drop");
                break;
            case ITEM_STATE.PICKED_UP:
                e_pickUp?.InvokeEvent(transform.position, Quaternion.identity, transform);
                break;
            default:
                break;
        }
    }

    public bool CompareState(ITEM_STATE checkState)
    {
        return itemState == checkState;
    }
    public ITEM_STATE GetState()=> itemState;

    //for collision
    public void IgnoreCollision(Collider objectToIgnore)
    {
        Collider itemCollider = GetComponent<Collider>();
        Physics.IgnoreCollision(itemCollider, objectToIgnore, true);
        ignoredColliderList.Add(objectToIgnore);
    }
    public void ResetIgnoreCollisions()
    {
        Collider itemCollider = GetComponent<Collider>();
        foreach (Collider ignoredColliders in ignoredColliderList)
        {
            Physics.IgnoreCollision(itemCollider, ignoredColliders, false);
        }
        ignoredColliderList.Clear();
    }
    public void Interact(KeyboardGameManager player)
    {
        Debug.Log("Item picked up dah");
    }
    public enum ITEM_STATE
    {
        NOT_PICKED_UP,
        PICKED_UP
    }
}
