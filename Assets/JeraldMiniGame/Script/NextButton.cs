using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextButton : VRButton
{
    [SerializeField] NewController newController;
    public override void PressedFunction()
    {
        newController.NextButtonToggle();
    }
    public override void ReleasedFunction()
    {
        newController.NextButtonToggleOFF();
        
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
