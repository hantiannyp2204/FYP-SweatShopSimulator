using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableHandModels : MonoBehaviour
{

    [SerializeField] private GameObject handModel;
    [SerializeField] private XRRayInteractor rayInteractor;
    public void DisableHandRender()
    {
        handModel.SetActive(false);
        //rayInteractor.enabled = false;
    }
    public void EnableHandRender()
    {
        handModel.SetActive(true);
        //rayInteractor.enabled = true;
    }
   public bool GetActive() => handModel.active;
}
