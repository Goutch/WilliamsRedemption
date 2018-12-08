using Game.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Entity.Enemies.Boss
{
    public struct BirthLifeTime
    {
        public float birth;
        public float lifeTime;

        public BirthLifeTime(float birth, float lifeTime)
        {
            this.birth = birth;
            this.lifeTime = lifeTime;
        }
    }

    public class SpawnedTilesManager : MonoBehaviour
    {
        //Cell, lifetime
        private Dictionary<Vector3Int, BirthLifeTime> spawnedTilesPosition;

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

            spawnedTilesPosition = new Dictionary<Vector3Int, BirthLifeTime>();
        }

        private void Update()
        {
            Dictionary<Vector3Int, BirthLifeTime> platformToDestroy = new Dictionary<Vector3Int, BirthLifeTime>();

            foreach (KeyValuePair<Vector3Int, BirthLifeTime> keyValuePair in spawnedTilesPosition)
            {
                if(Time.time - keyValuePair.Value.birth > keyValuePair.Value.lifeTime)
                {
                    platformToDestroy.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            DestroyTiles(platformToDestroy);
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag(Values.Tags.Player);
        }

        private void Health_OnDeath(GameObject receiver, GameObject attacker)
        {
            DestroyTiles(new Dictionary<Vector3Int, BirthLifeTime>(spawnedTilesPosition));
        }

        public Vector3Int ConvertLocalToCell(Vector2 position)
        {
            return platforms.LocalToCell(position);
        }

        public void SpawnTiles(Vector3Int center, List<Vector3Int> tilesPositionRelative, Tile tileToSpawn,
            float? tilesLifeTime = null)
        {
            Dictionary<Vector3Int, BirthLifeTime> platformPositions = new Dictionary<Vector3Int, BirthLifeTime>();

            foreach (Vector3Int relativePosition in tilesPositionRelative)
            {
                if (platforms.GetTile(new Vector3Int(relativePosition.x + center.x, relativePosition.y + center.y,
                        0)) == null || spawnedTilesPosition.ContainsKey(new Vector3Int(relativePosition.x + center.x, relativePosition.y + center.y,
                        0)))
                {
                    platforms.SetTile(new Vector3Int(relativePosition.x + center.x, relativePosition.y + center.y, 0),
                        tileToSpawn);
                    platformPositions.Add(new Vector3Int(relativePosition.x + center.x, relativePosition.y + center.y,
                        0),  new BirthLifeTime(Time.time, tilesLifeTime ?? 0));
                }
            }

            lightController.UpdateLightAtEndOfFrame(player.transform.position);

            foreach (KeyValuePair<Vector3Int, BirthLifeTime> keyValue in platformPositions)
            {
                if(!spawnedTilesPosition.ContainsKey(keyValue.Key))
                {
                    spawnedTilesPosition[keyValue.Key] = keyValue.Value;
                }
                else
                {
                    BirthLifeTime oldValue = spawnedTilesPosition[keyValue.Key];
                    BirthLifeTime newValue = keyValue.Value;

                    float oldExpectedDeath = oldValue.birth + oldValue.lifeTime;
                    float newExpectedDeath = newValue.birth + newValue.lifeTime;

                    if(newExpectedDeath > oldExpectedDeath)
                    {
                        spawnedTilesPosition[keyValue.Key] = newValue;
                    }
                }
            }
        }

        public void DestroyAllTiles()
        {
            DestroyTiles(new Dictionary<Vector3Int, BirthLifeTime>(spawnedTilesPosition));
        }

        private void DestroyTiles(Dictionary<Vector3Int, BirthLifeTime> cellToDestroy)
        {
            foreach (KeyValuePair<Vector3Int, BirthLifeTime> cellPos in cellToDestroy)
            {
                platforms.SetTile(cellPos.Key, null);
            }

            lightController.UpdateLightAtEndOfFrame(player.transform.position);

            foreach (KeyValuePair<Vector3Int, BirthLifeTime> position in cellToDestroy)
                spawnedTilesPosition.Remove(position.Key);
        }

        public bool IsAnySpawnedTiles(Func<KeyValuePair<Vector3Int, BirthLifeTime>, bool> condition)
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
            Dictionary<Vector3Int, BirthLifeTime> positonTilesToDestroy = new Dictionary<Vector3Int, BirthLifeTime>();
            Vector3Int positionInCell = ConvertLocalToCell(transform.position);
            int directionX = transform.rotation == Quaternion.AngleAxis(0, Vector3.up) ? 1 : -1;

            foreach (KeyValuePair<Vector3Int, BirthLifeTime> tilesPosition in spawnedTilesPosition)
            {
                if (directionX > 0 && tilesPosition.Key.x >= positionInCell.x)
                {
                    positonTilesToDestroy.Add(tilesPosition.Key, tilesPosition.Value);
                }
                else if (directionX < 0 && tilesPosition.Key.x <= positionInCell.x)
                {
                    positonTilesToDestroy.Add(tilesPosition.Key, tilesPosition.Value);
                }
            }

            DestroyTiles(positonTilesToDestroy);
        }
    }
}