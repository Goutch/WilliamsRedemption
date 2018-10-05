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
        private float upwardForceOnLandingWhenPlayerIsInAir;
        private float upwardForceOnLandingWhenPlayerIsOnGround;

        public Jump(Tilemap plateforms, 
            Tile spawnTile, 
            LightController lightController, 
            float delayDestructionJumpPlatforms, 
            GameObject leftFoot, 
            GameObject rightFoot, 
            float upwardForceOnLandingWhenPlayerIsInAir, 
            float upwardForceOnLandingWhenPlayerIsOnGround)
        {
            this.plateforms = plateforms;
            this.spawnTile = spawnTile;
            this.lightController = lightController;
            this.delayDestructionJumpPlatforms = delayDestructionJumpPlatforms;
            this.leftFoot = leftFoot;
            this.rightFoot = rightFoot;
            this.upwardForceOnLandingWhenPlayerIsInAir = upwardForceOnLandingWhenPlayerIsInAir;
            this.upwardForceOnLandingWhenPlayerIsOnGround = upwardForceOnLandingWhenPlayerIsOnGround;
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
                edgarController.transform.Translate(new Vector2(-edgarController.Speed * Time.deltaTime, 0));
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
                edgarController.rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

                edgarController.OnJumpFinish();
                Vector3Int cellPos = plateforms.LocalToCell(edgarController.transform.position);
                cellPos += new Vector3Int(0, -1, 0);

                List<Vector3Int> platformsPosition = new List<Vector3Int>();

                if(plateforms.GetTile(cellPos + new Vector3Int(2, 1, 0)) == null)
                {
                    platformsPosition.Add(cellPos + new Vector3Int(2, 1, 0));
                }

                if (plateforms.GetTile(cellPos + new Vector3Int(-2, 1, 0)) == null)
                {
                    platformsPosition.Add(cellPos + new Vector3Int(-2, 1, 0));
                }

                if (plateforms.GetTile(cellPos + new Vector3Int(1, 0, 0)) == null)
                {
                    platformsPosition.Add(cellPos + new Vector3Int(1, 0, 0));
                }

                if (plateforms.GetTile(cellPos + new Vector3Int(-1, 0, 0)) == null)
                {
                    platformsPosition.Add(cellPos + new Vector3Int(-1, 0, 0));
                }

                foreach (Vector3Int postionCell in platformsPosition)
                    plateforms.SetTile(postionCell, spawnTile);

                lightController.UpdateLightAtEndOfFrame();

                GameObject plateformsDestroyer = new GameObject();
                TimedPlateformeDestroyer timedPlatformsDestroyer = plateformsDestroyer.AddComponent<TimedPlateformeDestroyer>();
                timedPlatformsDestroyer.Init(platformsPosition, plateforms, delayDestructionJumpPlatforms, lightController);

                PlayerController.instance.kRigidBody.Velocity = new Vector2(0, PlayerController.instance.kRigidBody.Velocity.y);
                float directionX = Math.Sign(PlayerController.instance.transform.position.x - edgarController.transform.position.x);
               // if (PlayerController.instance.IsOnGround)
                    //PlayerController.instance.kRigidBody.AddForce(new Vector2(directionX, upwardForceOnLandingWhenPlayerIsOnGround), ForceMode2D.Impulse);
                //else
                    //PlayerController.instance.kRigidBody.AddForce(new Vector2(directionX, upwardForceOnLandingWhenPlayerIsInAir), ForceMode2D.Impulse);
            }
        }
    }
}