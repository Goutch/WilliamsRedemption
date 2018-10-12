using System.Runtime.Serialization.Formatters;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.Experimental.UIElements.Slider;

namespace Playmode.EnnemyRework
{
    public abstract class WalkTowardPlayerEnnemy : Enemy
    {
        [SerializeField] private Vector2 jumpForce;
        protected RootMover rootMover;
        protected SpriteRenderer spriteRenderer;
        protected int currenDirection = 1;

        protected override void Init()
        {
            
            spriteRenderer = GetComponent<SpriteRenderer>();
            rootMover = GetComponent<RootMover>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void FixedUpdate()
        {
            currenDirection = PlayerController.instance.transform.position.x - transform.root.position.x > 0
                ? 1
                : -1;
            if (currenDirection == 1)
                spriteRenderer.flipX = false;
            else
            {
                spriteRenderer.flipX = true;
            }

            rootMover.WalkToward(currenDirection);
            int surroundingRange = 1;
            bool[,] surrounding = new bool[surroundingRange*2+1,surroundingRange*2+1];
            surrounding = PathFinder.instance.GetSurrounding(surroundingRange, transform.position);
            if (surrounding[currenDirection + surroundingRange, 0 + surroundingRange])
            {
                if (!rootMover.IsJumping)
                    rootMover.Jump();
            }
        }
    }
}