using Game.Puzzle.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Death
{
    class DeathPhase3 : NonSequentialPhase
    {
        [SerializeField] private MeshLight meshLight;

        private Rigidbody2D rb;

        protected override void Init()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public override bool CanEnter()
        {
            return true;
        }

        public override void Enter()
        {
            base.Enter();

            meshLight.Close();

            rb.bodyType = RigidbodyType2D.Static;
        }
    }
}
