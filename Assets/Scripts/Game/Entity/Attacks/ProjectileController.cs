using Harmony;
using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    [RequireComponent(typeof(HitStimulus))]
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float delayBeforeDestruction;
        [SerializeField] private bool destroyOnPlatformsCollision = true;
        [SerializeField] private AudioClip projectileSound;
        [SerializeField] private GameObject soundToPlayPrefab;

        private GameObject soundToPlay;
       
        private HitStimulus hitStimulus;

        public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }
        public bool DestroyOnPlatformsCollision
        {
            get
            {
                return destroyOnPlatformsCollision;
            }

            set
            {
                destroyOnPlatformsCollision = value;
            }
        }

        protected virtual void Awake()
        {
            UseSound();
            hitStimulus = GetComponent<HitStimulus>();
            hitStimulus.OnHitStimulusSensed += HitStimulus_OnHitStimulusSensed;

            Destroy(gameObject, delayBeforeDestruction);
        }

        private void HitStimulus_OnHitStimulusSensed(HitSensor hitSensor)
        {
            Destroy(gameObject);
        }

        void FixedUpdate()
        {
            transform.Translate(Speed * Time.deltaTime, 0, 0);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(Values.Layers.Platform) && destroyOnPlatformsCollision)
            {
                Destroy(gameObject);
            }
        }

        private void UseSound()
        {
            soundToPlay=Instantiate(soundToPlayPrefab,this.transform.position,Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(projectileSound, false, this.gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }

}

