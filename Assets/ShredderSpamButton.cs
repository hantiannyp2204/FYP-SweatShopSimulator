using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderSpamButton : VRButton
{
    [SerializeField] private MachineShredder shredder;

    public override void PressedFunction()
    {
        shredder.RunSpamButton();
    }
}
