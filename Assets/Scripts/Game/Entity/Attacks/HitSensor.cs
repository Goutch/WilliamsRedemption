using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    public delegate bool OnHitEventHandler(HitStimulus hitStimulus);

    public class HitSensor : MonoBehaviour
    {
        public event OnHitEventHandler OnHit;

        //BEN_REVIEW : J'aurais appellé ça "Hit" non ?
        public bool Notify(HitStimulus hitStimulus)
        {
            bool hit = false;
            if (OnHit != null)
            {
                foreach (OnHitEventHandler onHitDelegate in OnHit.GetInvocationList())
                {
                    hit = hit || onHitDelegate(hitStimulus);
                }
            }

            return hit;
        }
    }
}