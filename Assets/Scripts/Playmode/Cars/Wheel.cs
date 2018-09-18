using System;
using Harmony;
using UnityEngine;

namespace Playmode.Cars
{
    /// <summary>
    /// Basic Wheel implementation.
    /// </summary>
    [AddComponentMenu("Game/Cars/Wheel")]
    public class Wheel : MonoBehaviour
    {
        private Car car;

        private float desiredPower;
        private float desiredSteeringAngle;

        private new Rigidbody2D rigidbody;
        private new HingeJoint2D hingeJoint;
        public Rigidbody2D Rigidbody => rigidbody != null ? rigidbody : (rigidbody = CreateRigidbody());

        public WheelConfig Config => car.Config.WheelConfig;
        public Vector2 Forward => Rigidbody.transform.up;
        public Vector2 Right => Rigidbody.transform.right;
        private float Mass => Rigidbody.mass;
        private float Inertia => Rigidbody.inertia;
        private Vector2 Velocity => Rigidbody.velocity;
        private Vector2 LateralVelocity => Vector2.Dot(Right, Velocity) * Right;
        private float AngularVelocity => Rigidbody.angularVelocity;
        public float Speed => Velocity.magnitude;
        public float Skid => LateralVelocity.magnitude - Vector2.ClampMagnitude(LateralVelocity, Config.LateralFriction).magnitude;

        private Rigidbody2D CreateRigidbody()
        {
            //Warning! : Creating a "HingeJoint" also automatically creates a Rigidbody. Thus, we must be cautious
            //           when creating the "RigidBody" here. See "AttachTo" method.
            //Also, please note that multiple RigidBodies are permited on an entity.
            var rigidbody = gameObject.AddOrGetComponent<Rigidbody2D>();
            rigidbody.mass = Config.Weight;
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0f;
            rigidbody.gravityScale = 0f;
            return rigidbody;
        }

        public void AttachTo(Car car)
        {
            this.car = car;

            hingeJoint = gameObject.AddOrGetComponent<HingeJoint2D>();
            hingeJoint.useLimits = true;
            hingeJoint.limits = new JointAngleLimits2D {min = 0f, max = 0f};
            hingeJoint.connectedBody = car.Rigidbody;
        }

#if UNITY_EDITOR
        private void Start()
        {
            if (car == null)
                throw new ArgumentException("Wheel needs to be attached to a car to work. See AttachTo method.");
        }

        private void Update()
        {
            Rigidbody.mass = Config.Weight; //Enable mass ajustments while in playmode in editor
        }
#endif

        public void AddPower(float power)
        {
            desiredPower = power;
        }

        public void Steer(float streeringAngle)
        {
            desiredSteeringAngle = streeringAngle;
        }

        private void FixedUpdate()
        {
            UpdateSpining();
            UpdateSteering();
        }

        private void UpdateSpining()
        {
            AddPower();
            AddFowardFriction();
            AddLateralFriction();
            AddAngularFriction();
        }

        private void AddPower()
        {
            var powerForce = Forward * desiredPower;

            Rigidbody.AddForce(powerForce);
            Rigidbody.velocity = Vector2.ClampMagnitude(Rigidbody.velocity, Speed > 0
                ? car.Config.BodyConfig.MaxForwardSpeed
                : car.Config.BodyConfig.MaxBackwardSpeed);
        }
        
        private void AddFowardFriction()
        {
            var fowardDrag = -Config.FowardFriction * Velocity.normalized;
            
            Rigidbody.AddForce(fowardDrag);
        }

        private void AddLateralFriction()
        {
            var lateralCorrection = Mass * Vector2.ClampMagnitude(-LateralVelocity, Config.LateralFriction);

            Rigidbody.AddForce(lateralCorrection, ForceMode2D.Impulse);
        }

        private void AddAngularFriction()
        {
            var angularForceCorrection = (1 - Config.AngularFriction) * Inertia * -AngularVelocity;

            Rigidbody.AddTorque(angularForceCorrection, ForceMode2D.Impulse);
        }

        private void UpdateSteering()
        {
            var powerDirection = Mathf.Clamp(desiredPower, -1, 1);
            var steeringDirection = -Mathf.Clamp(desiredSteeringAngle, -1, 1);
            var steeringAssistance = Speed * powerDirection * steeringDirection * Config.SteeringAssistance;
            Rigidbody.AddTorque(steeringAssistance, ForceMode2D.Impulse);

            hingeJoint.limits = new JointAngleLimits2D {min = desiredSteeringAngle, max = desiredSteeringAngle};
        }
    }
}