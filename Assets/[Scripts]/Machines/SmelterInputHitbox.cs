using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmelterInputHitbox : MonoBehaviour
{
    [SerializeField] LayerMask ignoreLayers;

    List<Scrap> scrapList = new();
    List<GameObject> destroyList = new();

    public LayerMask GetIgnoreLayer() => ignoreLayers;
    public List<Scrap> GetScrapList() => scrapList;
    public List<GameObject> GetDestroyList() => destroyList;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Item>() == null)
        {
            return;
        }
        //dont add player items
        // Check if the other collider's layer is in the ignoreLayers mask
        // Check if the other collider's layer is in the ignoreLayers mask
        if (ignoreLayers == (ignoreLayers | (1 << other.gameObject.layer)))
        {
            return; // If it is, ignore this collider and return immediately
        }
        //check all gameobject in collider containing Scrap.cs
        Scrap scrapComponent = other.GetComponent<Scrap>();
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
        if (other.GetComponent<Item>() == null)
        {
            return;
        }
        Scrap scrapComponent = other.GetComponent<Scrap>();
        if (scrapComponent != null && scrapList.Contains(scrapComponent))
        {
            scrapList.Remove(scrapComponent);
        }
        else if(destroyList.Contains(other.gameObject))
        {
            destroyList.Remove(other.gameObject);
        }
    }
    public void ClearList()
    {
        destroyList.Clear();
        scrapList.Clear();
    }

    public bool IsListEmpty()
    {
        return scrapList == null;
    }
}
