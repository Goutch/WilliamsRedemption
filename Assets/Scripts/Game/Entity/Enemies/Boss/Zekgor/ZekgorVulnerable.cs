using Game.Entity.Enemies.Attack;
using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Zekgor
{
    class ZekgorVulnerable : Vulnerable
    {
        private Animator animator;
        private Health health;
        private Enemy enemy;
        private float hurtTimeInSecond = .5f;
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
            enemy = GetComponent<Enemy>();
        }

        private void OnHurt(GameObject receiver, GameObject attacker)
        {
            animator.SetTrigger(Values.AnimationParameters.ZekGor.Hurt);
            StartCoroutine(hurtRoutine());
        }

        public override void Enter()
        {
            base.Enter();
            health.OnHealthChange += OnHurt;
            animator.SetTrigger(Values.AnimationParameters.ZekGor.Vulnerable);
        }

        public override void Finish()
        {
            health.OnHealthChange -= OnHurt;
            base.Finish();
        }

        private IEnumerator hurtRoutine()
        {
            enemy.IsInvulnerable = true;

            yield return new WaitForSeconds(hurtTimeInSecond);

            enemy.IsInvulnerable = false;
            Finish();
        }
    }
}