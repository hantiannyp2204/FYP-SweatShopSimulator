using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;
using static VRHandManager;

public class VRPlayerInvenetory : MonoBehaviour
{
    [SerializeField] Item currentHoldingItem;
    HandType handType = HandType.Left;

    Collider[] handColliders;

    [SerializeField] float collisionRecoverDelay = 1.5f;

    Coroutine collisionRevoverCoroutineHandler;
    public HandType GetHandType() => handType;
    public void Init()
    {
        //set the hand type
        if (transform.name == "Right hand" || transform.name == "Right Hand Physics")
        {
            handType = HandType.Right;
        }
        // Get all Collider components in this GameObject and its children
        handColliders = GetComponentsInChildren<Collider>();

    }
    public void AddItem(Item item, Transform handTransform)
    {
        //if theres item, ignore
        if (currentHoldingItem != null) return;

        //stop coroutine if it exist
        if(collisionRevoverCoroutineHandler != null)
        {
            StopCoroutine(collisionRevoverCoroutineHandler);
            collisionRevoverCoroutineHandler = null;
        }
        //plays the pickup sound
        currentHoldingItem = item;
        item.ChangeState(ITEM_STATE.PICKED_UP);
        item.transform.SetParent(handTransform, true);
        //ignore colliders
        foreach(Collider collider in handColliders)
        {
            item.IgnoreCollision(collider);
        }

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
        //reset the phyics colliders
        collisionRevoverCoroutineHandler = StartCoroutine(RecoverCollisionCoroutine(currentHoldingItem));

        Rigidbody rb = currentHoldingItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(handVelocity * rb.mass, ForceMode.Impulse); // Apply force based on hand velocity and object mass
        }
        currentHoldingItem = null;
    }
    IEnumerator RecoverCollisionCoroutine(Item itemToRecover)
    {
        yield return new WaitForSeconds(collisionRecoverDelay);
        itemToRecover.ResetIgnoreCollisions();
        collisionRevoverCoroutineHandler = null;
    }
}
