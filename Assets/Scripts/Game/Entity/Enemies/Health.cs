using AnimatorExtension;
using Game.Controller;
using Game.Controller.Events;
using Game.Entity.Enemies;
using Game.Entity.Enemies.Attack;
using UnityEngine;

namespace Game.Entity
{
    public delegate void HealthEventHandler(GameObject receiver, GameObject attacker);

    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        public int MaxHealth => maxHealth;

        public event HealthEventHandler OnDeath;
        public event HealthEventHandler OnHealthChange;
        private GameController gameController;

        private Animator animator;

        private bool isKilledByPlayer = true;
        public int HealthPoints { get; private set; }

        public void Hit(GameObject attacker)
        {
            HealthPoints = HealthPoints - 1;

            OnHealthChange?.Invoke(gameObject, attacker);

            if (animator != null && animator.ContainsParam(Values.AnimationParameters.Enemy.Hurt))
                animator.SetTrigger(Values.AnimationParameters.Enemy.Hurt);

            if (IsDead())
            {
                OnDeath?.Invoke(transform.root.gameObject, attacker);
            }
        }

        void Awake()
        {
            HealthPoints = MaxHealth;
            animator = GetComponent<Animator>();
            gameController = GameObject.FindGameObjectWithTag(Values.Tags.GameController)
                .GetComponent<GameController>();
        }

        public void Kill(GameObject killer)
        {
            HealthPoints = 0;

            OnHealthChange?.Invoke(gameObject, killer);
            OnDeath?.Invoke(transform.root.gameObject, killer);
        }

        private bool IsDead()
        {
            return HealthPoints <= 0;
        }

        private void AddEnemyScoreToGameScore()
        {
            int score = GetComponent<Enemy>().ScoreValue;
            GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<GameController>().AddScore(score);
        }

        private bool IsAnEnemy()
        {
            return GetComponent<Enemy>() != null;
        }

        public void ResetHealth()
        {
            HealthPoints = MaxHealth;
        }
    }
}