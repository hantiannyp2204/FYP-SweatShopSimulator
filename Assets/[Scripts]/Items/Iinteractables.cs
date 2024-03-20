using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Inspired by Raqib
public interface Iinteractable
{
    public bool CanInteract();
    public void Interact(GameManager player);
    public string GetInteractName();
    public float GetInteractingLast(); // how long does the interaction last
}
public interface IinteractableExtensionFail
{
    public string GetCantInteractName();
    public void InteractFail(GameManager player);

}

public interface IinteractableExtensionHover
{
    public void Enter(GameManager player);
    public void Exit(GameManager player);
}
public interface IiinterableBeginCancel
{
    public void OnStartInteract();
    public void OnCancelInteract();

}
