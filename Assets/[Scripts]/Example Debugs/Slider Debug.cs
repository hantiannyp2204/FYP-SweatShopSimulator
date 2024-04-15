using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SliderDebug : MonoBehaviour
{
    public XRSlider slider;
    public TMP_Text debugTxt;

    private void Start()
    {
        debugTxt.text = "Lever not connected";
    }
    // Update is called once per frame
    void Update()
    {
        if (slider == null) return;
        debugTxt.text = "Slider Value:\n" + slider.value.ToString();
    }
}
