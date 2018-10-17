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
        protected SpriteRenderer spriteRenderer;
        protected int currenDirection = 1;

        private void OnEnable()
        {
            
        }

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
            bool[,] surrounding = PathFinder.instance.GetSurrounding(surroundingRange, transform.position);
            
            if (isDumbEnoughToFall==false && IsThereAHole(surrounding)==false || isDumbEnoughToFall)
            {
                rootMover.WalkToward(currenDirection);
            }

            if (isDumbEnoughToFall == false && IsThereAHole(surrounding))
            {
                Debug.Log("Trou");
                rootMover.WalkToward(0);
            }
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