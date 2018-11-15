using Harmony;
using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float delayBeforeDestruction;
        [SerializeField] private bool canBeReturned;
        [SerializeField] private bool destroyOnPlatformsCollision = true;

        protected int direction;
        public bool CanBeReturned
        {
            get { return canBeReturned; }
            private set { canBeReturned = value; }
        }

        public float Speed { get { return speed; } set { speed = value; } }

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
            StartCoroutine(Destroy());
            direction = 1;
            GetComponent<HitSensor>().OnHit += HandleCollision;
        }
        void FixedUpdate()
        {
            transform.Translate(Speed * Time.deltaTime * direction, 0, 0);
        }
        private IEnumerator Destroy()
        {
            yield return new WaitForSecondsRealtime(delayBeforeDestruction);
            Destroy(this.gameObject);
        }

        protected virtual void HandleCollision(HitStimulus other)
        {
            if (CanBeReturned && other.GetComponent<HitStimulus>().DamageSource == HitStimulus.DamageSourceType.Reaper && other.GetComponent<MeleeAttackController>())
            {
                this.GetComponent<HitStimulus>().SetDamageSource(other.GetComponent<HitStimulus>().DamageSource);
                direction *= -1;
            }
            else if (other.Root().CompareTag(Values.Tags.Player) && this.GetComponent<HitStimulus>().DamageSource == HitStimulus.DamageSourceType.Enemy)
            {
                Destroy(this.gameObject);
            }
            else if (other.Root().CompareTag(Values.Tags.Enemy) && this.GetComponent<HitStimulus>().DamageSource == HitStimulus.DamageSourceType.William)
            {
                Destroy(this.gameObject);
            }
            else if (other.Root().CompareTag(Values.Tags.Enemy) && this.GetComponent<HitStimulus>().DamageSource == HitStimulus.DamageSourceType.Reaper)
            {
                Destroy(this.gameObject);
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(Values.Tags.Plateforme) && destroyOnPlatformsCollision)
                Destroy(gameObject);

            if (collision.gameObject.Root().CompareTag(Values.Tags.Player) && this.GetComponent<HitStimulus>().DamageSource == HitStimulus.DamageSourceType.Enemy && collision.gameObject.GetComponent<HitSensor>())
            {
                Destroy(this.gameObject);
            }
            else if (collision.gameObject.Root().CompareTag(Values.Tags.Enemy) && this.GetComponent<HitStimulus>().DamageSource == HitStimulus.DamageSourceType.William && collision.gameObject.GetComponent<HitSensor>())
            {
                Destroy(this.gameObject);
            }
            else if (collision.gameObject.Root().CompareTag(Values.Tags.Enemy) && this.GetComponent<HitStimulus>().DamageSource == HitStimulus.DamageSourceType.Reaper && collision.gameObject.GetComponent<HitSensor>())
            {
                Destroy(this.gameObject);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Values.Tags.Plateforme) && destroyOnPlatformsCollision)
                Destroy(gameObject);

            if (other.Root().CompareTag(Values.Tags.Player) && this.GetComponent<HitStimulus>().DamageSource == HitStimulus.DamageSourceType.Enemy && other.GetComponent<HitSensor>())
            {
                Destroy(this.gameObject);
            }
            else if (other.Root().CompareTag(Values.Tags.Enemy) && this.GetComponent<HitStimulus>().DamageSource == HitStimulus.DamageSourceType.William && other.GetComponent<HitSensor>())
            {
                Destroy(this.gameObject);
            }
            else if (other.Root().CompareTag(Values.Tags.Enemy) && this.GetComponent<HitStimulus>().DamageSource == HitStimulus.DamageSourceType.Reaper && other.GetComponent<HitSensor>())
            {
                Destroy(this.gameObject);
            }
        }
    }

}

