using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SmelterDoor : XRDoor
{
    [SerializeField]private SmelterWheel smelterWheel;
    [SerializeField] MachineSmelter smelter;
    // Start is called before the first frame update
    public override void OnDoorLocked()
    {
        base.OnDoorLocked();
        smelter.DisableItemGrab();
        smelterWheel.enabled = true;
    }
    public override void OnDoorUnlocked()
    {
        base.OnDoorUnlocked();
        smelter.EnableItemGrab();
        smelterWheel.enabled = false;
    }

}
