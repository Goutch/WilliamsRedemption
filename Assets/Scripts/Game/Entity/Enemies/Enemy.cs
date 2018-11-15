using Game.Entity.Enemies.Attack;
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
        protected PlayerController player;
        protected HitSensor hitSensor;

        public int ScoreValue => scoreValue;
        public bool IsInvulnerable { get; set; }

        protected void Awake()
        {
            player = GameObject.FindWithTag(Values.Tags.Player).GetComponent<PlayerController>();
            health = GetComponent<Health>();
            health.OnDeath += OnDeath;
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            hitSensor = GetComponent<HitSensor>();
            hitSensor.OnHitEnter += HitSensor_OnHitEnter;

            Init();
        }

        private void HitSensor_OnHitEnter(HitStimulus hitStimulus)
        {
            if(hitStimulus.Type != HitStimulus.DamageType.Enemy)
            {
                health.Hit();

                if (hitStimulus.DestroyOnCollision)
                    Destroy(hitStimulus.gameObject);
            }
        }

        protected abstract void Init();


        private void OnDeath(GameObject gameObject)
        {
            Destroy(this.gameObject);
        }
        protected int UpdateDirection()
        {
            float dist = player.transform.position.x - transform.root.position.x;
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