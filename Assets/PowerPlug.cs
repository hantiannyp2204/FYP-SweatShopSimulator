using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
public class PowerPlug : MonoBehaviour
{
    [SerializeField] XRGrabInteractable _DropPlug;
    public Transform Start_Plug;
    public Transform End_Plug;
    //public TMP_Text Text;

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

    public void Update()
    {
        DropPowerPlug();
    }
    public void DropPowerPlug()
    {
        // Shoot a raycast from Start_Plug to End_Plug
        RaycastHit hit;
        if (Physics.Raycast(Start_Plug.position, End_Plug.position - Start_Plug.position, out hit))
        {
            // Check if the distance between Start_Plug and End_Plug is more than 1.85 units
            float distance = Vector3.Distance(Start_Plug.position, End_Plug.position);
            if (distance > 2f)
            {
                XRInteractionManager interactionManager = _DropPlug.interactionManager;
                interactionManager.CancelInteractableSelection(_DropPlug);
                // Do something if the distance is more than 1.85 units
                Debug.Log("Distance between Start_Plug and End_Plug is more than 1.85 units.");
                //Text.text = "Too long";
            }
            else
            {
                //Text.text = "Inside length";
            }
        }
    }
}
