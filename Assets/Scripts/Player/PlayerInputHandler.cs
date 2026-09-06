using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }

        public Vector2 LookInput { get; private set; }

        public bool IsSprinting { get; private set; }

        public bool InputEnabled { get; private set; } = true;

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

        public void EnableInput()
        {
            InputEnabled = true;
        }

        public void DisableInput()
        {
            InputEnabled = false;

            MoveInput = Vector2.zero;
            LookInput = Vector2.zero;

            IsSprinting = false;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            if (!InputEnabled)
            {
                MoveInput = Vector2.zero;
                return;
            }

            MoveInput = context.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            if (!InputEnabled)
            {
                LookInput = Vector2.zero;
                return;
            }

            LookInput = context.ReadValue<Vector2>();
        }

        private void OnSprint(InputAction.CallbackContext context)
        {
            if (!InputEnabled)
            {
                IsSprinting = false;
                return;
            }

            IsSprinting = context.ReadValueAsButton();
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (!InputEnabled)
                return;

            Jump.Press();
        }

        // Intentionally NOT disabled.
        // We still want E to work while storage is open.
        private void OnInteract(InputAction.CallbackContext context)
        {
            Interact.Press();
        }
    }
}
