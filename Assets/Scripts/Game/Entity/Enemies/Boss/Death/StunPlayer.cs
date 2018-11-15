using Game.Entity.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Death
{
    [RequireComponent(typeof(Animator))]
    public class StunPlayer : Capacity
    {
        [SerializeField] private float stunDuration;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public override void Act()
        {
            
        }

        public override bool CanEnter()
        {
            return true;
        }

        public override void Enter()
        {
            base.Enter();

            animator.SetTrigger(Values.AnimationParameters.Death.StunPlayer);

            PlayerController.instance.StunPlayer(stunDuration);

            StartCoroutine(PlayerStunFinish());
        }

        private IEnumerator PlayerStunFinish()
        {
            yield return new WaitForSeconds(stunDuration);
            Finish();
        }
    }
}
