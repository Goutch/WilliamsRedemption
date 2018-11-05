using Game.Entity.Player;
using UnityEngine;

namespace Game.Puzzle
{
    public class PlatformMover : MonoBehaviour
    {

        [Tooltip("Distance traveled by the platform when heading left.(From start to this)")]
        [SerializeField] private float MaxDistanceLeft; //BEN_CORRECTION : Standard de nommage.
        [Tooltip("Distance traveled by the platform when heading right.(From start to this this)")]
        [SerializeField] private float MaxDistanceRight; //BEN_CORRECTION : Standard de nommage.
        [Tooltip("Distance Travelled by the platform when heading upwards.(From start to this")]
        [SerializeField] private float MaxDistanceUp; //BEN_CORRECTION : Standard de nommage.
        [Tooltip("Distance traveled by the platform when heading downwards.(From start to this")]
        [SerializeField] private float MaxDistanceDown; //BEN_CORRECTION : Standard de nommage.
        [Tooltip("Speed at which the platform moves horizontaly.")]
        [SerializeField] private float HorizontalSpeed; //BEN_CORRECTION : Standard de nommage.
        [Tooltip("Speed at which the platform moves verticaly.")]
        [SerializeField] private float VerticalSpeed; //BEN_CORRECTION : Standard de nommage.
        [Tooltip("True when heading towards the right. (Checking this will make the platform head towards the right first.")]
        [SerializeField] private bool isHeadingRight;
        [Tooltip("True when heading up. (Checking this will make the platform head upwards first.")]
        [SerializeField] private bool isHeadingUpwards;
        [Header("Quadratic function options:")]
        [Tooltip("True when checked. Enables the platform to follow a curving path.")]
        [SerializeField] private bool isUsingQuadraticCurve;
        [Tooltip("Quadratic function coefficient. Affects steepness and narrowness of the curve.")]
        [SerializeField] private float quadraticA;
        [Tooltip("Quadratic function coefficient. Is added to Y everytime the value of X is increased by one. ")]
        [SerializeField] private float quadraticB;
        [Tooltip("Quadratic function coefficient. Value of Y ")]
        [SerializeField] private float quadraticC;

        private float initialPositionX;
        private float initialPositionY;
        private float quadraticX;
        private Rigidbody2D rb;
        private Vector2 horizontalDirection;
        private Vector2 verticalDirection;
        private float quadraticFucntion;
        private float positionX;



        //BEN_CORRECTION : Commentaire inutile + private manquant.
        // Use this for initialization
        void Start()
        {
            //BEN_CORRECTION : Nommage. rb.
            rb = GetComponent<Rigidbody2D>();
            initialPositionX = rb.position.x;
            initialPositionY = rb.position.y;
            positionX = initialPositionX;
            quadraticX = 0;
        }

        //BEN_CORRECTION : Commentaire inutile + private manquant.
        // Update is called once per frame
        void Update()
        {
            CheckHorizontalDirection();
            checkVertialDirection();
        }



        private void FixedUpdate()
        {
            if (!isUsingQuadraticCurve)
            {
                //BEN_REVIEW : Dans l'autre fonction (useQuadraticCurve), vous utilisez "rb.MovePosition".
                //             Est-ce vraiment ce que vous voulez ?
                rb.velocity = new Vector2(horizontalDirection.x * HorizontalSpeed, verticalDirection.y * VerticalSpeed);
            }
            else
            {
                useQuadraticCurve();
            }

        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.collider.CompareTag(Values.Tags.Player))
            {
                //BEN_REVIEW : OH, je vois. IsMoving, en vérité, c'est "IsPlayerWantsToMove" ou quelque chose du genre...
                //
                //             Je trouve que vos plateformes mobiles sont un peu compliquées pour rien. Pourquoi ne pas
                //             tout simplement additionner le mouvement de la plateforme à ce que la plateforme touche ?
                //
                //             Du genre :
                //
                //             other.gameObject.transform.position += plateformeMovement;
                //
                //             Avec un setup comme ça, "OnCollisionExit2D" serait plus nécessaire.
                if (!other.gameObject.GetComponent<PlayerController>().IsMoving)
                {
                    other.transform.parent = gameObject.transform;
                }
                else
                {
                    other.transform.parent = null;
                }

            }

        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider.CompareTag(Values.Tags.Player))
            {
                other.transform.parent = null;
            }
        }

        //BEN_CORRECTION : Ne retourne pas un boolean, mais s'appelle "Check".
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
            quadraticX = rb.position.x - initialPositionX;
            positionX += HorizontalSpeed * horizontalDirection.x * Time.fixedDeltaTime;
            quadraticFucntion = (quadraticA * (quadraticX * quadraticX) + quadraticB * (quadraticX) + quadraticC);
            Vector2 curve = new Vector2(positionX, quadraticFucntion + initialPositionY);
            rb.MovePosition(curve);
        }
    }
}

