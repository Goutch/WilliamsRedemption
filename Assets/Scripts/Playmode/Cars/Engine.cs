using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Playmode.Cars
{
    /// <summary>
    /// Basic implementation of a car engine. Provide forces to apply to the car.
    /// </summary>
    [AddComponentMenu("Game/Cars/Engine")]
    public class Engine : MonoBehaviour
    {
        private const float SleepThreshold = 0.01f;

        private Car car;
        private IEnumerable<Wheel> poweredWheels;

        private float desiredThrottlePercentage;

        public EngineConfig Config => car.Config.EngineConfig;
        public float Power => poweredWheels.Max(it => it.Speed) / car.Config.BodyConfig.MaxForwardSpeed;

        public void AttachTo(Car car, IEnumerable<Wheel> poweredWheels)
        {
            this.car = car;
            this.poweredWheels = poweredWheels;

            foreach (var poweredWheel in this.poweredWheels)
            {
                poweredWheel.AttachTo(car);
            }
        }

#if UNITY_EDITOR
        private void Start()
        {
            if (car == null)
                throw new ArgumentException("Engine needs to be attached to a car to work. See AttachTo method.");
        }
#endif

        public void PressThrottle(float throttlePercentage)
        {
            desiredThrottlePercentage = throttlePercentage;
        }

        private void Update()
        {
            DeliverPowerToWheels();
        }

        private void DeliverPowerToWheels()
        {
            foreach (var wheel in poweredWheels)
                wheel.AddPower(desiredThrottlePercentage * Config.Power);
        }
    }
}