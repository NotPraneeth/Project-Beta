using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraRoot;

    [Header("Settings")]
    [SerializeField] private float sensitivityX = 0.15f;
    [SerializeField] private float sensitivityY = 0.15f;
    [SerializeField] private float maxLookAngle = 90f;

    private PlayerInputHandler input;

    private float pitch;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();
    }

    private void Look()
    {
        Vector2 lookInput = input.LookInput;

        // Horizontal rotation (Yaw)
        transform.Rotate(Vector3.up * lookInput.x * sensitivityX);

        pitch -= lookInput.y * sensitivityY;

        // Vertical rotation (Pitch)
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        cameraRoot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}