using UnityEngine;

namespace Light
{
    public interface MeshLight
    {
        bool isWithinLightLimits(Vector2 position);

    }
}