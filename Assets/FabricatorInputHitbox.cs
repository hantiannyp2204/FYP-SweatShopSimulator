using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FabricatorInputHitbox : MonoBehaviour
{
    public List<Item> scrapList = new();
    List<GameObject> destroyList = new();

    public FabricatorCrafting fabricator;
    public List<Item> GetScrapList() => scrapList;
    public List<GameObject> GetDestroyList() => destroyList;
    private void OnTriggerEnter(Collider other)
    {
        //check all gameobject in collider containing Scrap.cs
        Item scrapComponent = other.GetComponent<Item>();
        if (scrapComponent != null)
        {
            scrapList.Add(scrapComponent);
            
        }
        else
        //if item is not a scrap, burn it (destroy)
        {
            destroyList.Add(other.gameObject);
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Item scrapComponent = other.GetComponent<Item>();
        if (scrapComponent != null && scrapList.Contains(scrapComponent))
        {
            scrapList.Remove(scrapComponent);
        }
        else if(destroyList.Contains(other.gameObject))
        //if item is not a scrap, burn it (destroy)
        {
            destroyList.Remove(other.gameObject);
        }
    }
    public void ClearList()
    {
        destroyList.Clear();
        scrapList.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Scrap List:");
            foreach (Item Item in scrapList)
            {
                Debug.Log(Item.name);
            }
        }
    }
}
