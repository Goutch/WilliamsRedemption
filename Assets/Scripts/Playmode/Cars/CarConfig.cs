using UnityEngine;

namespace Playmode.Cars
{
    [CreateAssetMenu(fileName = "New CarConfig", menuName = "Game/Cars/CarConfig")]
    public class CarConfig : ScriptableObject
    {
        [Header("Body")] [SerializeField] private BodyConfig bodyConfig;
        [Header("Engine")] [SerializeField] private EngineConfig engineConfig;
        [Header("Wheels")] [SerializeField] private WheelConfig wheelConfig;

        public BodyConfig BodyConfig => bodyConfig;
        public EngineConfig EngineConfig => engineConfig;
        public WheelConfig WheelConfig => wheelConfig;
    }
}