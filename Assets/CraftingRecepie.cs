using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecepie : MonoBehaviour
{
    public int CraftingNumber = 0;
    public List<GameObject> spawnedObjects = new List<GameObject>();
    [Header("References")]
    public FabricatorCrafting fabricatorCrafting;

    [Header("Raw Material")]
    [SerializeField] private Item RawMetalMaterial;
    [SerializeField] private Item RawPlasticMaterial;
    [SerializeField] private Item RawWoodMaterial;

    [Header("Scraps")]
    [SerializeField] private Item MetalScap;
    [SerializeField] private Item PlasticScrap;
    [SerializeField] private Item WoodenScrap;

    [Header("Broken Sword Crafting Recipe")]
    [SerializeField] private int RequiredMetal_Broken_Sword;
    public GameObject Broken_Sword;

    [Header("Sword Crafting Recipe")]
    [SerializeField] private int RequiredMetal_Sword;
    public GameObject Sword;

    [Header("Pocket Clock Crafting Recipe")]
    [SerializeField] private int RequiredMetal_Pocket_Clock;
    public GameObject PocketClock;

    [Header("Wheel Crafting Recipe")]
    [SerializeField] private int RequiredWheel;
    public GameObject Wheel;


    public void _ConfirmSelection(int Confirmnumber)
    {
        switch (Confirmnumber)
        {
            case 0:
                for (int i = 0; i < RequiredMetal_Broken_Sword; i++)
                {
                    fabricatorCrafting._WhatINeed.Add(RawMetalMaterial);      
                }
                spawnedObjects.Add(Broken_Sword);
                //fabricatorCrafting.SpawnOBJ(Broken_Sword);
                break;
            case 1:
                for (int i = 0; i < RequiredMetal_Sword; i++)
                {
                    fabricatorCrafting._WhatINeed.Add(RawMetalMaterial);                   
                }
                spawnedObjects.Add(Sword);
                //fabricatorCrafting.SpawnOBJ(Sword);
                break;
            case 2:
                for (int i = 0; i < RequiredMetal_Pocket_Clock; i++)
                {
                    fabricatorCrafting._WhatINeed.Add(RawMetalMaterial);
                }
                spawnedObjects.Add(PocketClock);
                //fabricatorCrafting.SpawnOBJ(PocketClock);
                break;
            case 3:
                for (int i = 0; i < RequiredWheel; i++)
                {
                    fabricatorCrafting._WhatINeed.Add(RawMetalMaterial);
                    fabricatorCrafting._WhatINeed.Add(RawPlasticMaterial);
                }
                spawnedObjects.Add(Wheel);
                //fabricatorCrafting.SpawnOBJ(PocketClock);
                break;
        }


    }
}

