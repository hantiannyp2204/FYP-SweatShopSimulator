using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrMachineItemCollider : MonoBehaviour
{
    public string layerCheck;
    private int _layer;
    private bool _isCollided = false;
    private GameObject _product;

    private Collider _collider;
    private void Start()
    {
        _layer = LayerMask.NameToLayer(layerCheck);
        _collider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_isCollided)
        {
            if (other.gameObject.layer == _layer)
            {
                _isCollided = true;
                _product = other.gameObject;
                Rigidbody rb = _product.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                _product.transform.position = _product.transform.position;
            }
        }
    }

    private void Update()
    {
        if (_isCollided)
        {
            _product.transform.position = _collider.transform.position;
        }
    }
}
