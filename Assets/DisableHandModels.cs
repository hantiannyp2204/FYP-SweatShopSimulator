using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableHandModels : MonoBehaviour
{

    [SerializeField] private GameObject handModel;

    public void DisableHandRender()
    {
        handModel.SetActive(false);
    }
    public void EnableHandRender()
    {
        handModel.SetActive(true);
    }
   public bool GetActive() => handModel.active;
}
