using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDoor : MonoBehaviour
{
    private Rigidbody doorRb;
    [SerializeField] private GameObject mainDoor;
    public bool doorLocked = false;
    private XRGrabInteractable grabInteractable;
    bool grabbed = false;

    bool abilityToGrab = true;

    Vector3 startingPosition;
    Quaternion startingRotation;
    void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        doorRb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Subscribe to the selectEntered event to detect when the door is grabbed
        grabInteractable.selectEntered.AddListener(OnDoorGrabbed);
        grabInteractable.selectExited.AddListener(OnDoorUnGrabbed);
    }

    public void SetAbilityToGrab(bool ability)
    {
        abilityToGrab = ability;
        if (abilityToGrab)
        {
            grabInteractable.enabled = true;
        }
        else
        {
            grabInteractable.enabled = false;
        }
    }
    void OnDestroy()
    {
        // It's important to unsubscribe when the object is destroyed
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnDoorGrabbed);
            grabInteractable.selectExited.RemoveListener(OnDoorUnGrabbed);
        }
    }

    // Update is called once per frame
    void Update()
    {
     
        // Use Euler angles to correctly check the rotation
        float yRotation = mainDoor.transform.localEulerAngles.y;
        // Lock the door if unlocked and its Y rotation goes below 90 degrees
        if (yRotation <= 90 && !doorLocked && !grabbed)
        {
            doorLocked = true;

            mainDoor.transform.rotation = startingRotation;
            mainDoor.transform.position = startingPosition;
        }
        //door lock rb
        if(doorLocked && !doorRb.isKinematic)
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

    public bool IsDoorLocked() => doorLocked;
}