using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class SmelterWheel : XRKnob
{
    [Header("References")]
    [SerializeField] MachineSmelter smelter;
    [SerializeField] XRDoor smelterDoor;
    [SerializeField] XRBaseInteractable grabInteractable;

    [Header("Feedback Events")]
    [SerializeField] private FeedbackEventData e_lock;
    [SerializeField] private FeedbackEventData e_unlock;

    private bool fullyTurned = false;
    public void CheckTurnStatus()
    {
        if(!fullyTurned && value == 1)
        {
            e_lock?.InvokeEvent(transform.position, Quaternion.identity, transform);
            smelter.AbilityToStart = true;
            fullyTurned = true;
        }
        else if(fullyTurned && value != 1)
        {
            e_unlock?.InvokeEvent(transform.position, Quaternion.identity, transform);

            smelter.AbilityToStart = false;
            fullyTurned = false;
        }
        if(value != 0)
        {
            smelterDoor.SetAbilityToGrab(false);
        }
        else
        {
            smelterDoor.SetAbilityToGrab(true);
        }
    }
    void ResetWheel()
    {
        value = 0;
    }
}
