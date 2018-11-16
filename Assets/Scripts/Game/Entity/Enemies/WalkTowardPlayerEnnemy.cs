using UnityEngine;

namespace Game.Entity.Enemies
{
    public abstract class WalkTowardPlayerEnnemy : Enemy
    {
        [SerializeField] private bool isDumbEnoughToFall;
        [SerializeField] private int surroundingRange;

        public bool[,] surrounding;

        protected RootMover rootMover;
        protected int currenDirection = 1;

        protected override void Init()
        {
            rootMover = GetComponent<RootMover>();
        }

        protected virtual void FixedUpdate()
        {
            currenDirection= UpdateDirection();

            surrounding = PathFinder.instance.GetSurrounding(surroundingRange, transform.position);
            
            
            UpdateMovement(surrounding);
            UpdateJump(surrounding);
        }

        private void UpdateMovement(bool[,] surrounding)
        {
            if (!isDumbEnoughToFall && IsThereAHole(surrounding))
            {
                rootMover.WalkToward(0);
            }
            else
            {
                rootMover.WalkToward(currenDirection);
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
            if (!surrounding[currenDirection + surroundingRange, surroundingRange-1]&&!surrounding[currenDirection + surroundingRange, surroundingRange-2])
            {
                return true;
            }
            return false;
        }

        private void OnDrawGizmos()
        {
            if(surrounding!=null)
            for (int x = -surroundingRange; x < surroundingRange+1; x++)
            {
                for (int y = -surroundingRange; y < surroundingRange+1; y++)
                {
                    if (surrounding[x+surroundingRange, y+surroundingRange] == true)
                        Gizmos.DrawCube(new Vector3(transform.position.x + (x*.32f), transform.position.y + (y*.32f), 0),Vector3.one*.16f);
                    else
                    {
                        Gizmos.DrawSphere(new Vector3(transform.position.x + (x*.32f), transform.position.y + (y*.32f), 0), .16f);
                    }
                }
            }
        }
    }
}