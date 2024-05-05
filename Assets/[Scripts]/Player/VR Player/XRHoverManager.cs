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
        }
        if (activeInteractors[target].Count == 0)
        {
            MeshRenderer[] renderers = target.GetComponentsInChildren<MeshRenderer>();
            foreach (var renderer in renderers)
            {
                List<Material> materials = new List<Material>(renderer.materials);
                materials.Add(hoverMaterial);
                renderer.materials = materials.ToArray();
            }
        }
        activeInteractors[target].Add(interactor);
    }

    private void RemoveHoverMaterial(Transform target, IXRInteractor interactor)
    {
        if (activeInteractors.ContainsKey(target))
        {
            activeInteractors[target].Remove(interactor);
            if (activeInteractors[target].Count == 0)
            {
                MeshRenderer[] renderers = target.GetComponentsInChildren<MeshRenderer>();
                foreach (var renderer in renderers)
                {
                    List<Material> materials = new List<Material>(renderer.materials);
                    materials.Remove(hoverMaterial);
                    renderer.materials = materials.ToArray();
                }
                activeInteractors.Remove(target);
            }
        }
    }
}
