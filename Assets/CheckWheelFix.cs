using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWheelFix : GenericQuest
{
    [SerializeField] private BrokenWheelCollisionManager collisionManager;

    // Update is called once per frame
    void Update()
    {
        if (collisionManager.IsWheelFixed())
        {
            Destroy(gameObject);
        }
    }
}
