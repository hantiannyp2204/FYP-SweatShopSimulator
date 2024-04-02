using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRButton : MonoBehaviour
{
    [SerializeField] float threshold = .1f;
    [SerializeField] private float deadZone = 0.025f;
    [SerializeField] private bool isToggle = false;

    private bool toggled = false;
    private bool isPressed = false;
    private Vector3 startPos;
    private ConfigurableJoint joint;

    private void Start()
    {
        startPos = transform.localPosition;
        joint = GetComponent<ConfigurableJoint>();
    }

    private void Update()
    {
        if (!isPressed && GetValue() + threshold >= 1)
        {
            OnPressed();
        }
        else if (isPressed && GetValue() - threshold <= 0) // Modified condition for release
        {
            OnRelease();
        }

        // Prevent the button from moving "upwards" beyond its starting position
        if (transform.localPosition.y > startPos.y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, startPos.y, transform.localPosition.z);
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

    public void OnPressed()
    {
        if (isToggle)
        {
            // Toggle mode
            if (!toggled)
            {
                Debug.Log("PRESSED - Toggle On");
                toggled = true; // Toggle is now on
            }
            else
            {
                Debug.Log("Release - Toggle Off");
                toggled = false; // Toggle is now off
            }
        }
        else
        {
            // Non-toggle mode
            Debug.Log("PRESSED");
        }
        isPressed = true; // Mark as pressed to prevent continuous triggering in Update
    }

    public void OnRelease()
    {
        if (!isToggle)
        {
            Debug.Log("Release");

        }
        isPressed = false; // Reset the press state
    }
}
