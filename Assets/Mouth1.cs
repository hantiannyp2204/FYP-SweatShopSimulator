using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth1 : RedirectorBase
{
    public void CheckIfDetection()
    {
        if (detection.GetHasEntered())
        {
            return;    // still in collision
        }
        else
        {
            // play anim event
            Debug.Log("yes");
        }
    }
}
