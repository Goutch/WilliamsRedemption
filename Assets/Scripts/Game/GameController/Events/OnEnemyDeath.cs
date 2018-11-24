using Game.Entity.Enemies;
using Harmony;

namespace Game.Controller.Events
{
    public class OnEnemyDeath:IEvent
    {
        private Enemy enemy;
        public Enemy Enemy => enemy;

        public OnEnemyDeath(Enemy enemy)
        {
           this.enemy =enemy;
        }
    }
}