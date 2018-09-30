using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Edgar
{
    class Jump : State
    {
        private EdgarController edgarController;
        private Tilemap plateforms;
        private Tile spawnTile;
        private bool jumped = false;
        private LightController lightController;
        private float delayDestructionJumpPlatforms;

        public Jump(Tilemap plateforms, Tile spawnTile, LightController lightController, float delayDestructionJumpPlatforms)
        {
            this.plateforms = plateforms;
            this.spawnTile = spawnTile;
            this.lightController = lightController;
            this.delayDestructionJumpPlatforms = delayDestructionJumpPlatforms;
        }

        public void Act()
        {
            if (!jumped)
            {
                edgarController.Jump();
                jumped = true;
            }
            else if (edgarController.rb.velocity.y == 0)
            {
                edgarController.OnJumpFinish();
                Vector3Int cellPos = plateforms.LocalToCell(edgarController.transform.position);
                cellPos += new Vector3Int(0, -2, 0);

                Vector3Int[] platformsPosition = new Vector3Int[] {
                    cellPos + new Vector3Int(3, 1, 0),
                    cellPos + new Vector3Int(-3, 1, 0),
                    cellPos + new Vector3Int(2, 0, 0),
                    cellPos + new Vector3Int(-2, 0, 0)
                };

                plateforms.SetTile(platformsPosition[0], spawnTile);
                plateforms.SetTile(platformsPosition[1], spawnTile);
                plateforms.SetTile(platformsPosition[2], spawnTile);
                plateforms.SetTile(platformsPosition[3], spawnTile);

                lightController.UpdateLightAtEndOfFrame();

                GameObject plateformsDestroyer = new GameObject();
                TimedPlateformeDestroyer timedPlatformsDestroyer = plateformsDestroyer.AddComponent<TimedPlateformeDestroyer>();
                timedPlatformsDestroyer.Init(platformsPosition, plateforms, delayDestructionJumpPlatforms, lightController);
            }
            else
            {
                edgarController.transform.Translate(new Vector2(Math.Sign(PlayerController.instance.transform.position.x - edgarController.transform.position.x) * edgarController.speed * Time.deltaTime, 0));
            }
        }

        public bool Init(EdgarController edgarController)
        {
            this.edgarController = edgarController;
            return true;
        }
    }
}