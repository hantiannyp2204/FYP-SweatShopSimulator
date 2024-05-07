using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmelterCoalDoor : SlidingDoors
{
    [SerializeField] private Collider coalHitbox;
    // Start is called before the first frame update
    public override void OnDoorLocked()
    {
        base.OnDoorLocked();
        coalHitbox.enabled = false;
    }
    public override void OnDoorUnlocked()
    {
        base.OnDoorUnlocked();
        coalHitbox.enabled = true;
    }
}
