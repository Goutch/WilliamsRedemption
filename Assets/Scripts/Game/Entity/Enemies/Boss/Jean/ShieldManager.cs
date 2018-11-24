using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jean
{
    public class ShieldManager : MonoBehaviour
    {
        [SerializeField] private Vector2 minScale;
        [SerializeField] private Vector2 maxScale;
        [SerializeField] private GameObject shield;
        private float shieldPercent = 1;

        public float ShieldPercent
        {
            get { return shieldPercent; }
            set
            {
                shieldPercent = value;
                if (ShieldPercent < 0)
                    ShieldPercent = 0;

                shield.transform.localScale = (maxScale * ShieldPercent) + (minScale - minScale * ShieldPercent);
            }
        }

        public bool IsShieldActive
        {
            get { return shield.activeSelf; }

            set { shield.SetActive(value); }
        }

        public void UseShield(float percentageUsed)
        {
            ShieldPercent -= percentageUsed;

            if (ShieldPercent < 0)
                ShieldPercent = 0;

            shield.transform.localScale = (maxScale * ShieldPercent) + (minScale - minScale * ShieldPercent);
        }
    }
}