using UnityEngine;

namespace Playmode.Cars
{
    [CreateAssetMenu(fileName = "New BodyConfig", menuName = "Game/Cars/BodyConfig")]
    public class BodyConfig : ScriptableObject
    {
        [Header("Body")] [SerializeField] private float weight = 10f;
        [Header("Handling")] [SerializeField] private float maxSteeringAngle = 40f;
        [SerializeField] private float maxForwardSpeed = 70f;
        [SerializeField] private float maxBackwardSpeed = 35f;

        public float Weight => weight;
        public float MaxSteeringAngle => maxSteeringAngle;
        public float MaxForwardSpeed => maxForwardSpeed;
        public float MaxBackwardSpeed => maxBackwardSpeed;
    }
}