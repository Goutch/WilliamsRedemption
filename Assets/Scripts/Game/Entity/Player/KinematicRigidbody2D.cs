using System.Collections.Generic;
using System.Linq;
using Game.Puzzle;
using UnityEngine;

namespace Game.Entity.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class KinematicRigidbody2D : MonoBehaviour
    {
        private const int NbPreallocatedRaycastHit = 16;

        [Header("Physics")] [SerializeField] [Tooltip("Gravity force.")]
        private Vector2 gravity = new Vector2(0, -9.81f);

        [SerializeField] [Tooltip("How much gravity affects this object.")]
        private float gravityMultiplier = 1f;

        [SerializeField] [Tooltip("Layers player collides with.")]
        private LayerMask layerMask;

        [SerializeField] [Tooltip("Arctan value of the maximum slope angle considered as ground.")]
        private float maxGroundSlopeAngleArctan = 1 - 0.65f; //About 33°.

        [SerializeField] [Tooltip("Simulation is ignored when velocity is bellow this threshold.")]
        private float sleepVelocity = 0.001f;

        [SerializeField] [Tooltip("Precision of the simulation. Don't make it lower than 0.01.")]
        private float deltaPrecision = 0.01f;

        [SerializeField]
        private float deltaPrecision2 = 0.03f;

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField]
        [Tooltip("Show debug informatio like velocity, ground normal and ground movement vector.")]
        private bool showDebugInformation;
