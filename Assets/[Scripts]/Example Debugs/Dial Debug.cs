using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class DialDebug : MonoBehaviour
{
    public XRKnob knob;
    public TMP_Text debugTxt;

    private void Start()
    {
        debugTxt.text = "Dial not connected";
    }
    // Update is called once per frame
    void Update()
    {
        if(knob == null) return;
        debugTxt.text = "Dial Value:\n" + knob.value.ToString();
    }
}
