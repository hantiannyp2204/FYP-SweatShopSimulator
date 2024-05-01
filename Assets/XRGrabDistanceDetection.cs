using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabDistanceDetection : MonoBehaviour
{
    [SerializeField] private XRBaseInteractable baseInteractable;
    [SerializeField] private float distanceThreshold = 0.8f; // Adjustable distance threshold
    [SerializeField] GameObject detectionCollider;
    private float distanceBetweenObjects;
    private XRDirectInteractor directGrabInteractor;
    private Transform interactorTransform; // Store the interactor's transform
    private bool isGrabbed = false; // Flag to check if the object is currently grabbed


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
            // Calculate the distance between the current game object and the interactor

            if(detectionCollider == null)
            {
                distanceBetweenObjects = Vector3.Distance(baseInteractable.transform.position, interactorTransform.position);
            }
            //in case we are using secondary collider (door handle collider but script is in door frame)
            //aka using the colliders array in Grab interactable script
            else
            {
                distanceBetweenObjects = Vector3.Distance(detectionCollider.transform.position, interactorTransform.position);
            }
          
            Debug.Log(interactorTransform.position);
            // Log "left" if the distance exceeds the threshold
            if (distanceBetweenObjects > distanceThreshold)
            {
                directGrabInteractor.interactionManager.CancelInteractableSelection(baseInteractable);
            }
        }
    }

    void StartSelectGrabDetection(SelectEnterEventArgs args)
    {
        if(!(args.interactorObject is XRDirectInteractor))
        {
            return;
        }
        // When grabbed, store the interactor's transform
        interactorTransform = args.interactorObject.transform;
        directGrabInteractor = (XRDirectInteractor)args.interactorObject;
        isGrabbed = true; // Set the grabbed flag to true
    }

    void EndSelectGrabDetection(SelectExitEventArgs args)
    {
        // When released, clear the interactor's transform and reset the grabbed flag
        interactorTransform = null;
        isGrabbed = false;
    }
}