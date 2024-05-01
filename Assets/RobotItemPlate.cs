using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotItemPlate : MonoBehaviour
{
    [SerializeField] private RobotMovement robotMovement;
    [SerializeField] private GameObject machineDestination;
    [SerializeField] private Item _itemToSend;


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("cheebye: " + other.gameObject.name);
        Item isItem = other.transform.GetComponent<Item>();
        if (isItem == null)
        {
            return;
        }
        _itemToSend = isItem;
        isItem.transform.SetParent(transform);

        robotMovement.SetNewWaypoint(machineDestination);
    }

    public Item GetRobotHoldingItem()
    {
        return _itemToSend;
    }
}
