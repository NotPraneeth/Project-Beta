using UnityEngine;

namespace Vehicles.Data
{
    [CreateAssetMenu(menuName = "Vehicles/Vehicle Stats", fileName = "New Vehicle Stats")]
    public class VehicleStats : ScriptableObject
    {
        [Header("Engine")]
        [Min(0f)] public float maxMotorTorque = 1500f;
        [Min(0f)] public float topSpeed = 150f;
        [Range(0f, 1f)] public float reverseTorqueMultiplier = 0.5f;
        [Min(0f)] public float reverseTopSpeed = 40f;

        [Header("Steering")]
        [Min(0f)] public float maxSteerAngle = 30f;
        [Min(0f)] public float minSteerAngleAtTopSpeed = 10f;
        [Min(1f)] public float steerReferenceSpeed = 60f;
        [Min(0f)] public float steerSpeed = 200f;
        [Range(0f, 1f)] public float throttleReductionWhileSteering = 0.5f;

        [Header("Stability")]
        [Min(0f)] public float antiRollBarStiffness = 5000f;

        [Header("Braking")]
        [Min(0f)] public float maxBrakeTorque = 2500f;
        [Min(0f)] public float handbrakeTorque = 4000f;
        [Min(0f)] public float idleBrakeTorque = 300f;
        [Min(0f)] public float coastBrakeTorque = 800f;

        [Header("Drivetrain")]
        public DrivetrainType drivetrain = DrivetrainType.RWD;
    }
}
