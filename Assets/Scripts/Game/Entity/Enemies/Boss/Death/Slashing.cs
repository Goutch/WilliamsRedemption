using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Audio;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Death
{
    [RequireComponent(typeof(Animator))]
    public class Slashing : Capacity
    {
        [Header("Sound")] [SerializeField] private AudioClip attackSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        
        private Animator animator;

        protected override void Init()
        {
            animator = GetComponent<Animator>();
        }

        public override void Act()
        {
        }

        public override void Enter()
        {
            base.Enter();

            animator.SetTrigger(Values.AnimationParameters.Death.Slash);
            SoundCaller.CallSound(attackSound, soundToPlayPrefab, gameObject, false);
        }

        public override bool CanEnter()
        {
            return true;
        }

        [UsedImplicitly]
        public void SlashingEnd()
        {
            Finish();
        }
    }
}