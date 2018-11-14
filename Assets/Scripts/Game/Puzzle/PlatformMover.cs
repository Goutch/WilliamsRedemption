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
        
        
        //Contains every transform from colliding objects. (Named transformers to avoid conflict with transform.)
        private HashSet<Transform> transformers;
        //Translation vector used by the platform and it's colliding objects.
        private Vector2 translation;


        private void Awake()
        {
            initialPositionX = transform.position.x;
            initialPositionY = transform.position.y;
            positionX = initialPositionX;
            quadraticX = 0;
            transformers = new HashSet<Transform>();
            lastPosition = transform.position;
            translation = new Vector2(0, 0);
            verticalCapacityPrecisionOffset = 0.0001f;
        }

        private void Update()
        {
            CheckHorizontalDirection();
            checkVertialDirection();
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
                if (!transformers.Contains(other.transform))
                {
                    transformers.Add(other.transform);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (transformers.Contains(other.transform))
            {
                transformers.Remove(other.transform);
            }
        }

        IEnumerator FollowPlatform()
        {
            while (isActiveAndEnabled)
            {
                if (!isUsingQuadraticCurve)
                {
                    translation =
                        new Vector2(horizontalDirection.x * HorizontalSpeed, verticalDirection.y * VerticalSpeed) *
                        Time.deltaTime;
                }
                else
                {
                    translation = useQuadraticCurve();
                }

                transform.Translate(translation);

                if (transformers.Count > 0)
                {
                    foreach (var transformer in transformers)
                    {
                        transformer.Translate(translation);
                    }
                }
                yield return null;
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
                }
            }
        }

        private void checkVertialDirection()
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
            return VerticalSpeed +verticalCapacityPrecisionOffset * Time.deltaTime;
        }
    }
}