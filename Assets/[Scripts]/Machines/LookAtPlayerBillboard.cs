using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayerBillboard : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private Quaternion rotationOffset = Quaternion.identity;
    private void Start()
    {
        // Find the main camera, which should be tagged appropriately
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            // Make the text look at the camera
            transform.LookAt(cameraTransform.transform);
            // Adjust the rotation to face the camera directly
            transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.transform.position);

            // Apply the additional rotation offset
            transform.rotation *= rotationOffset;
        }
    }

    public void SetRotationOffset(Quaternion newOffset)
    {
        rotationOffset = newOffset;
    }
}
