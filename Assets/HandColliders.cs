using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandColliders : MonoBehaviour
{
    Collider[] handColliders;

    public void Init()
    {
        // Get all Collider components in this GameObject and its children

        handColliders = GetComponentsInChildren<Collider>();
    }

    [SerializeField] float collisionRecoverDelay = 1.5f;
    public float GetCollisionRecoverDelay() => collisionRecoverDelay;
    public Collider[] GetHandColliders() => handColliders;
}
