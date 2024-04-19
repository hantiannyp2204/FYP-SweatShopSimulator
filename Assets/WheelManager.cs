using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Content.Interaction;
public class WheelManager : MonoBehaviour
{
    [SerializeField] private MachineShredder shredder;

    public UnityEvent canStartShredding;

    private void Start()
    {
        if (canStartShredding == null)
        {
            canStartShredding = new UnityEvent();
        }

        shredder.lever.GetComponentInChildren<XRLever>().onLeverDeactivate.AddListener(ActivateWheel);
    }
    private void ActivateWheel()
    {
        //if list is empty, there is nothing in collider
        if (shredder.shredderItemCollider.GetProductList().Count == 0)
        {
            shredder.shredderFuelText.text = "Nothing to Shred";
            return;
        }

        shredder.wheel.SetActive(true);
        canStartShredding.Invoke();
    }
}
