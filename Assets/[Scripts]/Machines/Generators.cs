using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Generators : MonoBehaviour
{
    public GameObject ScrapPrefab; // Assign this in the inspector

    private XRBaseInteractor interactorUsingThis;

    //public TMP_Text debugtxt;

    protected void OnEnable()
    {
        // Get the interactable component and subscribe to the select entered event
        GetComponent<XRBaseInteractable>().selectEntered.AddListener(OnGrabbed);
    }

    protected void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        GetComponent<XRBaseInteractable>().selectEntered.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        //if(debugtxt != null)
        //{
        //    debugtxt.text = "grabbed";
        //}

        interactorUsingThis = args.interactor;
        GenerateAndGrabMetalScrap();
    }

    private void GenerateAndGrabMetalScrap()
    {
        if (ScrapPrefab != null && interactorUsingThis != null)
        {
            // Instantiate the metal scrap prefab
            GameObject ScrapInstance = Instantiate(ScrapPrefab, interactorUsingThis.transform.position, Quaternion.identity);

            // Force the interactor to select the newly created metal scrap
            Debug.Log("OBTAINED");
            interactorUsingThis.GetComponent<XRBaseInteractor>().StartManualInteraction(ScrapInstance.GetComponent<IXRSelectInteractable>());
            ScrapInstance.AddComponent<GeneratorGeneratedItem>().SetHandInteractorAndAnimator(interactorUsingThis);

            //disable the hand
            DisableHandModels interactorHandModel = interactorUsingThis.GetComponent<DisableHandModels>();
            if (interactorHandModel!=null && interactorHandModel.GetActive())
            {
                interactorHandModel.DisableHandRender();
            }
        }
    }
}
