using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;

public class GameManager : MonoBehaviour, Iinteracted
{
    [SerializeField] PlayerMovement playerMovement;
    public PlayerInventory playerInventory;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] GameFeedback gameFeedback;
    public Objective playerObjective;
    [SerializeField] CustomerTable customerTable;
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
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.UpdateTransform();
        playerInteraction.UpdateInteraction();
        playerInventory.UpdateInventory();
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
}
