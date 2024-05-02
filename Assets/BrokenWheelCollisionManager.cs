using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BrokenWheelCollisionManager : MonoBehaviour
{
    [SerializeField] private MachineShredder shredder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag != "Wheel")
        {
            return; // if not wheel layer then return
        }
        
        var haveWheelManager = other.gameObject.GetComponent<WheelManager>(); // check if wheel has wheel manager
        if (haveWheelManager == null)
        {
            return;
        }

        shredder.SetWheelCurrState(WheelStatus.WORKING);
        shredder.enableWheelEvent.Invoke();
        Destroy(other.gameObject);
    }
}
