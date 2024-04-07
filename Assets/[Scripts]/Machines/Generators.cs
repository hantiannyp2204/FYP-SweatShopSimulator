using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generators : MonoBehaviour,Iinteractable
{
    [SerializeField] GameObject ItemToGenerate;

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
        return $"obtain {ItemToGenerate.name}";
    }

    public void Interact(KeyboardGameManager player)
    {
        //return if prefab don't have item script
        if (ItemToGenerate.GetComponent<Item>() == null)
        {
            Debug.Log("Allocated prefab is NOT an Item");
            return;
        }

        //spawn the object
        Item generatedItem = Instantiate(ItemToGenerate.GetComponent<Item>());
        //add into player
        player.playerInventory.AddItem(generatedItem);
    }
}
