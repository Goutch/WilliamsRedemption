using Game.Entity.Player;
using Harmony;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Puzzle
{
    public class FloorTile : MonoBehaviour
    {
        [SerializeField] private float distanceMoving;
        [SerializeField] private float speed;
        [SerializeField] private Transform grid;

        private Vector2 initialePosition;
        private Transform parent;
        private bool hasParentAtStart;
        private bool playerOnFloor = false;
        private FloorTile[] initialChild;

        private void Awake()
        {
            initialChild = new FloorTile[transform.childCount];

            for (int j = 0; j < transform.childCount; ++j)
            {
                initialChild[j] = transform.GetChild(j).GetComponent<FloorTile>();
            }

            initialePosition = transform.position;
            parent = transform.parent;

            hasParentAtStart = parent.GetComponent<FloorTile>() != null ? true : false;
        }

        public void MoveUp()
        {
            StopAllCoroutines();
            StartCoroutine(Move(1));
        }

        private void Update()
        {
            if(hasParentAtStart && transform.parent == grid && Mathf.Abs(parent.transform.position.y - transform.position.y) < 0.05f)
            {
                transform.parent = parent;
                transform.localPosition = new Vector2(initialePosition.x - parent.transform.position.x, 0);
            }
        }

        private IEnumerator Move(int directionY)
        {
            Vector2 targetPosition = directionY == 1 ? initialePosition : initialePosition - new Vector2(0, distanceMoving);

            if (hasParentAtStart)
            {
                transform.parent = grid;
            }

            while (Mathf.Abs(transform.position.y - targetPosition.y) > 0.02f)
            {
                float deplacementY = directionY * speed;

                transform.Translate(0, deplacementY,0);

                if (playerOnFloor)
                    PlayerController.instance.transform.position += new Vector3(0, deplacementY,0);

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

        public void MoveUpAllChild()
        {
            foreach (FloorTile floorTile in initialChild)
            {
                floorTile.MoveUp();
            }
        }

        public void MoveDown()
        {
            StopAllCoroutines();
            StartCoroutine(Move(-1));
        }
    }
}
