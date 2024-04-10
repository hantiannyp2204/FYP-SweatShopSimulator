using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollider : MonoBehaviour
{
   [SerializeField]List<Collider> colliderList;
    private void Start()
    {
        foreach(Collider colliderA in colliderList) {

            foreach (Collider colliderB in colliderList)
            {
                if (colliderA == colliderB) continue;
                Physics.IgnoreCollision(colliderA, colliderB,true);

            }
        }
    }
}
