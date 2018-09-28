using System.Runtime.Serialization.Formatters;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.Experimental.UIElements.Slider;

namespace Playmode.EnnemyRework
{
    [CreateAssetMenu(fileName = "WalkToward", menuName = "EnnemyStrategy/WalkToward", order = 1)]
    public abstract class WalkTowardPlayerEnnemy : Enemy
    {
        [SerializeField] private Vector2 jumpForce;
        protected RootMover rootMover;
        protected SpriteRenderer spriteRenderer;
        protected int currenDirection = 1;

        protected void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rootMover = GetComponent<RootMover>();
        }

        protected void FixedUpdate()
        {
            currenDirection = PlayerController.instance.transform.position.x - rootMover.transform.root.position.x > 0
                ? 1
                : -1;
            if (currenDirection == 1)
                spriteRenderer.flipX = false;
            else
            {
                spriteRenderer.flipX = true;
            }

            rootMover.WalkToward(currenDirection, speed);
            int surroundingRange = 1;
            bool[,] surrounding = new bool[surroundingRange*2+1,surroundingRange*2+1];
            surrounding = PathFinder.instance.GetSurrounding(surroundingRange, rootMover.transform.position);
            if (surrounding[currenDirection + surroundingRange, 0 + surroundingRange])
            {
                if (!rootMover.IsJumping)
                    rootMover.Jump(new Vector2(jumpForce.x * currenDirection, jumpForce.y));
            }
        }

        public override void ReceiveDamage()
        {
            throw new System.NotImplementedException();
        }
    }
}