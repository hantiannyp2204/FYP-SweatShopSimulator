using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Content.Interaction;

public enum WheelStatus
{
    WORKING, 
    BROKEN
}

public class WheelManager : MonoBehaviour
{   
    [HideInInspector]public UnityEvent canStartShredding;
    [SerializeField] private MachineShredder shredder;
    [SerializeField] private VrMachineItemCollider _check;


    public ProbabilityManager chance;
    [Header("FEEDBACK")]
    [SerializeField] private FeedbackEventData e_pulledLever;
    public FeedbackEventData e_wheelturning;
    
    private void Awake()
    {
        chance = GetComponent<ProbabilityManager>();
        if (canStartShredding == null)
        {
            canStartShredding = new UnityEvent();
        }

        shredder.lever.GetComponentInChildren<XRLever>().onLeverDeactivate.AddListener(ActivateWheel);
    }
    private void ActivateWheel()
    {
        e_pulledLever?.InvokeEvent(transform.position, Quaternion.identity, transform);

        if (shredder.currWheelStatus != WheelStatus.WORKING)
        {
            return;
        }
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
