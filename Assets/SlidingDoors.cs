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
    [SerializeField] private SlideDirection slideDirectionType;
    [SerializeField] private SlideMagnituteType slideMagnitute;
    private enum SlideDirection
    {
        X,
        Y,
        Z
    }
    //to differentiate between Up is lock or down is lock
    //is Left lock or Right lock
    //since x can be left or right
    //y can be up or down
    //we need this to tell the door should slide whcih direciton to open
    private enum SlideMagnituteType
    {
        Negative,
        Positive
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
        if (!doorLocked && !grabbed)
        {
            switch (slideMagnitute)
            {
                case SlideMagnituteType.Positive:
                    if(currentSlideValue >= minSlideValue)
                    {
                        OnDoorLocked();
                    }
                    break;
                case SlideMagnituteType.Negative:
                    if (currentSlideValue <= minSlideValue)
                    {
                        OnDoorLocked();
                    }
              
                    break;
            }
          
        }

        if (doorLocked && !doorRb.isKinematic)
        {
            doorRb.isKinematic = true;
        }
        else if (!doorLocked && doorRb.isKinematic)
        {
            doorRb.isKinematic = false;
        }
    }

    private float GetCurrentSlideValue()
    {
        switch (slideDirectionType)
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
