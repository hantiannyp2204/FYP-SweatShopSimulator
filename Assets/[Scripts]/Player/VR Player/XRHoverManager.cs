using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRRayHoverManager : MonoBehaviour
{
    [SerializeField] private List<XRBaseInteractor> interactors; // Include both ray and direct interactors
    [SerializeField] private Material hoverMaterial;

    private Dictionary<Transform, HashSet<IXRInteractor>> activeInteractors = new();
    private Dictionary<Transform, string> selectedInteractableLayers = new(); // Track selected interactables and their layers

    void OnEnable()
    {
        foreach (var interactor in interactors)
        {
            interactor.hoverEntered.AddListener(HandleHoverEntered);
            interactor.hoverExited.AddListener(HandleHoverExited);
            interactor.selectEntered.AddListener(HandleSelectEntered);
            interactor.selectExited.AddListener(HandleSelectExited);
        }
    }

    void OnDisable()
    {
        foreach (var interactor in interactors)
        {
            interactor.hoverEntered.RemoveListener(HandleHoverEntered);
            interactor.hoverExited.RemoveListener(HandleHoverExited);
            interactor.selectEntered.RemoveListener(HandleSelectEntered);
            interactor.selectExited.RemoveListener(HandleSelectExited);
        }
    }

    private void HandleHoverEntered(HoverEnterEventArgs args)
    {
        if (args.interactableObject != null)
        {
            // Check if the interactable has a Generator script
            bool hasGenerator = args.interactableObject.transform.GetComponent<Generators>() != null;

            // Perform the layer check only if the interactable does not have a Generator script
            if (!hasGenerator && selectedInteractableLayers.TryGetValue(args.interactableObject.transform, out var selectedLayer) &&
                args.interactorObject.transform.gameObject.layer == LayerMask.NameToLayer(selectedLayer))
            {
                // If it has the same layer and no Generator script, return early to avoid adding hover material
                return;
            }

            // Additional check for Clipboard tag
            if (args.interactableObject.transform.CompareTag("Clipboard") && selectedInteractableLayers.ContainsKey(args.interactableObject.transform))
            {
                return;
            }

            AddHoverMaterial(args.interactableObject.transform, args.interactorObject);
        }
    }

    private void HandleHoverExited(HoverExitEventArgs args)
    {
        if (args.interactableObject != null)
        {
            RemoveHoverMaterial(args.interactableObject.transform, args.interactorObject);
        }
    }

    private void HandleSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject != null)
        {
            selectedInteractableLayers[args.interactableObject.transform] = LayerMask.LayerToName(args.interactorObject.transform.gameObject.layer);
            ClearAllInteractors(args.interactableObject.transform);

            // Additional check for Clipboard tag
            if (args.interactableObject.transform.CompareTag("Clipboard"))
            {
                RemoveHoverMaterial(args.interactableObject.transform);
            }
        }
    }

    private void HandleSelectExited(SelectExitEventArgs args)
    {
        if (args.interactableObject != null)
        {
            selectedInteractableLayers.Remove(args.interactableObject.transform);
        }
    }

    private void AddHoverMaterial(Transform target, IXRInteractor interactor)
    {
        if (!activeInteractors.ContainsKey(target))
        {
            activeInteractors[target] = new HashSet<IXRInteractor>();
            ApplyHoverMaterial(target);
        }
        activeInteractors[target].Add(interactor);
    }

    private void RemoveHoverMaterial(Transform target, IXRInteractor interactor)
    {
        if (activeInteractors.TryGetValue(target, out var interactorsSet))
        {
            interactorsSet.Remove(interactor);
            if (interactorsSet.Count == 0)
            {
                RemoveHoverMaterial(target);
                activeInteractors.Remove(target);
            }
        }
    }

    private void ClearAllInteractors(Transform target)
    {
        if (activeInteractors.ContainsKey(target))
        {
            RemoveHoverMaterial(target); // Remove hover material only once
            activeInteractors.Remove(target); // Clear all interactors for this target
        }
    }

    private void ApplyHoverMaterial(Transform target)
    {
        MeshRenderer[] renderers = target.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            List<Material> materials = new List<Material>(renderer.materials);
            materials.Add(hoverMaterial);
            renderer.materials = materials.ToArray();
        }
    }

    private void RemoveHoverMaterial(Transform target)
    {
        MeshRenderer[] renderers = target.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            List<Material> materials = new List<Material>(renderer.materials);
            materials.RemoveAll(material => material.name.Contains("HologramHover"));
            renderer.materials = materials.ToArray();
        }
    }
}
