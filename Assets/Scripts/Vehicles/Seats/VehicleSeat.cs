using Player;
using UnityEngine;
using Vehicles.Core;

namespace Vehicles.Seats
{
    public class VehicleSeat : MonoBehaviour, IInteractable
    {
        [SerializeField] private VehicleController vehicleController;
        [SerializeField] private VehicleSeats vehicleSeats;
        [SerializeField] private bool isDriverSeat = true;

        [SerializeField] private Transform mountPoint;
        [SerializeField] private Transform exitPoint;

        public bool IsDriverSeat => isDriverSeat;
        public bool IsOccupied => Occupant != null;
        public PlayerVehicleMount Occupant { get; private set; }

        public Transform MountPoint => mountPoint != null ? mountPoint : transform;
        public Transform ExitPoint => exitPoint != null ? exitPoint : transform;
        public Transform VehicleTransform => vehicleController != null ? vehicleController.transform : transform;

        public string InteractionPrompt => "Enter Vehicle";

        public Transform InteractionPoint => MountPoint;

        public void Interact(PlayerInteractor interactor)
        {
            Debug.Log($"[VehicleSeat] {gameObject.name}.Interact() called.");

            PlayerVehicleMount mount = interactor.GetComponent<PlayerVehicleMount>();
            if (mount == null)
            {
                Debug.Log($"[VehicleSeat] {gameObject.name}: interactor has no PlayerVehicleMount component.");
                return;
            }

            if (vehicleSeats == null)
            {
                Debug.Log($"[VehicleSeat] {gameObject.name}: Vehicle Seats field is not assigned.");
                return;
            }

            // Which door you actually pressed E on doesn't decide your seat —
            // the vehicle's fixed fill order does (see VehicleSeats).
            bool occupied = vehicleSeats.TryOccupyNextAvailable(mount);
            Debug.Log($"[VehicleSeat] {gameObject.name}: TryOccupyNextAvailable returned {occupied}.");
        }

        public void SetOccupant(PlayerVehicleMount occupant)
        {
            Occupant = occupant;

            if (isDriverSeat && vehicleController != null)
            {
                vehicleController.SetHasDriver(occupant != null);
            }
        }
    }
}
