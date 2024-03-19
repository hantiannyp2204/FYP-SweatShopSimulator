using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MachineSmelter : MonoBehaviour,Iinteractable
{
    Item inputItem;
    [SerializeField]List<ItemData> OutputItemList;
    [SerializeField]Transform itemPosition;
    ItemData outputItemData;
    GameObject outputItem;
    [SerializeField] float smeltTime = 3;
    float elapsedTime;
    [SerializeField] TMP_Text timerText;
    Coroutine smeltingCoroutineHandler;

    // Feedback
    [Header("FEEDBACK")]
    [SerializeField] private FeedbackEventData e_inputItem;
    [SerializeField] private FeedbackEventData e_takeOutputItem;
    [SerializeField] private FeedbackEventData e_run;
    [SerializeField] private FeedbackEventData e_done;
    public bool CanInteract()
    {
        return true;
    }

    public float GetInteractingLast()
    {
        throw new System.NotImplementedException();
    }

    public string GetInteractName()
    {
        return "smelt with smelter" ;

    }
    private void Awake()
    {
        timerText.text = "Ready";
    }
    IEnumerator SmeltCoroutine(GameManager player, Item currentItem, Scrap currentScrapType)
    {
        //set the input
        inputItem = currentItem;
        //remove the item from inventory
        player.playerInventory.RemoveAtCurrentSlot();
        //move the input item inside the oven
        inputItem.transform.position = itemPosition.position;
        //reset it's rotation
        inputItem.transform.rotation = Quaternion.identity;
        //set the parent the item position
        inputItem.transform.SetParent(itemPosition);
        e_run?.InvokeEvent(transform.position, Quaternion.identity, transform);
        //timer
        while (elapsedTime <= smeltTime)
        {
            elapsedTime += Time.deltaTime;
            timerText.text = ((int)(smeltTime - elapsedTime)+1).ToString();
            yield return null;
        }
        e_done?.InvokeEvent(transform.position, Quaternion.identity, transform);
        timerText.text = "Done";
        elapsedTime = 0;
        //convert scrap to its specific raw material
        //0 is plastic, 1 is wood, 2 is metal
        int selectedRawMaterial = 0;
        switch (currentScrapType.GetScrapType())
        {
            case Scrap.ScrapType.Plastic:
                selectedRawMaterial = 0;
                break;
            case Scrap.ScrapType.Wood:
                selectedRawMaterial = 1;
                break;
            case Scrap.ScrapType.Metal:
                selectedRawMaterial = 2;
                break;
        }

        //set the output item
        outputItemData = OutputItemList[selectedRawMaterial];
        //spawn the raw material
        outputItem = Instantiate(outputItemData.GetPrefab(), itemPosition);
        //delete the scrap
        Destroy(inputItem.gameObject);

        smeltingCoroutineHandler = null;

        yield return null;
    }
    public void Interact(GameManager player)
    {
        //cant interact if smelting
        if(smeltingCoroutineHandler != null )
        {
            return;
        }
        //put in item if no output
        if(outputItemData == null)
        {  
            Item currentItem = player.playerInventory.GetCurrentItem();
            //stop this fucntion if theres nothing in hand 
            if (currentItem == null) return;
            Scrap currentScrapType = currentItem.GetComponent<Scrap>();
            //stop this fucntion if item is not a scrap
            if (currentScrapType == null)
            {
                return;
            }
            //play input item sound
            e_inputItem?.InvokeEvent(transform.position, Quaternion.identity, transform);
            smeltingCoroutineHandler = StartCoroutine(SmeltCoroutine(player, currentItem, currentScrapType));


        }
        //take out item if have output
        else
        {
            //inventory full, cant take
            if(player.playerInventory.IsFull())
            {
                return;
            }
            //play take out output item sound
            e_takeOutputItem?.InvokeEvent(transform.position, Quaternion.identity, transform);
            player.playerInventory.AddItem(outputItem.GetComponent<Item>());
            //reset
            outputItemData = null;
            timerText.text = "Ready";
  
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
