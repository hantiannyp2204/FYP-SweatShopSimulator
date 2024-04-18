using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : VRItemUsable
{
    [SerializeField] ParticleSystem fireExtenguisherSmoke;
    public override void UseFunction()
    {
        fireExtenguisherSmoke.Play();
    }
    public override void UseReleaseFunction()
    {
        fireExtenguisherSmoke.Stop();
    }
}
