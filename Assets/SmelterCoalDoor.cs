using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmelterCoalDoor : SlidingDoors
{
    [SerializeField] private Collider coalHitbox;
    // Start is called before the first frame update
    public override void OnUnlocked()
    {   
        coalHitbox.enabled = true;
    }
    public override void Onlocked()
    {
        coalHitbox.enabled = false;
    }
}
