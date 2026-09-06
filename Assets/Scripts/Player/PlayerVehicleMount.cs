using UnityEngine;
using Vehicles.Camera;
using Vehicles.Seats;

namespace Player
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerVehicleMount : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerLook playerLook;
        [SerializeField] private PlayerInteractor playerInteractor;
        [SerializeField] private VehicleChaseCamera vehicleChaseCamera;

        public bool IsMounted { get; private set; }

        private VehicleSeat currentSeat;
        private PlayerInputHandler input;

        private void Awake()
        {
            input = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            if (!IsMounted)
                return;

            if (input.Interact.Consume())
            {
                Dismount();
            }
        }

        public void Mount(VehicleSeat seat)
        {
            if (IsMounted || seat == null)
                return;

            currentSeat = seat;
            seat.SetOccupant(this);
            IsMounted = true;

            characterController.enabled = false;
            playerMovement.enabled = false;
            playerLook.enabled = false;
            playerInteractor.enabled = false;

            // Parent to the seat so the player rides along with the vehicle
            // instead of staying frozen at the world-space mount position.
            transform.SetParent(seat.MountPoint, false);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            if (seat.IsDriverSeat)
            {
                vehicleChaseCamera.Activate(seat.VehicleTransform);
            }
        }

        public void Dismount()
        {
            if (!IsMounted)
                return;

            Vector3 exitPosition = currentSeat.ExitPoint.position;
            Quaternion exitRotation = currentSeat.ExitPoint.rotation;
            bool wasDriver = currentSeat.IsDriverSeat;

            currentSeat.SetOccupant(null);
            currentSeat = null;
            IsMounted = false;

            if (wasDriver)
            {
                vehicleChaseCamera.Deactivate();
            }

            transform.SetParent(null, true);
            transform.SetPositionAndRotation(exitPosition, exitRotation);

            characterController.enabled = true;
            playerMovement.enabled = true;
            playerLook.enabled = true;
            playerInteractor.enabled = true;
        }
    }
}
