using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireExtinguisher : VRItemUsable
{
    [SerializeField] ParticleSystem fireExtenguisherSmoke;
    [SerializeField] XRGrabInteractable fireExinguisherNozzleGrab;
    [Header("Feedback Events")]
    [SerializeField] private FeedbackEventData e_fireLoop;

    private void Start()
    {
        base.Start();
        DisableNozzleGrab();
    }
    public override void UseFunction()
    {
        base.UseFunction();
        e_fireLoop?.InvokeEvent(transform.position, Quaternion.identity, transform);
        fireExtenguisherSmoke.Play();

    }
    public override void UseReleaseFunction()
    {
        fireExtenguisherSmoke.Stop();
        base.UseReleaseFunction();
    }
    public void EnableNozzleGrab()
    {
        if (fireExinguisherNozzleGrab == null) return;
        fireExinguisherNozzleGrab.enabled = true;
    }
    public void DisableNozzleGrab()
    {
        if (fireExinguisherNozzleGrab == null) return;
        // Check if the nozzle is currently being held
        if (fireExinguisherNozzleGrab.isSelected)
        {
            // Get the interaction manager from the interactable
            XRInteractionManager interactionManager = fireExinguisherNozzleGrab.interactionManager;

            // If an interaction manager is found and the nozzle is currently selected by an interactor
            if (interactionManager != null && fireExinguisherNozzleGrab.selectingInteractor != null)
            {
                // Force the selecting interactor to end interaction with the nozzle
                interactionManager.CancelInteractableSelection(fireExinguisherNozzleGrab);
            }
        }

        // Finally, disable the grab interactable component
        fireExinguisherNozzleGrab.enabled = false;
    }
}
