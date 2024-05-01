using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricatorButton : VRButton
{
    [SerializeField] MacineFab macineFab;
    public NewController _newcon;

    public override void PressedFunction()
    {
        macineFab.RunActive();
    }
    public override void ReleasedFunction()
    {
        macineFab.RunDective();
    }
    public override void ToggleOnFunction()
    {
        macineFab.ToggleOn();
        Debug.Log("Toggle On");
        if (_newcon.gameEnded && _newcon.winORloseText.text == "WIN")
        {
            _newcon.ChangeLevel();
        }
    }
    public override void ToggleOffFunction()
    {
        macineFab.ToggleOFF();
        Debug.Log("Toggle Off");
        if (_newcon.gameEnded && _newcon.winORloseText.text == "WIN")
        {
            _newcon.ChangeLevel();
        }
    }
}
