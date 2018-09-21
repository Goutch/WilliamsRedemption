using UnityEngine;

namespace Light
{
    public interface MeshLight
    {
        LightSensor IsWithinLightLimits(Vector2 position);

    }
}