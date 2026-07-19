using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private CharacterController controller;
    private PlayerControls controls;

    private Vector2 moveInput;

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
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;

        controls.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(
            moveInput.x,
            0f,
            moveInput.y
        );

        controller.Move(movement * moveSpeed * Time.deltaTime);
    }
}