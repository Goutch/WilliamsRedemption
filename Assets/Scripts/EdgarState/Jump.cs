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
        private GameObject leftFoot;
        private GameObject rightFoot;

        public Jump(Tilemap plateforms, Tile spawnTile, LightController lightController, float delayDestructionJumpPlatforms, GameObject leftFoot, GameObject rightFoot)
        {
            this.plateforms = plateforms;
            this.spawnTile = spawnTile;
            this.lightController = lightController;
            this.delayDestructionJumpPlatforms = delayDestructionJumpPlatforms;
            this.leftFoot = leftFoot;
            this.rightFoot = rightFoot;
        }

        public void Act()
        {
            if (!jumped)
            {
                edgarController.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                edgarController.Jump();
                jumped = true;
            }
            else
            {
                edgarController.transform.Translate(new Vector2(Math.Sign(PlayerController.instance.transform.position.x - edgarController.transform.position.x) * edgarController.speed * Time.deltaTime, 0));
                if (edgarController.rb.velocity.y < 0)
                    landOnGround();
            }
        }

        public bool Init(EdgarController edgarController)
        {
            this.edgarController = edgarController;
            return true;
        }

        private void landOnGround()
        {

            RaycastHit2D leftFeetSupportOnGound = Physics2D.Raycast(leftFoot.transform.position, new Vector2(0, -1), 0.32f, 1 << LayerMask.NameToLayer("Default"));
            RaycastHit2D rightFeetSupportOnGround = Physics2D.Raycast(leftFoot.transform.position, new Vector2(0, -1), 0.32f, 1 << LayerMask.NameToLayer("Default"));

            Debug.DrawLine(leftFoot.transform.position, leftFoot.transform.position + new Vector3(0, -0.32f),Color.blue);
            Debug.DrawLine(rightFoot.transform.position, rightFoot.transform.position + new Vector3(0, -0.32f), Color.blue);

            if (leftFeetSupportOnGound.collider != null || rightFeetSupportOnGround.collider != null)
            {
                Debug.Log("Yolo");

                edgarController.rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

                edgarController.OnJumpFinish();
                Vector3Int cellPos = plateforms.LocalToCell(edgarController.transform.position);
                cellPos += new Vector3Int(0, -1, 0);

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

                PlayerController.instance.Rigidbody.velocity = new Vector2(0, PlayerController.instance.Rigidbody.velocity.y);
                //PlayerController.instance.LockMovement(1);
                if (PlayerController.instance.IsOnGround)
                    PlayerController.instance.Rigidbody.AddForce(new Vector2(-1, 5), ForceMode2D.Impulse);
                else
                    PlayerController.instance.Rigidbody.AddForce(new Vector2(-1, 1), ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("Fail");
            }
        }
    }
}