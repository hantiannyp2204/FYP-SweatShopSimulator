using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI.BodyUI;
using static Item;
using static VRHandManager;

public class VRGameManager : MonoBehaviour
{
    //to auto disable keyboard player if running on Andriod build (delted and removed)
    [SerializeField] PlatformChecker platformChecker;
    public enum GameMode
    {
        Levels,
        Test_Chamber
    }
    public GameMode gameMode;

    //Game variables
    //time given to complete task
    public float SecondsGiven;
    //score needed to win
    public float ScoreNeeded;

    //[SerializeField] PlayerMovement playerMovement; 
    [SerializeField] GameObject VRPlayer;
    [SerializeField] List<VRHandManager> vrHandInteractionManagerList;
    //[SerializeField] List<VRPlayerInvenetory> vrPlayerInventoryList;
    [SerializeField] List<HandPresencePhysics> vrPlayerHandPhysicsList;
    [SerializeField] List<HandColliders> vrPlayerHandColliderList;
    [SerializeField] GameFeedback gameFeedback;
    //public Objective playerObjective;
    [SerializeField] CustomerTable customerTable;
    [SerializeField] TMP_Text leftHandTimerText;
    [SerializeField] TMP_Text leftHandGrabText;
    [SerializeField] TMP_Text rightHandGrabText;

    ////Score system
    //[SerializeField] PlayerScore playerScore;
    //public static event Action<float> OnScoreAdded;
    public static void AddScore(float score)
    {
        //OnScoreAdded?.Invoke(score);
    }

    //Pause menu
    //[SerializeField] PauseMenu pauseMenu;
    bool isPaused = false;

    //game end system
    bool gameEnded = false;
    //[SerializeField] EndMenu endMenu;

  
    void Start()
    {
        //disable this and VR player if running on PC
#if UNITY_STANDALONE_WIN
        if (platformChecker == null || platformChecker.swapPlatform == false)
        {
            DisableVRSystem();
        }
#else
        if (platformChecker != null && platformChecker.swapPlatform == true)
        {
            DisableVRSystem();
        }
#endif
        if (leftHandTimerText == null) return; // null check
        if (leftHandGrabText == null) return;
        if (rightHandGrabText == null) return;

        //playerMovement.Init();
        foreach (VRHandManager handManager in vrHandInteractionManagerList)
        {
            handManager.Init();
        }
        //foreach (VRPlayerInvenetory handInv in vrPlayerInventoryList)
        //{
        //    handInv.Init();
        //}
        
        foreach (HandPresencePhysics handPhysics in vrPlayerHandPhysicsList)
        {
            handPhysics.Init();
        }
        foreach (HandColliders handColliders in vrPlayerHandColliderList)
        {
            handColliders.Init();
        }
        gameFeedback.InIt();
        customerTable.Init(leftHandTimerText, gameMode);
       // playerObjective.Init();     
        //playerScore.Init();
        //pauseMenu.gameObject.SetActive(false);
        //endMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    private void FixedUpdate()
    {
        foreach (HandPresencePhysics handPhysics in vrPlayerHandPhysicsList)
        {
            handPhysics.HandPhysicsFixedUpdate();
        }
    }
    // Update is called once per frame
    void Update()
    {
        // UNCOMMENT THIS LATER JERALD IS LESBIAN
        if (gameEnded)
        {
           // endMenu.gameObject.SetActive(true);
            //endMenu.GameEnd(ScoreNeeded, playerScore.GetScore());
            StopAllCoroutines();
            return;
        }

        if (!isPaused)
        {
            //playerMovement.UpdateTransform();
            foreach(var hand in vrHandInteractionManagerList)
            {
                hand.UpdateInteractions();
            }
            //foreach (var handInv in vrPlayerInventoryList)
            //{
            //    handInv.UpdateItemPositions();
            //}

            //update table timer
            customerTable.UpdateTimer(leftHandTimerText);
        }
        //check for puase menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }
    
    void DisableVRSystem()
    {
        VRPlayer.SetActive(false);
        this.gameObject.SetActive(false);
    }


    void TogglePauseMenu()
    {
        isPaused = !isPaused;
        if(isPaused)
        {
            Time.timeScale = 0;
           // pauseMenu.gameObject.SetActive(true);
            //pauseMenu.EnableCursor();
        }
        else
        {
            Time.timeScale = 1.0f;
           // pauseMenu.DisableCursor();
           // pauseMenu.gameObject.SetActive(false);
        }
    }


}
