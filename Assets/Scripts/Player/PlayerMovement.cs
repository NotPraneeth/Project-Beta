using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Sprint")]
    [SerializeField] private float sprintSpeed = 1f;
    [SerializeField] private bool isSprinting = false;

    private CharacterController controller;
    private PlayerControls controls;

    private Vector2 moveInput;
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
        controls.Player.Sprint.performed += OnSprint;
        controls.Player.Sprint.canceled += OnSprint;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;
        controls.Player.Jump.performed -= OnJump;
        controls.Player.Sprint.performed -= OnSprint;
        controls.Player.Sprint.canceled -= OnSprint;

        controls.Disable();
    }

    private void Update()
    {
        ApplyGravity();
        Move();
    }

    private void Move()
    {
        Vector3 moveDirection =
            transform.forward * moveInput.y +
            transform.right * moveInput.x;

        if (moveDirection.sqrMagnitude > 1f)
            moveDirection.Normalize();

        if(isSprinting)
        {
            moveDirection = moveDirection * moveSpeed * sprintSpeed;
        }
        else
        {
            moveDirection = moveDirection * moveSpeed;
        }
        moveDirection.y = verticalVelocity;

        controller.Move(moveDirection * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
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

    private void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }
}