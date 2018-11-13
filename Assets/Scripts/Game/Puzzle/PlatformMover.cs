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
        private Vector2 lastRbPosition;
        private Vector2 horizontalDirection;
        private Vector2 verticalDirection;
        private float quadraticFunction;
        private float positionX;
        private HashSet<Transform> colliders;
        private Vector2 curve;
        private Vector3 heightOffset;


        // Use this for initialization
        private void Awake()
        {
            initialPositionX = transform.position.x;
            initialPositionY = transform.position.y;
            positionX = initialPositionX;
            quadraticX = 0;
            colliders = new HashSet<Transform>();
            lastRbPosition = transform.position;
            heightOffset = new Vector3(0, GetComponent<BoxCollider2D>().size.y,0);
        }

        // Update is called once per frame
        private void Update()
        {
            CheckHorizontalDirection();
            checkVertialDirection();   
        }


        private void FixedUpdate()
        {
            Vector3 temp = transform.position;
            if (!isUsingQuadraticCurve)
            {
                
                Vector3 t= new Vector3(horizontalDirection.x * HorizontalSpeed, verticalDirection.y * VerticalSpeed)*Time.fixedDeltaTime;
                transform.Translate(t);
                
                if (colliders.Count >0)
                {
                    foreach (var collider in colliders)
                    {
                        collider.Translate(t);
                    }
                }
            }
            else
            {
                useQuadraticCurve();
                
                if (colliders.Count >0)
                {
                    foreach (var collider in colliders)
                    {
                        
                        collider.Translate(curve);
                    }
                }
            }

            

            
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(Values.Tags.Player))
            {
                if (!colliders.Contains(other.transform))
                {
                    colliders.Add(other.transform);
                }
            }
        }
        

        private void OnCollisionExit2D(Collision2D other)
        {
            if (colliders.Contains(other.transform))
            {
                colliders.Remove(other.transform);
            }
        }

        private void OnCollisionStay2D(Collision2D other) //bouge ce qui touche a la plateforme (Donne y le meme mouvement) Liste des colliders pis bouge tout ce qui colide avec dans le fixedUpdate.
        {
//            if (other.collider.CompareTag(Values.Tags.Player))
//            {
//                PlayerController player = other.gameObject.GetComponent<PlayerController>();
//
//                Vector3 offset = player.transform.position - transform.position;
//                
//
//                if (!isUsingQuadraticCurve)
//                {
//                   // Vector2 v = new Vector2(horizontalDirection.x*HorizontalSpeed*Time.fixedDeltaTime,VerticalSpeed*verticalDirection.y*Time.fixedDeltaTime);
//                    player.transform.position = transform.position + offset;     
//                    
//                    
//                }
//                else
//                {
//                    float xDistance = rb.position.x - lastRbPosition.x;
//                    float yDistance = rb.position.y - lastRbPosition.y;
//                    Vector2 v = new Vector2((xDistance),(0));
//                   
//                    player.kRigidBody.Velocity +=v/Time.fixedDeltaTime;
//                }             
//            }        
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            
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

        private void useQuadraticCurve()
        {
//            lastRbPosition = rb.position;
//            quadraticX = rb.position.x - initialPositionX;
//            positionX += HorizontalSpeed * horizontalDirection.x *Time.fixedDeltaTime;
//            quadraticFucntion = (quadraticA * (quadraticX * quadraticX) + quadraticB * (quadraticX) + quadraticC);
//            Vector2 curve = new Vector2(positionX, quadraticFucntion + initialPositionY);
//            rb.MovePosition(curve);

            lastRbPosition = transform.position;
            quadraticX = transform.position.x - initialPositionX;
            positionX += HorizontalSpeed * horizontalDirection.x * Time.fixedDeltaTime;
            quadraticFunction = (quadraticA * (quadraticX * quadraticX) + quadraticB * (quadraticX) + quadraticC);
            curve = new Vector2(positionX,quadraticFunction + initialPositionY);
            transform.Translate(curve - lastRbPosition);
        }
    }
}