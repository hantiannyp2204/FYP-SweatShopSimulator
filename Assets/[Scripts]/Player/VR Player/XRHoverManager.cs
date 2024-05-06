using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRRayHoverManager : MonoBehaviour
{
    [SerializeField] private List<XRBaseInteractor> interactors; // Include both ray and direct interactors
    [SerializeField] private Material hoverMaterial;

    private Dictionary<Transform, HashSet<IXRInteractor>> activeInteractors = new();

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
        // Optionally, you could handle this differently if you want the material to change or remain upon grabbing
        AddHoverMaterial(args.interactableObject.transform, args.interactorObject);
    }

    private void HandleSelectExited(SelectExitEventArgs args)
    {
        RemoveHoverMaterial(args.interactableObject.transform, args.interactorObject);
    }

    private void AddHoverMaterial(Transform target, IXRInteractor interactor)
    {
        if (!activeInteractors.ContainsKey(target))
        {
            activeInteractors[target] = new HashSet<IXRInteractor>();
            // Apply the hover material only if this is the first interactor to hover over
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
                // Remove the material only when the last interactor has exited
                RemoveHoverMaterial(target);
                activeInteractors.Remove(target);
            }
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
            foreach (var material in materials)
            {
                if(material.name.Contains("Hologramhover"))
                {
                    materials.Remove(material);
                    continue;
                }
            }
          
            renderer.materials = materials.ToArray();
        }
    }
}
