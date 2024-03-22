using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Oculus.Interaction;

public class VRHandManager : MonoBehaviour, ISubscribeEvents<Iinteracted>
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private TMP_Text DebugText; // Make sure this is properly referenced in the Inspector
    private List<GameObject> currentlyTouching = new();
    private GameObject grabbedObject = null;
    private Vector3 lastHandPosition;
    private Vector3 handVelocity;

    System.Action<GameObject> OnInteracted;
    [SerializeField] private FeedbackEventData e_interactError;
    // Start is called before the first frame update
    void Start()
    {
        lastHandPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
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
        if (gripValue > 0.5f && grabbedObject == null) // Adjust grip threshold as needed
        {
            Grab();
        }
        else if (gripValue <= 0.5f && grabbedObject != null) // Adjust release threshold as needed
        {
            Release();
        }
    }

    void Grab()
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
            grabbedObject.transform.SetParent(transform, true);
            grabbedObject.transform.localPosition = Vector3.zero;
            grabbedObject.transform.localRotation = Quaternion.identity;
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }

    void Release()
    {
        if (grabbedObject != null)
        {
            grabbedObject.transform.SetParent(null);
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(handVelocity * rb.mass, ForceMode.Impulse); // Apply force based on hand velocity and object mass
            }
            grabbedObject = null;
        }
    }
    void Interact()
    {

        if (currentInteractable == null || currentInteractable != null && !currentInteractable.CanInteract())
        {
            //play interact error sound
            e_interactError?.InvokeEvent(transform.position, Quaternion.identity, transform);
            return;
        }

        Debug.Log("Press E to " + currentInteractable.GetInteractName());
        interacted = true;
        currentInteractable.Interact(targetPlayer);
        OnInteracted?.Invoke(interactObj);
        currentInteractable = null;
        interactObj = null;
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

    public void SubcribeEvents(Iinteracted action) => OnInteracted += action.OnInteracted;
    public void UnsubcribeEvents(Iinteracted action) => OnInteracted -= action.OnInteracted;
}
