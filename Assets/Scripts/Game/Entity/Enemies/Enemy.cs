using Game.Entity.Player;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] private int scoreValue = 0;
        
        protected Health health;
        protected Animator animator;
        protected SpriteRenderer spriteRenderer;
        public int ScoreValue => scoreValue;
        public bool IsInvulnerable { get; set; }

        protected void Awake()
        {
            health = GetComponent<Health>();
            GetComponentInChildren<HitSensor>().OnHit  += OnHit;
            health.OnDeath += OnDeath;
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            Init();
        }

        protected abstract void Init();

        protected virtual void OnHit(HitStimulus other)
        {
            if (!IsInvulnerable && (other.DamageSource == HitStimulus.DamageSourceType.Reaper || other.DamageSource == HitStimulus.DamageSourceType.William))
                health.Hit();
        }

        private void OnDeath(GameObject gameObject)
        {
            Destroy(this.gameObject);
        }
        protected int UpdateDirection()
        {
            float dist = PlayerController.instance.transform.position.x - transform.root.position.x;
            int dir=0;
            if (dist > -0.1 && dist < 0.01)
                dir = 0;
            else
                dir = dist > 0
                    ? 1
                    : -1;
            UpdateSpriteDirection(dir);
            return dir;
        }
        protected void UpdateSpriteDirection(int dir)
        {
            if (dir == 1)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
    }
}