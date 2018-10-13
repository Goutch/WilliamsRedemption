using Boss;
using UnityEngine;

namespace Jacob
{
    [RequireComponent(typeof(Health))]
    class JacobVulnerable : Vulnerable
    {
        private Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            health.OnHealthChange += Health_OnHealthChange;
        }

        private void OnDisable()
        {
            health.OnHealthChange -= Health_OnHealthChange;
        }

        private void Health_OnHealthChange(GameObject gameObject)
        {
            Finish();
        }
    }
}
