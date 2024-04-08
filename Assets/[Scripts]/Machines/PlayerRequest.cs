using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRequest : MonoBehaviour
{
    ItemData requestedItem;

    public void SetRequestedItem(ItemData newRequestedItem) => requestedItem = newRequestedItem;

}
