using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRRayHoverManager : MonoBehaviour
{
    [SerializeField] private XRRayInteractor rayInteractor; // Assign this in the inspector
    [SerializeField] private Material hoverMaterial;        // Assign the hover material in the inspector
    private List<Material[]> initialMaterials = new();
    void OnEnable()
    {
        // Subscribe to the hover events
        rayInteractor.hoverEntered.AddListener(HandleHoverEntered);
        rayInteractor.hoverExited.AddListener(HandleHoverExited);
        rayInteractor.selectEntered.AddListener(HandleSelectEntered);
    }

    void OnDisable()
    {
        // Always make sure to unsubscribe when the script is disabled
        rayInteractor.hoverEntered.RemoveListener(HandleHoverEntered);
        rayInteractor.hoverExited.RemoveListener(HandleHoverExited);
        rayInteractor.selectEntered.RemoveListener(HandleSelectEntered);
    }

    private void HandleHoverEntered(HoverEnterEventArgs args)
    {
        if (args.interactableObject != null)
        {
            AddHoverMaterial(args.interactableObject.transform);
        }
    }

    private void HandleHoverExited(HoverExitEventArgs args)
    {
        if (args.interactableObject != null)
        {
            RemoveHoverMaterial(args.interactableObject.transform);
        }
    }
    private void HandleSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject != null)
        {
            RemoveHoverMaterial(args.interactableObject.transform);
        }
    }

    private void AddHoverMaterial(Transform target)
    {
        // Retrieve all MeshRenderer components on the GameObject and its children
        MeshRenderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>();

        int currentIndex = 0;
        foreach (MeshRenderer renderer in meshRenderers)
        {
            // Get the current materials array of the renderer
            initialMaterials.Add(renderer.materials);

            // Create a new array with one extra slot for the hotMaterial
            Material[] newMaterials = new Material[initialMaterials[currentIndex].Length + 1];

            // Copy the existing materials to the new array
            for (int i = 0; i < initialMaterials[currentIndex].Length; i++)
            {
                newMaterials[i] = initialMaterials[currentIndex][i];
            }

            // Add the hotMaterial to the last slot of the new array
            newMaterials[newMaterials.Length - 1] = hoverMaterial;

            // Assign the new materials array back to the renderer
            renderer.materials = newMaterials;

            currentIndex++;
        }
    }

    private void RemoveHoverMaterial(Transform target)
    {
        MeshRenderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>();

        int currentIndex = 0;
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.materials = initialMaterials[currentIndex];    
            currentIndex++;
        }
        initialMaterials.Clear();
    }
}
