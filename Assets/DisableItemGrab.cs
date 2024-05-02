using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
public class DisableItemGrab : MonoBehaviour
{
    ConfigurableJoint drawerJoint;
    float openPositionThreshold; // Threshold for determining if drawer is open
    float closedPositionThreshold; // Threshold for determining if drawer is closed
    bool isDrawerOpen;
    [SerializeField] Generators generator;

    void Update()
    {
        if (IsDrawerOpen())
        {
            // Drawer is open, handle accordingly
            // For example, enable interaction with items in the drawer
        }
        else
        {
            // Drawer is closed, handle accordingly
            // For example, disable interaction with items in the drawer
        }
    }

    void IsDrawerOpen()
    {
        // Get current position of the configurable joint along the x-axis
        float currentPosition = drawerJoint.currentPosition.x;

        // Check if drawer is beyond open position threshold
        if (currentPosition >= openPositionThreshold)
        {
            isDrawerOpen = true;
        }
        else if (currentPosition <= closedPositionThreshold)
        {
            isDrawerOpen = false;
        }

        return isDrawerOpen;
    }
}
