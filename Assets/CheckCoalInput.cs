using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCoalInput : GenericQuest
{
    [SerializeField] private SmelterCoalManager coalManager;
    [SerializeField] private float numberOfCoalToNext;

    // Update is called once per frame
    void Update()
    {
        //if (coalManager.GetAddedCoalCheck())
        //{
        //    Destroy(gameObject);
        //}
        if (coalManager.GetCoalInputCounter() >= numberOfCoalToNext)
        {
            Destroy(gameObject);
        }
    }
}
