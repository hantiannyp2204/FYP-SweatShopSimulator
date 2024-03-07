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

    bool dispose = false;
    public bool Dispose => dispose;
    protected virtual void Start()
    {
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        //raycast to make sure it spawns on the floor
        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y;
            transform.position = newPosition;
        }
    }




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

    public void SetDispose(bool dispose)
    {
        this.dispose = dispose;
    }

    public void Interact(GameManager player)
    {
        Debug.Log("Item picked up dah");
    }
    public enum ITEM_STATE
    {
        NOT_PICKED_UP,
        PICKED_UP
    }
}
