using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWheelCollisionManager : MonoBehaviour
{
    [SerializeField] private LayerMask layer;

    private int _wheelLayer;
    private Collider _collider;
    private void Start()
    {
        _wheelLayer = LayerMask.NameToLayer(layer.ToString());
        _collider = transform.GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _wheelLayer)
        {
            Debug.Break();
            return; // if not wheel layer then return
        }

        other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        other.gameObject.GetComponent<Rigidbody>().useGravity = false; // reset physic changes

        other.gameObject.transform.position = other.gameObject.transform.position;

        // disable the
        //_collider.enabled = false;
    }
}
