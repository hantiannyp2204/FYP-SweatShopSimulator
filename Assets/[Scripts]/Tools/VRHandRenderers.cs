using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRHandRenderers : MonoBehaviour
{

    [SerializeField] private GameObject handModel;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private XRDirectInteractor grabInteractor;
    [SerializeField] private TMP_Text handHoverItemName;
    public void DisableHandRender()
    {
        handModel.SetActive(false);
        rayInteractor.allowHover = false;
        grabInteractor.allowHover = false;
    }
    public void EnableHandRender()
    {
        handModel.SetActive(true);
        rayInteractor.allowHover = true;
        grabInteractor.allowHover = true;
    }
    public void SetItemHoverName(string itemName)
    {
        handHoverItemName.text = itemName;
    }
    public void ResetItemHoverName()
    {
        handHoverItemName.text = null;
    }
    public bool GetActive() => handModel.active;
}
