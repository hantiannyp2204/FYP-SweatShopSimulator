using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandWatch : MonoBehaviour
{
    [SerializeField] HandPresencePhysics leftHandPhysicalTransform;
    [SerializeField] TMP_Text clockTxt;
    private bool hasEntered = false;

    private void Start()
    {
        SetTextAlpha(0);
    }

    private void SetTextAlpha(float alpha)
    {
        Color color = clockTxt.color;
        color.a = alpha;
        clockTxt.color = color;
    }

    public void UpdateWatchAlpha()
    {
        float zRotation = leftHandPhysicalTransform.transform.eulerAngles.z;

        if (zRotation > 180)
        {
            zRotation -= 360;
        }

        // Check if the rotation is within the range of 130 to 230 degrees
        if (zRotation >= (270-180) && zRotation <= 270)
        {
            if (!hasEntered)
            {
                OnEnter();
                hasEntered = true;
            }
        }
        else
        {
            if (hasEntered)
            {
                OnExit();
                hasEntered = false;
            }
        }
    }

    private void OnEnter()
    {
        Debug.Log("Enter");
        LeanTween.value(clockTxt.gameObject, SetTextAlpha, 0, 1, 0.2f).setEase(LeanTweenType.easeInSine);
    }

    private void OnExit()
    {
        Debug.Log("Exit");
        LeanTween.value(clockTxt.gameObject, SetTextAlpha, 1, 0, 0.2f).setEase(LeanTweenType.easeOutSine);
    }
}
