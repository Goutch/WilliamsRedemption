using UnityEngine;

namespace Playmode.EnnemyRework
{
    [CreateAssetMenu(fileName = "Boss1", menuName = "EnnemyStrategy/Boss1", order = 1)]
    public class Boss1:Enemy
    {
        private RootMover mover;
        public override void Init(GameObject enemyControllerObject)
        {
            mover = enemyControllerObject.GetComponent<RootMover>();
        }

        public override void Act()
        {
            mover.FlyToward(PlayerController.instance.transform.position,speed);
        }
    }
}