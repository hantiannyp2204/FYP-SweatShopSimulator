using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricatorCrafting : MonoBehaviour
{
    public bool EnoughMaterials = false;
    public bool HasChosenCraftingItem = false;
    public FabricatorInputHitbox inputHitbox;
    public Item item;
    public Item Plasticitem;
    public List<Item> _WhatINeed = new List<Item>();
    List<Item> _ToDestroy = new List<Item>();
    public int foundCount = 0; // Counter to track the number of found items


    private void Update()
    {
       Debugging();
    }
    public void CheckIfPresent()
    {


        foreach (Item neededItem in _WhatINeed)
        {
            foreach (Item availableItem in inputHitbox.GetScrapList())
            {

                if (neededItem == availableItem)
                {
                    foundCount++; // Increment the counter for each found item
                    break; // Exit the inner loop since the item is found
                }
            }
        }

        if (foundCount == _WhatINeed.Count)
        {
            EnoughMaterials = true;
        }
        else
        {
            //foundCount = 0;
            EnoughMaterials = false;
            Debug.Log("Not enough");

        }
    }

   

    public void ClearLists()
    {
        _WhatINeed.Clear();
        inputHitbox.GetScrapList().Clear();
        foundCount = 0;
    }

    public void LogMissingItems(List<Item> missingItems)
    {
        Debug.Log("Missing items:");
        foreach (Item missingItem in missingItems)
        {
            Debug.Log(missingItem.name);
        }
    }


    public void SpawnNDestroyItem()
    {
        Debug.Log("Itemn spawned");
    }
    public void Debugging()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _WhatINeed.Add(item);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            HasChosenCraftingItem = true;
            for (int i = 0; i < 3; i++) // Change 3 to the number of items you want to add
            {
                _WhatINeed.Add(item); // Assuming you want to add the same item multiple times
                _WhatINeed.Add(Plasticitem);
            }

            foreach (Item Item in _WhatINeed)
            {
                Debug.Log(Item.name);
            }
        }


        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Number of items in _WhatINeed: " + _WhatINeed.Count);
            Debug.Log("Number of items found: " + foundCount);
            foreach (Item Item in _WhatINeed)
            {
                Debug.Log(Item.name);
            }
        }

    }

}


