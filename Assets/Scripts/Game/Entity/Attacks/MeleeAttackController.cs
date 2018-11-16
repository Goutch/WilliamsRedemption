using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    class MeleeAttackController : MonoBehaviour
    {
        [SerializeField] private float delayBeforeDestruction;

        private void Awake()
        {
            Destroy(this.gameObject, delayBeforeDestruction);
        }
    }
}


