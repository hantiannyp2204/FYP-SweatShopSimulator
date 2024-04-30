using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SmelterSlidingDoor : MonoBehaviour
{
    [SerializeField] private float minYValue;
    [SerializeField] private XRGrabInteractable grabInteractable;
    [SerializeField] private Collider coalHitbox;
    [SerializeField] private FeedbackEventData e_doorOpen;
    [SerializeField] private FeedbackEventData e_doorClose;

    private bool doorLocked = false;
    private bool grabbed = false;
    private Vector3 startingPosition;
    private Rigidbody doorRb;
    private void Start()
    {
        startingPosition = transform.position;
        doorRb = GetComponent<Rigidbody>();

        grabInteractable.selectEntered.AddListener(OnDoorGrabbed);
        grabInteractable.selectExited.AddListener(OnDoorUnGrabbed);
    }
    void Update()
    {
        // Lock the door if unlocked and its Y rotation goes below 90 degrees
        if (transform.position.y <= startingPosition.y && !doorLocked && !grabbed)
        {
            OnDoorLocked();

        }
        if (doorLocked && !doorRb.isKinematic)
        {
            coalHitbox.enabled = false;
            doorRb.isKinematic = true;
        }
        else if (!doorLocked && doorRb.isKinematic)
        {
            coalHitbox.enabled = true;
            doorRb.isKinematic = false;
        }
    }
    public virtual void OnDoorLocked()
    {
        doorLocked = true;
        e_doorClose?.InvokeEvent(transform.position, Quaternion.identity, transform);
        transform.position = startingPosition;
    }
    public virtual void OnDoorUnlocked()
    {
        e_doorOpen?.InvokeEvent(transform.position, Quaternion.identity, transform);
    }
    private void OnDoorUnGrabbed(SelectExitEventArgs args)
    {
        grabbed = false;
    }
    private void OnDoorGrabbed(SelectEnterEventArgs args)
    {
        if (doorLocked)
        {
            OnDoorUnlocked();
        }
        grabbed = true;

        doorLocked = false;

    }

}
