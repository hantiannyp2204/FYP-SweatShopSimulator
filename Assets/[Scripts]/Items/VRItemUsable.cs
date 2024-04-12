using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VRItemUsable : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("FEEDBACK")]
    [SerializeField] private FeedbackEventData e_use;
    [SerializeField] private FeedbackEventData e_useRelease;
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(x=>Use());
        grabInteractable.deactivated.AddListener(x=> ReleaseUse());
    }

    void Use()
    {
        e_use?.InvokeEvent(transform.position, Quaternion.identity, transform);
        UseFunction();
    }
    void ReleaseUse()
    {
        e_useRelease?.InvokeEvent(transform.position, Quaternion.identity, transform);
        UseReleaseFunction();

    }
    public virtual void UseFunction()
    {
        Debug.Log("Use");
    }
    public virtual void UseReleaseFunction()
    {
        Debug.Log("Released Use");
    }
}
