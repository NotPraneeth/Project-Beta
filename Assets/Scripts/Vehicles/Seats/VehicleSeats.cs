using Player;
using UnityEngine;

namespace Vehicles.Seats
{
    // The vehicle's fill order, flattened to a single array: index 0 fills first
    // (driver), then 1 (co-passenger), then whatever order the rest are dragged
    // in. Which door a player actually interacted with doesn't matter — every
    // VehicleSeat on this vehicle routes here and always fills the same way.
    public class VehicleSeats : MonoBehaviour
    {
        [SerializeField] private VehicleSeat[] seats;

        public bool TryOccupyNextAvailable(PlayerVehicleMount mount)
        {
            foreach (VehicleSeat seat in seats)
            {
                if (seat.IsOccupied)
                    continue;

                mount.Mount(seat);
                return true;
            }

            return false;
        }
    }
}
