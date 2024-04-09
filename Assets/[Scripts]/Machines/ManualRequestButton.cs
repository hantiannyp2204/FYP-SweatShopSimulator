using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualRequestButton : MonoBehaviour
{
    [SerializeField] CustomerTable customerTable;
    public bool CanInteract()
    {
        return true;
    }

    public float GetInteractingLast()
    {
        throw new System.NotImplementedException();
    }

    public string GetInteractName()
    {
        return "to request order";
    }

    //switch to a button
    //public void Interact(KeyboardGameManager player)
    //{
    //    customerTable.RequestOrder();
    //}

}
