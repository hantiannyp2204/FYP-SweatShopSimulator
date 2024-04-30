using UnityEngine;

public class PowerPlug : MonoBehaviour
{
    public LayerMask socketLayer; // Set this in the inspector to the layer you want the plug to stick to
    private bool isStuckInSocket = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is on the specified socket layer
        if ((socketLayer.value & 1 << other.gameObject.layer) != 0)
        {
            // If the plug is not already stuck in a socket
            if (!isStuckInSocket)
            {
                // Set the plug as kinematic so it sticks to the socket
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                // Set the flag indicating that the plug is now stuck in a socket
                isStuckInSocket = true;
            }
        }
    }

    public bool IsStuckInSocket()
    {
        return isStuckInSocket;
    }
}
