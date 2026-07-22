using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }

    public Vector2 LookInput { get; private set; }

    public bool IsSprinting { get; private set; }

    public InputButton Jump { get; } = new();

    public InputButton Interact { get; } = new();

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;

        controls.Player.MouseLook.performed += OnLook;
        controls.Player.MouseLook.canceled += OnLook;

        controls.Player.Sprint.performed += OnSprint;
        controls.Player.Sprint.canceled += OnSprint;

        controls.Player.Jump.performed += OnJump;

        controls.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;

        controls.Player.MouseLook.performed -= OnLook;
        controls.Player.MouseLook.canceled -= OnLook;

        controls.Player.Sprint.performed -= OnSprint;
        controls.Player.Sprint.canceled -= OnSprint;

        controls.Player.Jump.performed -= OnJump;

        controls.Player.Interact.performed -= OnInteract;

        controls.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        IsSprinting = context.ReadValueAsButton();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Jump.Press();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Interact.Press();
    }
}