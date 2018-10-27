using UnityEngine;

namespace Playmode.EnnemyRework.Boss.Jean
{
    public class ShieldManager : MonoBehaviour
    {
        private float shieldPercent = 1;
        private Vector2 baseScale;

        private void Awake()
        {
            baseScale = transform.localScale;
        }

        public float ShieldPercent
        {
            get
            {
                return shieldPercent;
            }
        }

        public void UseShield(float percentageUsed)
        {
            shieldPercent -= percentageUsed;

            if (shieldPercent < 0)
                shieldPercent = 0;

            transform.localScale = baseScale * shieldPercent;
        }
    }
}


