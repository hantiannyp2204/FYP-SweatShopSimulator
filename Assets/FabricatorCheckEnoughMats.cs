using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricatorCheckEnoughMats : GenericQuest 
{
    [SerializeField] private FabricatorCrafting crafting;
    // Update is called once per frame
    void Update()
    {
        if (crafting.EnoughMaterials)
        {
            Destroy(gameObject);
        }
    }
}
