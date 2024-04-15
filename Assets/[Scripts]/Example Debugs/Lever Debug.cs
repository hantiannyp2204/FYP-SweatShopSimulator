using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class LeverDebug : MonoBehaviour
{
    public XRLever lever;
    public TMP_Text debugTxt;

    private void Start()
    {
        debugTxt.text = "Lever not connected";
    }
    // Update is called once per frame
    void Update()
    {
        if (lever == null) return;
        debugTxt.text = "Lever Value:\n" + lever.value.ToString();
    }
}
