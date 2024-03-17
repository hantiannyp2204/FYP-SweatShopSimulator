using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;


//Inspired by Raqib
public class PlayerInteraction : MonoBehaviour
    , ISubscribeEvents<Iinteracted>
    , ISubscribeEvents<IinteractableInteracting>
    , ISubscribeEvents<IinteractableExtensionRetrieve>
    , ISubscribeEvents<IinteractableExtensionRetrieveObj>
{
    [SerializeField] private Transform head;
    [SerializeField] private float range = 5f;
    [SerializeField] private LayerMask ignoreLayer;
    Iinteractable currentInteractable;
    GameObject interactObj;
    [SerializeField] GameManager targetPlayer;

    System.Action<GameObject> OnInteracted;
    System.Action<GameObject> OnEnterObj;
    System.Action<GameObject> OnExitObj;

    System.Action<Iinteractable> OnEnter;
    System.Action<Iinteractable> OnExit;
    System.Action<Iinteractable> OnInteracting;
    System.Action<Iinteractable> OnStopInteracting;

    bool interacted = false;

    [SerializeField] private FeedbackEventData e_interactError;
    public void UpdateInteraction()
    {
        if (Physics.Raycast(head.transform.position, head.transform.forward, out RaycastHit hit, range, ~ignoreLayer))
        {
            Iinteractable interactable = hit.collider.GetComponent<Iinteractable>();
            if (interactable != null)
            {

                if (interactable != currentInteractable)
                {
                    // Exit previous interactable
                    IinteractableExtensionHover prevInteractable = currentInteractable as IinteractableExtensionHover;
                    if (prevInteractable != null)
                        prevInteractable.Exit(targetPlayer);

                    // Enter current
                    IinteractableExtensionHover interactableFocused = interactable as IinteractableExtensionHover;
                    if (interactableFocused != null)
                    {
                        interactableFocused.Enter(targetPlayer);
                    }

                    if (currentInteractable != null)
                    {
                        OnExit?.Invoke(currentInteractable);
                        OnExitObj?.Invoke(interactObj);
                    }


                    OnEnter?.Invoke(interactable);
                    OnEnterObj?.Invoke(hit.collider.gameObject);

                }
                interactObj = hit.collider.gameObject;
                currentInteractable = interactable;

            }
            else
            {
                ExitFocus();
            }
        }
        else
        {
            ExitFocus();
        }


        // Input to interact
        if (Input.GetKey(KeyCode.E) && !interacted)
        {
            if (currentInteractable != null)
            {

                Interact();
            }

        }


        if (Input.GetKeyUp(KeyCode.E))
        {
            if (currentInteractable != null)
            {
                OnStopInteracting?.Invoke(currentInteractable);
                IiinterableBeginCancel icancel = currentInteractable as IiinterableBeginCancel;
                if (icancel != null)
                    icancel.OnCancelInteract();

                Debug.Log("Cancel interaction");
            }
            interacted = false;
        }
    }
    void ExitFocus()
    {
        IinteractableExtensionHover interactableFocused = currentInteractable as IinteractableExtensionHover;
        if (interactableFocused != null)
            interactableFocused.Exit(targetPlayer);

        IiinterableBeginCancel icancel = currentInteractable as IiinterableBeginCancel;
        if (icancel != null)
            icancel.OnCancelInteract();

        if (currentInteractable != null)
        {
            OnExit?.Invoke(currentInteractable);
            OnExitObj?.Invoke(interactObj);
            OnStopInteracting?.Invoke(currentInteractable);
        }

        currentInteractable = null;
        interactObj = null;
        //Debug.Log("Exit focus");

    }
    void Interact()
    {

        if (currentInteractable == null || currentInteractable != null && !currentInteractable.CanInteract())
        {
            //play interact error sound
            e_interactError?.InvokeEvent(transform.position, Quaternion.identity, transform);
            return;
        }

        Debug.Log("Press E to " + currentInteractable.GetInteractName());
        interacted = true;
        currentInteractable.Interact(targetPlayer);
        OnInteracted?.Invoke(interactObj);
        currentInteractable = null;
        interactObj = null;
    }
    public Iinteractable GetCurrent()
    {
        return currentInteractable;
    }

    #region EVENTS
    public void SubcribeEvents(Iinteracted action) => OnInteracted += action.OnInteracted;
    public void UnsubcribeEvents(Iinteracted action) => OnInteracted -= action.OnInteracted;
    public void SubcribeEvents(IinteractableInteracting action)
    {
        OnInteracting += action.OnInteracting;
        OnStopInteracting += action.OnStopInteracting;
    }

    public void UnsubcribeEvents(IinteractableInteracting action)
    {
        OnInteracting -= action.OnInteracting;
        OnStopInteracting -= action.OnStopInteracting;
    }
    public void SubcribeEvents(IinteractableExtensionRetrieve action)
    {
        OnEnter += action.OnEnter;
        OnExit += action.OnExit;
    }

    public void UnsubcribeEvents(IinteractableExtensionRetrieve action)
    {
        OnEnter -= action.OnEnter;
        OnExit -= action.OnExit;
    }
    public void SubcribeEvents(IinteractableExtensionRetrieveObj action)
    {
        OnEnterObj += action.OnEnter;
        OnExitObj += action.OnExit;
    }
    public void UnsubcribeEvents(IinteractableExtensionRetrieveObj action)
    {
        OnEnterObj -= action.OnEnter;
        OnExitObj -= action.OnExit;
    }
    #endregion
}
public interface Iinteracted
{
    public void OnInteracted(GameObject obj);

}

public interface IinteractableExtensionRetrieve
{
    public void OnEnter(Iinteractable interactable);
    public void OnExit(Iinteractable interactable);
}
public interface IinteractableExtensionRetrieveObj
{
    public void OnEnter(GameObject interactable);
    public void OnExit(GameObject interactable);
}
public interface IinteractableInteracting
{
    public void OnInteracting(Iinteractable interactable);
    public void OnStopInteracting(Iinteractable interactable);
}