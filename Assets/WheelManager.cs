using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Content.Interaction;


public enum WheelStatus
{
    WORKING, 
    BROKEN,
    NOT_ATTACHED
}

public class WheelManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent enableUnusedWheelPhysics;
    [HideInInspector] public UnityEvent canStartShredding;
    [SerializeField] private MachineShredder shredder;
    [SerializeField] private VrMachineItemCollider _check;

    public ProbabilityManager chance; 
    [Header("FEEDBACK")]
    [SerializeField] private FeedbackEventData e_pulledLever;
    public FeedbackEventData e_wheelturning;
    private XRKnob _wheel;

    public WheelStatus _status;
    private Rigidbody _rb;

    private int _wheelLayer;
    private void Start()
    {
        chance = GetComponent<ProbabilityManager>();
        if (canStartShredding == null)
        {
            canStartShredding = new UnityEvent();
        }

        shredder.lever.GetComponentInChildren<XRLever>().onLeverDeactivate.AddListener(ActivateWheel);

        _wheel = GetComponent<XRKnob>();
        
        _rb = transform.GetComponent<Rigidbody>();

        _wheelLayer = LayerMask.NameToLayer("Wheel");

        if (shredder.GetAttachedWheel() != this)
        {
            SetWheelCurrState(WheelStatus.NOT_ATTACHED);
        }
    }

    private void ActivateWheel()
    {
        _wheel.enabled = true;
        e_pulledLever?.InvokeEvent(transform.position, Quaternion.identity, transform);

        //if (shredder.currWheelStatus != WheelStatus.WORKING)
        //{
        //    return;
        //}

        if (shredder.GetWheelHandler().GetWheelCurrState() != WheelStatus.WORKING)
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

    public void SetWheelCurrState(WheelStatus STATUS)
    {
        _status = STATUS;
    }

    public WheelStatus GetWheelCurrState()
    {
        return _status;
    }
}
