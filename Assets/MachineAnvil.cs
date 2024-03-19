using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAnvil : MonoBehaviour, Iinteractable
{
    Item inputItem;
    [SerializeField] List<ItemData> inputItemList;
    [SerializeField] List<ItemData> OutputItemList;
    [SerializeField] Transform Anvil_itemPosition;
    ItemData outputItemData;
    GameObject outputItem;
    private bool itemInside=false;

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
        return "using Anvil";

    }
    public void PutItemInside(GameManager player, Item currentItem)
    {
        //set the input
        inputItem = player.playerInventory.GetCurrentItem();
        //remove the item from inventory
        player.playerInventory.RemoveAtCurrentSlot();
        //move the input item on the anvil
        inputItem.transform.position = Anvil_itemPosition.position;
        //reset it's rotation
        inputItem.transform.rotation = Quaternion.identity;
        //set the parent the item position
        inputItem.transform.SetParent(Anvil_itemPosition);
        e_run?.InvokeEvent(transform.position, Quaternion.identity, transform);
        e_done?.InvokeEvent(transform.position, Quaternion.identity, transform);
        //ensures there's an item inside
        Debug.Log("Item inside Anvil");
        itemInside = true;
    }
    public void Interact(GameManager player)
    {
       
       /* if (itemInside)*/
        //{ 
            //cant interact if in use
            if (outputItemData == null)
            {
           
                // Item currenttool = player.playerInventory.GetCurrentItem();
                Item currentItem = player.playerInventory.GetCurrentItem();
                if (currentItem == null) return;
                RawMaterial currentRawType = currentItem.GetComponent<RawMaterial>();
                //check if Hammer is equipped
                //if (currenttool == null || currenttool.name != "Hammer")
                //{
                //    Debug.Log("pick up Hammer");
                //    return;
                //}
                //else
                if (currentRawType == null)//checks if raw material is in hand
                {
                    Debug.Log("pick up Raw Material");
                    return;
                }

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

                //spawn the flattened material
                outputItem = Instantiate(outputItemData.GetPrefab(), Anvil_itemPosition);
                //delete the raw material
                Destroy(inputItem.gameObject);


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
                Debug.Log("Taken out");
                // timerText.text = "Ready";

            }
        //}
        //else
        //{
        //    Debug.Log("No item in anvil");
        //}
       
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

