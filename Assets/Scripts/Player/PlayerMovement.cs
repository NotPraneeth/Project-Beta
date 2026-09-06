using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("Jumping")]
        [SerializeField] private float jumpHeight = 1.5f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Sprint")]
        [SerializeField] private float sprintMultiplier = 1.5f;

        private CharacterController controller;
        private PlayerInputHandler input;

        private float verticalVelocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            input = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            ApplyGravity();
            HandleJump();
            Move();
        }

        private void Move()
        {
            Vector2 moveInput = input.MoveInput;

            Vector3 moveDirection =
                transform.forward * moveInput.y +
                transform.right * moveInput.x;

            if (moveDirection.sqrMagnitude > 1f)
                moveDirection.Normalize();

            float currentSpeed = input.IsSprinting
                ? moveSpeed * sprintMultiplier
                : moveSpeed;

            moveDirection *= currentSpeed;
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

        private void HandleJump()
        {
            if (!input.Jump.Consume())
                return;

            if (!controller.isGrounded)
                return;

            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
