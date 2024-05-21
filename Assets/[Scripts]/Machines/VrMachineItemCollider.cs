using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VrMachineItemCollider : MonoBehaviour
{
    [SerializeField] private MachineShredder shredder;
    public ShredderMouthCollision mouthHandler;
    public List<GeneralItem> _productList = new();
    public GeneralItem _tracker;
    private GeneralItem _saver;
    private bool _collided = false;
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        if (shredder != null)
        {
            shredder.finishedShreddingEvent.AddListener(DestroyProduct);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _collided = true;
        mouthHandler.GetComponent<Collider>().isTrigger = false;

        //check if it has Item Script, else ignore
        GeneralItem itemComponenet = other.GetComponent<GeneralItem>();
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
        GeneralItem itemComponenet = other.GetComponent<GeneralItem>();
        if (itemComponenet == null) return;
        _productList.Remove(itemComponenet);
    }

    public void ClearProductList()
    {
        _productList.Clear();
    }
    public List<GeneralItem> GetProductList()  
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
            if (mouthHandler.mouth1 == null || mouthHandler.mouth2 == null) return;
            if (!mouthHandler.mouth1.GetComponent<Animator>().GetBool("isActivated") && !mouthHandler.mouth2.GetComponent<Animator>().GetBool("isActivated")) return; // check if mouth is open 
        }
    }

    public void DestroyProduct()
    {
        mouthHandler.GetComponent<Collider>().isTrigger = true;
        Destroy(_saver);
    }
}

