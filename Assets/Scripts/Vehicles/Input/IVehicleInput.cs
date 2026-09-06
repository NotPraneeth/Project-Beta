namespace Vehicles.Input
{
    public interface IVehicleInput
    {
        float Throttle { get; }

        float Brake { get; }

        float Steer { get; }

        bool Handbrake { get; }
    }
}
