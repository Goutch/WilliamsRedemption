using Harmony;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Entity.Enemies.Boss.Edgar
{
    public class PlasmaGroundController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private float speed;
        [SerializeField] private float maxWidth;

        [Header("Collision effect")]
        [SerializeField] private GameObject explosionEffect;
        [SerializeField] private Tile tileToSpawn;
        [SerializeField] private int numberOfTilesToSpawn;
        [Tooltip("The distance between the collision and the tiles spawned on the Y axis.")]
        [SerializeField] private int yOffSetTileToSpawn;
        [SerializeField] private float tilesDuration;

        private const float RAYCAST_LENGTH = 0.32f;

        private new Rigidbody2D rigidbody;
        private new Collider2D collider;
        private SpawnedTilesManager spawnedTilesManager;
        private Vector2 originSize;
        private Vector2 explosionEffectSize;

        private int flipFactor;
        private bool grounded = false;
        private float scale = 1;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();

            originSize = GetComponent<SpriteRenderer>().size;
            explosionEffectSize = explosionEffect.GetComponent<SpriteRenderer>().size;

            scale = transform.localScale.x;
        }

        public void Init(SpawnedTilesManager spawnedTilesManager)
        {
            this.spawnedTilesManager = spawnedTilesManager;
        }

        private void OnEnable()
        {
            flipFactor = transform.rotation.y == -1 ? -1 : 1;
        }

        private void Update()
        {
            if (grounded)
            {
                Scale();
                MoveForward();

                if (IsCollidingWall())
                    OnWallCollision();
            }
        }

        private void Scale()
        {
            if (transform.localScale.x * originSize.x < maxWidth)
            {
                scale += speed * Time.deltaTime;

                transform.localScale = new Vector3(scale,
                    transform.localScale.y);
            }
        }
        private void MoveForward()
        {
            rigidbody.Translate(new Vector3(flipFactor * speed * Time.deltaTime * (originSize.x / 2),
                0));
        }

        private bool IsCollidingWall()
        {
            RaycastHit2D hit = Physics2D.Linecast(
                rigidbody.position + flipFactor * new Vector2(originSize.x / 2 * scale - RAYCAST_LENGTH, flipFactor * originSize.y),
                rigidbody.position + flipFactor * new Vector2(originSize.x / 2 * scale, flipFactor * originSize.y),
                 1 << LayerMask.NameToLayer(Values.Layers.Platform));

            Debug.DrawLine(
                rigidbody.position + flipFactor * new Vector2(originSize.x / 2 * scale - RAYCAST_LENGTH, flipFactor * originSize.y),
                rigidbody.position + flipFactor * new Vector2(originSize.x / 2 * scale, flipFactor * originSize.y),
                Color.blue);

            if (hit.collider != null)
            {
                return true;
            }

            return false;
        }
        private void OnWallCollision()
        {
            GameObject explosionObject = Instantiate(explosionEffect, rigidbody.position +
                new Vector2(flipFactor * (originSize.x / 2 * scale - explosionEffectSize.x / 2),
                explosionEffectSize.y / 2 - originSize.y / 2),
                Quaternion.identity);


            SpawnTiles();

            Destroy(gameObject);
        }
        private void SpawnTiles()
        {
            Vector3Int cellPos = spawnedTilesManager.ConvertLocalToCell(rigidbody.position +
                new Vector2(flipFactor * (originSize.x / 2 * scale - explosionEffectSize.x / 2),
                explosionEffectSize.y / 2 - originSize.y / 2));

            cellPos.y += yOffSetTileToSpawn;

            List<Vector3Int> relativePositions = new List<Vector3Int>();
            for (int i = -numberOfTilesToSpawn / 2; i <= numberOfTilesToSpawn / 2; ++i)
                relativePositions.Add(new Vector3Int(i, 0, 0));

            spawnedTilesManager.SpawnTiles(cellPos, relativePositions, tileToSpawn, tilesDuration);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Values.Tags.Plateforme))
            {
                if (!grounded)
                    OnFloorCollision();
            }
        }
        private void OnFloorCollision()
        {
            grounded = true;
            rigidbody.constraints = rigidbody.constraints | RigidbodyConstraints2D.FreezePositionY;
        }
    }
}

