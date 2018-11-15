using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    public class HitStimulus : MonoBehaviour
    {
        public enum DamageType
        {
            Darkness,
            Physical,
            Enemy
        }

        public enum AttackRange
        {
            Melee,
            Ranger
        }

        [SerializeField] private DamageType type;
        [SerializeField] private AttackRange range;
        [SerializeField] private bool destroyOnCollision;

        public DamageType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }
        public AttackRange Range
        {
            get
            {
                return range;
            }

            set
            {
                range = value;
            }
        }
        public bool DestroyOnCollision
        {
            get
            {
                return destroyOnCollision;
            }

            set
            {
                destroyOnCollision = value;
            }
        }

    }
}
