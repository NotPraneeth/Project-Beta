using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float sensitivityX = 0.15f;
    [SerializeField] private float sensitivityY = 0.15f;
    [SerializeField] private float maxLookAngle = 90f;

    [Header("References")]
    [SerializeField] private Transform cameraRoot;

    private PlayerControls controls;

    private Vector2 lookInput;
    private float pitch;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Player.MouseLook.performed += OnLook;
        controls.Player.MouseLook.canceled += OnLook;

        // Lock and hide the cursor while playing.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        controls.Player.MouseLook.performed -= OnLook;
        controls.Player.MouseLook.canceled -= OnLook;

        controls.Disable();

        // Restore the cursor when the script is disabled.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        Look();
    }

    /// <summary>
    /// Rotates the player horizontally and the camera vertically.
    /// </summary>
    private void Look()
    {
        // Rotate the player left and right (Yaw).
        transform.Rotate(Vector3.up * lookInput.x * sensitivityX);

        // Rotate the camera up and down (Pitch).
        pitch -= lookInput.y * sensitivityY;

        // Prevent looking beyond straight up or down.
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        cameraRoot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    /// <summary>
    /// Reads mouse movement from the Input System.
    /// </summary>
    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
}