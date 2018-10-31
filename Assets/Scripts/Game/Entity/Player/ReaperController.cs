using System.Collections;
using System.Net;
using System.Xml.Xsl;
using UnityEngine;

namespace Game.Entity.Player
{
    public class ReaperController : EntityController
    {
        [Tooltip("Distance travelled by the player when teleporting.")]
        [SerializeField] private float teleportationDistance;
        [SerializeField] private GameObject tpEffect;
        [SerializeField] private GameObject meleeAttack;
        [Tooltip("Amount of time before the teleportation visual effect vanishes.")]
        [SerializeField] private float timeBeforeTpEffectVanish;

        [Tooltip("Amount of time between teleportations.")]
        [SerializeField] private float TeleportationCoolDown;

        private bool capacityCanBeUsed;
        private float timerStartTime;
        private SpriteRenderer sr;
        private Vector2 tpOffset;
        

        private void Start()
        {
            capacityCanBeUsed = true;
            timerStartTime = 0;
            sr = GetComponent<SpriteRenderer>();
            tpOffset = sr.size*0.5f;
        }

        public override void UseCapacity(PlayerController player)
        {
            Transform root = transform.parent;
            GameObject tpEffectTemp = Instantiate(tpEffect, root.position, Quaternion.identity);
            StartCoroutine(TeleportEffectRemove(tpEffectTemp, player));

            Debug.DrawLine(root.position,
                new Vector3(root.position.x + teleportationDistance * player.playerHorizontalDirection.x, root.position.y, root.position.z), Color.blue,
                10);
            RaycastHit2D hit =
                        Physics2D.Raycast(
                            root.position,
                            player.playerHorizontalDirection, teleportationDistance ,
                            player.ReaperLayerMask);

            if (hit.collider == null)
            {
                player.GetComponent<Rigidbody2D>().position = new Vector2(player.transform.position.x+ teleportationDistance* player.playerHorizontalDirection.x , player.transform.position.y);
            }
            else
            {
               player.GetComponent<Rigidbody2D>().position = new Vector2(player.transform.position.x + hit.distance*player.playerHorizontalDirection.x -(tpOffset.x* player.playerHorizontalDirection.x),player.transform.position.y);
            }

            capacityCanBeUsed = false;
            timerStartTime = Time.time;
        }

        public override bool CapacityUsable(PlayerController player)
        {
            if (capacityCanBeUsed && player.IsOnGround)
            {
                return true;
            }
            if (!capacityCanBeUsed && (Time.time - timerStartTime) >= TeleportationCoolDown)
            {
                capacityCanBeUsed = true;
                if (player.IsOnGround)
                {
                    return true;
                }
            }
            return false;
        }

        IEnumerator TeleportEffectRemove(GameObject tpEffect, PlayerController player)
        {
            player.LockTransformation();
            yield return new WaitForSeconds(timeBeforeTpEffectVanish);
            Destroy(tpEffect);
            player.UnlockTransformation();
        }

        public override void UseBasicAttack(PlayerController player)
        {
            Quaternion angle = Quaternion.identity;

            if (player.playerHorizontalDirection == Vector2.left)
                angle = Quaternion.AngleAxis(180, Vector3.up);

            if (player.playerHorizontalDirection == Vector2.down && !player.IsOnGround)
                angle = Quaternion.AngleAxis(-90, Vector3.forward);
            else if (player.playerHorizontalDirection == Vector2.up)
                angle = Quaternion.AngleAxis(90, Vector3.forward);

            GameObject meleeAttackObject = Instantiate(meleeAttack, transform);
            meleeAttackObject.transform.localRotation = angle;
            animator.SetTrigger(Values.AnimationParameters.Player.Attack);
        }
        
    }
}

