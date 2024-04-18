using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilButton : VRButton
{
    [SerializeField] private MachineAnvil anvil;
    public override void PressedFunction()
    {
        //if list is empty, there is nothing in collider
        if (anvil.anvilItemCollider.GetProductList() == null)
        {
            Debug.Log("no item to flatten");
            return;
        }
        else
        {
            anvil.RunMachine();
        }
    }
    public override void ReleasedFunction()
    {
        Debug.Log("released");
        anvil.RunDeactive();
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