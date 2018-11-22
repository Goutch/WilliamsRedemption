using Game.Entity.Player;
using Harmony;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Puzzle
{

    public class FloorTile : IFloorTile
    {
        [SerializeField] private float distanceMoving;
        [SerializeField] private float speed;
    
        private Vector2 initialePosition;
        private bool playerOnFloor = false;

        private PlayerController player;

        public bool IsAtInitialPosition
        {
            get
            {
                return Mathf.Abs(transform.position.y - initialePosition.y) < 0.02f;
            }
        }

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag(Values.Tags.Player).GetComponent<PlayerController>();
            initialePosition = transform.position;
        }

        public override void MoveUp()
        {
            StopAllCoroutines();
            StartCoroutine(Move(1));
        }

        private IEnumerator Move(int directionY)
        {
            Vector2 targetPosition = directionY == 1 ? initialePosition : initialePosition - new Vector2(0, distanceMoving);

            while (Mathf.Abs(transform.position.y - targetPosition.y) > 0.02f)
            {
                float deplacementY = directionY * speed;

                transform.Translate(0, deplacementY,0);

                if (playerOnFloor)
                    player.transform.position += new Vector3(0, deplacementY,0);

                yield return new WaitForFixedUpdate();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.Root().CompareTag(Game.Values.Tags.Player))
                playerOnFloor = true;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.Root().CompareTag(Game.Values.Tags.Player))
                playerOnFloor = false;
        }

        public override void MoveDown()
        {
            StopAllCoroutines();
            StartCoroutine(Move(-1));
        }
    }
}
