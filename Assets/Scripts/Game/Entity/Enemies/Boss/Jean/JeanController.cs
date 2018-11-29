using Game.Entity.Enemies.Attack;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jean
{
    class JeanController : BossController
    {
        [Header("Sound")] [SerializeField] private AudioClip woundedSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        
        private ShieldManager shieldManager;

        private new void Awake()
        {
            shieldManager = GetComponent<ShieldManager>();
            base.Awake();
        }

        protected override bool OnHit(HitStimulus hitStimulus)
        {
            if (!shieldManager.IsShieldActive)
                return base.OnHit(hitStimulus);
            else
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
            SoundCaller.CallSound(woundedSound, soundToPlayPrefab, gameObject, true);
        }
    }
}