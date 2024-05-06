using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLogic : MonoBehaviour
{
    [SerializeField] private float timeToCool = 5;
    [SerializeField] private FeedbackEventData e_materialSizzleSound;
    private void OnTriggerEnter(Collider other)
    {
        FreshRawMaterial freshRawMaterialSript = other.GetComponent<FreshRawMaterial>();
        if(freshRawMaterialSript != null)
        {
            e_materialSizzleSound?.InvokeEvent(transform.position,Quaternion.identity);
            freshRawMaterialSript.CoolMaterial(timeToCool);
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
