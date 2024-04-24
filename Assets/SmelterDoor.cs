using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SmelterDoor : XRDoor
{
    XRKnob wheel;
    // Start is called before the first frame update
    public override void OnDoorLocked()
    {
        base.OnDoorLocked();
        wheel.enabled = true;
    }
    public override void OnDoorUnlocked()
    {
        base.OnDoorUnlocked();
        wheel.enabled = false;
    }
}
