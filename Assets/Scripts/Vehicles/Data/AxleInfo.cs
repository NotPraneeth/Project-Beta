using UnityEngine;

namespace Vehicles.Data
{
    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;

        public Transform leftWheelVisual;
        public Transform rightWheelVisual;

        public bool motor;
        public bool steer;
        public bool handbrake;
    }
}
