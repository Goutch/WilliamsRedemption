using Harmony;

namespace Game.Controller.Events
{
    public class OnCollectableFound:IEvent
    {
        private Level level;
      
        public Level Level => level;

        public OnCollectableFound(Level level)
        {
            this.level = level;
        }
    }
}