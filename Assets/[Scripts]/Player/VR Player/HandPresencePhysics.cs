using System.Collections;
using UnityEngine;
using static VRHandManager;

public class HandPresencePhysics : MonoBehaviour
{
    public Transform handXRController;
    private Rigidbody rb;
    [SerializeField] private float zRotationOffset;
    private bool isLocked = false;

    // Preserved these offsets as private fields.
    private Vector3 lockedPositionOffset = Vector3.zero;
    private Quaternion lockedRotationOffset = Quaternion.identity;
    HandType handType = HandType.Left;

    Coroutine collisionRecoverCoroutineHandler;
    GameObject grabbedCollisionObject;
    GameObject previousGrabbedCollisionObject;
    Collider[] itemColliderArray;

    public void Init()
    {
        // Set the hand type based on the object name
        if (transform.name.Contains("Right Hand")) // Simplified check
        {
            handType = HandType.Right;
        }
        rb = GetComponent<Rigidbody>();
    }

    public HandType GetHandType() => handType;

    public void HandPhysicsFixedUpdate()
    {
        if (isLocked)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.MovePosition(lockedPositionOffset);
            rb.MoveRotation(lockedRotationOffset);
        }
        else
        {
            // Normal behavior
            rb.velocity = (handXRController.position - transform.position) / Time.fixedDeltaTime;

            Quaternion targetRotationWithOffset = handXRController.rotation * Quaternion.Euler(0, 0, zRotationOffset);
            Quaternion rotationDifference = targetRotationWithOffset * Quaternion.Inverse(transform.rotation);
            rotationDifference.ToAngleAxis(out float angleInDegrees, out Vector3 rotationAxis);

            if (angleInDegrees > 180) // Properly handle angle wrapping
            {
                angleInDegrees -= 360;
            }

            if (rotationAxis != Vector3.zero)
            {
                Vector3 rotationDifferenceInDegrees = angleInDegrees * rotationAxis;
                rb.angularVelocity = (rotationDifferenceInDegrees * Mathf.Deg2Rad / Time.fixedDeltaTime);
            }
            else
            {
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    public void LockHand(GameObject itemToLockOn, Vector3 positionOffset = default, Quaternion rotationOffset = default)
    {
        if (collisionRecoverCoroutineHandler != null && previousGrabbedCollisionObject == itemToLockOn)
        {
            StopCoroutine(collisionRecoverCoroutineHandler);
            collisionRecoverCoroutineHandler = null;
        }
        grabbedCollisionObject = itemToLockOn;

        isLocked = true;
        lockedPositionOffset = positionOffset;
        lockedRotationOffset = rotationOffset;

        rb.MovePosition(lockedPositionOffset);
        rb.MoveRotation(lockedRotationOffset);
        //transform.rotation = Quaternion.Euler(0, 0, zRotationOffset)* lockedRotationOffset; // Adjusted rotation update

        itemColliderArray = itemToLockOn.GetComponentsInChildren<Collider>();
        foreach (Collider handCollider in GetComponent<HandColliders>().GetHandColliders())
        {
            foreach (Collider itemCollider in itemColliderArray)
            {
                Physics.IgnoreCollision(handCollider, itemCollider, true);
            }
        }
    }

    public void UnlockHand()
    {
        if (grabbedCollisionObject == null) return;
        isLocked = false;
        lockedPositionOffset = Vector3.zero;
        lockedRotationOffset = Quaternion.identity;

        previousGrabbedCollisionObject = grabbedCollisionObject;
        grabbedCollisionObject = null;
        collisionRecoverCoroutineHandler = StartCoroutine(RecoverCollisionCoroutine(itemColliderArray));
        itemColliderArray = null;
    }

    IEnumerator RecoverCollisionCoroutine(Collider[] colliderToRecoverList)
    {
      
        yield return new WaitForSeconds(GetComponent<HandColliders>().GetCollisionRecoverDelay());

        foreach (Collider handCollider in GetComponent<HandColliders>().GetHandColliders())
        {
            foreach (Collider itemCollider in colliderToRecoverList)
            {
                Physics.IgnoreCollision(handCollider, itemCollider, false);
            }
        }

        previousGrabbedCollisionObject = null;
        collisionRecoverCoroutineHandler = null;
    }
}