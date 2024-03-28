using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRButton : MonoBehaviour
{
    [SerializeField] float threshold = .1f;
    [SerializeField] float deadZone = 0.025f;

    bool isPressed = false;
    Vector3 startPos;
    ConfigurableJoint joint;
    private void Start()
    {
        startPos = transform.localPosition;
        joint = GetComponent<ConfigurableJoint>();
    }

    private void Update()
    {
        if(!isPressed && GetValue() + threshold >= 1) {
            OnPressed();
        }
        if(isPressed && GetValue() - threshold <= 0)
        {
            OnRelease();
        }
    }

    private float GetValue()
    {
        var value = Vector3.Distance(startPos, transform.localPosition) / joint.linearLimit.limit;
        if (Math.Abs(value) < deadZone)
        {
            value = 0;
        }
        return Mathf.Clamp(value, -1, 1);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!isPressed)
    //    {
    //        button.transform.localPosition = new Vector3(0, -0.07f, 0);
    //        presser = other.gameObject;
    //        isPressed = true;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.gameObject == presser)
    //    {
    //        button.transform.localPosition=new Vector3(0, 0, 0);
    //        isPressed = false;
    //    }
    //}
    public void OnPressed()
    {
        isPressed = true;
    }
    public void OnRelease()
    {
        isPressed = false;
    }
}
