using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI.BodyUI;
using static Item;
using static VRHandManager;

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
    [SerializeField] List<VRHandManager> vrHandInteractionManager;
    [SerializeField] List<VRPlayerInvenetory> vrPlayerInventory;
    [SerializeField] List<HandPresensePhysics> vrPlayerHandPhysics;
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

    public void OnInteracted(GameObject obj, HandType handType)
    {
        //check if current hand type is the one calling
        foreach(var hand in vrHandInteractionManager)
        {
            if (hand.GetHandType() != handType) continue;
            Debug.Log("Grab");
            Item item = obj.GetComponent<Item>();
            if (item != null)
            {
                switch (item.GetState())
                {
                    case ITEM_STATE.NOT_PICKED_UP:
                        foreach (VRPlayerInvenetory handInv in vrPlayerInventory)
                        {
                            if (handInv.GetHandType() != handType) continue;
                            handInv.AddItem(item, hand.gameObject.transform);
                        }
            
                        break;
                    case ITEM_STATE.PICKED_UP:
                        break;
                    default:
                        break;
                }
            }
        }

    }
    public void OnRelease(Vector3 handVelocity, HandType handType)
    {
        foreach (var hand in vrHandInteractionManager)
        {
            Debug.Log("Release");
            foreach (VRPlayerInvenetory handInv in vrPlayerInventory)
            {
                if (handInv.GetHandType() != handType) continue;
                handInv.RemoveItem(handVelocity);
            }

        }


    }
    void Start()
    {
        //playerMovement.Init();
        foreach(VRHandManager handManager in vrHandInteractionManager)
        {
            handManager.Init();
        }
        foreach (VRPlayerInvenetory handInv in vrPlayerInventory)
        {
            handInv.Init();
        }

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
    private void FixedUpdate()
    {
        foreach (HandPresensePhysics handPhysics in vrPlayerHandPhysics)
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
            foreach(var hand in vrHandInteractionManager)
            {
                hand.UpdateInteractions();
            }
            foreach (var handInv in vrPlayerInventory)
            {
                handInv.UpdateItemPositions();
            }

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
        foreach (var hand in vrHandInteractionManager)
        {
            hand.SubcribeEvents((Iinteracted)this);
            hand.SubcribeEvents((IRelease)this);
        }
       // customerTable.SubcribeEvents();
    }
    private void OnDisable()
    {
        foreach (var hand in vrHandInteractionManager)
        {
            hand.UnsubcribeEvents((Iinteracted)this);
            hand.UnsubcribeEvents((IRelease)this);
        }

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
