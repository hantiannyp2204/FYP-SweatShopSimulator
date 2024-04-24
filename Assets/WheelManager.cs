using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Content.Interaction;
public class WheelManager : MonoBehaviour
{
    [SerializeField] private MachineShredder shredder;
    [SerializeField] private VrMachineItemCollider _check;

    public UnityEvent canStartShredding;

    private void Start()
    {
        if (canStartShredding == null)
        {
            canStartShredding = new UnityEvent();
        }

        shredder.lever.GetComponentInChildren<XRLever>().onLeverDeactivate.AddListener(ActivateWheel);
        //_check = shredder.GetComponentInChildren<VrMachineItemCollider>();
    }
    private void ActivateWheel()
    {
        Debug.Log("it is : " + _check.CheckIsProduct());
        if (_check.CheckIsProduct())
        {
            shredder.shredderFuelText.text = "Not A Product!";
            return;
        }

        //if list is empty, there is nothing in collider
        if (shredder.shredderItemCollider.GetProductList().Count == 0)
        {
            shredder.shredderFuelText.text = "Nothing to Shred";
            return;
        }

        shredder.initShredding = true;
        shredder.wheel.SetActive(true);
        canStartShredding.Invoke();
    }
}
