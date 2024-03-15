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
        FlatMaterials currentRawType = currentItem.GetComponent<FlatMaterials>();
        if (currenttool == null || currenttool.name != "Hammer") //check if Hammer is equipped
        {
            Debug.Log("pick up Hammer");
            return;
        }
        else if (currentRawType == null)//checks if scrap is in hand
        {
            Debug.Log("pick up Raw Material");
            return;
        }
        else //carries out function 
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
            switch (currentRawType.GetMaterialType())
            {
                case FlatMaterials.FlatMaterialType.Plastic:
                    selectedFlatMaterial = 0;
                    Debug.Log("Flat_Plastic");
                    break;
                case FlatMaterials.FlatMaterialType.Wood:
                    selectedFlatMaterial = 1;
                    Debug.Log("Flat_Wood");
                    break;
                case FlatMaterials.FlatMaterialType.Metal:
                    selectedFlatMaterial = 2;
                    Debug.Log("Flat_Metal");
                    break;
            }
            //spawn the flat material
            Instantiate(OutputItemList[selectedFlatMaterial].GetPrefab(), itemPosition);
            //delete the raw material
            Destroy(inputItem);

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
}
