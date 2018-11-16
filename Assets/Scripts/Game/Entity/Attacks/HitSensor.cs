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
        public event OnHitEventHandler OnHit;

        public void Notify(HitStimulus hitStimulus)
        {
            OnHit?.Invoke(hitStimulus);
        }
    }
}
