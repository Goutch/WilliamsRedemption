using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    public delegate void OnHitStimulusSensedEventHandler(HitSensor hitSensor);

    public class HitStimulus : MonoBehaviour
    {
        public event OnHitStimulusSensedEventHandler OnHitStimulusSensed;

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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            HitSensor sensor;
            if (!collision.GetComponent<MarkerIgnoreStimulus>() && (sensor = collision.transform.root.GetComponent<HitSensor>()))
            {
                if(sensor.Notify(this))
                    OnHitStimulusSensed?.Invoke(sensor);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HitSensor sensor;
            if (!collision.gameObject.GetComponent<MarkerIgnoreStimulus>() && (sensor = collision.transform.root.GetComponent<HitSensor>()))
            {
                if (sensor.Notify(this))
                    OnHitStimulusSensed?.Invoke(sensor);
            }
        }
    }
}
