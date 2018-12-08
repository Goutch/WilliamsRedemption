using Game.Audio;
using Game.Entity;
using Game.Entity.Enemies.Attack;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Edgar
{
    public class EdgarController : BossController
    {
        [Header("Sound")] [SerializeField] private AudioClip woundedSound;
        [SerializeField] private GameObject soundToPlayPrefab;

        private RootMover mover;
        private ProjectileManager projectileManager;

        protected override bool OnHit(HitStimulus hitStimulus)
        {
            if (hitStimulus.Type == HitStimulus.DamageType.Darkness)
            {
                base.OnHit(hitStimulus);
                return true;
            }
            else if (hitStimulus.Type == HitStimulus.DamageType.Physical)
            {
                return true;
            }

            return false;
        }

        private void OnEnable()
        {
            mover = GetComponent<RootMover>();
            projectileManager = GetComponent<ProjectileManager>();
            mover.LookAtPlayer();
            health.OnHealthChange += CallWoundedSound;
            health.OnDeath += Health_OnDeath;
        }

        private void Health_OnDeath(GameObject receiver, GameObject attacker)
        {
            projectileManager.Clear();
        }

        private void OnDisable()
        {
            health.OnDeath -= Health_OnDeath;
            health.OnHealthChange -= CallWoundedSound;
        }

        private void CallWoundedSound(GameObject receiver, GameObject attacker)
        {
            SoundCaller.CallSound(woundedSound, soundToPlayPrefab, gameObject, true);
        }
    }
}