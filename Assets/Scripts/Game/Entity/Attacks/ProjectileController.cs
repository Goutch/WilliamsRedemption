using Harmony;
using System.Collections;
using UnityEngine;
using Game.Entity.Player;

namespace Game.Entity.Enemies.Attack
{
    [RequireComponent(typeof(HitStimulus))]
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float delayBeforeDestruction;
        [SerializeField] private bool destroyOnPlatformsCollision = true;
        
        [Header("Sound")] [SerializeField] private AudioClip projectileSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        [SerializeField] private int maximumDistanceBetweenPlayerAndObjectSound;
       
        protected HitStimulus hitStimulus;

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
            if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag(Values.Tags.Player)
                    .transform.position) < maximumDistanceBetweenPlayerAndObjectSound)
            {
                SoundCaller.CallSound(projectileSound, soundToPlayPrefab, gameObject, false);
            }
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
    }

}

