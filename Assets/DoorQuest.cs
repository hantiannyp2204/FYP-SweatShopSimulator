using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorQuest : GenericQuest
{
    [SerializeField] private SmelterDoor checkDoor;
    // Update is called once per frame
    void Update()
    {
        if (!checkDoor.doorLocked)
        {
            Destroy(gameObject);
        }
    }
}