#endif

        private Vector2 velocity;
        private Vector2 targetVelocity;
        private Vector2 latestVelocity;
        private bool isGrounded;
        private float lastGroundedTime;
        private Vector2 groundNormal; //Vector perpenticular to current ground surface.

        private new Rigidbody2D rigidbody;
        private ContactFilter2D contactFilter;
        private RaycastHit2D[] preallocaRaycastHits;
        public bool isOnMovingGround;

        public LayerMask LayerMask
        {
            get { return layerMask; }
            set { layerMask = value; }
        }

        public Vector2 Velocity
        {
            get { return latestVelocity; }
            set
            {
                targetVelocity = value;
                latestVelocity = value;
            }
        }

        public Vector2 VelocityModifier { get; set; }

        public bool IsGravityIgnored { get; set; }

        public bool IsGrounded
        {
            get { return isGrounded; }
        }

        public float TimeSinceAirborne
        {
            get { return Time.time - lastGroundedTime; }
        }

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            contactFilter.useTriggers = false;
            contactFilter.useLayerMask = true;
            preallocaRaycastHits = new RaycastHit2D[NbPreallocatedRaycastHit];
            IsGravityIgnored = false;
            isOnMovingGround = false;
        }

        private void FixedUpdate()
        {
            ResetValuesBeforeSimulation();

            AddGravityToVelocity();
            AddTargetVelocityToVelocity();
            AddTargetVelocityToVelocityY();

            var deltaPosition = GetVelocityDeltaPosition();
            var groundMovementVector = GetGroundMovementVector();
            var horizontalDeltaPosition = groundMovementVector * deltaPosition.x;
            var verticalDeltaPosition = Vector2.up * deltaPosition.y;

            ApplyDeltaPosition(horizontalDeltaPosition, false);
            ApplyDeltaPosition(Vector2.left * Time.fixedDeltaTime, false);
            ApplyDeltaPosition(Vector2.right * Time.fixedDeltaTime, false);
            ApplyDeltaPosition(verticalDeltaPosition, true);

            VelocityModifier = Vector2.Lerp(VelocityModifier, Vector2.zero, Time.fixedDeltaTime * 2);

            latestVelocity = velocity;

#if UNITY_EDITOR
            if (showDebugInformation)
            {
                Debug.DrawLine(rigidbody.position, rigidbody.position + velocity, Color.red);
            }
#endif
        }

        private void ResetValuesBeforeSimulation()
        {
            isGrounded = false;
            isOnMovingGround = false;
            contactFilter.layerMask = layerMask;
        }

        private void AddGravityToVelocity()
        {
            velocity += GetGravityDeltaPosition();
        }

        private void AddTargetVelocityToVelocity()
        {
            //X velocity is entirely controlled by the object (like the player or an ennemy)
            velocity.x = targetVelocity.x + VelocityModifier.x;
            //Y velocity is controlled by the object when it's target y velocity is greater than 0 or if gravity is ignored.
            //Otherwise, current velocity is used.
            //velocity.y = targetVelocity.y > 0 || IsGravityIgnored ? targetVelocity.y : velocity.y;
        }

        /// <summary>
        /// Since our game does not require a continous input to jump , we have to update the Y axis in update. 
        /// </summary>
        private void AddTargetVelocityToVelocityY()
        {
            //Y velocity is controlled by the object when it's target y velocity is greater than 0 or if gravity is ignored.
            //Otherwise, current velocity is used.
            velocity.y = (targetVelocity.y > 0 || IsGravityIgnored ? targetVelocity.y : velocity.y) +
                         VelocityModifier.y;
        }

        public void AddForce(Vector2 force)
        {
            VelocityModifier = force * Vector2.right;
            Velocity = force * Vector2.up;
        }

        private Vector2 GetGravityDeltaPosition()
        {
            return gravity * (IsGravityIgnored ? 0f : gravityMultiplier) * Time.fixedDeltaTime;
        }

        private Vector2 GetVelocityDeltaPosition()
        {
            return velocity * Time.fixedDeltaTime;
        }

        private Vector2 GetGroundMovementVector()
        {
            return new Vector2(groundNormal.y, -groundNormal.x);
        }

        private bool CanPassThrough(Vector2 position, Vector2 hitPosition)
        {
            return position.y <= hitPosition.y;
        }

        private void ApplyDeltaPosition(Vector2 deltaPosition, bool isVerticalDelta)
        {
            var deltaMagnitude = deltaPosition.magnitude;

            var attachedColliderCount = rigidbody.attachedColliderCount;
            var allColliders = new Collider2D[attachedColliderCount];
            rigidbody.GetAttachedColliders(allColliders);

            if (deltaMagnitude >= sleepVelocity)
            {
                // ReSharper disable once LocalVariableHidesMember
                foreach (var selfCollider in allColliders.Where(it => !it.isTrigger))
                {
                    var nbCollidersDetected = selfCollider.Cast(deltaPosition,
                        contactFilter,
                        preallocaRaycastHits,
                        deltaMagnitude + deltaPrecision);

                    for (int i = 0; i < nbCollidersDetected; i++)
                    {
                        var raycastHit = preallocaRaycastHits[i];
                        var colliderNormal = raycastHit.normal;

                        //Pass through Platforms.
                        if (raycastHit.collider.CompareTag(Values.Tags.PassThrough))
                        {
                            if (CanPassThrough(rigidbody.position, raycastHit.point))
                                continue;
                        }

                        if (raycastHit.collider.CompareTag(Values.Tags.MovingPlatform))
                        {
                            isOnMovingGround = true;
                        }


                        //If this a useable ground ?
                        if (colliderNormal.y > 1 - maxGroundSlopeAngleArctan)
                        {
                            isGrounded = true;
                            lastGroundedTime = Time.time;

                            if (isVerticalDelta)
                            {
                                groundNormal = colliderNormal;
                                colliderNormal.x = 0;
#if UNITY_EDITOR
                                if (showDebugInformation)
                                {
                                    Debug.DrawLine(raycastHit.point, raycastHit.point + colliderNormal, Color.green);
                                    Debug.DrawLine(raycastHit.point, raycastHit.point + GetGroundMovementVector(),
                                        Color.yellow);
                                }
#endif
                            }
                        }

                        //How much this collider should affect the velocity. The more the velocity vector
                        //and the collider normal vector are opposed, the more the collider should absorb the velocity
                        //
                        //Using the Dot product, we know how much theses two vectors are opposed (if they are).
                        //Negative number means vectors are opposed.
                        var velocityNegationForce = Vector2.Dot(velocity, colliderNormal);
                        if (velocityNegationForce < 0)
                        {
                            velocity -= velocityNegationForce * colliderNormal;
                        }

                        //Snap object to collider bound if distance between the object and the collider is less than
                        //the delta precison. This prevent the object from going though the collider.
                        var snappedDeltaMagnitude = raycastHit.distance - deltaPrecision;
                        deltaMagnitude = snappedDeltaMagnitude < deltaMagnitude
                            ? snappedDeltaMagnitude
                            : deltaMagnitude;
                    }
                }
            }

            rigidbody.position += deltaPosition.normalized * deltaMagnitude;

            if (isVerticalDelta)
            {
                var bottomY = allColliders.Where(it => !it.isTrigger).Min(it => it.bounds.min.y);
                var heightHalf = rigidbody.position.y - bottomY;

#if UNITY_EDITOR
                if (showDebugInformation)
                {
                    Debug.DrawLine(transform.position, transform.position - Vector3.up * heightHalf, Color.magenta);
                }
#endif

                var nbRays = Physics2D.Raycast(transform.position, Vector2.down, contactFilter, preallocaRaycastHits);
                Vector3? smallestPosition = null;
                for (var i = 0; i < nbRays; i++)
                {
                    var raycastHit = preallocaRaycastHits[i];

                    if (!allColliders.Contains(raycastHit.collider) &&
                        !raycastHit.collider.CompareTag(Values.Tags.PassThrough))
                    {
                        var floorPosition = raycastHit.point;
                        
                        if (floorPosition.y > bottomY)
                        {
                            if (smallestPosition == null || floorPosition.y < smallestPosition.Value.y)
                                smallestPosition = floorPosition;
                        }
                    }
                }

                if(smallestPosition != null)
                {
                    var rectifiedPosition = transform.position;
                    rectifiedPosition.y =  smallestPosition.Value.y + heightHalf + deltaPrecision2;
                    rigidbody.position = rectifiedPosition;
                }
            }
        }
    }
}