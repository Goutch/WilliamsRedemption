using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        [Tooltip("Only odd number of tiles is permitted")]
        [SerializeField] private int numberOfTilesToSpawn;
        [Tooltip("The distance between the collision and the tiles spawned on the Y axis.")]
        [SerializeField] private int yOffSetTileToSpawn;
        [SerializeField] private float tilesDuration;

        private const float raycastLength = 0.32f;

        private LightController lightController;
        private Tilemap platforms;

        private new Rigidbody2D rigidbody;
        private new Collider2D collider;
        private Vector2 originSize;
        private float originHeight = 0.02f;
        private Vector2 explosionEffectSize;

        private Vector2 direction;
        private bool grounded = false;
        private float scale = 1;

        private void Awake()
        {
            if (numberOfTilesToSpawn % 2 != 1)
            {
                Debug.LogError("Stop playing, error found. - Only odd number of tiles is permitted");
                UnityEditor.EditorApplication.isPlaying = false;
            }

            lightController = GameObject.FindGameObjectWithTag(R.S.Tag.LightManager).GetComponent<LightController>();
            platforms = GameObject.FindGameObjectWithTag(R.S.Tag.Plateforme).GetComponent<Tilemap>();

            rigidbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
            originSize = GetComponent<SpriteRenderer>().size;
            originSize.y = originHeight;
            explosionEffectSize = explosionEffect.GetComponent<SpriteRenderer>().size;
        }

        private void OnEnable()
        {
            direction.x = Mathf.Sign(transform.position.x - PlayerController.instance.transform.position.x);
        }

        private void FixedUpdate()
        {
            if (grounded)
            {
                Scale();
                MoveForward();

                RaycastHit2D hit = Physics2D.Linecast(
                    transform.localPosition -
                    direction.x * new Vector3(scale * (originSize.x / 2) - raycastLength, -originSize.y * direction.x),
                    transform.localPosition - direction.x * new Vector3(scale * (originSize.x / 2), -originSize.y * direction.x),
                    1 << LayerMask.NameToLayer(R.S.Layer.TransparentFX) | 1 << LayerMask.NameToLayer(R.S.Layer.Default));

                if (hit.collider != null)
                {
                    OnWallCollision();
                }
            }
        }

        private void MoveForward()
        {
            Scale();

            rigidbody.MovePosition(new Vector3(transform.localPosition.x - direction.x * speed * Time.deltaTime * (originSize.x / 2),
                transform.localPosition.y));
        }

        private void Scale()
        {
            if (transform.localScale.x * originSize.x < maxWidth)
            {
                transform.localScale = new Vector3(transform.localScale.x + (speed / 2 * Time.deltaTime),
                    transform.localScale.y);

                scale += speed / 2 * Time.deltaTime;
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
            GameObject explosionObject = Instantiate(explosionEffect, transform.localPosition - direction.x * new Vector3(scale * (originSize.x / 2) - explosionEffectSize.x / 2,
                    -direction.x * explosionEffectSize.y / 2), Quaternion.identity);
            explosionObject.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Ennemy);

            SpawnTiles();

            Destroy(gameObject);
        }

        private void SpawnTiles()
        {
            Vector3Int cellPos = platforms.LocalToCell(
                transform.localPosition - direction.x * new Vector3(scale * (originSize.x / 2) - explosionEffectSize.x / 2,
                -direction.x * explosionEffectSize.y / 2));

            cellPos.y += yOffSetTileToSpawn;

            List<Vector3Int> platformPositions = new List<Vector3Int>();

            for (int i = -numberOfTilesToSpawn / 2; i <= numberOfTilesToSpawn / 2; ++i)
            {
                if (platforms.GetTile(new Vector3Int(cellPos.x + i, cellPos.y, 0)) == null)
                {
                    platforms.SetTile(new Vector3Int(cellPos.x + i, cellPos.y, 0), tileToSpawn);
                    platformPositions.Add(new Vector3Int(cellPos.x + i, cellPos.y, 0));
                }
            }

            lightController.UpdateLightAtEndOfFrame();

            GameObject tilesDestroyer = new GameObject();
            NewTimedTilesDestroyer newTimedTilesDestroyer = tilesDestroyer.AddComponent<NewTimedTilesDestroyer>();
            newTimedTilesDestroyer.Init(platformPositions, tilesDuration);
        }
    }
}

