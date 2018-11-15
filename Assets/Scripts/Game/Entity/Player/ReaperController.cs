using System.Collections;
using System.Net;
using System.Xml.Xsl;
using UnityEngine;

namespace Game.Entity.Player
{
    public class ReaperController : EntityController
    {
        [SerializeField] private GameObject meleeAttack;


        [Tooltip("Distance travelled by the player when teleporting.")] [SerializeField]
        private float teleportationDistance;

        [Tooltip("Amount of time between teleportations.")] [SerializeField]
        private float TeleportationCoolDown;

        [SerializeField] private GameObject tpEffect1;
        [SerializeField] private GameObject tpEffect2;
        [SerializeField] private AudioClip teleportSound;

        private bool capacityCanBeUsed;
        private float timerStartTime;
        private BoxCollider2D bc;
        private Vector2 tpOffset;
        private Vector2 tpPosition;
        private bool mustTeleport;
        private Rigidbody2D rb;

        private void Start()
        {
            capacityCanBeUsed = true;
            timerStartTime = 0;
            bc = GetComponent<BoxCollider2D>();
            tpOffset = bc.size;
            mustTeleport = false;
            rb = GetComponentInParent<Rigidbody2D>();
        }

        private void OnDisable()
        {
            mustTeleport = false;
        }

        public override void UseCapacity(PlayerController player)
        {
            UseSound();
            Transform root = transform.parent;
            Destroy(Instantiate(tpEffect1, root.position, Quaternion.identity), 5);

            Debug.DrawLine(root.position,
                new Vector3(root.position.x + teleportationDistance * player.playerHorizontalDirection.x,
                    root.position.y, root.position.z), Color.blue,
                10);
            RaycastHit2D hit =
                Physics2D.Raycast(
                    root.position,
                    player.playerHorizontalDirection, teleportationDistance,
                    player.ReaperLayerMask);

            if (hit.collider == null)
            {
                tpPosition =
                    new Vector2(
                        player.transform.position.x + teleportationDistance * player.playerHorizontalDirection.x -
                        (tpOffset.x * player.playerHorizontalDirection.x), player.transform.position.y);
            }
            else
            {
                tpPosition =
                    new Vector2(
                        player.transform.position.x + hit.distance * player.playerHorizontalDirection.x -
                        (tpOffset.x * player.playerHorizontalDirection.x), player.transform.position.y);
            }

            mustTeleport = true;
            capacityCanBeUsed = false;
            timerStartTime = Time.time;
            Destroy(Instantiate(tpEffect2, root.position, Quaternion.identity), 5);
            OnAttackFinish();
        }

        private void FixedUpdate()
        {
            if (mustTeleport)
            {
                rb.MovePosition(tpPosition);
                mustTeleport = false;
            }
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
        
        private void UseSound()
        {
            GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<AudioManager>()
                .PlaySound(teleportSound);
        }
    }
}