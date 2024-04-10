using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRButton : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] float threshold = .1f;
    [SerializeField] private float deadZone = 0.025f;
    [SerializeField] private bool usesToggle = false;

    private bool toggled = false;
    private bool isPressed = false;
    private Vector3 startPos;
    private ConfigurableJoint joint;

    protected void Start()
    {
        startPos = button.transform.localPosition;
        joint = button.GetComponent<ConfigurableJoint>();
    }

    private void Update()
    {
        if (!isPressed && GetValue() + threshold >= 1)
        {
            OnPressed();
        }
        else if (isPressed && GetValue() - threshold <= 0) 
        {
            OnRelease();
        }

        // Prevent the button from moving "upwards" beyond its starting position
        if (button.transform.localPosition.y > startPos.y)
        {
            button.transform.localPosition = new Vector3(button.transform.localPosition.x, startPos.y, button.transform.localPosition.z);
        }
    }

    private float GetValue()
    {
        var value = Vector3.Distance(startPos, button.transform.localPosition) / joint.linearLimit.limit;
        if (Math.Abs(value) < deadZone)
        {
            value = 0;
        }
        return Mathf.Clamp(value, -1, 1);
    }

    void OnPressed()
    {
        if (usesToggle)
        {
            // Toggle mode
            if (!toggled)
            {
                ToggleOnFunction();
                toggled = true; // Toggle is now on
            }
            else
            {
                ToggleOffFunction();
                toggled = false; // Toggle is now off
            }
        }
        PressedFunction();
        isPressed = true; // Mark as pressed to prevent continuous triggering in Update
    }

    void OnRelease()
    {
        ReleasedFunction();
        isPressed = false; // Reset the press state
    }
    public virtual void PressedFunction()
    {
        Debug.Log("PRESSED");
    }
    public virtual void ReleasedFunction()
    {
        Debug.Log("RELEASED");
    }
    public virtual void ToggleOnFunction()
    {
        Debug.Log("Toggle On");
    }
    public virtual void ToggleOffFunction()
    {
        Debug.Log("Toggle Off");
    }
}
