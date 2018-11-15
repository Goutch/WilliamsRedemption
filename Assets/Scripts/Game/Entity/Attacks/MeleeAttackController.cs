using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    class MeleeAttackController : MonoBehaviour
    {
        [SerializeField] private float delayBeforeDestruction;

        public AudioClip meleeSound;

        private void Awake()
        {
            UseSound();
            Destroy(this.gameObject, delayBeforeDestruction);
        }

        private void UseSound()
        {
            GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<AudioManager>()
                .PlaySound(meleeSound);
        }

    }
}


