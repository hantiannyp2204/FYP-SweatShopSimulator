using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;
using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;
using static VRHandManager;

public class VRPlayerInvenetory : MonoBehaviour
{
    [SerializeField] Item currentHoldingItem;
    HandType handType = HandType.Left;

    public HandType GetHandType() => handType;
    public void Init()
    {
        //set the hand type
        if (transform.name == "Right hand")
        {
            handType = HandType.Right;
        }
    }
    public void AddItem(Item item, Transform handTransform)
    {
        //if theres item, ignore
        if (currentHoldingItem != null) return;
        //plays the pickup sound
        currentHoldingItem = item;
        item.ChangeState(ITEM_STATE.PICKED_UP);
        item.transform.SetParent(handTransform, true);

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }
    public void UpdateItemPositions()
    {
        if (currentHoldingItem == null) return;
        currentHoldingItem.transform.localPosition = Vector3.zero;
        currentHoldingItem.transform.localRotation = Quaternion.identity;
    }
    public void RemoveItem(Vector3 handVelocity)
    {
        //drops the item

        //return if theres already no item in the hand
        if (currentHoldingItem == null) return;
        currentHoldingItem.transform.SetParent(null);
        // Change item state to not picked up
        currentHoldingItem.ChangeState(ITEM_STATE.NOT_PICKED_UP);
        Rigidbody rb = currentHoldingItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(handVelocity * rb.mass, ForceMode.Impulse); // Apply force based on hand velocity and object mass
        }
        currentHoldingItem = null;
    }

}
