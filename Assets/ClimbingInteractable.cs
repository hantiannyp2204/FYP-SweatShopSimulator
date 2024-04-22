using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class ClimbingInteractable : XRBaseInteractable // contain everything in baseinteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            ClimbManager.climbingHand = args.interactorObject.transform.GetComponent<ActionBasedController>();
        }
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (ClimbManager.climbingHand && ClimbManager.climbingHand.name == args.interactorObject.transform.name)
        {
            ClimbManager.climbingHand = null;
        }

        base.OnSelectExited(args);
    }
}
