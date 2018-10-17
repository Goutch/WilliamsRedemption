using System.Runtime.Serialization.Formatters;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.Experimental.UIElements.Slider;

namespace Playmode.EnnemyRework
{
    public abstract class WalkTowardPlayerEnnemy : Enemy
    {
        [SerializeField] private bool isDumbEnoughToFall;
        [SerializeField] private int surroundingRange;

        protected RootMover rootMover;
        private SpriteRenderer spriteRenderer;
        protected int currenDirection = 1;      

        protected override void Init()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rootMover = GetComponent<RootMover>();
        }

        protected virtual void FixedUpdate()
        {
            UpdateDirection();
            UpdateSpriteDirection();
          
            bool[,] surrounding = PathFinder.instance.GetSurrounding(surroundingRange, transform.position);  
            UpdateMovement(surrounding);         
            UpdateJump(surrounding);                
        }

        private void UpdateDirection()
        {
            currenDirection = PlayerController.instance.transform.position.x - transform.root.position.x > 0
                ? 1
                : -1;
        }
        
        private void UpdateSpriteDirection()
        {
            if (currenDirection == 1)
            {
                spriteRenderer.flipX = false;
            }       
            else
            {
                spriteRenderer.flipX = true;
            }
        }
        
        private void UpdateMovement(bool[,] surrounding)
        {
            if (IsThereAHole(surrounding)==false || isDumbEnoughToFall)
            {
                rootMover.WalkToward(currenDirection);
            }
            else if (IsThereAHole(surrounding))
            {
                rootMover.WalkToward(0);
            }
        }

        private void UpdateJump(bool[,] surrounding)
        {
            if (surrounding[currenDirection + surroundingRange, surroundingRange])
            {
                if (!rootMover.IsJumping)
                    rootMover.Jump();
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