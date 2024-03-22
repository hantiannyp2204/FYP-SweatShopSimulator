using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI.BodyUI;
using static Item;

public class VRGameManager : MonoBehaviour, Iinteracted
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

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] GameFeedback gameFeedback;
    public Objective playerObjective;
    [SerializeField] CustomerTable customerTable;
    [SerializeField] GameTimer gameTimer;

    //Score system
    [SerializeField] PlayerScore playerScore;
    public static event Action<float> OnScoreAdded;
    public static void AddScore(float score)
    {
        OnScoreAdded?.Invoke(score);
    }

    //Pause menu
    [SerializeField] PauseMenu pauseMenu;
    bool isPaused = false;

    //game end system
    bool gameEnded = false;
    [SerializeField] EndMenu endMenu;

    public void OnInteracted(GameObject obj)
    {
        Item item = obj.GetComponent<Item>();
       
    }

    void Start()
    {
        playerMovement.Init();
        gameFeedback.InIt();
        playerObjective.Init();     
        if(gameMode == GameMode.Levels)
        {
            gameTimer.SetTimer(SecondsGiven);
        }
        playerScore.Init();
        pauseMenu.gameObject.SetActive(false);
        endMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // UNCOMMENT THIS LATER JERALD IS LESBIAN
        //if (gameEnded)
        //{
        //    endMenu.gameObject.SetActive(true);
        //    endMenu.GameEnd(ScoreNeeded,playerScore.GetScore());
        //    StopAllCoroutines();
        //    return;
        //}

        if(!isPaused)
        {
            playerMovement.UpdateTransform();
            playerInteraction.UpdateInteraction();
            //update table timer
            if (gameMode == GameMode.Levels)
            {
                customerTable.UpdateTimer();
                gameEnded = gameTimer.UpdateTime();
            }
            else
            {
                gameTimer.NoTime();
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
        playerInteraction.SubcribeEvents(this);
        customerTable.SubcribeEvents();
    }
    private void OnDisable()
    {
        playerInteraction.UnsubcribeEvents(this);
        customerTable.UnsubcribeEvents();
    }
    void TogglePauseMenu()
    {
        isPaused = !isPaused;
        if(isPaused)
        {
            Time.timeScale = 0;
            pauseMenu.gameObject.SetActive(true);
            pauseMenu.EnableCursor();
        }
        else
        {
            Time.timeScale = 1.0f;
            pauseMenu.DisableCursor();
            pauseMenu.gameObject.SetActive(false);
        }
    }
}
