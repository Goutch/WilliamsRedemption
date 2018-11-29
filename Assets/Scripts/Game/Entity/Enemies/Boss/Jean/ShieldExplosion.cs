using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jean
{
    public class ShieldExplosion : Capacity
    {
        [SerializeField] private GameObject shieldExplosion;
        [SerializeField] private float delayBetweenExplosion;
        [SerializeField] private int numberOfExplosion;

        private ShieldManager shieldManager;
        private Animator animator;

        private int numberOfExplosionSpawned = 0;
        private int numberOfExplosionAlive = 0;

        protected override void Init()
        {
            shieldManager = GetComponent<ShieldManager>();
            animator = GetComponent<Animator>();
        }

        public override void Finish()
        {
            base.Finish();
        }

        public override void Act()
        {
        }

        public override bool CanEnter()
        {
            if (shieldManager.ShieldPercent == 0)
                return true;
            else
                return false;
        }

        public override void Enter()
        {
            base.Enter();

            numberOfExplosionAlive = 0;
            numberOfExplosionSpawned = 0;

            shieldManager.IsShieldActive = false;

            animator.SetTrigger(Values.AnimationParameters.Jean.ShieldExplosion);

            StartCoroutine(SpawnShieldExplosions());
        }

        private IEnumerator SpawnShieldExplosions()
        {
            for (int i = 0; i < numberOfExplosion; ++i)
            {
                SpawnShieldExplosion();
                yield return new WaitForSeconds(delayBetweenExplosion);
            }
        }

        private void SpawnShieldExplosion()
        {
            GameObject explosion = Instantiate(shieldExplosion, shieldManager.transform.position,
                shieldManager.transform.rotation);


            explosion.GetComponent<ShieldExplosionController>().OnDestroy += ShieldExplosion_OnDestroy;

            numberOfExplosionAlive++;
            numberOfExplosionSpawned++;
        }

        private void ShieldExplosion_OnDestroy(GameObject gameObject)
        {
            gameObject.GetComponent<ShieldExplosionController>().OnDestroy -= ShieldExplosion_OnDestroy;

            numberOfExplosionAlive--;

            if (numberOfExplosionSpawned == numberOfExplosion && numberOfExplosionAlive == 0)
            {
                Finish();
            }
        }
    }
}