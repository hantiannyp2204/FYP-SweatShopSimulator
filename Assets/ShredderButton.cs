using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderButton : VRButton  
{
    [SerializeField] private MachineShredder shredder;

    public override void PressedFunction()
    {
        shredder.RunActive();
    }

    public override void ReleasedFunction()
    {
        shredder.RunDeactive();
    }
}
