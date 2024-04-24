using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricatorCrafting : MonoBehaviour
{
    public bool EnoughMaterials = false;
    public bool HasChosenCraftingItem = false;
    public FabricatorInputHitbox inputHitbox;
    public Item item;
    //public Item Plasticitem;
    public List<Item> _WhatINeed = new List<Item>();
    public List<GameObject> _ToDestroy = new List<GameObject>();
    public int foundCount = 0; // Counter to track the number of found items
    public Collider _SpawnPlace;
    public GameObject item2Spawn;


    private void Update()
    {
        Debugging();
    }
    public void CheckIfPresent()
    {

        List<Item> AvailableItems = inputHitbox.GetScrapList();
        
        foreach (Item neededItem in _WhatINeed)
        {
            if (neededItem.TryGetComponent(out RawMaterial RawMat))
            {
                foreach (Item availableItem in AvailableItems)
                {
                    if (availableItem.TryGetComponent(out RawMaterial RawMat2))
                    {
                        if (RawMat.GetRawMaterialType() == RawMat2.GetRawMaterialType())
                        {
                            foundCount++; // Increment the counter for each found item
                            _ToDestroy.Add(availableItem.gameObject);
                            Debug.Log(AvailableItems.Count);
                            AvailableItems.Remove(availableItem);
                            break; // Exit the inner loop since the item is found
                        }
                    }
                   
                }
            }

            if (neededItem.TryGetComponent(out Scrap ScrapCom))
            {
                foreach (Item availableItem in AvailableItems)
                {
                    if (availableItem.TryGetComponent(out Scrap ScrapCom2))
                    {
                        if (ScrapCom.GetScrapType() == ScrapCom2.GetScrapType())
                        {
                            foundCount++; // Increment the counter for each found item
                            _ToDestroy.Add(availableItem.gameObject);
                            Debug.Log(AvailableItems.Count);
                            AvailableItems.Remove(availableItem);
                            break; // Exit the inner loop since the item is found
                        }
                    }

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

    public void DestroyOBJ()
    {
        foreach (GameObject ToDestroy in _ToDestroy)
        {
            Destroy(ToDestroy.gameObject);
        }
    }

    public void ClearLists()
    {
        _WhatINeed.Clear();
        inputHitbox.GetScrapList().Clear();
        _ToDestroy.Clear();
        foundCount = 0;
        HasChosenCraftingItem = false;
        EnoughMaterials = false;
    }

    public void SpawnOBJ(GameObject spawnItem)
    {
        Instantiate(spawnItem, _SpawnPlace.transform.position,Quaternion.identity);
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
                //_WhatINeed.Add(Plasticitem);
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


