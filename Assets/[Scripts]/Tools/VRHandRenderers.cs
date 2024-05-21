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
    private void Start()
    {
        ResetItemHoverName();
    }
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
        //animate pop up
        LeanTween.scale(handHoverItemName.gameObject, new Vector3(1, 1, 1), .2f).setEase(LeanTweenType.easeInSine);
        LeanTween.moveLocalY(handHoverItemName.gameObject, 0, .2f).setEase(LeanTweenType.easeInSine);
    }
    public void ResetItemHoverName()
    {
        handHoverItemName.text = null;
        LeanTween.scale(handHoverItemName.gameObject, Vector3.zero, 0);
        LeanTween.moveLocalY(handHoverItemName.gameObject, -0.025f, 0);
    }
    public bool GetActive() => handModel.active;
}
