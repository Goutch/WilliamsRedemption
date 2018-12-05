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
        private float deplacementY;

        public bool IsAtInitialPosition
        {
            get { return Mathf.Abs(transform.position.y - initialePosition.y) < 0.02f; }
        }

        private void Awake()
        {
            initialePosition = transform.position;
        }

        private void FixedUpdate()
        {
            GetComponent<Rigidbody2D>().Translate(new Vector2(0, deplacementY) * Time.fixedDeltaTime);
            deplacementY = 0;
        }

        public override void MoveUp()
        {
            StopAllCoroutines();
            StartCoroutine(Move(1));
        }

        private IEnumerator Move(int directionY)
        {
            Vector2 targetPosition =
                directionY == 1 ? initialePosition : initialePosition - new Vector2(0, distanceMoving);

            while (Mathf.Abs(transform.position.y - targetPosition.y) > 0.01f)
            {
                deplacementY = directionY * speed;

                yield return new WaitForFixedUpdate();
            }
        }

        public override void MoveDown()
        {
            StopAllCoroutines();
            StartCoroutine(Move(-1));
        }
    }
}