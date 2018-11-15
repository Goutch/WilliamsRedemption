using Harmony;
using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float delayBeforeDestruction;
        [SerializeField] private bool destroyOnPlatformsCollision = true;

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
            Destroy(gameObject, delayBeforeDestruction);
        }

        void FixedUpdate()
        {
            transform.Translate(Speed * Time.deltaTime, 0, 0);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Values.Tags.Plateforme) && destroyOnPlatformsCollision)
            {
                Destroy(gameObject);
            }
        }
    }

}

