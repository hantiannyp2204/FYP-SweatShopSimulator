using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GeneratorGeneratedItem : MonoBehaviour
{
    XRBaseInteractor interactedHandInteractor;
    VRHandManager interactedHandManager;
    Rigidbody rb;
    public void SetHandInteractorAndAnimator(XRBaseInteractor interactedHand)
    {
        interactedHandInteractor = interactedHand;
        interactedHandManager = interactedHand.GetComponentInParent<VRHandManager>();
    }
    // Update is called once per frame
    void Update()
    {
        //stop maual interaction the moment value goes below 0.4
        if(interactedHandManager.GetGripValue() <= 0.4f)
        {
            interactedHandInteractor.EndManualInteraction();
            VRHandRenderers interactorHandModel = interactedHandInteractor.GetComponent<VRHandRenderers>();
            if (interactorHandModel != null &&!interactorHandModel.GetActive())
            {
                interactedHandInteractor.GetComponent<VRHandRenderers>().EnableHandRender();
            }
         
            Destroy(this);
        }
    }
}
