using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Boss;

namespace Edgar
{
    public class NewPlasmaGroundController : MonoBehaviour
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

        private const float raycastLength = 0.32f;

        private new Rigidbody2D rigidbody;
        private new Collider2D collider;
        private SpawnedTilesManager spawnedTilesManager;
        private Vector2 originSize;
        private float originHeight = 0.02f;
        private Vector2 explosionEffectSize;

        private int flipFactor;
        private bool grounded = false;
        private float scale = 1;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();

            originSize = GetComponent<SpriteRenderer>().size;
            originSize.y = originHeight;
            explosionEffectSize = explosionEffect.GetComponent<SpriteRenderer>().size;

        }

        public void Init(SpawnedTilesManager spawnedTilesManager)
        {
            this.spawnedTilesManager = spawnedTilesManager;
        }

        private void OnEnable()
        {
            flipFactor = transform.rotation.y == -1 ? 1 : -1;
        }

        private void Update()
        {
            if (grounded)
            {
                Scale();
                MoveForward();

                RaycastHit2D hit = Physics2D.Linecast(
                    rigidbody.position + flipFactor * new Vector2(originSize.x / 2 * scale - raycastLength, flipFactor * originSize.y),
                    rigidbody.position + flipFactor * new Vector2(originSize.x / 2 * scale, flipFactor * originSize.y),
                    1 << LayerMask.NameToLayer(R.S.Layer.TransparentFX) | 1 << LayerMask.NameToLayer(R.S.Layer.Default));

                Debug.DrawLine(
                    rigidbody.position + flipFactor * new Vector2(originSize.x / 2 * scale - raycastLength, flipFactor * originSize.y),
                    rigidbody.position + flipFactor * new Vector2(originSize.x / 2 * scale, flipFactor * originSize.y),
                    Color.blue);

                if (hit.collider != null)
                {
                    OnWallCollision();
                }
            }
        }

        private void MoveForward()
        {
            rigidbody.Translate(new Vector3(flipFactor * speed * Time.deltaTime * (originSize.x / 2),
                0));
        }

        private void Scale()
        {
            if (transform.localScale.x * originSize.x < maxWidth)
            {
                transform.localScale = new Vector3(transform.localScale.x + (speed * Time.deltaTime),
                    transform.localScale.y);

                scale += speed * Time.deltaTime;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(R.S.Tag.Plateforme))
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

        private void OnWallCollision()
        {
            GameObject explosionObject = Instantiate(explosionEffect, rigidbody.position + flipFactor * new Vector2(originSize.x / 2 * scale - explosionEffectSize.x/2, - explosionEffectSize.y/2), Quaternion.identity);
            explosionObject.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Ennemy);

            SpawnTiles();

            Destroy(gameObject);
        }

        private void SpawnTiles()
        {
            Vector3Int cellPos = spawnedTilesManager.ConvertLocalToCell(rigidbody.position + flipFactor * new Vector2(originSize.x / 2 * scale - explosionEffectSize.x / 2, -explosionEffectSize.y / 2));

            cellPos.y += yOffSetTileToSpawn;

            List<Vector3Int> relativePositions = new List<Vector3Int>();
            for (int i = -numberOfTilesToSpawn / 2; i <= numberOfTilesToSpawn / 2; ++i)
                relativePositions.Add(new Vector3Int(i,0,0));

            spawnedTilesManager.SpawnTiles(cellPos, relativePositions, tileToSpawn, tilesDuration);
        }
    }
}

