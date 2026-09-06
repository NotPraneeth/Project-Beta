using Player;
using UnityEngine;

namespace Vehicles.Camera
{
    // A rigid orbit camera: position/rotation are computed directly from the
    // current yaw/pitch every frame, no lag on the camera itself, so mouse
    // input always has full, instant authority. The only thing ever eased is
    // yaw/pitch drifting back to directly-behind-the-car, and only once the
    // player has left the mouse alone for recenterDelay seconds.
    public class VehicleChaseCamera : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera targetCamera;
        [SerializeField] private PlayerInputHandler input;

        [Header("Orbit")]
        [SerializeField] private float orbitSensitivity = 3f;
        [SerializeField] private float minPitch = -10f;
        [SerializeField] private float maxPitch = 60f;
        [SerializeField] private float defaultPitch = 15f;

        [Header("Follow")]
        [SerializeField] private float distance = 6f;
        [SerializeField] private float height = 2.5f;

        [Header("Auto Re-Center")]
        [SerializeField] private float recenterDelay = 2f;
        [SerializeField] private float recenterSpeed = 2f;

        private Transform followTarget;
        private bool isActive;

        private float yaw;
        private float pitch;
        private float timeSinceInput;

        private Transform homeParent;
        private Vector3 homeLocalPosition;
        private Quaternion homeLocalRotation;

        private void Awake()
        {
            homeParent = targetCamera.transform.parent;
            homeLocalPosition = targetCamera.transform.localPosition;
            homeLocalRotation = targetCamera.transform.localRotation;
        }

        public void Activate(Transform vehicleTransform)
        {
            followTarget = vehicleTransform;
            yaw = vehicleTransform.eulerAngles.y;
            pitch = defaultPitch;
            timeSinceInput = 0f;
            isActive = true;

            targetCamera.transform.SetParent(null, true);
        }

        public void Deactivate()
        {
            isActive = false;
            followTarget = null;

            targetCamera.transform.SetParent(homeParent, false);
            targetCamera.transform.SetLocalPositionAndRotation(homeLocalPosition, homeLocalRotation);
        }

        private void LateUpdate()
        {
            if (!isActive || followTarget == null)
                return;

            Vector2 look = input.LookInput;

            // Mouse input always wins outright — no blending with the recenter pull.
            if (look.sqrMagnitude > 0.0001f)
            {
                yaw += look.x * orbitSensitivity;
                pitch -= look.y * orbitSensitivity;
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
                timeSinceInput = 0f;
            }
            else
            {
                timeSinceInput += Time.deltaTime;

                if (timeSinceInput >= recenterDelay)
                {
                    float targetYaw = followTarget.eulerAngles.y;
                    yaw = Mathf.LerpAngle(yaw, targetYaw, Time.deltaTime * recenterSpeed);
                    pitch = Mathf.Lerp(pitch, defaultPitch, Time.deltaTime * recenterSpeed);
                }
            }

            Quaternion orbitRotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 desiredPosition = followTarget.position
                + Vector3.up * height
                - orbitRotation * Vector3.forward * distance;

            // Rigid follow: snap straight to the computed pose every frame.
            targetCamera.transform.position = desiredPosition;
            targetCamera.transform.rotation = Quaternion.LookRotation(
                (followTarget.position + Vector3.up * height * 0.5f) - desiredPosition);
        }
    }
}
