using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI.BodyUI;
using static Item;
using static VRHandManager;

public class VRGameManager : MonoBehaviour, IVRInteracted, IVRRelease
{
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
    [SerializeField] List<VRPlayerInvenetory> vrPlayerInventoryList;
    [SerializeField] List<HandPresensePhysics> vrPlayerHandPhysicsList;
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

        //playerMovement.Init();
        foreach (VRHandManager handManager in vrHandInteractionManagerList)
        {
            handManager.Init();
        }
        foreach (VRPlayerInvenetory handInv in vrPlayerInventoryList)
        {
            handInv.Init();
        }
        foreach (HandPresensePhysics handPhysics in vrPlayerHandPhysicsList)
        {
            handPhysics.Init();
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
        foreach (HandPresensePhysics handPhysics in vrPlayerHandPhysicsList)
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
            foreach (var handInv in vrPlayerInventoryList)
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
    public void OnInteracted(GameObject obj, HandType handType)
    {
        bool interacted = false;
        //check if current hand type is the one calling
        foreach (var hand in vrHandInteractionManagerList)
        {
            if (hand.GetHandType() != handType) continue;
            Debug.Log("Grab");
            //for item
            Item item = obj.GetComponent<Item>();
            if (item != null)
            {
                switch (item.GetState())
                {
                    case ITEM_STATE.NOT_PICKED_UP:
                        foreach (VRPlayerInvenetory handInv in vrPlayerInventoryList)
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
                interacted = true;
            }
            //for grabables
            Grabables grabable = obj.GetComponent<Grabables>();
            if (grabable != null && !interacted)
            {
                grabable.Grab();
                interacted = true;
                foreach (HandPresensePhysics handPhysics in vrPlayerHandPhysicsList)
                {
                    if (handPhysics.GetHandType() != hand.GetHandType()) continue;
                    handPhysics.LockHand(grabable.transform.position);
                }
            }
        }

    }
    public void OnRelease(Vector3 handVelocity, HandType handType)
    {
        bool released = false;
        foreach (var hand in vrHandInteractionManagerList)
        {
        
            //check if holding item
            foreach (VRPlayerInvenetory handInv in vrPlayerInventoryList)
            {
                if (handInv.GetHandType() != handType) continue;
                if(handInv.GetCurrentHeldItem() != null)
                {
                    //if true, release them
                    handInv.RemoveItem(handVelocity);
                    released = true;
                    Debug.Log("Released item");
                    break;
                }

            }
            //release grabables
            foreach (HandPresensePhysics handPhysics in vrPlayerHandPhysicsList)
            {
                if (handPhysics.GetHandType() != hand.GetHandType() || released) continue;
                handPhysics.UnlockHand();
                released = true;
            }

        }
    }
    void DisableVRSystem()
    {
        VRPlayer.SetActive(false);
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        foreach (var hand in vrHandInteractionManagerList)
        {
            hand.SubcribeEvents((IVRInteracted)this);
            hand.SubcribeEvents((IVRRelease)this);
        }
       // customerTable.SubcribeEvents();
    }
    private void OnDisable()
    {
        foreach (var hand in vrHandInteractionManagerList)
        {
            hand.UnsubcribeEvents((IVRInteracted)this);
            hand.UnsubcribeEvents((IVRRelease)this);
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
