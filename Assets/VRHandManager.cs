using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public class VRHandManager : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    [SerializeField] Animator handAnimator;
    [SerializeField] TMP_Text Debug;
    List<GameObject> currentlyTouching = new();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        float gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);
        handAnimator.SetFloat("Grip", gripValue);

        if(gripValue >= 0.8f)
        {
            //Grabbing
            Grab();
        }
    }
    void Grab()
    {
        foreach(GameObject go in currentlyTouching)
        {
            //if its player, continue
            if(go.name == "VR Player")
            {
                continue;
            }
            else
            {
                Debug.text = go.name;
                break;
            }
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Debug == null) return;
        currentlyTouching.Add(other.gameObject);
      
    }
    private void OnTriggerExit(Collider other)
    {
        if (Debug == null) return;
        currentlyTouching.Remove(other.gameObject);
    }
}
