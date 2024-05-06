using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollider : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjectList;

    private void Start()
    {
        // Collect all colliders from the GameObjects in the list
        List<Collider> allColliders = new List<Collider>();
        foreach (GameObject obj in gameObjectList)
        {
            allColliders.AddRange(obj.GetComponentsInChildren<Collider>());
        }

        // Ignore collision between all pairs of collected colliders
        for (int i = 0; i < allColliders.Count; i++)
        {
            for (int j = i + 1; j < allColliders.Count; j++)
            {
                Physics.IgnoreCollision(allColliders[i], allColliders[j], true);
            }
        }
    }
}