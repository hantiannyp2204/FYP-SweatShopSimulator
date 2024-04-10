using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrMachineItemCollider : MonoBehaviour
{
    [SerializeField] private MachineShredder shredder;
    public string layerCheck;
    private int _layer;
    public bool isCollided = false;
    private Item _product;

    private Collider _collider;
    private void Start()
    {
        _layer = LayerMask.NameToLayer(layerCheck);
        _collider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isCollided)
        {
            if (other.gameObject.layer == _layer)
            {
                isCollided = true;
                _product = other.GetComponent<Item>();
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
        if (isCollided)
        {
            if (_product != null)
            {   
                _product.transform.position = _collider.transform.position;
            }
        }
    }

    public Item GetProduct()
    {
        return _product;
    }
}
