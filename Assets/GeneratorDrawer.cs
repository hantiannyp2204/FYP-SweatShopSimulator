using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorDrawer : SlidingDoors
{
    [SerializeField] Generators generator;
    [SerializeField] Rigidbody handleRb;
    public override void OnDoorLocked()
    {
        base.OnDoorLocked();
        handleRb.isKinematic = true;
        generator.enabled= false;
    }
    public override void OnDoorUnlocked()
    {
        base.OnDoorUnlocked();
        handleRb.isKinematic = false;
        generator.enabled= true;
    }
}
