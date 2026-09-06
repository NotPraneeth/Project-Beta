using UnityEngine;
using UnityEngine.InputSystem;

namespace Vehicles.Input
{
    public class PlayerVehicleInput : MonoBehaviour, IVehicleInput
    {
        public float Throttle { get; private set; }

        public float Brake { get; private set; }

        public float Steer { get; private set; }

        public bool Handbrake { get; private set; }

        private PlayerControls controls;

        private void Awake()
        {
            controls = new PlayerControls();
        }

        private void OnEnable()
        {
            controls.Enable();

            controls.Vehicle.Accelerate.performed += OnAccelerate;
            controls.Vehicle.Accelerate.canceled += OnAccelerate;

            controls.Vehicle.Brake.performed += OnBrake;
            controls.Vehicle.Brake.canceled += OnBrake;

            controls.Vehicle.Steer.performed += OnSteer;
            controls.Vehicle.Steer.canceled += OnSteer;

            controls.Vehicle.Handbrake.performed += OnHandbrake;
            controls.Vehicle.Handbrake.canceled += OnHandbrake;
        }

        private void OnDisable()
        {
            controls.Vehicle.Accelerate.performed -= OnAccelerate;
            controls.Vehicle.Accelerate.canceled -= OnAccelerate;

            controls.Vehicle.Brake.performed -= OnBrake;
            controls.Vehicle.Brake.canceled -= OnBrake;

            controls.Vehicle.Steer.performed -= OnSteer;
            controls.Vehicle.Steer.canceled -= OnSteer;

            controls.Vehicle.Handbrake.performed -= OnHandbrake;
            controls.Vehicle.Handbrake.canceled -= OnHandbrake;

            controls.Disable();
        }

        private void OnAccelerate(InputAction.CallbackContext context)
        {
            Throttle = context.ReadValueAsButton() ? 1f : 0f;
        }

        private void OnBrake(InputAction.CallbackContext context)
        {
            Brake = context.ReadValueAsButton() ? 1f : 0f;
        }

        private void OnSteer(InputAction.CallbackContext context)
        {
            Steer = context.ReadValue<float>();
        }

        private void OnHandbrake(InputAction.CallbackContext context)
        {
            Handbrake = context.ReadValueAsButton();
        }
    }
}
