using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VRHandManager;

public class HandColliders : MonoBehaviour
{
    HandType handType = HandType.Left;
    Collider[] handColliders;

    public void Init()
    {
        // Set the hand type
        if (transform.name.Contains("Right Hand"))
        {
            handType = HandType.Right;
        }

        // Get all Collider components in this GameObject and its children
        Collider[] allColliders = GetComponentsInChildren<Collider>();
        List<Collider> filteredColliders = new List<Collider>();

        // Filter out colliders with names containing "Hand Interactor"
        foreach (Collider collider in allColliders)
        {
            if(collider.isTrigger)
            {
                continue;
            }
            if (!collider.gameObject.name.Contains("Hand Interactor"))
            {
                filteredColliders.Add(collider);
            }
        }
        // Convert the filtered list back to an array
        handColliders = filteredColliders.ToArray();
    }

    [SerializeField] private float collisionRecoverDelay = 1.5f;
    public float GetCollisionRecoverDelay() => collisionRecoverDelay;
    public Collider[] GetHandColliders() => handColliders;
}
