using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SlidingDoors : MonoBehaviour
{
    [SerializeField] private float minSlideValue;
    [SerializeField] private XRGrabInteractable grabInteractable;
    [SerializeField] private FeedbackEventData e_doorOpen;
    [SerializeField] private FeedbackEventData e_doorClose;
    [SerializeField] private SlideDirection slideDirection;

    private enum SlideDirection
    {
        X,
        Y,
        Z
    }

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
        float currentSlideValue = GetCurrentSlideValue();
        if (currentSlideValue <= minSlideValue && !doorLocked && !grabbed)
        {
            OnDoorLocked();
        }

        doorRb.isKinematic = doorLocked;
    }

    private float GetCurrentSlideValue()
    {
        switch (slideDirection)
        {
            case SlideDirection.X:
                return transform.position.x - startingPosition.x;
            case SlideDirection.Y:
                return transform.position.y - startingPosition.y;
            case SlideDirection.Z:
                return transform.position.z - startingPosition.z;
            default:
                return 0f;
        }
    }

    public virtual void OnDoorLocked()
    {
        doorLocked = true;
        e_doorClose?.InvokeEvent(transform.position, Quaternion.identity, transform);
        ResetToStartingPosition();
    }

    private void ResetToStartingPosition()
    {
        switch (slideDirection)
        {
            case SlideDirection.X:
                transform.position = new Vector3(startingPosition.x, transform.position.y, transform.position.z);
                break;
            case SlideDirection.Y:
                transform.position = new Vector3(transform.position.x, startingPosition.y, transform.position.z);
                break;
            case SlideDirection.Z:
                transform.position = new Vector3(transform.position.x, transform.position.y, startingPosition.z);
                break;
        }
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
