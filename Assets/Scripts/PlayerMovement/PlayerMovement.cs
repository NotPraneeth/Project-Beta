using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float gravity = -9.81f;

    [Header("References")]
    [SerializeField] private Transform playerVisual;
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private PlayerControls controls;

    private Vector2 moveInput;

    private Vector3 cameraForward;
    private Vector3 cameraRight;
    private Vector3 movement;

    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;
        controls.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;
        controls.Player.Jump.performed -= OnJump;

        controls.Disable();
    }

    private void Update()
    {
        UpdateCameraDirections();
        ApplyGravity();
        CalculateMovement();
        RotateVisual();
        MoveCharacter();
    }

    private void UpdateCameraDirections()
    {
        cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        cameraRight = cameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    private void CalculateMovement()
    {
        movement =
            cameraForward * moveInput.y +
            cameraRight * moveInput.x;

        if (movement.sqrMagnitude > 1f)
        {
            movement.Normalize();
        }

        movement *= moveSpeed;
    }

    private void RotateVisual()
    {
        if (movement.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(movement);

        playerVisual.rotation = Quaternion.Slerp(
            playerVisual.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void MoveCharacter()
    {
        movement.y = verticalVelocity;

        controller.Move(movement * Time.deltaTime);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!controller.isGrounded)
            return;

        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}