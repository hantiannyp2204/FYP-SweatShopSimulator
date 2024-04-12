using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAnvil : MonoBehaviour
{
    Item inputItem;
    [SerializeField] List<ItemData> OutputItemList;
    [SerializeField] Transform Anvil_itemPosition;
    [SerializeField] private State currentState;
    ItemData outputItemData;
    GameObject outputItem;

    // Feedback
    [Header("FEEDBACK")]
    [SerializeField] private FeedbackEventData e_inputItem;
    [SerializeField] private FeedbackEventData e_takeOutputItem;
    [SerializeField] private FeedbackEventData e_run;
    [SerializeField] private FeedbackEventData e_done;

    public enum State
    {
        Empty,
        HasItem
    }
    void Start()
    {
        currentState = State.Empty;
    }
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
    //public void PutItemInside(GameManager player, Item currentItem)
    //{ 
    //    itemInside = true;
    //}
    public void Interact(KeyboardGameManager player)
    {
        Item currentItem = player.playerInventory.GetCurrentItem();
       
        Item currenttool = player.playerInventory.GetCurrentItem();
        //cant interact if in use
        if (outputItemData == null && currentState == State.Empty)
        {
            //Checks for item in hand

            if (currentItem == null)
            {
                return;
                Debug.Log("nothing in hand");
            }
            //checks if raw material is in hand
            RawMaterial currentRawType = currentItem.GetComponent<RawMaterial>();
            if (currentRawType == null)
            {
                Debug.Log("pick up Raw Material");
                return;
            }
            //set the input
            inputItem = player.playerInventory.GetCurrentItem();
            //remove the item from inventory
            player.playerInventory.RemoveAtCurrentSlot(true);
            //move the input item on the anvil
            inputItem.transform.position = Anvil_itemPosition.position;
            //reset it's rotation
            inputItem.transform.rotation = Quaternion.identity;
            //set the parent the item position
            inputItem.transform.SetParent(Anvil_itemPosition);
            e_run?.InvokeEvent(transform.position, Quaternion.identity, transform);
            e_done?.InvokeEvent(transform.position, Quaternion.identity, transform);
            //Changes State to HasItem
            ChangeState();
            //if (inputItem == null)
            //{

            //    currentState = State.Empty;
            //    Debug.Log("State changed back");
            //}
            //else
            //{
            //   
            //    Debug.Log("State changed");
            //}
        }
        //needs to stop code and check if player is holding a hammer
        else if (currentState == State.HasItem)
        {
            //check if Hammer is equipped
            if (currentItem == null || currentItem.name != "Hammer")
            {
                Debug.Log("pick up Hammer");
                return;

            }
            else
            {
                RawMaterial currentRawType = inputItem.GetComponent<RawMaterial>();
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
                //Changes state to 
                ChangeState();


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
            Debug.Log("Taken out");
            // timerText.text = "Ready";

        }

        void ChangeState()
        {
            switch (currentState)
            {
                case State.Empty:
                    currentState = State.HasItem;
                    break;
                case State.HasItem:
                    currentState = State.Empty;
                    Debug.Log("State changed to Crafting");
                    break;
                //case State.Crafting:
                //    currentState = State.Empty;
                //    Debug.Log("State changed to Empty");
                //    break;
            }
        }
    }



    //// Start is called before the first frame update
    

    //// Update is called once per frame
    //void Update()
    //{

    //}
}

