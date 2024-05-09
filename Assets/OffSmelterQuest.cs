using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffSmelterQuest : GenericQuest
{
    [SerializeField] private MachineSmelter smelter;

    // Update is called once per frame
    void Update()
    {
        if (smelter.GetDeactivatedButtonStatus())
        {
            Destroy(gameObject);
        }
    }
}
