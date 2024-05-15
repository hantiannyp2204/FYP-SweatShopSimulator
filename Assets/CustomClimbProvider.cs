using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomClimbProvider : ClimbProvider
{
    [SerializeField] Rigidbody playerRb;
    [SerializeField] ContinuousMovementPhysics playerMovement;
    protected override void Update()
    {
        base.Update();
        if(locomotionPhase == LocomotionPhase.Moving)
        {
            playerRb.isKinematic = true;
            playerMovement.speed = 0;
        }
        else
        {
            playerRb.isKinematic = false;
            playerMovement.speed = 1;
        }
    }
}
