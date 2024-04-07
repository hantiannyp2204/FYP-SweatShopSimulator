using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VRHandManager;

public class HandPresensePhysics : MonoBehaviour
{
    public Transform target;
    private Rigidbody rb;
    [SerializeField] private float zRotationOffset;
    private bool isLocked = false; // Flag to control locking behavior

    // Optional: To precisely control the locked position and rotation
    private Vector3 lockedPositionOffset = Vector3.zero;
    private Quaternion lockedRotationOffset = Quaternion.identity;
    HandType handType = HandType.Left;
    // Start is called before the first frame update
    public void Init()
    {
        //set the hand type
        if (transform.name == "Right Hand Model" || transform.name == "Right Hand Physics")
        {
            handType = HandType.Right;
        }
        rb = GetComponent<Rigidbody>();
    }
    public HandType GetHandType() => handType;
    public void HandPhysicsFixedUpdate()
    {
        if (isLocked)
        {
            // When locked, directly set position and rotation
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.MovePosition(target.position + lockedPositionOffset);
            rb.MoveRotation(target.rotation * Quaternion.Euler(0, 0, zRotationOffset) * lockedRotationOffset);
        }
        else
        {
            // Normal behavior
            rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;

            Quaternion targetRotationWithOffset = target.rotation * Quaternion.Euler(0, 0, zRotationOffset);
            Quaternion rotationDifference = targetRotationWithOffset * Quaternion.Inverse(transform.rotation);
            rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

            if (rotationAxis != Vector3.zero)
            {
                Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;
                rb.angularVelocity = (rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
            }
            else
            {
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    public void LockHand(Vector3 positionOffset = default(Vector3), Quaternion rotationOffset = default(Quaternion))
    {
        isLocked = true;
        lockedPositionOffset = positionOffset;
        lockedRotationOffset = rotationOffset;
        // Optional: Immediately move to locked position/rotation
        rb.MovePosition(target.position + lockedPositionOffset);
        rb.MoveRotation(target.rotation * Quaternion.Euler(0, 0, zRotationOffset) * lockedRotationOffset);
    }

    public void UnlockHand()
    {
        isLocked = false;
        // Optional: Reset offsets if necessary
        lockedPositionOffset = Vector3.zero;
        lockedRotationOffset = Quaternion.identity;
    }
}