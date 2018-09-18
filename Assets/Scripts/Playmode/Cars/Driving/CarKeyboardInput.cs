using Harmony;
using UnityEngine;

namespace Playmode.Cars.Driving
{
    /// <summary>
    /// Car drinving handled by the Player using a keyboard.
    /// </summary>
    [AddComponentMenu("Game/Cars/Driving/CarKeyboardInput")]
    public class CarKeyboardInput : MonoBehaviour
    {
        [SerializeField] private float throttleIncrease = 5f;
        [SerializeField] private float throttleDecrease = 5f;
        [SerializeField] private float steeringIncrease = 2f;
        [SerializeField] private float steeringDecrease = 10f;
        [SerializeField] private float sleepThreshold = 0.1f;

        private float desiredMovement;
        private float desiredSteering;

        private Car car;

        private void Awake()
        {
            car = this.GetComponentInSiblings<Car>();
        }

        private void Update()
        {
            desiredMovement = GetAxis(KeyCode.W, KeyCode.S, desiredMovement, throttleIncrease, throttleDecrease);
            desiredSteering = GetAxis(KeyCode.D, KeyCode.A, desiredSteering, steeringIncrease, steeringDecrease);

            car.Move(desiredMovement);
            car.Steer(desiredSteering);
        }

        private float GetAxis(
            KeyCode positiveKey,
            KeyCode negativeKey,
            float value,
            float increase,
            float decrease)
        {
            if (Input.GetKey(positiveKey) && !Input.GetKey(negativeKey))
                value = value < 0 ? 0 : value + increase * Time.deltaTime;
            else if (Input.GetKey(negativeKey) && !Input.GetKey(positiveKey))
                value = value > 0 ? 0 : value - increase * Time.deltaTime;
            else if (Mathf.Abs(value) > sleepThreshold)
                value = value > 0
                    ? value - decrease * Time.deltaTime
                    : value + decrease * Time.deltaTime;
            else
                value = 0;

            return Mathf.Clamp(value, -1, 1);
        }
    }
}