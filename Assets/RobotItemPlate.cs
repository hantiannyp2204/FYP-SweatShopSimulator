using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotItemPlate : MonoBehaviour
{
    public GameObject machineDestination;
    [SerializeField] private RobotMovement robotMovement;
    [SerializeField] private GeneralItem _itemToSend;

    public RequestBox box;
    private Transform _tableParent;

    public CustomerTable table;

    private Collider _collider;

    public Collider GetCollider()
    {
        return _collider;
    }
    private void Start()
    {
        _tableParent = machineDestination.GetComponentInParent<Transform>(); // store gameobject

        if (_tableParent == null)
        {
            return;
        }

        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        GeneralItem isItem = other.transform.GetComponent<GeneralItem>();
        if (isItem == null)
        {
            return;
        }

        _itemToSend = isItem;
        _itemToSend.gameObject.GetComponent<Rigidbody>().useGravity = false;
        _itemToSend.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        _itemToSend.transform.SetParent(transform , true);
    }

    public GeneralItem GetRobotHoldingItem()
    {
        return _itemToSend;
    }
}
