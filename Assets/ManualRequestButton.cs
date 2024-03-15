using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualRequestButton : MonoBehaviour,Iinteractable
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

    public void Interact(GameManager player)
    {
        customerTable.RequestOrder();
    }

}
