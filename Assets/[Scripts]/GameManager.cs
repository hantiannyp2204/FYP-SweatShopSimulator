using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;

public class GameManager : MonoBehaviour, Iinteracted
{
    public enum GameMode
    {
        Levels,
        Test_Chamber
    }
    public GameMode gameMode;
    //set the time
    public float SecondsGiven;
    [SerializeField] PlayerMovement playerMovement;
    public PlayerInventory playerInventory;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] GameFeedback gameFeedback;
    public Objective playerObjective;
    [SerializeField] CustomerTable customerTable;
    [SerializeField] GameTimer gameTimer;

    //Pause menu
    [SerializeField] PauseMenu pauseMenu;
    bool isPaused = false;
    public void OnInteracted(GameObject obj)
    {
        Item item = obj.GetComponent<Item>();
        //bool itemCanBePicked = item.GetCanBePicked();
        if (item != null)
        {
            switch (item.GetState())
            {
                case ITEM_STATE.NOT_PICKED_UP:
                    playerInventory.AddItem(item);
                    break;
                case ITEM_STATE.PICKED_UP:
                    break;
                default:
                    break;
            }
        }
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
        pauseMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.UpdateTransform();
        playerInteraction.UpdateInteraction();
        playerInventory.UpdateInventory();
        //update table timer
        if(gameMode == GameMode.Levels)
        {
            customerTable.UpdateTimer();
            gameTimer.UpdateTime();
        }
        //check for puase menu
        if(Input.GetKeyDown(KeyCode.Escape))
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
            pauseMenu.gameObject.SetActive(true);
            pauseMenu.EnableCursor();
        }
        else
        {
            pauseMenu.DisableCursor();
            pauseMenu.gameObject.SetActive(false);
        }
    }
}
