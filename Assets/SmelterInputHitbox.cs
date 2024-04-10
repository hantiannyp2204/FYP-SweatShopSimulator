using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmelterInputHitbox : MonoBehaviour
{
    List<Scrap> scrapList = new();
    List<GameObject> destroyList = new();

    public List<Scrap> GetScrapList() => scrapList;
    public List<GameObject> GetDestroyList() => destroyList;
    private void OnTriggerEnter(Collider other)
    {
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
        Scrap scrapComponent = other.GetComponent<Scrap>();
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
}
