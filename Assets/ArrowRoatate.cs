using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Content.Interaction;

public class ArrowRoatate : MonoBehaviour
{
    public FabricatorXRKnob xRKnob;

    public void RoatateArrow()
    {
        transform.rotation = Quaternion.Euler(-xRKnob.value/2,180, 0);
    }

    private void Update()
    {
        RoatateArrow();
    }


}
