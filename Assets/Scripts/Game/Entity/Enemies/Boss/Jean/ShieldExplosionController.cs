using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jean
{
    class ShieldExplosionController : MonoBehaviour
    {
        public delegate void OnDestroyHandler(GameObject gameObject);

        public event OnDestroyHandler OnDestroy;

        [SerializeField] private float maxScale;
        [SerializeField] private float scaleSpeed;

        private void FixedUpdate()
        {
            if (transform.localScale.x > maxScale)
            {
                Destroy(this.gameObject);
                OnDestroy?.Invoke(this.gameObject);
            }
            else
            {
                transform.localScale = transform.localScale + new Vector3(scaleSpeed, scaleSpeed, 0);
            }
        }
    }
}