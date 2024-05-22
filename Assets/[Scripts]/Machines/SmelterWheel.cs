using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;
using UnityEngine.Events;

public class SmelterWheel : XRKnob
{
    [Header("References")]
    [SerializeField] MachineSmelter smelter;
    [SerializeField] XRDoor smelterDoor;
    [SerializeField] XRBaseInteractable grabInteractable;

    [Header("Feedback Events")]
    [SerializeField] private FeedbackEventData e_lock;
    [SerializeField] private FeedbackEventData e_unlock;
    [SerializeField] private FeedbackEventData e_fullyUnlocked;
    [SerializeField] private FeedbackEventData e_wheelTurnSound;

    private bool fullyTurned = false;

    public bool GetTurnStatus()
    {
        return fullyTurned;
    }
    private float currentValue = 0;
    private bool wheelWasFullyLocked = false;
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

            if (Mathf.Abs(value - currentValue) >= 0.2f)
            {
                Debug.Log("PLAY");
                e_wheelTurnSound?.InvokeEvent(transform.position, Quaternion.identity, transform);
                currentValue = value;
            }
            smelterDoor.SetAbilityToGrab(false);
            wheelWasFullyLocked = false;
        }
        else
        {
            smelterDoor.SetAbilityToGrab(true);
            if(!wheelWasFullyLocked)
            {
                e_fullyUnlocked?.InvokeEvent(transform.position, Quaternion.identity, transform);
                wheelWasFullyLocked = true; 
            }
        
        }
    }
    void ResetWheel()
    {
        value = 0;
    }
}
