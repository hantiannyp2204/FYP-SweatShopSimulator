using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRequest : MonoBehaviour
{
    ItemData requestedItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetRequestedItem(ItemData newRequestedItem) => requestedItem = newRequestedItem;

}