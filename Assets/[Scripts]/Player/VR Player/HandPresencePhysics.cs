using System.Collections;
using UnityEngine;
using static VRHandManager;

public class HandPresencePhysics : MonoBehaviour
{
    public Transform handXRController;
    private Rigidbody rb;
    [SerializeField] private float zRotationOffset;

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
    public void IgnoreCollision(GameObject itemToIgnore)
    {
        if (collisionRecoverCoroutineHandler != null && previousGrabbedCollisionObject == itemToIgnore)
        {
            StopCoroutine(collisionRecoverCoroutineHandler);
            collisionRecoverCoroutineHandler = null;
        }
        grabbedCollisionObject = itemToIgnore;


        itemColliderArray = itemToIgnore.GetComponentsInChildren<Collider>();
        foreach (Collider handCollider in GetComponent<HandColliders>().GetHandColliders())
        {
            foreach (Collider itemCollider in itemColliderArray)
            {
                Physics.IgnoreCollision(handCollider, itemCollider, true);
            }
        }
    }

    public void ResetIgnoreCollision()
    {
        if (grabbedCollisionObject == null) return;
       
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