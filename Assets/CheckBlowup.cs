using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBlowup : GenericQuest
{
    //[SerializeField] private SmelterCoalManager coalManager;
    //[SerializeField] private float numberOfCoalToNext;
    [SerializeField] private MachineSmelter smelter;

    // Update is called once per frame
    void Update()
    {
        //if (coalManager.GetAddedCoalCheck())
        //{
        //    Destroy(gameObject);
        //}
        //if (coalManager.GetCoalInputCounter() >= numberOfCoalToNext)
        //{
        //    Destroy(gameObject);
        //}
        if (smelter.HasBlownUp())
        {
            Destroy(gameObject);
        }
    }
}
