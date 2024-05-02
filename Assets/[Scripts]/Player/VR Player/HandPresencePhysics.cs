using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
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
    [SerializeField] List<XRBaseInteractor> interactorList= new List<XRBaseInteractor>();
    public TMP_Text DebugTxt;
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
        foreach(XRBaseInteractor interactor in  interactorList)
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
        ResetIgnoreCollision();
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
    public void IgnoreCollision  (GameObject itemToIgnore)
    {
        //dont run if its a generator
        if (itemToIgnore.GetComponent<Generators>()) return;
        if (DebugTxt != null)
        {
            DebugTxt.text = "IGNORE " + itemToIgnore.name;
        }
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
                if (itemCollider != null)
                {
                    Physics.IgnoreCollision(handCollider, itemCollider, false);
                }
            }
        }

        previousGrabbedCollisionObject = null;
        collisionRecoverCoroutineHandler = null;
    }
}