using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Zekgor
{
    public class ZekgorJumpPhase : SequentialPhase
    {
        [SerializeField] private float cooldown;

        private float lastTimeUsed;

        public override bool CanEnter()
        {
            return Time.time - lastTimeUsed > cooldown;
        }

        public override void Finish()
        {
            base.Finish();

            lastTimeUsed = Time.time;
        }
    }
}