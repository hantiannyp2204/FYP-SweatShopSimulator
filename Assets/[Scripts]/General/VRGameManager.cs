using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI.BodyUI;
using static Item;

public class VRGameManager : MonoBehaviour, Iinteracted, IRelease
{
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
    [SerializeField] VRHandManager vrHandInteractionManager;
    [SerializeField] VRPlayerInvenetory vrPlayerInventory;
    [SerializeField] GameFeedback gameFeedback;
    //public Objective playerObjective;
    //[SerializeField] CustomerTable customerTable;
    //[SerializeField] GameTimer gameTimer;

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

    public void OnInteracted(GameObject obj)
    {
        Debug.Log("Grab");
        Item item = obj.GetComponent<Item>();
        if (item != null)
        {
            switch (item.GetState())
            {
                case ITEM_STATE.NOT_PICKED_UP:
                    vrPlayerInventory.AddItem(item, vrHandInteractionManager.gameObject.transform);
                    break;
                case ITEM_STATE.PICKED_UP:
                    break;
                default:
                    break;
            }
        }
    }
    public void OnRelease(Vector3 handVelocity)
    {
        Debug.Log("Release");
        vrPlayerInventory.RemoveItem(handVelocity);
    }
    void Start()
    {
        //playerMovement.Init();
        vrHandInteractionManager.Init();
        gameFeedback.InIt();
       // playerObjective.Init();     
        if(gameMode == GameMode.Levels)
        {
            //gameTimer.SetTimer(SecondsGiven);
        }
        //playerScore.Init();
        //pauseMenu.gameObject.SetActive(false);
        //endMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
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
            vrHandInteractionManager.UpdateInteractions();
            vrPlayerInventory.UpdateItemPositions();
            //update table timer
            if (gameMode == GameMode.Levels)
            {
                //customerTable.UpdateTimer();
                //gameEnded = gameTimer.UpdateTime();
            }
            else
            {
               // gameTimer.NoTime();
            }
        }
        //check for puase menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }
    private void OnEnable()
    {
        vrHandInteractionManager.SubcribeEvents((Iinteracted)this);
        vrHandInteractionManager.SubcribeEvents((IRelease)this);
       // customerTable.SubcribeEvents();
    }
    private void OnDisable()
    {
        vrHandInteractionManager.UnsubcribeEvents((Iinteracted)this);
        vrHandInteractionManager.UnsubcribeEvents((IRelease)this);
        //customerTable.UnsubcribeEvents();
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
