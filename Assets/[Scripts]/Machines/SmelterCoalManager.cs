using Oculus.Interaction;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SmelterCoalManager : MonoBehaviour
{
    public MachineSmelter smelter;
    [SerializeField] float maxPurityValue;
    [SerializeField] float minPurityValue;
    private void OnTriggerEnter(Collider other)
    {
        //only accept if its coal and is not being grabbed
        if (other.gameObject.name.Contains("Coal") && !other.GetComponent<XRBaseInteractable>().isSelected && other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log("Entered");
            // Here you can also call a method on the smelter to refill fuel
            // Assuming such a method exists
            smelter.AddFuel(Random.Range(minPurityValue, maxPurityValue));
            Destroy(other.gameObject);
        }
    }
}