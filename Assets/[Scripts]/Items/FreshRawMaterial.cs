using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreshRawMaterial : MonoBehaviour
{
    [SerializeField] Material hotMaterial;

    private void Start()
    {
        // Retrieve all MeshRenderer components on the GameObject and its children
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in meshRenderers)
        {
            // Get the current materials array of the renderer
            Material[] currentMaterials = renderer.materials;

            // Create a new array with one extra slot for the hotMaterial
            Material[] newMaterials = new Material[currentMaterials.Length + 1];

            // Copy the existing materials to the new array
            for (int i = 0; i < currentMaterials.Length; i++)
            {
                newMaterials[i] = currentMaterials[i];
            }

            // Add the hotMaterial to the last slot of the new array
            newMaterials[newMaterials.Length - 1] = hotMaterial;

            // Assign the new materials array back to the renderer
            renderer.materials = newMaterials;
        }
    }
}
