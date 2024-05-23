using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardSwitcher : MonoBehaviour
{
    public List<Material> materials; // List of materials
    public GameObject objectWithMaterial; // GameObject with the material you want to switch

    //Feedback
    [Header("FEEDBACK")]
    [SerializeField] private FeedbackEventData e_turnpage;
    private int currentIndex = 0; // Index of the current material in the list

    public void SwitchNextMaterial()
    {
        if (materials.Count == 0) // Check if the list is empty
        {
            Debug.LogWarning("No pages");
            return;
        }
        ApplyMaterial();
        
    }
    public void SwitchToPreviousMaterial()
    {
        if (materials.Count == 0) // Check if the list is empty
        {
            Debug.LogWarning("No materials available.");
            return;
        }

        currentIndex = (currentIndex - 1 + materials.Count) % materials.Count; // Decrement the index, looping back to the last material if it goes below 0
        ApplyMaterial();
    }
    private void ApplyMaterial()
    {
        Renderer renderer = objectWithMaterial.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = materials[currentIndex]; // Assign the material at the current index
            Debug.Log("Material changed to: " + materials[currentIndex].name);
            e_turnpage?.InvokeEvent(transform.position, Quaternion.identity, transform);
        }
        else
        {
            Debug.LogWarning("Renderer not found on the target object.");
        }
    }
}
