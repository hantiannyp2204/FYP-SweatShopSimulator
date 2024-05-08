using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardSwitcher : MonoBehaviour
{
    public List<Material> materials; // List of materials
    public GameObject objectWithMaterial; // GameObject with the material you want to switch

    private int currentIndex = 0; // Index of the current material in the list

    public void SwitchMaterial()
    {
        if (materials.Count == 0) // Check if the list is empty
        {
            Debug.LogWarning("No pages");
            return;
        }

        Renderer renderer = objectWithMaterial.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = materials[currentIndex]; // Assign the material at the current index
            currentIndex = (currentIndex + 1) % materials.Count; // Increment the index, looping back to 0 if it exceeds the list size
        }
    }
}
