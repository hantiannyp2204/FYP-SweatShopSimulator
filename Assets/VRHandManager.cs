using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Platform;
using Oculus.Interaction.HandGrab;
using static VRHandManager;

public class VRHandManager : MonoBehaviour, ISubscribeEvents<Iinteracted>, ISubscribeEvents<IRelease>
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private TMP_Text DebugText; // Make sure this is properly referenced in the Inspector
    private List<GameObject> currentlyTouching = new();
    private GameObject grabbedObject = null;
    private Vector3 lastHandPosition;
    private Vector3 handVelocity;
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

    public HandType GetHandType()=> handType;

    // Start is called before the first frame update
    public void Init()
    {
        currentHandAction = HandAction.Releasing;
        lastHandPosition = transform.position;
        //set the hand type
        if(transform.name == "Right hand")
        {
            handType = HandType.Right;
        }
    }

    // Update is called once per frame
    public void UpdateInteractions()
    {
        // Update hand animations
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        float gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);
        handAnimator.SetFloat("Grip", gripValue);

        // Calculate hand velocity
        handVelocity = (transform.position - lastHandPosition) / Time.deltaTime;
        lastHandPosition = transform.position;

        // Check for grab or release
        //makes sure it can only grab 1 object at a time
        if (gripValue > 0.5f && currentHandAction == HandAction.Releasing) // Adjust grip threshold as needed
        {
            currentHandAction = HandAction.Grabbing;
            FindNearestObject();
        }
        else if (gripValue <= 0.5f && currentHandAction == HandAction.Grabbing) // Adjust release threshold as needed
        {
            currentHandAction = HandAction.Releasing;
            Release();
        }
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
    private void OnTriggerEnter(Collider other)
    {
        //only accept grabbing item typed items
        if (DebugText == null || other.GetComponent<Item>() == null) return;
        currentlyTouching.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (DebugText == null) return;
        currentlyTouching.Remove(other.gameObject);
    }

    public void SubcribeEvents(Iinteracted action)
    {
        OnGrabbed += action.OnInteracted;
    }

    public void UnsubcribeEvents(Iinteracted action)
    {
        OnGrabbed -= action.OnInteracted;
    }
    public void SubcribeEvents(IRelease action)
    {
        OnRelease += action.OnRelease;
    }

    public void UnsubcribeEvents(IRelease action)
    {
        OnRelease -= action.OnRelease;
    }
}
public interface Iinteracted
{
    public void OnInteracted(GameObject obj, HandType handType);
}
public interface IRelease
{
    public void OnRelease(Vector3 handVelocity, HandType handType);
}

public interface IinteractableExtensionRetrieve
{
    public void OnEnter(Iinteractable interactable);
    public void OnExit(Iinteractable interactable);
}
public interface IinteractableExtensionRetrieveObj
{
    public void OnEnter(GameObject interactable);
    public void OnExit(GameObject interactable);
}
public interface IinteractableInteracting
{
    public void OnInteracting(Iinteractable interactable);
    public void OnStopInteracting(Iinteractable interactable);
}

