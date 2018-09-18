using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using UnityEngine;

namespace Playmode.Cars
{
    /// <summary>
    /// Basic Car implementation using <see cref="Wheel"/>.
    /// </summary>
    [AddComponentMenu("Game/Cars/Car")]
    public class Car : MonoBehaviour
    {
        [Header("Body parts")] [SerializeField] private Engine engine;
        [SerializeField] private Wheel frontLeftWheel;
        [SerializeField] private Wheel frontRightWheel;
        [SerializeField] private Wheel rearLeftWheel;
        [SerializeField] private Wheel rearRightWheel;
        [Header("Config")] [SerializeField] private CarConfig carConfig;

        private float desiredThrottlePercentage;
        private float desiredStreeringPercentage;

        private new Rigidbody2D rigidbody;
        public Rigidbody2D Rigidbody => rigidbody != null ? rigidbody : (rigidbody = CreateRigidbody());

        public CarConfig Config => carConfig;
        public BodyConfig BodyConfig => Config.BodyConfig;
        public Vector3 Position => Rigidbody.position;
        public Vector3 Forward => Rigidbody.transform.up;
        public Vector3 Right => Rigidbody.transform.right;

        private void Awake()
        {
            ValidateSerializedFields();
            InitializeComponent();
        }

        private void ValidateSerializedFields()
        {
            if (engine == null)
                throw new ArgumentException("Car must have an engine.");
            if (frontLeftWheel == null)
                throw new ArgumentException("Car must have a front left wheel.");
            if (frontRightWheel == null)
                throw new ArgumentException("Car must have a front right wheel.");
            if (rearLeftWheel == null)
                throw new ArgumentException("Car must have a rear left wheel.");
            if (rearRightWheel == null)
                throw new ArgumentException("Car must have a rear right wheel.");
            if (carConfig == null)
                throw new ArgumentException("Car needs a CarConfig.");
            if (carConfig.BodyConfig == null)
                throw new ArgumentException("Car CarConfig need a BodyConfig.");
            if (carConfig.EngineConfig == null)
                throw new ArgumentException("Car CarConfig need a EngineConfig.");
            if (carConfig.WheelConfig == null)
                throw new ArgumentException("Car CarConfig need a WheelConfig.");
        }

        private void InitializeComponent()
        {
            engine.AttachTo(this, new[] {frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel});
        }

        private Rigidbody2D CreateRigidbody()
        {
            //Please note that multiple RigidBodies are permited on an entity.
            var rigidbody = this.Root().AddOrGetComponent<Rigidbody2D>();
            rigidbody.mass = BodyConfig.Weight;
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0f;
            rigidbody.gravityScale = 0f;
            return rigidbody;
        }

        /// <summary>
        /// Make the car move.
        /// </summary>
        /// <remarks>
        /// Will be done next fixed update.
        /// </remarks>
        /// <param name="throttlePercentage">Throttle power and direction. 1 for foward, -1 for backward.</param>
        public void Move(float throttlePercentage)
        {
            desiredThrottlePercentage = Mathf.Clamp(throttlePercentage, -1, 1);
        }

        /// <summary>
        /// Make the car steer.
        /// </summary>
        /// <remarks>
        /// Will be done next fixed update.
        /// </remarks>
        /// <param name="steeringPercentage">Steering power and direction. 1 for right, -1 for left.</param>
        public void Steer(float steeringPercentage)
        {
            desiredStreeringPercentage = Mathf.Clamp(steeringPercentage, -1, 1);
        }

        private void Update()
        {
            UpdateThrottle();
            UpdateSteering();
            
#if UNITY_EDITOR
            Rigidbody.mass = Config.BodyConfig.Weight; //Enable mass ajustments while in playmode in editor
#endif
        }

        private void UpdateThrottle()
        {
            engine.PressThrottle(desiredThrottlePercentage);

            desiredThrottlePercentage = 0;
        }

        private void UpdateSteering()
        {
            var streeringAngle = desiredStreeringPercentage * BodyConfig.MaxSteeringAngle;
            frontLeftWheel.Steer(streeringAngle);
            frontRightWheel.Steer(streeringAngle);

            desiredStreeringPercentage = 0;
        }
    }
}