using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualRequestText : MonoBehaviour
{
    public Camera playerCam;
    private void Update()
    {
        transform.LookAt(transform.position + playerCam.transform.rotation * Vector3.forward, playerCam.transform.rotation * Vector3.up);
    }
}
