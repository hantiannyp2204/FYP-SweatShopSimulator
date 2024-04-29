using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthEventHandler : MonoBehaviour
{
    [Header("FEEDBACK")]
    [SerializeField] private FeedbackEventData e_openmouth;
    [SerializeField] private FeedbackEventData e_closemouth;

    public void OnCallbackOpenMouth()
    {
        e_openmouth?.InvokeEvent(transform.position, Quaternion.identity, transform);
    }

    public void OnCallbackCloseMouth()
    {
        e_closemouth?.InvokeEvent(transform.position, Quaternion.identity, transform);
    }
}
