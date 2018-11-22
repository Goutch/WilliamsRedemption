using Game.Controller;
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
        private GameController gameController;
        public int ScoreValue => scoreValue;
        public bool IsInvulnerable { get; set; }

        protected void Awake()
        {
            player = GameObject.FindWithTag(Values.Tags.Player).GetComponent<PlayerController>();
            gameController = GameObject.FindGameObjectWithTag(Values.GameObject.GameController)
                .GetComponent<GameController>();
            health = GetComponent<Health>();
            health.OnDeath += OnDeath;
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            hitSensor = GetComponent<HitSensor>();
            hitSensor.OnHit += OnHit;

            Init();
        }

        protected virtual bool OnHit(HitStimulus hitStimulus)
        {
            if (hitStimulus.Type != HitStimulus.DamageType.Enemy)
            {
                health.Hit(hitStimulus.gameObject);
                return true;
            }

            return false;
        }

        protected abstract void Init();


        private void OnDeath(GameObject receiver, GameObject attacker)
        {
            HitStimulus attackerStimulus = attacker.GetComponent<HitStimulus>();

            if (attackerStimulus != null&&
                (attackerStimulus.Type == HitStimulus.DamageType.Darkness ||
                 attackerStimulus.Type == HitStimulus.DamageType.Physical))
            {
                gameController.AddScore(scoreValue);
            }
            Destroy(this.gameObject);
        }
    }
}