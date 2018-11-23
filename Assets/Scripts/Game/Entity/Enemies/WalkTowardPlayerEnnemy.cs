using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Entity.Enemies
{
    public abstract class WalkTowardPlayerEnnemy : Enemy
    {
        [SerializeField] private bool isDumbEnoughToFall;
        [SerializeField] private int surroundingRange;

        private PathFinder pathFinder;
        private Tilemap[] obstacles;
        public bool[,] surrounding;

        protected RootMover rootMover;

        protected override void Init()
        {
            rootMover = GetComponent<RootMover>();
            obstacles=new Tilemap[]
            {
                GameObject.FindGameObjectWithTag(Values.Tags.Plateforme).GetComponent<Tilemap>(),
                GameObject.FindGameObjectWithTag(Values.Tags.PassThrough).GetComponent<Tilemap>()
            };
            pathFinder = new PathFinder(obstacles);
        }

        protected virtual void FixedUpdate()
        {
            rootMover.LookAtPlayer();

            surrounding = pathFinder.GetSurrounding(surroundingRange, transform.position);
            
            
            UpdateMovement(surrounding);
        }

        private void UpdateMovement(bool[,] surrounding)
        {
            int direction = (transform.rotation.y == 1 ? -1 : 1);
            Vector2Int center = new Vector2Int(surroundingRange, surroundingRange);

            animator.SetBool(Values.AnimationParameters.Enemy.Walking, false);

            if (isDumbEnoughToFall || !IsThereAHole(surrounding))
            {
                rootMover.Walk();
            }
            else
            {
                rootMover.StopWalking();
            }

            if (surrounding[center.x + direction, center.y] && !rootMover.IsJumping)
                rootMover.Jump();
        }

        private bool IsThereAHole(bool[,] surrounding)
        {
            int direction = (transform.rotation.y == 1 ? -1 : 1);
            Vector2Int center = new Vector2Int(surroundingRange, surroundingRange);

            if (!surrounding[center.x + direction, center.y - 1] && !surrounding[center.x + direction, center.y - 2])
            {
                return true;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            if (surrounding != null)
                for (int y = -surroundingRange; y < surroundingRange + 1; y++)
                {
                    for (int x = -surroundingRange; x < surroundingRange + 1; x++)
                    {
                        if (surrounding[x + surroundingRange, y + surroundingRange] == true)
                            Gizmos.DrawCube(new Vector3(transform.position.x + (x * .32f), transform.position.y + (y * .32f), 0), Vector3.one * .16f);
                        else
                        {
                            Gizmos.DrawSphere(new Vector3(transform.position.x + (x * .32f), transform.position.y + (y * .32f), 0), .16f);
                        }
                    }
                }
        }
    }
}