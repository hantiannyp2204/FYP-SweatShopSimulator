using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BrokenWheelCollisionManager : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogue;
    [SerializeField] private MachineShredder shredder;
    private bool _wheelFixed = false;

    public MachineShredder GetShredder()
    {
        return shredder;
    }

    public bool IsWheelFixed()
    {
        return _wheelFixed;
    }
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

        _wheelFixed = true;
        shredder.SetWheelCurrState(WheelStatus.WORKING);


        if (!dialogue.isDialogueActive && dialogue != null) // dont enable in tutorial
        { 
            shredder.SetUpWheelProbability();
        }
        shredder.enableWheelEvent.Invoke();
        Destroy(other.gameObject);
    }
}
