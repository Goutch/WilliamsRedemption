using Game.Entity;

namespace Game.Entity.Enemies.Boss.Edgar
{
    public class EdgarController : BossController
    {
        private RootMover mover;

        private void OnEnable()
        {
            mover = GetComponent<RootMover>();
            mover.LookAtPlayer();
        }
    }
}


