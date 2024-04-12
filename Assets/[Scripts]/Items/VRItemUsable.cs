using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VRItemUsable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(x=>Use());
        grabInteractable.deactivated.AddListener(x=>Use());
    }

    public virtual void Use()
    {
        Debug.Log("Use");
    }
    public virtual void ReleaseUse()
    {
        Debug.Log("Released Use");
    }
}
