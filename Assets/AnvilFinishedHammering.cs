using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilFinishedHammering : GenericQuest
{
    [SerializeField] private AnvilGame2 game;
    // Update is called once per frame
    void Update()
    {
        if (game.finishedGame)
        {
            Destroy(gameObject);
        }
    }
}
