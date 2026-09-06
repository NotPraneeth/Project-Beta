using UnityEngine;
using Vehicles.Data;
using Vehicles.Input;

namespace Vehicles.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class VehicleController : MonoBehaviour
    {
        private enum Gear
        {
            Forward,
            Reverse
        }

        [SerializeField] private VehicleStats stats;
        [SerializeField] private AxleInfo[] axles;

        [SerializeField, Min(0f)]
        private float stoppedSpeedThreshold = 0.5f;

        [SerializeField]
        private Vector3 centerOfMassOffset = new Vector3(0f, -0.8f, 0f);

        private Rigidbody vehicleRigidbody;
        private IVehicleInput input;
        private float currentSteerAngle;
        private Gear currentGear = Gear.Forward;

        public bool HasDriver { get; private set; }

        private void Awake()
        {
            vehicleRigidbody = GetComponent<Rigidbody>();
            input = GetComponent<IVehicleInput>();
            vehicleRigidbody.centerOfMass += centerOfMassOffset;

            // Parked by default: kinematic so nothing (player bumps, stray
            // physics) can shove an unmanned vehicle around.
            vehicleRigidbody.isKinematic = true;
        }

        public void SetHasDriver(bool hasDriver)
        {
            HasDriver = hasDriver;
            vehicleRigidbody.isKinematic = !hasDriver;
        }

        private void FixedUpdate()
        {
            if (stats == null)
                return;

            if (!HasDriver || input == null)
            {
                HoldStationary();
                return;
            }

            // Total speed, not a facing-relative component — a spin/donut can
            // rotate the car's facing independently of which way it's actually
            // moving, so anything derived from velocity-vs-facing alignment
            // (like a forward/backward dot product) is unreliable mid-maneuver.
            float speed = vehicleRigidbody.linearVelocity.magnitude;
            bool isNearStopped = speed < stoppedSpeedThreshold;

            // Gear only ever changes near a stop, so a spin's yaw rate can
            // never flip it mid-maneuver the way a per-frame velocity/facing
            // check could — this was the actual cause of torque randomly
            // cutting out and "restarting" partway through a donut.
            if (isNearStopped)
            {
                if (input.Brake > 0f)
                {
                    currentGear = Gear.Reverse;
                }
                else if (input.Throttle > 0f)
                {
                    currentGear = Gear.Forward;
                }
            }

            float motorTorque = ComputeMotorTorque(speed);
            float brakeTorque = ComputeBrakeTorque(speed, isNearStopped);

            // Less steering lock the faster you're going, and the wheels turn
            // toward that target over time instead of snapping instantly —
            // both are what stop full-speed full-lock turns from breaking
            // traction and oscillating between over/understeer. Falls off
            // against steerReferenceSpeed (well below top speed), not top
            // speed itself, or the car would still have most of its lock at
            // everyday driving speeds.
            float speedFraction = Mathf.Clamp01(speed * 3.6f / stats.steerReferenceSpeed);
            float effectiveMaxSteerAngle = Mathf.Lerp(stats.maxSteerAngle, stats.minSteerAngleAtTopSpeed, speedFraction);
            float targetSteerAngle = input.Steer * effectiveMaxSteerAngle;
            currentSteerAngle = Mathf.MoveTowards(currentSteerAngle, targetSteerAngle, stats.steerSpeed * Time.fixedDeltaTime);

            // The front wheels are both driven and steered, so full torque
            // plus real steering angle at speed asks them for more combined
            // grip (accelerating + changing direction at once) than a tire
            // can actually provide — that's what was causing near-180 spins
            // from a light steering tap at speed. Ease off torque the harder
            // you're steering and the faster you're going; barely affects
            // low-speed or gentle-steer situations like parking maneuvers.
            float steerTorqueFactor = 1f - Mathf.Abs(input.Steer) * speedFraction * stats.throttleReductionWhileSteering;
            motorTorque *= steerTorqueFactor;

            foreach (AxleInfo axle in axles)
            {
                if (axle.steer)
                {
                    axle.leftWheel.steerAngle = currentSteerAngle;
                    axle.rightWheel.steerAngle = currentSteerAngle;
                }

                if (axle.motor)
                {
                    axle.leftWheel.motorTorque = motorTorque;
                    axle.rightWheel.motorTorque = motorTorque;
                }

                float wheelBrakeTorque = brakeTorque;
                if (input.Handbrake && axle.handbrake)
                {
                    wheelBrakeTorque = stats.handbrakeTorque;
                }

                axle.leftWheel.brakeTorque = wheelBrakeTorque;
                axle.rightWheel.brakeTorque = wheelBrakeTorque;

                ApplyAntiRoll(axle);
            }
        }

        // Standard Unity anti-roll bar: measures how much more one side's suspension
        // is compressed than the other's and pushes back proportionally, resisting
        // body roll independently of how soft/stiff the suspension itself is.
        private void ApplyAntiRoll(AxleInfo axle)
        {
            float travelLeft = 1f;
            float travelRight = 1f;

            bool groundedLeft = axle.leftWheel.GetGroundHit(out WheelHit hitLeft);
            if (groundedLeft)
            {
                travelLeft = (-axle.leftWheel.transform.InverseTransformPoint(hitLeft.point).y - axle.leftWheel.radius)
                    / axle.leftWheel.suspensionDistance;
            }

            bool groundedRight = axle.rightWheel.GetGroundHit(out WheelHit hitRight);
            if (groundedRight)
            {
                travelRight = (-axle.rightWheel.transform.InverseTransformPoint(hitRight.point).y - axle.rightWheel.radius)
                    / axle.rightWheel.suspensionDistance;
            }

            float antiRollForce = (travelLeft - travelRight) * stats.antiRollBarStiffness;

            if (groundedLeft)
            {
                vehicleRigidbody.AddForceAtPosition(
                    axle.leftWheel.transform.up * -antiRollForce,
                    axle.leftWheel.transform.position);
            }

            if (groundedRight)
            {
                vehicleRigidbody.AddForceAtPosition(
                    axle.rightWheel.transform.up * antiRollForce,
                    axle.rightWheel.transform.position);
            }
        }

        private void Update()
        {
            foreach (AxleInfo axle in axles)
            {
                SyncWheelVisual(axle.leftWheel, axle.leftWheelVisual);
                SyncWheelVisual(axle.rightWheel, axle.rightWheelVisual);
            }
        }

        // Nobody's driving — hold the vehicle in place instead of leaving whatever
        // torque/brake/steer values were last applied stuck on the wheels.
        private void HoldStationary()
        {
            foreach (AxleInfo axle in axles)
            {
                axle.leftWheel.motorTorque = 0f;
                axle.rightWheel.motorTorque = 0f;
                axle.leftWheel.brakeTorque = stats.idleBrakeTorque;
                axle.rightWheel.brakeTorque = stats.idleBrakeTorque;
            }
        }

        private float ComputeMotorTorque(float speed)
        {
            if (currentGear == Gear.Forward)
            {
                if (input.Throttle <= 0f)
                    return 0f;

                float topSpeedMs = stats.topSpeed / 3.6f;
                if (speed >= topSpeedMs)
                    return 0f;

                return input.Throttle * stats.maxMotorTorque;
            }
            else
            {
                if (input.Brake <= 0f)
                    return 0f;

                float reverseTopSpeedMs = stats.reverseTopSpeed / 3.6f;
                if (speed >= reverseTopSpeedMs)
                    return 0f;

                return -input.Brake * stats.maxMotorTorque * stats.reverseTorqueMultiplier;
            }
        }

        private float ComputeBrakeTorque(float speed, bool isNearStopped)
        {
            if (currentGear == Gear.Forward)
            {
                // Brake pedal actively brakes while in forward gear.
                if (input.Brake > 0f)
                    return input.Brake * stats.maxBrakeTorque;
            }
            else
            {
                // Accelerator actively brakes while in reverse gear — the
                // mirror image of the above, so both pedals behave symmetrically.
                if (input.Throttle > 0f)
                    return input.Throttle * stats.maxBrakeTorque;
            }

            if (input.Throttle <= 0f && input.Brake <= 0f)
            {
                // Nobody's pressing anything and we're basically stopped — hold
                // the wheels still so residual suspension/contact jitter can't
                // slowly creep them (and shake the body) with nothing actively
                // resisting it.
                if (isNearStopped)
                {
                    return stats.idleBrakeTorque;
                }

                // Still moving with nobody touching the pedals — bleed speed
                // faster than bare tire rolling resistance would (engine
                // braking), so letting off the gas actually slows the car
                // instead of coasting forever.
                return stats.coastBrakeTorque;
            }

            return 0f;
        }

        private static void SyncWheelVisual(WheelCollider collider, Transform visual)
        {
            if (collider == null || visual == null)
                return;

            collider.GetWorldPose(out Vector3 position, out Quaternion rotation);
            visual.SetPositionAndRotation(position, rotation);
        }
    }
}
