using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    class MeleeAttackController : MonoBehaviour
    {
        [SerializeField] private float delayBeforeDestruction;

        [Header("Sound")] [SerializeField] private AudioClip meleeSound;
        [SerializeField] private GameObject soundToPlayPrefab;

        private void Awake()
        {
            SoundCaller.CallSound(meleeSound, soundToPlayPrefab, gameObject, true);
            Destroy(gameObject, delayBeforeDestruction);
        }
    }
}