using UnityEngine;

namespace Playmode.EnnemyRework.Boss.Jean
{
    public class ShieldManager : MonoBehaviour
    {
        [SerializeField] private Vector2 minScale;
        [SerializeField] private Vector2 maxScale;
        private bool isShieldActive = true;

        private float shieldPercent = 1;

        public float ShieldPercent
        {
            get
            {
                return shieldPercent;
            }
        }

        public bool IsShieldActive
        {
            get
            {
                return isShieldActive;
            }

            set
            {
                isShieldActive = value;
            }
        }

        public void UseShield(float percentageUsed)
        {
            shieldPercent -= percentageUsed;

            if (shieldPercent < 0)
                shieldPercent = 0;

            transform.localScale = (maxScale * shieldPercent) + (minScale - minScale * shieldPercent);
        }
    }
}


