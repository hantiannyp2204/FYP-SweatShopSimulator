using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton:VRButton
{
    [SerializeField] MachineSmelter smelter;
    public override void PressedFunction()
    {
        smelter.RunMachine();
    }
    public override void ReleasedFunction()
    {
        smelter.RunDective();
    }
    public override void ToggleOnFunction()
    {
        Debug.Log("Toggle On");
    }
    public override void ToggleOffFunction()
    {
        Debug.Log("Toggle Off");
    }
}
