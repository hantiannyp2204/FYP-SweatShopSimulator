using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLogic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FreshRawMaterial freshRawMaterialSript = other.GetComponent<FreshRawMaterial>();
        if(freshRawMaterialSript != null)
        {
            freshRawMaterialSript.CoolMaterial();
        }    
    }
    private void OnTriggerExit(Collider other)
    {
        FreshRawMaterial freshRawMaterialSript = other.GetComponent<FreshRawMaterial>();
        if (freshRawMaterialSript != null)
        {
            freshRawMaterialSript.CoolMaterialPause();
        }
    }
}
