using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAnvil : MonoBehaviour, Iinteractable
{
    Item inputItem;
    [SerializeField] List<ItemData> OutputItemList;
    [SerializeField] Transform itemPosition;
    ItemData outputItemData;
    GameObject outputItem;
    [SerializeField] float anvTimer = 3;
    float elapsedTime;
    //[SerializeField] TMP_Text timerText;
    Coroutine AnvilCoroutineHandler;

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

    public string GetInteractName() => "Use " + name;

    IEnumerator AnvilCoroutine(GameManager player, Item currentItem, RawMaterial currentRawType)
    {
        //set the input
        inputItem = currentItem;
        //remove the item from inventory
        player.playerInventory.RemoveAtCurrentSlot();
        //move the input item on the anvil
        inputItem.transform.position = itemPosition.position;
        //set the parent the item position
        inputItem.transform.SetParent(itemPosition);

        //convert scrap to its specific raw material
        //0 is plastic, 1 is wood, 2 is metal
        int selectedFlatMaterial = 0;
        switch (currentRawType.GetRawMaterialType())
        {
            case RawMaterial.RawMaterialType.Plastic:
                selectedFlatMaterial = 0;
                Debug.Log("Flat_Plastic");
                break;
            case RawMaterial.RawMaterialType.Wood:
                selectedFlatMaterial = 1;
                Debug.Log("Flat_Wood");
                break;
            case RawMaterial.RawMaterialType.Metal:
                selectedFlatMaterial = 2;
                Debug.Log("Flat_Metal");
                break;
        }

        //set the output item
        outputItemData = OutputItemList[selectedFlatMaterial];
        //spawn the raw material
        outputItem = Instantiate(outputItemData.GetPrefab(), itemPosition);
        //delete the scrap
        Destroy(inputItem.gameObject);

        AnvilCoroutineHandler = null;

        yield return null;
    }

    public void Interact(GameManager player)
    {

        //cant interact if smelting
        if (AnvilCoroutineHandler != null)
        {
            return;
        }
        if (outputItemData == null)
        {
            Item currenttool = player.playerInventory.GetCurrentItem();
            Item currentItem = player.playerInventory.GetCurrentItem();
            RawMaterial currentRawType = currentItem.GetComponent<RawMaterial>();
            //check if Hammer is equipped
            if (currenttool == null || currenttool.name != "Hammer")
            {
                Debug.Log("pick up Hammer");
                return;
            }
            else if (currentRawType == null)//checks if scrap is in hand
            {
                Debug.Log("pick up Raw Material");
                return;
            }
        }
        //take out item if have output
        else
        {
            //inventory full, cant take
            if (player.playerInventory.IsFull())
            {
                return;
            }
            //play take out output item sound
            e_takeOutputItem?.InvokeEvent(transform.position, Quaternion.identity, transform);
            player.playerInventory.AddItem(outputItem.GetComponent<Item>());
            //reset
            outputItemData = null;
            // timerText.text = "Ready";

        }
    }



    //// Start is called before the first frame update
    //void Start()
    //{    

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}

