using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static VRHandManager;
using System.Linq;

public class HandPresencePhysics : MonoBehaviour
{
    public Transform handXRController;
    private Rigidbody rb;
    [SerializeField] private float zRotationOffset;
    HandType handType = HandType.Left;

    [SerializeField] List<XRBaseInteractor> interactorList = new List<XRBaseInteractor>();
    public TMP_Text DebugTxt;

    private List<(Coroutine, GameObject)> collisionRecoverCoroutines = new List<(Coroutine, GameObject)>();
    GameObject grabbedCollisionObject;
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

    private void OnEnable()
    {
        foreach (XRBaseInteractor interactor in interactorList)
        {
            interactor.selectEntered.AddListener(HandleSelectEntered);
            interactor.selectExited.AddListener(HandleSelectExited);
        }
    }

    private void OnDisable()
    {
        foreach (XRBaseInteractor interactor in interactorList)
        {
            interactor.selectEntered.RemoveListener(HandleSelectEntered);
            interactor.selectExited.RemoveListener(HandleSelectExited);
        }
    }

    private void HandleSelectEntered(SelectEnterEventArgs arg)
    {
        IgnoreCollision(arg.interactableObject.transform.gameObject);
    }

    private void HandleSelectExited(SelectExitEventArgs arg)
    {
        Debug.Log(arg.interactor.name + ": Let go");
        ResetIgnoreCollision(arg.interactableObject.transform.gameObject);
    }

    public HandType GetHandType() => handType;

    public void HandPhysicsFixedUpdate()
    {
        if (rb == null) return;
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
        // don't run if it's a generator
        if (itemToIgnore.CompareTag("Don't Ignore Collision")) return;
        if (DebugTxt != null)
        {
            DebugTxt.text = "IGNORE " + itemToIgnore.name;
        }

        // Stop and remove any ongoing coroutine for the item
        var existingCoroutine = collisionRecoverCoroutines.FirstOrDefault(tuple => tuple.Item2 == itemToIgnore);
        if (existingCoroutine != default)
        {
            StopCoroutine(existingCoroutine.Item1);
            collisionRecoverCoroutines.Remove(existingCoroutine);
        }

        grabbedCollisionObject = itemToIgnore;

        itemColliderArray = itemToIgnore.GetComponentsInChildren<Collider>();
        // add more colliders into the array if the item has LinkedColliders script
        LinkedColliders linkedColliderScript = itemToIgnore.GetComponent<LinkedColliders>();
        if (linkedColliderScript != null)
        {
            itemColliderArray = itemColliderArray.Concat(linkedColliderScript.GetAllLinkedColliders()).ToArray();
        }

        foreach (Collider handCollider in GetComponent<HandColliders>().GetHandColliders())
        {
            foreach (Collider itemCollider in itemColliderArray)
            {
                Physics.IgnoreCollision(handCollider, itemCollider, true);
            }
        }
    }

    public void ResetIgnoreCollision(GameObject itemToReset)
    {
        // Check if the item to reset exists in the list
        if (itemColliderArray == null || !itemColliderArray.Any(c => c.gameObject == itemToReset) || grabbedCollisionObject == null) return;

        grabbedCollisionObject = null;
        var coroutine = StartCoroutine(RecoverCollisionCoroutine(itemColliderArray, itemToReset));
        collisionRecoverCoroutines.Add((coroutine, itemToReset));
        itemColliderArray = null;
    }

    IEnumerator RecoverCollisionCoroutine(Collider[] colliderToRecoverList, GameObject itemToReset)
    {
        yield return new WaitForSeconds(GetComponent<HandColliders>().GetCollisionRecoverDelay());

        // Check if the itemToReset has been destroyed
        if (itemToReset == null)
        {
            collisionRecoverCoroutines.RemoveAll(tuple => tuple.Item2 == itemToReset);
            yield break;
        }

        foreach (Collider handCollider in GetComponent<HandColliders>().GetHandColliders())
        {
            foreach (Collider itemCollider in colliderToRecoverList)
            {
                if (itemCollider != null)
                {
                    Physics.IgnoreCollision(handCollider, itemCollider, false);
                }
            }
        }

        collisionRecoverCoroutines.RemoveAll(tuple => tuple.Item2 == itemToReset);
    }
}
