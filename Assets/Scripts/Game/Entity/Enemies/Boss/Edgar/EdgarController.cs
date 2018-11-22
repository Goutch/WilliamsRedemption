using Game.Entity;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Edgar
{
    public class EdgarController : BossController
    {
        [Header("Sound")] [SerializeField] private AudioClip woundedSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        private GameObject soundToPlay;

        private RootMover mover;

        private void OnEnable()
        {
            mover = GetComponent<RootMover>();
            mover.LookAtPlayer();
            health.OnHealthChange += CallWoundedSound;
        }

        private void OnDisable()
        {
            health.OnHealthChange -= CallWoundedSound;
        }

        protected override void OnHit(HitStimulus other)
        {
            if (other.DamageSource == HitStimulus.DamageSourceType.Reaper)
            {
                health.Hit();
                animator.SetTrigger(Values.AnimationParameters.Edgar.Hurt);
            }
        }

        private void CallWoundedSound(GameObject gameObject)
        {
            soundToPlay = Instantiate(soundToPlayPrefab, transform.position, Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(woundedSound, true, gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }
}