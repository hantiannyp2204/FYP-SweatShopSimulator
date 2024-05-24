using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerBoxCastChecker : MonoBehaviour
{
    [SerializeField] private Vector3 boxCastCenterOffset = Vector3.zero;
    [SerializeField] private Vector3 boxCastHalfExtents = Vector3.one;
    [SerializeField] private Quaternion boxCastOrientation = Quaternion.identity;

    public bool IsColliderInBox()
    {
        return Physics.CheckBox(transform.position + boxCastCenterOffset, boxCastHalfExtents, transform.rotation * boxCastOrientation);
    }

    private void OnDrawGizmos()
    {
        // Draw the box cast bounds in the editor
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + boxCastCenterOffset, transform.rotation * boxCastOrientation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxCastHalfExtents * 2);
    }

}
