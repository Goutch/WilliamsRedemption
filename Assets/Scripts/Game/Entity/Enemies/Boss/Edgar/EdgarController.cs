using Game.Entity;
using Game.Entity.Enemies.Attack;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Edgar
{
    public class EdgarController : BossController
    {
        [Header("Sound")] [SerializeField] private AudioClip woundedSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        private GameObject soundToPlay;

        private RootMover mover;

        protected override bool OnHit(HitStimulus hitStimulus)
        {
            if(hitStimulus.Type == HitStimulus.DamageType.Darkness)
            {
                health.Hit(hitStimulus.gameObject);
                return true;
            } 
            else if(hitStimulus.Type == HitStimulus.DamageType.Physical)
            {
                return true;
            }

            return false;
        }

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

        private void CallWoundedSound(GameObject gameObject, GameObject gameObject2)
        {
            soundToPlay = Instantiate(soundToPlayPrefab, transform.position, Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(woundedSound, true, gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }
}