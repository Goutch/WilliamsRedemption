using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    public delegate void OnHitEventHandler(HitStimulus hitStimulus);

    public class HitSensor : MonoBehaviour
    {
        public event OnHitEventHandler OnHitEnter;
        public event OnHitEventHandler OnHitExit;

        private List<HitStimulus> hitStimuli;

        private void Awake()
        {
            hitStimuli = new List<HitStimulus>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            HitStimulus stimulus;
            if (stimulus = collision.GetComponent<HitStimulus>())
            {
                hitStimuli.Add(stimulus);
                OnHitEnter?.Invoke(stimulus);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HitStimulus stimulus;
            if (stimulus = collision.gameObject.GetComponent<HitStimulus>())
            {
                hitStimuli.Add(stimulus);
                OnHitEnter?.Invoke(stimulus);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            HitStimulus stimulus;
            if (stimulus = collision.GetComponent<HitStimulus>())
            {
                hitStimuli.Remove(stimulus);
                OnHitExit?.Invoke(stimulus);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            HitStimulus stimulus;
            if (stimulus = collision.gameObject.GetComponent<HitStimulus>())
            {
                hitStimuli.Remove(stimulus);
                OnHitExit?.Invoke(stimulus);
            }
        }
    }
}
