using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : VRButton
{
    [SerializeField] MacineFab macineFab;
    public override void PressedFunction()
    {
        macineFab.PressedButtonToggle();
    }
    public override void ReleasedFunction()
    {
        return;
    }
    public override void ToggleOnFunction()
    {
        //macineFab.StartButtonToggle();
        Debug.Log("Toggle On");
    }
    public override void ToggleOffFunction()
    {
        macineFab.StartButtonToggleOFF();
        Debug.Log("Toggle Off");
    }
}
