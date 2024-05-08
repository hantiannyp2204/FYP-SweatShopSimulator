using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorDrawer : SlidingDoors
{
    [SerializeField] Generators generator;
    public override void OnDoorLocked()
    {
        base.OnDoorLocked();
        generator.enabled= false;
    }
    public override void OnDoorUnlocked()
    {
        base.OnDoorUnlocked();
        generator.enabled= true;
    }
}
