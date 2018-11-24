namespace Game.Entity.Enemies.Boss.Jean
{
    class JeanPhase : SequentialLoopPhase
    {
        private RootMover mover;

        protected override void Init()
        {
            mover = GetComponent<RootMover>();
        }

        public override void Act()
        {
            mover.LookAtPlayer();
            base.Act();
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override bool CanEnter()
        {
            return true;
        }

        protected override void EnterIdle()
        {
        }
    }
}