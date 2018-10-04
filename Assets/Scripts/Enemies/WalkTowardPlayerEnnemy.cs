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
        [SerializeField] private bool isDumbEnoughToFall;
        [SerializeField] private int surroundingRange;

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
          
            bool[,] surrounding = new bool[surroundingRange * 2 + 1, surroundingRange * 2 + 1];
            surrounding = PathFinder.instance.GetSurrounding(surroundingRange, transform.position);
            
            if (isDumbEnoughToFall==false && IsThereAHole(surrounding)==false || isDumbEnoughToFall)
            {
                rootMover.WalkToward(currenDirection, speed);
            }

            if (isDumbEnoughToFall == false && IsThereAHole(surrounding))
            {
                Debug.Log("Trou");
                rootMover.WalkToward(0, speed);
            }
            if (surrounding[currenDirection + surroundingRange, surroundingRange])
            {
                if (!rootMover.IsJumping)
                    rootMover.Jump(new Vector2(jumpForce.x * currenDirection, jumpForce.y));
            }           
        }

        private bool IsThereAHole(bool[,] surrounding)
        {
            for (int i = 0; i <= surroundingRange; ++i)
            {
                if (surrounding[currenDirection*surroundingRange + surroundingRange, surroundingRange-i])
                {                   
                    return false;
                }
            } 
            return true;
        }
    }
}