using UnityEngine;

namespace Playmode.Cars
{
    [CreateAssetMenu(fileName = "New EngineConfig", menuName = "Game/Cars/EngineConfig")]
    public class EngineConfig : ScriptableObject
    {
        [SerializeField] private float power = 90f;

        public float Power => power;
    }
}