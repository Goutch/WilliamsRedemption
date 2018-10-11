using Edgar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacob
{
    class JacobVulnerable : Vulnerable
    {
        private Health health;

        private new void Awake()
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

        private void Health_OnHealthChange(UnityEngine.GameObject gameObject)
        {
            Finish();
        }
    }
}
