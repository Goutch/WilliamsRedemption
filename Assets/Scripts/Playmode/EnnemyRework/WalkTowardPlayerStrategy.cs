using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.Experimental.UIElements.Slider;

namespace Playmode.EnnemyRework
{
    [CreateAssetMenu(fileName = "WalkToward", menuName = "EnnemyStrategy/WalkToward", order = 1)]
    public class WalkTowardPlayerStrategy : Enemy
    {
        [SerializeField] private Vector2 jumpForce;
        protected Animator animator;
        protected RootMover rootMover;
        protected SpriteRenderer spriteRenderer;
        protected Rigidbody2D ennemyRigidbody2D;
        protected int currenDirection = 1;

        public override void Init(GameObject enemyControllerObject)
        {
            spriteRenderer = enemyControllerObject.GetComponent<SpriteRenderer>();
            rootMover = enemyControllerObject.GetComponent<RootMover>();
            ennemyRigidbody2D = enemyControllerObject.GetComponent<Rigidbody2D>();
        }

        public override void Act()
        {
            
            currenDirection= PlayerController.instance.transform.position.x - rootMover.transform.root.position.x > 0
                ? 1
                : -1;

            if (currenDirection == 1)
                spriteRenderer.flipX = false;
            else
            {
                spriteRenderer.flipX = true;
            }
            rootMover.WalkToward(currenDirection,speed);
            int surroundingRange = 1;
            bool[,] surrounding=new bool[3,3];
            surrounding = PathFinder.instance.GetSurrounding(surroundingRange, rootMover.transform.position);
            if (surrounding[currenDirection+surroundingRange, 0+surroundingRange])
            {
                if(!rootMover.IsJumping)
                rootMover.Jump(new Vector2(jumpForce.x*currenDirection,jumpForce.y));
            }
        }
    }
}