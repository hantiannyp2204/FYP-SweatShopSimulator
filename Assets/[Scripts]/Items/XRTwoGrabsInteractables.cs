using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRTwoGrabsInteractables : XRGrabInteractable
{
    public Transform leftAttachedTransform;
    public Transform rightAttachedTransform;

    public override Transform GetAttachTransform(IXRInteractor interactor)
    {

        Transform i_attachTransform = null;

        if (interactor.transform.gameObject.name.Contains("Left"))
        {
            i_attachTransform = leftAttachedTransform;
        }
        if (interactor.transform.gameObject.name.Contains("Right"))
        {
            i_attachTransform = rightAttachedTransform;
        }
        return i_attachTransform != null ? i_attachTransform : base.GetAttachTransform(interactor);
    }
}
