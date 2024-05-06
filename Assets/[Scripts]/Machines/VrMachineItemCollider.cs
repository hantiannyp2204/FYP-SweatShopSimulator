using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrMachineItemCollider : MonoBehaviour
{
    [SerializeField] private MachineShredder shredder;
    public ShredderMouthCollision mouthHandler;
    public List<Item> _productList = new();
    public Item _tracker;
    private Item _saver;
    private bool _collided = false;

    private void Start()
    {
        if (shredder != null)
        {
            shredder.finishedShreddingEvent.AddListener(DestroyProduct);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _collided = true;
        //check if it has Item Script, else ignore
        Item itemComponenet = other.GetComponent<Item>();
        if (itemComponenet == null)
        {
            return;
        }
        _tracker = itemComponenet;
        _saver = itemComponenet;
        _productList.Add(itemComponenet);
    }
    private void OnTriggerExit(Collider other)
    {
        _collided = false;
        //check if it has Item Script, else ignore
        Item itemComponenet = other.GetComponent<Item>();
        if (itemComponenet == null) return;
        _productList.Remove(itemComponenet);
    }

    public void ClearProductList()
    {
        _productList.Clear();
    }
    public List<Item> GetProductList()
    {
        return _productList;
    }

    public bool CheckIsProduct()
    {
        return _tracker.Data.productContainable.Count == 0;
    }

    private void Update()
    {
        if (_collided)
        {
            if (mouthHandler.mouth1 == null || mouthHandler.mouth2 == null)
            if (!mouthHandler.mouth1.GetComponent<Animator>().GetBool("isActivated") && !mouthHandler.mouth2.GetComponent<Animator>().GetBool("isActivated")) return; // check if mouth is open 
        }
    }

    public void DestroyProduct()
    {
        Destroy(_saver);
    }
}

