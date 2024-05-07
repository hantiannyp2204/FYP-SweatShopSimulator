using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    //survive until this time to win the level
    public float SecondsGiven;

    //[SerializeField] PlayerMovement playerMovement; 
    [SerializeField] GameObject VRPlayer;
    [SerializeField] List<VRHandManager> vrHandInteractionManagerList;
    //[SerializeField] List<VRPlayerInvenetory> vrPlayerInventoryList;
    [SerializeField] List<HandPresencePhysics> vrPlayerHandPhysicsList;
    [SerializeField] List<HandColliders> vrPlayerHandColliderList;
    [SerializeField] List<VRHandRenderers> vrPlayerHandRenders;
    [SerializeField] ContinuousMovementPhysics vrPlayerMovement;

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
        if(gameMode == GameMode.Levels)
        {
            customerTable.SetTimeNeededToWin(SecondsGiven);
        }
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
        vrPlayerMovement.PlayerMovementFixedUpdate();
    }
    // Update is called once per frame
    void Update()
    {
        gameEnded = customerTable.isEndGame();

        //things that should not be affected by endgame/game paused
        foreach (var hand in vrHandInteractionManagerList)
        {
            hand.UpdateInteractions();
        }
        //stop if its levels
        if(!gameEnded && gameMode == GameMode.Levels)
        {
            vrPlayerMovement.PlayerMovementInputUpdate();
        }


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
        if(VRPlayer != null)
        {
            VRPlayer.SetActive(false);
            this.gameObject.SetActive(false);
        }

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
