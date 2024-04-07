using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float sensitivity = 2.0f;
    public float crouchHeight = 1.0f;
    public float standHeight = 2.0f;
    public float gravity = -9.81f; // Earth's gravity in m/s^2
    public Camera playerCamera;

    [SerializeField] GameObject playerBody;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private float verticalVelocity = 0; // Current velocity in the Y direction

    public void Init()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
    }

    public void UpdateTransform()
    {
        // Movement
        float moveHorizontal = Input.GetAxisRaw("Horizontal") * speed;
        float moveVertical = Input.GetAxisRaw("Vertical") * speed;
        moveDirection = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

        // Apply gravity
        if (characterController.isGrounded)
        {
            verticalVelocity = 0; // Reset vertical velocity if on the ground

            // Crouching mechanism
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                characterController.height = crouchHeight;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                characterController.height = standHeight;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // Apply gravity acceleration
        }

        // Crouching mechanism
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            characterController.height = crouchHeight;
            playerBody.transform.localScale = new Vector3(1,.5f,1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            characterController.height = standHeight;
            playerBody.transform.localScale = new Vector3(1, 1, 1);
        }

        // Include vertical velocity in the move direction
        Vector3 finalMoveDirection = moveDirection * speed + Vector3.up * verticalVelocity;
        characterController.Move(finalMoveDirection * Time.deltaTime);

        // Looking around with the mouse
        rotationX += -Input.GetAxisRaw("Mouse Y") * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -90, 90); // Clamp vertical rotation
        playerCamera.transform.localEulerAngles = new Vector3(rotationX, 0, 0);
        transform.Rotate(0, Input.GetAxisRaw("Mouse X") * sensitivity, 0);
    }
}