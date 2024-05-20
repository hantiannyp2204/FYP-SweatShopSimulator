using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyItemDissolve : DissolveMaterialManager
{
    [SerializeField] XRBaseInteractable interactableScript;

    public override void StartOfDissolve()
    {
        interactableScript.enabled = false;
        base.StartOfDissolve();
    }

    public override void EndOfDissolve()
    {
        interactableScript.enabled = true;
        base.EndOfDissolve();
        Destroy(this);
    }
}
