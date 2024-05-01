using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BrokenWheelCollisionManager : MonoBehaviour
{
    [SerializeField] private MachineShredder shredder;
    private int _wheelLayer;
    private Collider _collider;
    private XRSocketInteractor _socket;

    private void Start()
    {
        _wheelLayer = LayerMask.NameToLayer("Wheel");
        _collider = transform.GetComponent<Collider>();
        _socket = GetComponent<XRSocketInteractor>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _wheelLayer)
        {
            return; // if not wheel layer then return
        }
         var haveWheelManager = other.gameObject.GetComponentInChildren<WheelManager>();
         if (haveWheelManager == null) return;
         haveWheelManager.SetWheelCurrState(WheelStatus.WORKING);
        //if (_socket.GetOldestInteractableSelected().transform.gameObject != null) // something is inside socket
        //{
        //    //_collider.isTrigger = false;
        //    var grabManager = other.gameObject.GetComponentInChildren<XRVelocityRayGrab>();
        //    if (grabManager.enabled)
        //    {
        //        grabManager.enabled = false;
        //    }
        //}
        //other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        //other.gameObject.GetComponent<Rigidbody>().useGravity = false; // reset physic changes

        //other.gameObject.transform.position = other.gameObject.transform.position;
    }

    private void Update()
    {
        //if (_socket.GetOldestInteractableSelected().transform.gameObject != null) // inside socket
        //{
        //    _collider.isTrigger = false;
        //    //_socket.GetOldestInteractableSelected().transform.GetComponent<XRVelocityRayGrab>().enabled = false;
            
        //    shredder.SetAttachedWheel(_socket.GetOldestInteractableSelected().transform.gameObject);
        //    _socket.enabled = false;
        //    _collider.enabled = false;
        //}
        //else
        //{
        //    Debug.Log("H");
        //}
    }
}
