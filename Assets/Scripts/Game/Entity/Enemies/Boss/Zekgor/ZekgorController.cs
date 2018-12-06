using Game.Entity.Enemies.Attack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Zekgor
{   
    public class ZekgorController : BossController
    {
        [Header("Sound")] [SerializeField] private AudioClip woundedSound;
        [SerializeField] private AudioClip walkSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        
        protected override bool OnHit(HitStimulus other)
        {
            if (other.Type == HitStimulus.DamageType.Darkness && !IsInvulnerable)
            {
                health.Hit(other.gameObject);
                return true;
            }
            else if (other.Type == HitStimulus.DamageType.Physical)
            {
                return true;
            }

            return false;
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

        public void SoundDuringWalk()
        {
            Audio.SoundCaller.CallSound(walkSound, soundToPlayPrefab, gameObject, true);
        }
    }
}