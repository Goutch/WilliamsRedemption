using Game.Entity.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jean
{
    class ShieldRecuperation : Vulnerable
    {
        [SerializeField] private Vector2 endForceAddToPlayer;
        [SerializeField] private float delayBeforeBeingActive;

        private ShieldManager shieldManager;
        private Animator animator;

        private bool isFinishing = false;

        private void Awake()
        {
            shieldManager = GetComponent<ShieldManager>();
            animator = GetComponent<Animator>();
        }

        public override void Act()
        {
            base.Act();
        }

        public override void Enter()
        {
            base.Enter();
            isFinishing = false;

            animator.SetTrigger(Values.AnimationParameters.Jean.Idle);
        }

        public override void Finish()
        {
            if(!isFinishing)
                StartCoroutine(WaitBeforeFinish());
        }

        public IEnumerator WaitBeforeFinish()
        {
            isFinishing = true;

            float directionX = transform.rotation.y == 1 ? 1 : -1;

            player.GetComponent<KinematicRigidbody2D>().AddForce(new Vector2(directionX * endForceAddToPlayer.x, endForceAddToPlayer.y));

            shieldManager.ShieldPercent = 1;
            shieldManager.IsShieldActive = true;

            yield return new WaitForSeconds(delayBeforeBeingActive);
            base.Finish();
        }

        public override bool CanEnter()
        {
            if (shieldManager.ShieldPercent == 0)
                return true;
            else
                return false;
        }
    }
}