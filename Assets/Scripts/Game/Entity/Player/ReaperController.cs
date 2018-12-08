using System.Collections;
using System.Net;
using System.Xml.Xsl;
using Game.Audio;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Game.Entity.Player
{
    public class ReaperController : EntityController
    {
        [SerializeField] private GameObject meleeAttack;


        [Tooltip("Distance travelled by the player when teleporting.")] [SerializeField]
        private float teleportationDistance;

        [Tooltip("Amount of time between teleportations.")] [SerializeField]
        private float TeleportationCoolDown;

        [Tooltip("End position height offset. Only used when the raycast does not hit anything before a teleportation occurs. Prevents ending up in walls when teleporting on a moving platform.")]
        [SerializeField] private float TeleportationHeightOffset;

        [SerializeField] private GameObject tpEffect1;
        [SerializeField] private GameObject tpEffect2;

        [Header("Sound")] [SerializeField] private AudioClip teleportSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        
        


        private PlayerController player;
        private bool capacityCanBeUsed;
        private bool finishTpEffect;
        private float timerStartTime;
        private BoxCollider2D bc;
        private bool mustTeleport;
        private Rigidbody2D rb;
        private Vector2 tpOffset;
        private Vector2 tpPosition;
        private Vector3 raycastHeightOffset;


        private void Start()
        {
            player = GetComponentInParent<PlayerController>();
            capacityCanBeUsed = true;
            timerStartTime = 0;
            bc = GetComponent<BoxCollider2D>();
            raycastHeightOffset = new Vector2(0, bc.size.y / 2);
            tpOffset = bc.size;
            mustTeleport = false;
            rb = GetComponentInParent<Rigidbody2D>();
            finishTpEffect = false;
        }

        private void OnDisable()
        {
            mustTeleport = false;
            finishTpEffect = false;
        }

        public override void UseCapacity()
        {
            SoundCaller.CallSound(teleportSound, soundToPlayPrefab, gameObject, false);
            Transform root = transform.parent;
            Destroy(Instantiate(tpEffect1, root.position, Quaternion.identity), 5);

            Debug.DrawLine(root.position - raycastHeightOffset,
                new Vector3(root.position.x + teleportationDistance * player.playerHorizontalDirection.x,
                    root.position.y - raycastHeightOffset.y, root.position.z), Color.blue,
                10);
            RaycastHit2D hit =
                Physics2D.Raycast(
                    root.position - raycastHeightOffset,
                    player.playerHorizontalDirection, teleportationDistance,
                    1 << LayerMask.NameToLayer(Values.Layers.Platform));

            if (hit.collider == null)
            {
                if (!player.kRigidBody.isOnMovingGround)
                    tpPosition =
                        new Vector2(
                            player.transform.position.x + teleportationDistance * player.playerHorizontalDirection.x -
                            (tpOffset.x * player.playerHorizontalDirection.x), player.transform.position.y + TeleportationHeightOffset);
                else
                {
                    tpPosition =
                        new Vector2(
                            player.transform.position.x + teleportationDistance * player.playerHorizontalDirection.x -
                            (tpOffset.x * player.playerHorizontalDirection.x),
                            player.transform.position.y + TeleportationHeightOffset);
                }
            }
            else
            {
                if (!player.kRigidBody.isOnMovingGround)
                    tpPosition =
                        new Vector2(
                            player.transform.position.x + hit.distance * player.playerHorizontalDirection.x -
                            (tpOffset.x * player.playerHorizontalDirection.x), player.transform.position.y);
                else
                {
                    tpPosition =
                        new Vector2(
                            player.transform.position.x + hit.distance * player.playerHorizontalDirection.x -
                            (tpOffset.x * player.playerHorizontalDirection.x),
                            player.transform.position.y);
                }
            }

            mustTeleport = true;
            capacityCanBeUsed = false;
            timerStartTime = Time.time;
            OnAttackFinish();
        }

        private void FixedUpdate()
        {
            if (mustTeleport)
            {
                rb.MovePosition(tpPosition);
                mustTeleport = false;
                finishTpEffect = true;
            }
        }

        private void Update()
        {
            if (finishTpEffect)
            {
                Destroy(Instantiate(tpEffect2, rb.position, Quaternion.identity), 5);
                finishTpEffect = false;
            }
        }

        public override bool CapacityUsable()
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

        public override void UseBasicAttack()
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
            meleeAttackObject.transform.parent = transform;
            animator.SetTrigger(Values.AnimationParameters.Player.Attack);
        }
    }
}