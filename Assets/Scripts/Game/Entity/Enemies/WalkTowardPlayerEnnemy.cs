using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Entity.Enemies
{
    public delegate void PlayerSightEventHandler();

    public abstract class WalkTowardPlayerEnnemy : Enemy
    {
        public event PlayerSightEventHandler OnPlayerSightEnter;
        public event PlayerSightEventHandler OnPlayerSightExit;

        [SerializeField] private bool isDumbEnoughToFall;
        [SerializeField] private int surroundingRange;
        [SerializeField] private LayerMask layerVisibleByTheUnit;
        [SerializeField] private float sightRange;

        private PathFinder pathFinder;
        private Tilemap[] obstacles;

        public bool[,] surrounding;
        protected RootMover rootMover;
        private bool playerInSight = false;
        private Quaternion? savedState = null;

        private const float epsilonDistancePlayer = 0.05f;

        public bool PlayerInSight
        {
            get
            {
                return playerInSight;
            }

            set
            {
                if (value != playerInSight && value == true)
                    OnPlayerSightEnter?.Invoke();
                else if (value != playerInSight && value == false)
                    OnPlayerSightExit?.Invoke();

                playerInSight = value;
            }
        }

        protected override void Init()
        {
            rootMover = GetComponent<RootMover>();
            obstacles = new Tilemap[]
            {
                GameObject.FindGameObjectWithTag(Values.Tags.Plateforme).GetComponent<Tilemap>(),
                GameObject.FindGameObjectWithTag(Values.Tags.PassThrough).GetComponent<Tilemap>(),
                GameObject.FindGameObjectWithTag(Values.Tags.Wall).GetComponent<Tilemap>(),
                GameObject.FindGameObjectWithTag(Values.Tags.PhysicObjectAI).GetComponent<Tilemap>()
            };
            pathFinder = new PathFinder(obstacles);
        }

        protected virtual void OnEnable()
        {
            OnPlayerSightEnter += WalkTowardPlayerEnnemy_OnPlayerSightEnter;
            OnPlayerSightExit += WalkTowardPlayerEnnemy_OnPlayerSightExit;
        }

        protected virtual void OnDisable()
        {
            OnPlayerSightEnter -= WalkTowardPlayerEnnemy_OnPlayerSightEnter;
            OnPlayerSightExit -= WalkTowardPlayerEnnemy_OnPlayerSightExit;
        }

        private void WalkTowardPlayerEnnemy_OnPlayerSightExit()
        {
            transform.rotation = savedState.Value;
            savedState = null;
        }

        private void WalkTowardPlayerEnnemy_OnPlayerSightEnter()
        {
            if(savedState == null)
                savedState = transform.rotation; 
        }

        protected virtual void FixedUpdate()
        {
            surrounding = pathFinder.GetSurrounding(surroundingRange, transform.position);

            UpdateMovement(surrounding);
        }

        public bool IsPlayerInSight()
        {
            if (Vector2.Distance(player.transform.position, transform.position) < sightRange)
            {
                RaycastHit2D hit2D = Physics2D.Linecast(transform.position, player.transform.position, layerVisibleByTheUnit);

                if (hit2D.collider != null)
                    Debug.DrawLine(transform.position, hit2D.collider.transform.position, Color.red);
                else
                    Debug.DrawLine(transform.position, player.transform.position, Color.red);

                if (hit2D.collider == null)
                {
                    PlayerInSight = true;
                    return true;
                }
                else
                {
                    PlayerInSight = false;
                    return false;
                }
            }
            else
            {
                PlayerInSight = false;
                return false;
            }
        }

        private void UpdateMovement(bool[,] surrounding)
        {
            int direction = (transform.rotation.y == 1 || transform.rotation.y == -1 ? -1 : 1);
            Vector2Int center = new Vector2Int(surroundingRange, surroundingRange);

            animator.SetBool(Values.AnimationParameters.Enemy.Walking, false);

            if(IsPlayerInSight())
            {
                if((!isDumbEnoughToFall && IsThereAHole(surrounding)) || IsThereAWall(surrounding))
                {
                    rootMover.LookAtPlayer();
                    rootMover.StopWalking();
                }
                else
                {
                    rootMover.LookAtPlayer();

                    if (Mathf.Abs(player.transform.position.x - transform.position.x) < epsilonDistancePlayer)
                        rootMover.StopWalking();
                    else
                        rootMover.Walk();
                }
            }
            else
            {
                if((!isDumbEnoughToFall && IsThereAHole(surrounding)) || IsThereAWall(surrounding))
                {
                    if (direction == 1)
                        transform.rotation = Quaternion.AngleAxis(180, Vector2.up);
                    else
                        transform.rotation = Quaternion.AngleAxis(0, Vector2.up);

                    direction = (transform.rotation.y == 1 || transform.rotation.y == -1 ? -1 : 1);
                }

                rootMover.Walk();
            }

            if (surrounding[center.x + direction, center.y] && !rootMover.IsJumping && !IsThereAWall(surrounding))
                rootMover.Jump();
        }

        private bool IsThereAHole(bool[,] surrounding)
        {
            int direction = (transform.rotation.y == 1 || transform.rotation.y == -1 ? -1 : 1);
            Vector2Int center = new Vector2Int(surroundingRange, surroundingRange);

            if (!surrounding[center.x + direction, center.y - 1] && !surrounding[center.x + direction, center.y - 2])
            {
                return true;
            }

            return false;
        }

        private bool IsThereAWall(bool[,] surrounding)
        {
            int direction = (transform.rotation.y == 1 || transform.rotation.y == -1 ? -1 : 1);
            Vector2Int center = new Vector2Int(surroundingRange, surroundingRange);

            if (surrounding[center.x + direction, center.y] && surrounding[center.x + direction, center.y + 1])
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
                            Gizmos.DrawCube(
                                new Vector3(transform.position.x + (x * .32f), transform.position.y + (y * .32f), 0),
                                Vector3.one * .16f);
                        else
                        {
                            Gizmos.DrawSphere(
                                new Vector3(transform.position.x + (x * .32f), transform.position.y + (y * .32f), 0),
                                .16f);
                        }
                    }
                }
        }
    }
}