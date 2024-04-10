using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrMachineItemCollider : MonoBehaviour
{
    private List<Item> _productList = new();

    private void OnTriggerEnter(Collider other)
    {
        //check if it has Item Script, else ignore
        Item itemComponenet = other.GetComponent<Item>();
        if (itemComponenet == null) return;
        _productList.Add(itemComponenet);
    }
    private void OnTriggerExit(Collider other)
    {
        //check if it has Item Script, else ignore
        Item itemComponenet = other.GetComponent<Item>();
        if (itemComponenet == null) return;
        _productList.Remove(itemComponenet);
    }

    public void ClearProductList()
    {
        _productList.Clear();
    }
    public List<Item> GetProductList()
    {
        return _productList;
    }
}
