using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class VRHandManager : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private TMP_Text DebugText; // Make sure this is properly referenced in the Inspector
    private List<GameObject> currentlyTouching = new();
    private GameObject grabbedObject = null;
    private Vector3 lastHandPosition;
    private Vector3 handVelocity;

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
        if (gripValue > 0.5f) // Adjust grip threshold as needed
        {
            Grab();
        }
        else if (gripValue <= 0.5f) // Adjust release threshold as needed
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
            grabbedObject.transform.SetParent(transform);
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

    private void OnTriggerEnter(Collider other)
    {
        if (DebugText == null) return;
        currentlyTouching.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (DebugText == null) return;
        currentlyTouching.Remove(other.gameObject);
    }
}
