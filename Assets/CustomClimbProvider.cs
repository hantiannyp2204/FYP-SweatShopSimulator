using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomClimbProvider : ClimbProvider
{
    [SerializeField] Rigidbody playerRb;
    public override void StartClimbGrab(ClimbInteractable climbInteractable, IXRSelectInteractor interactor)
    {
        playerRb.isKinematic = true;
        base.StartClimbGrab(climbInteractable, interactor);
      
    }
    public override void FinishClimbGrab(IXRSelectInteractor interactor)
    {
        playerRb.isKinematic = false;
        base.FinishClimbGrab(interactor);

    }
}
