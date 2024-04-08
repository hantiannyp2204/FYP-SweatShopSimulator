using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : VRButton
{
    [SerializeField] MacineFab macineFab;
    public override void PressedFunction()
    {
        macineFab.StartButton();
    }
    public override void ReleasedFunction()
    {
        macineFab.EndButton();
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
