using Harmony;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Boss
{
    public class SpawnedTilesManager : MonoBehaviour
    {
        private List<Vector3Int> spawnedTilesPosition;

        private LightController lightController;
        private Tilemap platforms;
        private Health health;

        private void Awake()
        {
            lightController = GameObject.FindGameObjectWithTag(R.S.Tag.LightManager).GetComponent<LightController>();
            platforms = GameObject.FindGameObjectWithTag(R.S.Tag.Plateforme).GetComponent<Tilemap>();
            health = GetComponent<Health>();

            health.OnDeath += Health_OnDeath;

            spawnedTilesPosition = new List<Vector3Int>();
        }

        private void Health_OnDeath(GameObject gameObject)
        {
            DestroyTiles(new List<Vector3Int>(spawnedTilesPosition));
        }

        public Vector3Int ConvertLocalToCell(Vector2 position)
        {
            return platforms.LocalToCell(position);
        }

        public void SpawnTiles(Vector3Int center, List<Vector3Int> tilesPositionRelative, Tile tileToSpawn)
        {
            List<Vector3Int> platformPositions = new List<Vector3Int>();

            foreach (Vector3Int relativePosition in tilesPositionRelative)
            {
                if (platforms.GetTile(new Vector3Int(relativePosition.x + center.x, relativePosition.y + center.y, 0)) == null)
                {
                    platforms.SetTile(new Vector3Int(relativePosition.x + center.x, relativePosition.y + center.y, 0), tileToSpawn);
                    platformPositions.Add(new Vector3Int(relativePosition.x + center.x, relativePosition.y + center.y, 0));
                }
            }

            lightController.UpdateLightAtEndOfFrame();

            spawnedTilesPosition.AddRange(platformPositions);
        }

        public void SpawnTiles(Vector3Int center, List<Vector3Int> tilesPositionRelative, Tile tileToSpawn, float tilesLifeTime)
        {
            List<Vector3Int> platformPositions = new List<Vector3Int>();

            foreach (Vector3Int relativePosition in tilesPositionRelative)
            {
                if (platforms.GetTile(new Vector3Int(relativePosition.x + center.x, relativePosition.y + center.y, 0)) == null)
                {
                    platforms.SetTile(new Vector3Int(relativePosition.x + center.x, relativePosition.y + center.y, 0), tileToSpawn);
                    platformPositions.Add(new Vector3Int(relativePosition.x + center.x, relativePosition.y + center.y, 0));
                }
            }

            lightController.UpdateLightAtEndOfFrame();

            spawnedTilesPosition.AddRange(platformPositions);

            StartCoroutine(DestroyTiles(platformPositions, tilesLifeTime));
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

            lightController.UpdateLightAtEndOfFrame();

            foreach (Vector3Int position in cellToDestroy)
                spawnedTilesPosition.Remove(position);
        }

        public bool IsAnySpawnedTilesToLeftOfPosition(Vector3Int position)
        {
            foreach(Vector3Int tilesPosition in spawnedTilesPosition)
            {
                if(tilesPosition.x < position.x)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsAnySpawnedTilesToRightOfPosition(Vector3Int position)
        {
            foreach (Vector3Int tilesPosition in spawnedTilesPosition)
            {
                if (tilesPosition.x > position.x)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsAnyTilesSpawned()
        {
            return spawnedTilesPosition.Count > 0;
        }

        public void DestroyAllTilesToTheRight(Vector3Int position)
        {
            List<Vector3Int> positonTilesToDestroy = new List<Vector3Int>();

            foreach (Vector3Int tilesPosition in spawnedTilesPosition)
            {
                if (tilesPosition.x >= position.x)
                {
                    positonTilesToDestroy.Add(tilesPosition);
                }
            }

            DestroyTiles(positonTilesToDestroy);
        }

        public void DestroyAllTilesToTheLeft(Vector3Int position)
        {
            List<Vector3Int> positonTilesToDestroy = new List<Vector3Int>();

            foreach (Vector3Int tilesPosition in spawnedTilesPosition)
            {
                if (tilesPosition.x <= position.x)
                {
                    positonTilesToDestroy.Add(tilesPosition);
                }
            }

            DestroyTiles(positonTilesToDestroy);
        }
    }
}

