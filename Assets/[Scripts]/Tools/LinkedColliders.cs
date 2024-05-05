using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedColliders : MonoBehaviour
{
    // NOTE: MUST REFERENCE THE TOP MOST COLLIDER FOR BEST EFFICIENCY
    [SerializeField] private List<GameObject> linkedColliderParentsList;

    private Collider[] allLinkedColliders;

    private void Start()
    {
        // Use a list to collect all the colliders
        List<Collider> collidersList = new List<Collider>();

        // Collect colliders from each GameObject in the list
        foreach (GameObject gameObject in linkedColliderParentsList)
        {
            // Add the colliders to the list
            collidersList.AddRange(gameObject.GetComponentsInChildren<Collider>());
        }

        // Convert the list to an array
        allLinkedColliders = collidersList.ToArray();
    }

    public Collider[] GetAllLinkedColliders() => allLinkedColliders;
}