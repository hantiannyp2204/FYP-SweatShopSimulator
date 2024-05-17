using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MacineFab : MonoBehaviour
{
    [Header("Feedback")]
    [SerializeField] private FeedbackEventData Fabricator_run;
    [SerializeField] private Transform SpawnItemTransform;
    

    //public Power power;
    bool gameEnd = false;
    public bool HasGameStarted = false;
    public bool IsGameRunning = false;
    public GameObject _TextHolder;
    public FabricatorCrafting _Crafting;
    public PowerForFab _PowerFab;
    public GameObject _RedButton;
    public bool _audioPlayed;

    public GameObject _WinORLose;
    public NewController newController; // Reference to the NewController script
    private void Start()
    {
        _audioPlayed = false;
    }
    public bool CanInteract()
    {
        return true;
    }

    public float GetInteractingLast()
    {
        throw new System.NotImplementedException();
    }

    public string GetInteractName() => "Use " + name;

   

    public bool IsGameEnded() => gameEnd;
    public void RunActive()
    {
        Debug.Log("Machine");
        _PowerFab.CheckIfGotPower();
        if (_PowerFab == true)
        {
            Debug.Log(_PowerFab._PowerForFab);
            if (_Crafting.HasChosenCraftingItem == true)
            {
                _Crafting.CheckIfPresent();
                _audioPlayed = true;
            }
            if (_Crafting.EnoughMaterials == true && _Crafting.HasChosenCraftingItem == true)
            {
                if (_audioPlayed)
                {
                    Fabricator_run?.InvokeEvent(transform.position, Quaternion.identity, transform);
                    _audioPlayed = false;
                }
                //Enalble here
                _RedButton.SetActive(true);
                
                IsGameRunning = true;
                //_Crafting.foundCount = 0;
                HasGameStarted = true;
                newController.hold = true;
                gameEnd = true;
                _TextHolder.SetActive(true);
            }
        }
    }

    public void RunDective()
    { 
        Debug.Log("Machine NOt Active");
    }

    public void ToggleOn()
    {
        //Debug.Log("Machine");
        //_PowerFab.CheckIfGotPower();
        //if (_PowerFab == true)
        //{
        //    Debug.Log(_PowerFab._PowerForFab);
        //    if (_Crafting.HasChosenCraftingItem == true)
        //    {
        //        _Crafting.CheckIfPresent();
        //    }
        //    if (_Crafting.EnoughMaterials == true && _Crafting.HasChosenCraftingItem == true)
        //    {
        //        //Enalble here
        //        _RedButton.SetActive(true);
        //        Fabricator_run?.InvokeEvent(transform.position, Quaternion.identity, transform);
        //        IsGameRunning = true;
        //        //_Crafting.foundCount = 0;
        //        HasGameStarted = true;
        //        newController.hold = true;
        //        gameEnd = true;
        //        _TextHolder.SetActive(true);
        //        _NextButton.SetActive(true);
        //        _StartButton.SetActive(true);
        //    }
        //}
      

    }

    public void ToggleOFF()
    {
        if (IsGameRunning == false)
        {
            newController.hold = false;
            gameEnd = false;
            _TextHolder.SetActive(false);
        }
        return;
    }
    /// <summary>
    /// 
    /// </summary>
    public void PressedButtonToggle()
    {
     
        //newController.hold = true;
        newController.EndRotate();
        
        Debug.Log("Machine Active");
    }

    public void StartButtonToggleOFF()
    {
       
        newController.hold = false;
        Debug.Log("Machine Not Active");
    }




}
