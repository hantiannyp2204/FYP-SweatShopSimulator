using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnvilHitbox : MonoBehaviour
{
    [SerializeField] public List<RawMaterial> RMaterialList = new();
    List<GameObject> trashList = new();
    [SerializeField] VrMachineItemCollider itemCollider;

    public List<RawMaterial> GetRMaterialList() => RMaterialList;
    public List<GameObject> GetTrashList() => trashList;
    public bool ItemOnAnvil = false; 
    private void OnTriggerEnter(Collider other)
    {
        //check all gameobject in collider containing RawMaterial.cs
        RawMaterial RMComponent = other.GetComponent<RawMaterial>();
        if (RMComponent != null)
        {
            RMaterialList.Add(RMComponent);
            ItemOnAnvil = true;
            Debug.Log("Item on anvil");
        }
        //else if (RMComponent != null || other.gameObject.tag == "Hammer")
        //{
        //    ItemOnAnvil = true;
        //    Debug.Log("Item/hammer on anvil");
        //}
        //else 
        ////if item is not a RM, burn it (destroy)
        //{
        //    trashList.Add(other.gameObject);
        //    ItemOnAnvil = false;
        //}
        
    }
    private void OnTriggerExit(Collider other)
    {
        RawMaterial RMComponent = other.GetComponent<RawMaterial>();
        if (RMComponent != null && RMaterialList.Contains(RMComponent))
        {
            RMaterialList.Remove(RMComponent);
            ItemOnAnvil = false;
        }
        //else if (trashList.Contains(other.gameObject))
        ////if item is not a rawmater, burn it (destroy)
        //{
        //    Debug.Log("Take it awayy");
        //    ItemOnAnvil = false;
        //}
    }
    public void ClearList()
    {
        trashList.Clear();
        RMaterialList.Clear();
    }
}
