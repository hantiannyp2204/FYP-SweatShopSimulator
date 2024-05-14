using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestOrderButton : VRButton
{
    [SerializeField] CustomerTable customerTable;
    public override void PressedFunction()
    {
        customerTable.ToggleOrder(true);
        Destroy(gameObject);
    }
}
