using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRRayHoverManager : MonoBehaviour
{
    [SerializeField] private XRRayInteractor rayInteractor; // Assign this in the inspector
    [SerializeField] private Material hoverMaterial;        // Assign the hover material in the inspector

    void OnEnable()
    {
        // Subscribe to the hover events
        rayInteractor.hoverEntered.AddListener(HandleHoverEntered);
        rayInteractor.hoverExited.AddListener(HandleHoverExited);
    }

    void OnDisable()
    {
        // Always make sure to unsubscribe when the script is disabled
        rayInteractor.hoverEntered.RemoveListener(HandleHoverEntered);
        rayInteractor.hoverExited.RemoveListener(HandleHoverExited);
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
    List<Material> currentMaterialList;
    private void AddHoverMaterial(Transform target)
    {
        var renderer = target.GetComponent<Renderer>();
        if (renderer != null && hoverMaterial != null)
        {
            // Add the hover material to the existing materials array
            currentMaterialList = new List<Material>(renderer.materials);
            if (!currentMaterialList.Contains(hoverMaterial))
            {
                currentMaterialList.Add(hoverMaterial);
                renderer.materials = currentMaterialList.ToArray();
            }
        }
    }

    private void RemoveHoverMaterial(Transform target)
    {
        var renderer = target.GetComponent<Renderer>();
        if (renderer != null && hoverMaterial != null)
        {
            if (currentMaterialList.Contains(hoverMaterial))
            {
                currentMaterialList.Remove(hoverMaterial);
                renderer.materials = currentMaterialList.ToArray();
            }
            currentMaterialList.Clear();
        }
   
    }
}
