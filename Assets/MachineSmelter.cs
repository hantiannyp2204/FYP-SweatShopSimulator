using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSmelter : MonoBehaviour,Iinteractable
{
    Item inputItem;
    [SerializeField]List<ItemData> OutputItemList;
    [SerializeField]Transform itemPosition;
    float smeltTimer = 0;
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

    public void Interact(GameManager player)
    {
        Item currentItem = player.playerInventory.GetCurrentItem();
        Scrap currentScrapType = currentItem.GetComponent<Scrap>();
        //stop this fucntion if theres nothing in hand or it item is not a scrap
        if (currentItem == null || currentScrapType == null)
        {
            return;
        }
        //set the input
        inputItem = currentItem;
        //remove the item from inventory
        player.playerInventory.RemoveAtCurrentSlot();
        //move the input item inside the oven
        inputItem.transform.position = itemPosition.position;
        //set the parent the item position
        inputItem.transform.SetParent(itemPosition);

        //timer

        //convert scrap to its specific raw material
        //0 is plastic, 1 is wood, 2 is metal
        int selectedRawMaterial =0;
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
