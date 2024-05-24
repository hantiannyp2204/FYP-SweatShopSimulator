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
    [SerializeField] private GeneralItem RawMetalMaterial;
    [SerializeField] private GeneralItem RawPlasticMaterial;
    [SerializeField] private GeneralItem RawWoodMaterial;

    [Header("Scraps")]
    [SerializeField] private GeneralItem MetalScap;
    [SerializeField] private GeneralItem PlasticScrap;
    [SerializeField] private GeneralItem WoodenScrap;

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

    [Header("Robot Crafting Recipe")]
    //[SerializeField] private int RequiredToyRobotMetal;
    [SerializeField] private int RequiredToyRobotPlastic;
    public GameObject ToyRobot;

    [Header("Wood Wall Clock Recipe")]
    [SerializeField] private int RequiredWallClock;
    public GameObject WallClock;

    [Header("Lamp Clock Recipe")]
    [SerializeField] private int RequiredLamp;
    public GameObject Lamp;


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
                break;
            case 1:
                for (int i = 0; i < RequiredMetal_Sword; i++)
                {
                    fabricatorCrafting._WhatINeed.Add(RawMetalMaterial);                   
                }
                spawnedObjects.Add(Sword);
                break;
            case 2:
                for (int i = 0; i < RequiredMetal_Pocket_Clock; i++)
                {
                    fabricatorCrafting._WhatINeed.Add(RawMetalMaterial);
                }
                spawnedObjects.Add(PocketClock);        
                break;
            case 3:
                for (int i = 0; i < RequiredWheel; i++)
                {
                    fabricatorCrafting._WhatINeed.Add(RawMetalMaterial);
                    fabricatorCrafting._WhatINeed.Add(RawPlasticMaterial);
                }
                spawnedObjects.Add(Wheel);
                
                break;
            case 4:
                for (int i = 0; i < RequiredToyRobotPlastic; i++)
                {
                    fabricatorCrafting._WhatINeed.Add(RawPlasticMaterial);
                }
                spawnedObjects.Add(ToyRobot);   
                break;
            case 5:
                for (int i = 0; i < RequiredWallClock; i++)
                {
                    fabricatorCrafting._WhatINeed.Add(RawWoodMaterial);
                }
                spawnedObjects.Add(WallClock);
                break;
            case 6:
                for (int i = 0; i < RequiredLamp; i++)
                {
                    fabricatorCrafting._WhatINeed.Add(RawWoodMaterial);
                }
                spawnedObjects.Add(Lamp);
                break;

        }


    }
}

