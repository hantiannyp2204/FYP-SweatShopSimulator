using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Platform;
using Oculus.Interaction.HandGrab;
using static VRHandManager;
using System;

public class VRHandManager : MonoBehaviour, ISubscribeEvents<IVRInteracted>, ISubscribeEvents<IVRRelease>
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private TMP_Text DebugText;
    private List<GameObject> currentlyTouching = new();
    private GameObject grabbedObject = null;
    private Vector3 lastHandPosition;
    private Vector3 handVelocity;

    //for sphere cast
    Vector3 sphereCenter;
    [SerializeField]float sphereRadius = 0.5f;

    //Physics movement
 
    public enum HandType
    {
        Left,
        Right
    }
    enum HandAction
    {
        Grabbing,
        Releasing
    }
    HandType handType = HandType.Left;
    HandAction currentHandAction;
    System.Action<GameObject, HandType> OnGrabbed;
    System.Action<Vector3, HandType> OnRelease;
    [SerializeField] private FeedbackEventData e_interactError;
    float triggerValue, gripValue;
    public HandType GetHandType()=> handType;

    // Start is called before the first frame update
    public void Init()
    {
        currentHandAction = HandAction.Releasing;
        lastHandPosition = transform.position;
        //set the hand type
        if (transform.name.Contains("Right Hand"))
        {
            handType = HandType.Right;
        }

    }
    public float GetGripValue() => gripValue;
    // Update is called once per frame
    public void UpdateInteractions()
    {
        // Define sphere center and radius
        sphereCenter = transform.position;

        // Update hand animations
        triggerValue = pinchAnimationAction.action.ReadValue<float>();
        gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);
        handAnimator.SetFloat("Grip", gripValue);

        //// Calculate hand velocity
        //handVelocity = (transform.position - lastHandPosition) / Time.deltaTime;
        //lastHandPosition = transform.position;

        ////check for item entering hand
        //Collider[] hitColliders = Physics.OverlapSphere(sphereCenter, sphereRadius);
        //currentlyTouching.Clear(); // Clear the list before updating it
        //foreach (var hitCollider in hitColliders)
        //{
        //    if (hitCollider.GetComponent<Item>() != null|| hitCollider.GetComponent<Grabables>() != null)
        //    {
        //        currentlyTouching.Add(hitCollider.gameObject);
        //    }
        //}

        //// Check for grab or release
        ////makes sure it can only grab 1 object at a time
        //if (gripValue > 0.5f && currentHandAction == HandAction.Releasing) // Adjust grip threshold as needed
        //{
        //    currentHandAction = HandAction.Grabbing;
        //    FindNearestObject();
        //}
        //else if (gripValue <= 0.5f && currentHandAction == HandAction.Grabbing) // Adjust release threshold as needed
        //{
        //    currentHandAction = HandAction.Releasing;
        //    Release();
        //}


    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // Set gizmo color
   
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
    public Vector3 GetHandVelocity()
    {
        return handVelocity;
    }
    void FindNearestObject()
    {
        float closestDistance = float.MaxValue;
        GameObject closestObject = null;

        // Find the closest object to grab
        foreach (GameObject go in currentlyTouching)
        {
            if (go.name == "VR Player") continue;
            float distance = Vector3.Distance(go.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = go;
            }
        }
        if (closestObject != null)
        {
            grabbedObject = closestObject;
            OnGrabbed?.Invoke(grabbedObject,handType);
        }
    }

    void Release()
    {
        if (grabbedObject != null)
        {
            OnRelease?.Invoke(handVelocity,handType);
        }
    }

    public void SubcribeEvents(IVRInteracted action)
    {
        OnGrabbed += action.OnInteracted;
    }

    public void UnsubcribeEvents(IVRInteracted action)
    {
        OnGrabbed -= action.OnInteracted;
    }
    public void SubcribeEvents(IVRRelease action)
    {
        OnRelease += action.OnRelease;
    }

    public void UnsubcribeEvents(IVRRelease action)
    {
        OnRelease -= action.OnRelease;
    }
}

public interface IVRInteracted
{
    public void OnInteracted(GameObject obj, HandType handType);
}
public interface IVRRelease
{
    public void OnRelease(Vector3 handVelocity, HandType handType);
}

public interface IinteractableExtensionRetrieveObj
{
    public void OnEnter(GameObject interactable);
    public void OnExit(GameObject interactable);
}


