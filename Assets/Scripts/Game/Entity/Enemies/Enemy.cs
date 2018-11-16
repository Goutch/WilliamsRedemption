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
            hitSensor.OnHit += OnHit;

            Init();
        }

        protected virtual void OnHit(HitStimulus hitStimulus)
        {
            if(hitStimulus.Type != HitStimulus.DamageType.Enemy)
            {
                health.Hit();
            }
        }

        protected abstract void Init();


        private void OnDeath(GameObject gameObject)
        {
            Destroy(this.gameObject);
        }
    }
}