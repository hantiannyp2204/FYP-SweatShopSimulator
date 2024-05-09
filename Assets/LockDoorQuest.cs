using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoorQuest : GenericQuest
{
    [SerializeField] private SmelterDoor door;
    // Update is called once per frame
    void Update()
    {
        if (door.doorLocked)
        {
            Destroy(gameObject);
        }
    }
}
