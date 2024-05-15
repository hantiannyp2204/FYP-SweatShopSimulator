using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ContinuousMovementPhysics : MonoBehaviour
{
    public float speed = 1;
    public float turnSpeed = 60;
    public InputActionProperty moveInputSource;
    public InputActionProperty turnInputSource;
    public Rigidbody XRRigidbody;

    public Transform directionSource;
    public Transform turnSource;

    public CapsuleCollider bodyCollider;

    private Vector2 inputMoveAxis;
    private float inputTurnAxis;

    // Update is called once per frame
    public void PlayerMovementInputUpdate()
    {
        inputMoveAxis = moveInputSource.action.ReadValue<Vector2>();
        inputTurnAxis = turnInputSource.action.ReadValue<Vector2>().x;
    }

    public  void PlayerMovementFixedUpdate()
    {
        Quaternion yaw = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
        Vector3 direction = yaw * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

        Vector3 targetMovePosition = XRRigidbody.position + direction * Time.fixedDeltaTime * speed;

        Vector3 axis = Vector3.up;
        float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;

        Quaternion q = Quaternion.AngleAxis(angle, axis);

        XRRigidbody.MoveRotation(XRRigidbody.rotation * q);

        Vector3 newPosition = q * (targetMovePosition - turnSource.position) + turnSource.position;

        XRRigidbody.MovePosition(newPosition);
    }
}
