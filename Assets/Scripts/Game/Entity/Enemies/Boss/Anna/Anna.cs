using Game.Entity.Enemies.Attack;
using Game.Entity.Enemies.Boss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Anna
{
    public class Anna : BossController
    {
        [Header("Sound")] [SerializeField] private AudioClip woundedSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        
        protected override bool OnHit(HitStimulus hitStimulus)
        {
            if (IsInvulnerable && hitStimulus.Type == HitStimulus.DamageType.Enemy)
            {
                IsInvulnerable = false;
                animator.SetBool(Values.AnimationParameters.Anna.Invulnerable, false);

                return true;
            }
            else if(hitStimulus.Type == HitStimulus.DamageType.Enemy)
            {
                base.OnHit(hitStimulus);

                return true;
            }
            else if (!IsInvulnerable && (hitStimulus.Type == HitStimulus.DamageType.Darkness ||
                                         hitStimulus.Type == HitStimulus.DamageType.Physical))
            {
                health.Hit(hitStimulus.gameObject);
                return true;
            }

            return true;
        }
        
        private void OnEnable()
        {
            health.OnHealthChange += CallWoundedSound;
        }

        private void OnDisable()
        {
            health.OnHealthChange -= CallWoundedSound;
        }

        private void CallWoundedSound(GameObject receiver, GameObject attacker)
        {
            Audio.SoundCaller.CallSound(woundedSound, soundToPlayPrefab, gameObject, true);
        }
    }
}