using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricatorFinishGame : GenericQuest 
{
    [SerializeField] private NewController controller;
    // Update is called once per frame
    void Update()
    {
        if (controller.canSpawn)
        {
            Destroy(gameObject);
        }
    }
}
