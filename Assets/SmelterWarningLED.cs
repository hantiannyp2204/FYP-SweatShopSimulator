using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmelterWarningLED : MonoBehaviour
{
    [SerializeField] private Material turnedOn;
    [SerializeField] private Material turnedOff;
    [SerializeField] private FeedbackEventData e_warningSound;
    [SerializeField] private MeshRenderer meshRenderer;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        WarningLEDOff();
    }
    public void WarningLEDOn()
    {
        meshRenderer.material = turnedOn;
        e_warningSound?.InvokeEvent(transform.position, Quaternion.identity);
    }
    public void WarningLEDOff()
    {
        meshRenderer.material = turnedOff;
    }
}
