using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricatorConfirmedProduct : GenericQuest
{
    [SerializeField] private FabricatorCrafting crafting;

    // Update is called once per frame
    void Update()
    {
        if (crafting.HasChosenCraftingItem)
        {
            Destroy(gameObject);
        }
    }
}
