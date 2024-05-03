using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAnvil : MonoBehaviour
{
    Item inputItem;
    [SerializeField] List<ItemData> OutputItemList;    
    [SerializeField] Transform ItemSpawnLocation;
    [SerializeField] AnvilHitbox anvilHitbox;
    //[SerializeField] private State currentState;
    //[SerializeField] private GameObject player;
    
    public VrMachineItemCollider anvilItemCollider;
    
    //GameObject outputItem;

    // Feedback
    [Header("FEEDBACK")]
    [SerializeField] private FeedbackEventData e_inputItem;
    [SerializeField] private FeedbackEventData e_takeOutputItem;
    [SerializeField] private FeedbackEventData e_run;
    [SerializeField] private FeedbackEventData e_done;

    //public enum State
    //{
    //    Empty,
    //    HasItem
    //}
    void Start()
    {
        //currentState = State.Empty;
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
    public void RunMachine()
    {
        Debug.Log("using anvil");

        foreach (RawMaterial currentRawType in anvilHitbox.GetRMaterialList())
        {
            ItemData outputItemData;
            //convert scrap to its specific raw material
            //0 is plastic, 1 is wood, 2 is metal
            int selectedFlatMaterial = 0;
            switch (currentRawType.GetRawMaterialType())
            {
                case RawMaterial.RawMaterialType.Plastic:
                    selectedFlatMaterial = 0;
                    Debug.Log("Flat_Plastic");
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
            Instantiate(outputItemData.GetPrefab(), ItemSpawnLocation);
            Debug.Log(ItemSpawnLocation.position);

            //destroy the input materials
            Destroy(currentRawType.gameObject);
            anvilHitbox.RMaterialList.Remove(currentRawType);
            anvilHitbox.ItemOnAnvil = false;
            anvilItemCollider.ClearProductList();
        }

       //ChangeState();
    }
    public void RunDeactive()
   {
        //currentState = State.Empty;
        Debug.Log("Deactivated");
   }
  
}

