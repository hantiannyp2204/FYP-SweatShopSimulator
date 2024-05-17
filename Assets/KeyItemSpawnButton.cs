using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemSpawnButton : VRButton
{
    public delegate void ButtonPressedHandler();
    public event ButtonPressedHandler OnButtonPressed;

    public override void PressedFunction()
    {
        base.PressedFunction();
        OnButtonPressed?.Invoke();
    }

    public override void ReleasedFunction()
    {
        base.ReleasedFunction();
    }
}
