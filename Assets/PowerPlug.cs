using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
public class PowerPlug : MonoBehaviour
{
    [SerializeField] XRGrabInteractable _DropPlug;
    public Transform Start_Plug;
    public Transform End_Plug;
    public bool isStuckInSocket;
    public PowerForFab _powerForFab;
    public Transform _Socket_Point;
    public Transform _Calble_Point;
    [Header("Feedback Events")]
    [SerializeField] private FeedbackEventData e_PowerOn;
    [SerializeField] private Transform PowerOnTransform;

    //public TMP_Text Text;
    public LayerMask socketLayer; // Set this in the inspector to the layer you want the plug to stick to

    public void Start()
    {
       
    }
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
                Start_Plug.position = new Vector3(_Socket_Point.position.x, _Socket_Point.position.y, _Socket_Point.position.z);
                // Reset the rotation of Start_Plug to zero
                StartCoroutine(ResetRotationAfterDelay(1f));
                StartCoroutine(ResetRotationAfterDelays(1f));
                _powerForFab.Isin = true;
                // Set the flag indicating that the plug is now stuck in a socket
                isStuckInSocket = true;
                if (_powerForFab._CurrentPower <= 0)
                {
                    e_PowerOn?.InvokeEvent(transform.position, Quaternion.identity, PowerOnTransform);
                    _powerForFab.RandomPower();
                }
                _DropPlug.enabled = false;
                
            }
        }
    }

    IEnumerator ResetRotationAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Reset the rotation of Start_Plug to match _Socket_Point
        Start_Plug.rotation = Quaternion.identity;
       

    }

    IEnumerator ResetRotationAfterDelays(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        _Calble_Point.rotation = Quaternion.Euler(0f, 90f, 0f);

    }
    private void OnTriggerExit(Collider other)
    {
        // Check if the collided object is not on the specified socket layer
        if ((socketLayer.value & 1 << other.gameObject.layer) == 0)
        {
            // Set the plug as non-kinematic so it can move freely
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Set the flag indicating that the plug is no longer stuck in a socket
            isStuckInSocket = false;
            _DropPlug.enabled = true;



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
