using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MacineFab : MonoBehaviour, Iinteractable
{
    //public Power power;
    bool gameEnd = false;
    public bool HasGameStarted = false;
    public GameObject _TextHolder;
    public GameObject _StartButton;
    public GameObject _NextButton;
    public FabricatorCrafting _Crafting;
    public PowerForFab _PowerFab;

    public GameObject _WinORLose;
    public NewController newController; // Reference to the NewController script

    public bool CanInteract()
    {
        return true;
    }

    public float GetInteractingLast()
    {
        throw new System.NotImplementedException();
    }

    public string GetInteractName() => "Use " + name;

    public void Interact(KeyboardGameManager player)
    {
        
        Item currentItem = player.playerInventory.GetCurrentItem();

        if (currentItem == null)
        {
            return;
        }
        else
        {
           _TextHolder.SetActive(true);
            Debug.Log("Interacting " + name + " with " + player.playerInventory.GetCurrentItem().Data.name);
        }
    }

    public bool IsGameEnded() => gameEnd;
    public void RunActive()
    {
        Debug.Log("Machine Active");
    }

    public void RunDective()
    { 
        Debug.Log("Machine NOt Active");
    }

    public void ToggleOn()
    {
        Debug.Log("Machine");
        _PowerFab.CheckIfGotPower();
        if (_PowerFab == true)
        {
            Debug.Log(_PowerFab._PowerForFab);
            if (_Crafting.HasChosenCraftingItem == true)
            {
                _Crafting.CheckIfPresent();
            }
            if (_Crafting.EnoughMaterials == true && _Crafting.HasChosenCraftingItem == true)
            {
                //_Crafting.foundCount = 0;
                HasGameStarted = true;
                newController.hold = true;
                gameEnd = true;
                _TextHolder.SetActive(true);
                _NextButton.SetActive(true);
                _StartButton.SetActive(true);
            }
        }
      

    }

    public void ToggleOFF()
    {
        newController.hold = false;
        gameEnd = false;
        _TextHolder.SetActive(false);
    }
    /// <summary>
    /// 
    /// </summary>
    public void StartButtonToggle()
    {
       
        newController.hold = true;
        
        Debug.Log("Machine Active");
    }

    public void StartButtonToggleOFF()
    {
       
        newController.hold = false;
        Debug.Log("Machine Not Active");
    }




}
