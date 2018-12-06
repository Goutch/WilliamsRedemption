using Harmony;
using System.Collections;
using Game.Audio;
using UnityEngine;
using Game.Entity.Player;

namespace Game.Entity.Enemies.Attack
{
    [RequireComponent(typeof(HitStimulus))]
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] public float delayBeforeDestruction;
        [SerializeField] private bool destroyOnPlatformsCollision = true;

        [Header("Sound")] [SerializeField] private AudioClip projectileSound;
        [SerializeField] protected GameObject soundToPlayPrefab;
        [SerializeField] private float maximumDistanceSoundX;
        [SerializeField] private float maximumDistanceSoundY;

        protected HitStimulus hitStimulus;
        public GameObject target;
        protected float birth;

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public bool DestroyOnPlatformsCollision
        {
            get { return destroyOnPlatformsCollision; }

            set { destroyOnPlatformsCollision = value; }
        }

        protected virtual void Awake()
        {
            Vector2 distancePlayerAndProjectile = transform.position - GameObject.FindGameObjectWithTag(Values.Tags.Player).transform.position;
            if (Mathf.Abs(distancePlayerAndProjectile.x) < maximumDistanceSoundX &&
                Mathf.Abs(distancePlayerAndProjectile.y) < maximumDistanceSoundY)
            {
                SoundCaller.CallSound(projectileSound, soundToPlayPrefab, gameObject, false);
            }

            hitStimulus = GetComponent<HitStimulus>();
            hitStimulus.OnHitStimulusSensed += HitStimulus_OnHitStimulusSensed;

            birth = Time.time;
        }

        private void Update()
        {
            if (Time.time - birth > delayBeforeDestruction)
                Destroy(gameObject);
        }

        private void HitStimulus_OnHitStimulusSensed(HitSensor hitSensor)
        {
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if(target != null)
                transform.rotation = TargetDirection(target.transform.position);

            transform.Translate(Speed * Time.deltaTime, 0, 0);
        }

        private Quaternion TargetDirection(Vector3 target)
        {
            Vector2 dir = target - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion direction = Quaternion.AngleAxis(angle, Vector3.forward);
            return direction;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(Values.Layers.Platform) &&
                destroyOnPlatformsCollision)
            {
                Destroy(gameObject);
            }
        }
    }
}