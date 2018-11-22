using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Game.Entity.Player;
using Harmony;
using UnityEngine;
using UnityScript.Steps;

namespace Game.Puzzle
{
    public class PlatformMover : MonoBehaviour
    {
        [Tooltip("Distance traveled by the platform when heading left.(From start to this)")] [SerializeField]
        private float MaxDistanceLeft;

        [Tooltip("Distance traveled by the platform when heading right.(From start to this this)")] [SerializeField]
        private float MaxDistanceRight;

        [Tooltip("Distance Travelled by the platform when heading upwards.(From start to this")] [SerializeField]
        private float MaxDistanceUp;

        [Tooltip("Distance traveled by the platform when heading downwards.(From start to this")] [SerializeField]
        private float MaxDistanceDown;

        [Tooltip("Speed at which the platform moves horizontaly.")] [SerializeField]
        private float HorizontalSpeed;

        [Tooltip("Speed at which the platform moves verticaly.")] [SerializeField]
        private float VerticalSpeed;

        [Tooltip(
            "True when heading towards the right. (Checking this will make the platform head towards the right first.")]
        [SerializeField]
        private bool isHeadingRight;

        [Tooltip("True when heading up. (Checking this will make the platform head upwards first.")] [SerializeField]
        private bool isHeadingUpwards;
        
        [Tooltip("Amount of time before the platform starts moving again after reaching it's destination. Horizontal & Vertical")]
        [SerializeField] private float DirectionChangeDelay;

        [Header("Quadratic function options:")]
        [Tooltip("True when checked. Enables the platform to follow a curving path.")]
        [SerializeField]
        private bool isUsingQuadraticCurve;

        [Tooltip("Quadratic function coefficient. Affects steepness and narrowness of the curve.")] [SerializeField]
        private float quadraticA;

        [Tooltip("Quadratic function coefficient. Is added to Y everytime the value of X is increased by one. ")]
        [SerializeField]
        private float quadraticB;

        [Tooltip("Quadratic function coefficient. Value of Y ")] [SerializeField]
        private float quadraticC;

        
        private float initialPositionX;
        private float initialPositionY;
        private float quadraticX;
        private Vector2 lastPosition;
        private Vector2 horizontalDirection;
        private Vector2 verticalDirection;
        private float quadraticFunction;
        private float positionX;
        private float verticalCapacityPrecisionOffset;
        private float timeWhenPlatformFreezed;
        
        
        
        //Contains every transform from colliding objects. (Named transformers to avoid conflict with transform.)
        private HashSet<Transform> transforms;
        //Translation vector used by the platform and it's colliding objects.
        private Vector2 translation;
        


        private void Awake()
        {
            initialPositionX = transform.position.x;
            initialPositionY = transform.position.y;
            positionX = initialPositionX;
            quadraticX = 0;
            transforms = new HashSet<Transform>();
            lastPosition = transform.position;
            translation = new Vector2(0, 0);
            verticalCapacityPrecisionOffset = 0.0001f;
            timeWhenPlatformFreezed = 0;
            
        }

        private void Update()
        {
            if(HorizontalSpeed >0)
            CheckHorizontalDirection();
            if(VerticalSpeed>0)
            CheckVerticalDirection();
        }

        private void OnEnable()
        {
            StartCoroutine(FollowPlatform());
        }

        private void OnDisable()
        {
            StopCoroutine(FollowPlatform());
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(Values.Tags.Player))
            {
                if (!transforms.Contains(other.transform))
                {
                    transforms.Add(other.transform);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (transforms.Contains(other.transform))
            {
                transforms.Remove(other.transform);
            }
        }

        private IEnumerator FollowPlatform()
        {
            while (isActiveAndEnabled)
            {
                if (!isUsingQuadraticCurve)
                {
                    translation =
                        new Vector2(horizontalDirection.x * HorizontalSpeed, verticalDirection.y * VerticalSpeed) *
                        Time.fixedDeltaTime;
                    Force = translation;
                }
                else if(isUsingQuadraticCurve)
                {
                    translation = useQuadraticCurve();
                    Force = translation;
                }

                if (CanMove())
                {
                    transform.Translate(translation);
                }

                if (transforms.Count > 0 && CanMove())
                {
                    foreach (var transformer in transforms)
                    {
                        transformer.Translate(translation);
                    }
                }
                yield return new WaitForFixedUpdate();
            }
        }

        private void CheckHorizontalDirection()
        {
            if (isHeadingRight)
            {
                if (transform.position.x < initialPositionX + MaxDistanceRight)
                {
                    horizontalDirection = Vector2.right;
                }
                else
                {
                    isHeadingRight = false;
                    timeWhenPlatformFreezed = Time.time;
                }
            }
            else
            {
                if (transform.position.x > initialPositionX - MaxDistanceLeft)
                {
                    horizontalDirection = Vector2.left;
                }
                else
                {
                    isHeadingRight = true;
                    timeWhenPlatformFreezed = Time.time;
                }
            }
        }

        private void CheckVerticalDirection()
        {
            if (isHeadingUpwards)
            {
                if (transform.position.y < initialPositionY + MaxDistanceUp)
                {
                    verticalDirection = Vector2.up;
                }
                else
                {
                    isHeadingUpwards = false;
                    timeWhenPlatformFreezed = Time.time;
                }
            }
            else
            {
                if (transform.position.y > initialPositionY - MaxDistanceDown)
                {
                    verticalDirection = Vector2.down;
                }
                else
                {
                    isHeadingUpwards = true;
                    timeWhenPlatformFreezed = Time.time;
                }
            }
        }

        private Vector2 useQuadraticCurve()
        {
            lastPosition = transform.position;
            quadraticX = transform.position.x - initialPositionX;
            positionX += HorizontalSpeed * horizontalDirection.x * Time.deltaTime;
            quadraticFunction = (quadraticA * (quadraticX * quadraticX) + quadraticB * (quadraticX) + quadraticC);
            Vector2 curve = new Vector2(positionX, quadraticFunction + initialPositionY);

            return curve - lastPosition;
        }

        public float GetVerticalSpeed()
        {
            return VerticalSpeed * Time.deltaTime;
        }

        private bool CanMove()
        {
            return Time.time - timeWhenPlatformFreezed >= DirectionChangeDelay;
        }
    }
}