using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestBox : MonoBehaviour,Iinteractable
{
    bool boxOpened = false;
    ItemData requestedItem;
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
        return "open request";
    }

    public void Interact(GameManager player)
    {
        if(!boxOpened)
        {
            //get the request
            boxOpened = true;
        }
        else
        {

        }

    }
    public void SetRequestedItem(ItemData newRequestedItem) => requestedItem = newRequestedItem;
}
