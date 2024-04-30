using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabDistanceDetection : MonoBehaviour
{
    [SerializeField] private XRBaseInteractable baseInteractable;
    [SerializeField] private float distanceThreshold = 0.8f; // Adjustable distance threshold
    private Transform interactorTransform; // Store the interactor's transform
    private bool isGrabbed = false; // Flag to check if the object is currently grabbed
    private XRDirectInteractor directGrabInteractor;

    private void OnEnable()
    {
        baseInteractable.selectEntered.AddListener(StartSelectGrabDetection);
        baseInteractable.selectExited.AddListener(EndSelectGrabDetection);
    }

    private void OnDisable()
    {
        baseInteractable.selectEntered.RemoveListener(StartSelectGrabDetection);
        baseInteractable.selectExited.RemoveListener(EndSelectGrabDetection);
    }

    void Update()
    {
        if (isGrabbed && interactorTransform != null)
        {
            Vector3 interactablePosition = GetInteractablePosition();
            float distanceBetweenObjects = Vector3.Distance(interactablePosition, interactorTransform.position);

            if (distanceBetweenObjects > distanceThreshold)
            {
                directGrabInteractor.interactionManager.CancelInteractableSelection(baseInteractable);
            }
        }
    }

    void StartSelectGrabDetection(SelectEnterEventArgs args)
    {
        if (!(args.interactorObject is XRDirectInteractor))
        {
            return;
        }

        interactorTransform = args.interactorObject.transform;
        directGrabInteractor = (XRDirectInteractor)args.interactorObject;
        isGrabbed = true; // Set the grabbed flag to true
    }

    void EndSelectGrabDetection(SelectExitEventArgs args)
    {
        interactorTransform = null;
        isGrabbed = false;
    }

    Vector3 GetInteractablePosition()
    {
        // Check if there are any colliders set and use the first collider's position
        if (baseInteractable.colliders.Count > 0 && baseInteractable.colliders[0] != null)
        {
            return baseInteractable.colliders[0].bounds.center;
        }
        // Fallback to the interactable's transform position
        return baseInteractable.transform.position;
    }
}