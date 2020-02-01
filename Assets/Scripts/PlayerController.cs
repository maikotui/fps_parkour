using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Required components
    private CharacterController characterController;

    // Inspector variables
    [Header("References")]
    public Camera playerCamera;
    
    [Header("Movement")]
    public float movementSpeed;
    public float maximumMovementSpeed;

    [Header("Camera")]
    public float xAxisSensitivity = 100f;
    public float yAxisSensitivity = 100f;
    [Range(0, 180)]
    public float maximumYRotation = 89f;

    // Properties
    public bool IsGrounded { get; private set; }

    // Member variables
    private InputManager m_inputManager;
    private float m_playerCameraYRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        IsGrounded = true;

        m_inputManager = InputManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if on the ground, if so snap
        HandleGrounding();

        // Handle look input
        HandleLookInput();

        // Handle movement input
        HandleMovementInput();
    }

    private void HandleGrounding()
    {

    }

    private void HandleLookInput()
    {
        Vector2 mouseInput = m_inputManager.GetLookInput();

        // If rotating horizontally, move the whole character
        float rotationX = transform.localEulerAngles.y + (mouseInput.x * Time.deltaTime * xAxisSensitivity);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationX, 0);

        // If rotating vertically, move just the camera
        m_playerCameraYRotation -= mouseInput.y * Time.deltaTime * yAxisSensitivity;
        m_playerCameraYRotation = Mathf.Clamp(m_playerCameraYRotation, -maximumYRotation, maximumYRotation);
        playerCamera.transform.localRotation = Quaternion.Euler(m_playerCameraYRotation, 0f, 0f);
    }

    private void HandleMovementInput()
    {
        Vector3 velocity = characterController.velocity;

        if (IsGrounded)  // Grounded Movement
        {
            Vector2 movementInput = m_inputManager.GetMovementInput();
            Vector3 worldspaceMovementInput = transform.TransformVector(new Vector3(movementInput.x, 0f, movementInput.y));
            Vector3 targetVelocity = worldspaceMovementInput * maximumMovementSpeed * movementSpeed;
            velocity = Vector3.Lerp(velocity, targetVelocity, 15 * Time.deltaTime);
            // Reduce movement speed if crouching if applicable

            // Orient velocity on the current slope
        }
        else  // In Air Movement
        {

        }

        velocity += Vector3.down * 9.41f * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
