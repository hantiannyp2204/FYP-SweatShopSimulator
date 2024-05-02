using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotItemPlate : MonoBehaviour
{
    public GameObject machineDestination;
    [SerializeField] private RobotMovement robotMovement;
    [SerializeField] private Item _itemToSend;


    public RequestBox box;
    private Transform _tableParent;
    //private RequestBox _requestBox;

    public CustomerTable table;


    private void Start()
    {
        _tableParent = machineDestination.GetComponentInParent<Transform>(); // store gameobject
        //_requestBox = _tableParent.GetComponentInChildren<RequestBox>();
     
        //if (_requestBox == null)
        //{
        //    Debug.Break();
        //    return;
        //}

        if (_tableParent == null)
        {
            return;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Item isItem = other.transform.GetComponent<Item>();
        if (isItem == null)
        {
            return;
        }
        _itemToSend = isItem;
        _itemToSend.gameObject.GetComponent<Rigidbody>().useGravity = false;
        _itemToSend.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        _itemToSend.transform.SetParent(transform , true);

        robotMovement.SetNewWaypoint(machineDestination);
    }

    public Item GetRobotHoldingItem()
    {
        return _itemToSend;
    }

    //public RequestBox GetRequestBox()
    //{
    //    return _requestBox;
    //}
}
