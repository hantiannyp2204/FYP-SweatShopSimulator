using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPresensePhysics : MonoBehaviour
{
    public Transform target;
    Rigidbody rb;
    [SerializeField] float zRotationOffset;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void HandPhysicsFixedUpdate()
    {
        // Position
        rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;

        Quaternion targetRotationWithOffset = target.rotation * Quaternion.Euler(0, 0, zRotationOffset);
        Quaternion rotationDifference = targetRotationWithOffset * Quaternion.Inverse(transform.rotation);
        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

        // If the rotationAxis is close to zero, then the rotation difference is too small to be meaningful
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
