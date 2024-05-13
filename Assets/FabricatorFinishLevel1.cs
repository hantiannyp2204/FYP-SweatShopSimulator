using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricatorFinishLevel1 : GenericQuest
{
    [SerializeField] private NewController controller;
    // Update is called once per frame
    void Update()
    {
        if (controller.finishedLevel)
        {
            Destroy(gameObject);
            controller.finishedLevel = false;
        }
    }
}
