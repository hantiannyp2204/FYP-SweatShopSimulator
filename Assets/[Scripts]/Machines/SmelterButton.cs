using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmelterButton:VRButton
{
    [SerializeField] MachineSmelter smelter;
    public override void PressedFunction()
    {
        smelter.ToggleMachine();
    }
}
