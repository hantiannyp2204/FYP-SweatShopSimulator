using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SmelterDoor : XRDoor
{
    [SerializeField]private XRKnob smelterWheel;
    // Start is called before the first frame update
    public override void OnDoorLocked()
    {
        base.OnDoorLocked();
        smelterWheel.enabled = true;
    }
    public override void OnDoorUnlocked()
    {
        base.OnDoorUnlocked();
        smelterWheel.enabled = false;
    }

}
