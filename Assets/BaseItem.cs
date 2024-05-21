using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    // Feedback
    [Header("FEEDBACK")]
    [SerializeField] private FeedbackEventData e_pickUp;
    [SerializeField] private FeedbackEventData e_collisionNoise;
    public void PlayEquipSound()
    {
        e_pickUp?.InvokeEvent(transform.position, Quaternion.identity, transform);
    }
    public void PlayCollisionSound()
    {
        Debug.Log("Item collided with something");
        e_collisionNoise?.InvokeEvent(transform.position, Quaternion.identity, transform);
    }
}
