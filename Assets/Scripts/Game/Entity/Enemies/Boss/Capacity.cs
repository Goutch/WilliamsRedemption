namespace Game.Entity.Enemies.Boss
{
    public abstract class Capacity : State
    {
        public override State GetCurrentState()
        {
            return this;
        }
    }
}

