using Game.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Entity.Enemies.Boss
{
    public class SpawnedTilesManager : MonoBehaviour
    {
        private List<Vector3Int> spawnedTilesPosition;

        private LightController lightController;
        private Tilemap platforms;
        private Health health;

        private GameObject player;

        private void Awake()
        {
            lightController = GameObject.FindGameObjectWithTag(Values.Tags.LightManager)
                .GetComponent<LightController>();
            platforms = GameObject.FindGameObjectWithTag(Values.Tags.Plateforme).GetComponent<Tilemap>();
            health = GetComponent<Health>();

            if (health != null)
                health.OnDeath += Health_OnDeath;

            spawnedTilesPosition = new List<Vector3Int>();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag(Values.Tags.Player);
        }

        private void Health_OnDeath(GameObject receiver, GameObject attacker)
        {
            DestroyTiles(new List<Vector3Int>(spawnedTilesPosition));
        }

        public Vector3Int ConvertLocalToCell(Vector2 position)
        {
            return platforms.LocalToCell(position);
        }

        public void SpawnTiles(Vector3Int center, List<Vector3Int> tilesPositionRelative, Tile tileToSpawn,
            float? tilesLifeTime = null)
        {
            List<Vector3Int> platformPositions = new List<Vector3Int>();

            foreach (Vector3Int relativePosition in tilesPositionRelative)
            {
                if (platforms.GetTile(new Vector3Int(relativePosition.x + center.x, relativePosition.y + center.y,
                        0)) == null)
                {
                    platforms.SetTile(new Vector3Int(relativePosition.x + center.x, relativePosition.y + center.y, 0),
                        tileToSpawn);
                    platformPositions.Add(new Vector3Int(relativePosition.x + center.x, relativePosition.y + center.y,
                        0));
                }
            }

            lightController.UpdateLightAtEndOfFrame(player.transform.position);

            spawnedTilesPosition.AddRange(platformPositions);

            if (tilesLifeTime != null)
                StartCoroutine(DestroyTiles(platformPositions, tilesLifeTime.Value));
        }

        public void DestroyAllTiles()
        {
            DestroyTiles(new List<Vector3Int>(spawnedTilesPosition));
        }

        private IEnumerator DestroyTiles(List<Vector3Int> cellToDestroy, float delayBeforeDestruction)
        {
            yield return new WaitForSeconds(delayBeforeDestruction);

            DestroyTiles(cellToDestroy);
        }

        private void DestroyTiles(List<Vector3Int> cellToDestroy)
        {
            foreach (Vector3Int cellPos in cellToDestroy)
            {
                platforms.SetTile(cellPos, null);
            }

            lightController.UpdateLightAtEndOfFrame(player.transform.position);

            foreach (Vector3Int position in cellToDestroy)
                spawnedTilesPosition.Remove(position);
        }

        public bool IsAnySpawnedTiles(Func<Vector3Int, bool> condition)
        {
            var tiles = spawnedTilesPosition.Where(condition);
            if (tiles.Count() > 0)
                return true;
            else
                return false;
        }

        public bool IsAnySpawnedTiles()
        {
            return spawnedTilesPosition.Count > 0;
        }

        public void DestroyAllTilesInFront()
        {
            List<Vector3Int> positonTilesToDestroy = new List<Vector3Int>();
            Vector3Int positionInCell = ConvertLocalToCell(transform.position);
            int directionX = transform.rotation == Quaternion.AngleAxis(0, Vector3.up) ? 1 : -1;

            foreach (Vector3Int tilesPosition in spawnedTilesPosition)
            {
                if (directionX > 0 && tilesPosition.x >= positionInCell.x)
                {
                    positonTilesToDestroy.Add(tilesPosition);
                }
                else if (directionX < 0 && tilesPosition.x <= positionInCell.x)
                {
                    positonTilesToDestroy.Add(tilesPosition);
                }
            }

            DestroyTiles(positonTilesToDestroy);
        }
    }
}