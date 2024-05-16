using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorDrawer : SlidingDoors
{
    [SerializeField] Generators generator;
    [SerializeField] Rigidbody handleRb;
    public override void Onlocked()
    {
        handleRb.constraints |= RigidbodyConstraints.FreezePositionX;
        generator.enabled = false;

    }
    public override void OnUnlocked()
    {
        handleRb.constraints &= ~RigidbodyConstraints.FreezePositionX;
        generator.enabled = true;
    }
}
