using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class ShredderButton : VRButton  
{
    [SerializeField] private float productReleaseForce;
    [SerializeField] private VrMachineItemCollider machineCollider;
   
    public override void PressedFunction()
    {
         Debug.Break();
        if (machineCollider.GetProductList() == null)
        {
            return;
        }
        machineCollider.mouthHandler.DisableJaw();

        foreach (Item product in machineCollider.GetProductList())
        {
            product.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * productReleaseForce, ForceMode.Impulse);
        }

        machineCollider.mouthHandler.EnableJaw();
    }
}
