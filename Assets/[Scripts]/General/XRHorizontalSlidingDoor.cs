using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class XRHorizontalSlidingDoor : MonoBehaviour
{
    [SerializeField] float distance = 1;
    [SerializeField] Rigidbody doorRb;
    [SerializeField] XRGrabInteractable doorHandleGrabInteractable;

    Vector3 startingPosition;
    bool grabbed = false;
    public bool doorLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;

        // Subscribe to the selectEntered event to detect when the door is grabbed
        if (doorHandleGrabInteractable != null)
        {

            doorHandleGrabInteractable.selectEntered.AddListener(OnDoorGrabbed);
            doorHandleGrabInteractable.selectExited.AddListener(OnDoorUnGrabbed);
        }

    }
    void OnDestroy()
    {
        // It's important to unsubscribe when the object is destroyed
        if (doorHandleGrabInteractable != null)
        {
            doorHandleGrabInteractable.selectEntered.RemoveListener(OnDoorGrabbed);
            doorHandleGrabInteractable.selectExited.RemoveListener(OnDoorUnGrabbed);
        }
    }

    void Update()
    {
        float yPosition = transform.position.y;

        //if (yPosition <= startingPosition.y && !doorLocked && !grabbed)
        //{
        //    doorLocked = true;

        //    transform.position = startingPosition;
        //}
        //door lock rb
        if (doorLocked && !doorRb.isKinematic)
        {
            doorRb.isKinematic = true;
        }
        else if (!doorLocked && doorRb.isKinematic)
        {
            doorRb.isKinematic = false;
        }

    }
    private void OnDoorGrabbed(SelectEnterEventArgs args)
    {

        grabbed = true;

        doorLocked = false;

    }

    private void OnDoorUnGrabbed(SelectExitEventArgs args)
    {
        grabbed = false;
    }

}
