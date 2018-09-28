namespace Playmode.EnnemyRework
{
    public class Zombie:WalkTowardPlayerEnnemy
    {
        public override void ReceiveDamage()
        {
            if(PlayerController.instance.CurrentController is ReaperController)
            base.ReceiveDamage();
        }
    }
}