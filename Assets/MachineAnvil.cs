using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAnvil : MonoBehaviour, Iinteractable
{
    Item inputItem;
    [SerializeField] List<ItemData> OutputItemList;
    [SerializeField] Transform itemPosition;
    public bool CanInteract()
    {
        return true;
    }

    public float GetInteractingLast()
    {
        throw new System.NotImplementedException();
    }

    public string GetInteractName() => "Use " + name;


    public void Interact(GameManager player)
    {
        Item currenttool = player.playerInventory.GetCurrentItem();
        Item currentItem = player.playerInventory.GetCurrentItem();
        Scrap currentScrapType = currentItem.GetComponent<Scrap>();
        if (currenttool == null || currenttool.name != "Hammer") //check if Hammer is equipped
        {
            Debug.Log("pick up Hammer");
            return;
        }
        else if (currentScrapType == null)//checks if scrap is in hand
        {
            Debug.Log("pick up Scrap");
            return;
        }
        else //carries out function (using smelter as pseudo script first)
        {
            Debug.Log("Interacting " + name + " with " + player.playerInventory.GetCurrentItem().Data.name);
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
            //spawn the raw material
            Instantiate(OutputItemList[selectedRawMaterial].GetPrefab(), itemPosition);
            //delete the scrap
            Destroy(inputItem);

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
}
