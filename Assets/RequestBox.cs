using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestBox : MonoBehaviour,Iinteractable
{
    bool boxOpened = false;
    ItemData requestedItem;

    public delegate void OrderInteractionHandler();
    public static event OrderInteractionHandler OnOrderProcessed;
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
            player.playerObjective.UpdateObjetcive(requestedItem.GetName());
            boxOpened = true;
        }
        else
        {
            //check player's current equip
            Item playerCurrentEquip = player.playerInventory.GetCurrentItem();
            //see if its blank or not correct item
            if (playerCurrentEquip == null || playerCurrentEquip.Data != requestedItem)
            {
                return;
            }
            //if correct, remove it and set into the box as parent
            player.playerInventory.RemoveAtCurrentSlot(true,false);

            // Change parent
            playerCurrentEquip.transform.SetParent(this.transform);
            playerCurrentEquip.transform.position = this.transform.position;
            //and send object over
            OnOrderProcessed?.Invoke();

            //reset objective
            player.playerObjective.ResetObjective();

            boxOpened = false;

        }

    }
    public void SetRequestedItem(ItemData newRequestedItem) => requestedItem = newRequestedItem;
}
