using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAnvil : MonoBehaviour, Iinteractable
{
    public bool CanInteract()
    {
        return true;
    }

    public float GetInteractingLast()
    {
        throw new System.NotImplementedException();
    }

    public string GetInteractName()=> "Use "+ name;


    public void Interact(GameManager player)
    {
        Item currentItem = player.playerInventory.GetCurrentItem();
        if (currentItem.name != "Hammer")
        {
            Debug.Log("pick up Hammer");
            return;
        }
        else 
        {

            Debug.Log("Interacting " + name + " with " + player.playerInventory.GetCurrentItem().Data.name);
            
        }
      
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
