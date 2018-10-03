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
        [SerializeField] private bool IsDumbEnoughToFall;

        protected RootMover rootMover;
        protected SpriteRenderer spriteRenderer;
        protected int currenDirection = 1;
        private const int SURROUNDING_RANGE = 1;

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

            rootMover.WalkToward(currenDirection, speed);

            bool[,] surrounding = new bool[SURROUNDING_RANGE * 2 + 1, SURROUNDING_RANGE * 2 + 1];
            surrounding = PathFinder.instance.GetSurrounding(SURROUNDING_RANGE, transform.position);
            if (surrounding[currenDirection + SURROUNDING_RANGE, SURROUNDING_RANGE])
            {
                if (!rootMover.IsJumping)
                    rootMover.Jump(new Vector2(jumpForce.x * currenDirection, jumpForce.y));
            }

            if (IsDumbEnoughToFall == false && IsThereAHole(surrounding))
            {
                
            }
        }

        private bool IsThereAHole(bool[,] surrounding)
        {
            for (int i = 0; i < SURROUNDING_RANGE; ++i)
            {
                if (surrounding[currenDirection + SURROUNDING_RANGE, SURROUNDING_RANGE+i])
                {                   
                    return false;
                }
            } 
            return true;
        }
    }
}