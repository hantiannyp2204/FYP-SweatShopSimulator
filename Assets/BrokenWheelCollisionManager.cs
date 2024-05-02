using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BrokenWheelCollisionManager : MonoBehaviour
{
    [SerializeField] private MachineShredder shredder;

    private Collider _collider;
    private XRSocketInteractor _socket;

    private void Start()
    {
        _collider = transform.GetComponent<Collider>();
        _socket = transform.GetComponent<XRSocketInteractor>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag != "Wheel")
        {
            return; // if not wheel layer then return
        }
        
        var haveWheelManager = other.gameObject.GetComponent<WheelManager>();
        if (haveWheelManager == null)
        {
            return;
        }
        //haveWheelManager.SetWheelCurrState(WheelStatus.WORKING);
        //if (_socket.GetOldestInteractableSelected() != null) // something is inside socket
        //{
        //    //_collider.isTrigger = false;
        //    var grabManager = other.gameObject.GetComponentInChildren<XRVelocityRayGrab>();
        //    if (grabManager.enabled)
        //    {
        //        grabManager.enabled = false;
        //    }
        //}

        shredder.enableWheelEvent.Invoke();
        Destroy(other.gameObject);
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
