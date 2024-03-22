using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Inspired by Raqib
public interface Iinteractable
{
    public bool CanInteract();
    public void Interact(KeyboardGameManager player);
    public string GetInteractName();
    public float GetInteractingLast(); // how long does the interaction last
}
public interface IinteractableExtensionFail
{
    public string GetCantInteractName();
    public void InteractFail(KeyboardGameManager player);

}

public interface IinteractableExtensionHover
{
    public void Enter(KeyboardGameManager player);
    public void Exit(KeyboardGameManager player);
}
public interface IiinterableBeginCancel
{
    public void OnStartInteract();
    public void OnCancelInteract();

}
