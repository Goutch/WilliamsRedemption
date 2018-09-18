using UnityEngine;

namespace Playmode.Cars
{
    [CreateAssetMenu(fileName = "New WheelConfig", menuName = "Game/Cars/WheelConfig")]
    public class WheelConfig : ScriptableObject
    {
        [Header("Handling")] [SerializeField] private float weight = 1f;
        [SerializeField] private float fowardFriction = 1f;
        [SerializeField] private float lateralFriction = 4f;
        [SerializeField] private float angularFriction = 0.999f;
        [Header("Assistance")] [SerializeField] private float steeringAssistance = 0.01f;

        public float Weight => weight;
        public float FowardFriction => fowardFriction;
        public float LateralFriction => lateralFriction;
        public float AngularFriction => angularFriction;
        public float SteeringAssistance => steeringAssistance;
    }
}