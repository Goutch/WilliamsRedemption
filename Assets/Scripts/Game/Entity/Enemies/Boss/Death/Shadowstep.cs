using Game.Entity.Enemies.Boss;
using Game.Entity.Player;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Entity.Enemies.Boss.Death
{
    [RequireComponent(typeof(RootMover), typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class Shadowstep : Capacity
    {
        [SerializeField] private float distanceFromPlayer;
        [SerializeField] private float cooldown;
        [SerializeField] private bool tpInFront;

        private new Rigidbody2D rigidbody;
        private new Collider2D collider;
        private RootMover mover;

        private float lastTimeUsed;

        [UsedImplicitly]
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
            mover = GetComponent<RootMover>();
        }

        public override void Act()
        {

        }

        private void TeleportBehindPlayer()
        {
            Vector2 playerPosition = PlayerController.instance.transform.position;
            EntityController currentPlayerController = PlayerController.instance.CurrentController;
            SpriteRenderer currentPlayerSpriteRenderer = currentPlayerController.GetComponent<SpriteRenderer>();

            int playerDirectionFacing = currentPlayerSpriteRenderer.flipX ? -1 : 1;
            int direction = (tpInFront ? 1 : -1) * playerDirectionFacing;

            RaycastHit2D hit = Physics2D.Raycast(playerPosition, new Vector2(direction, 0), distanceFromPlayer + collider.bounds.size.x / 2, 1 << LayerMask.NameToLayer(Values.Layers.Default));

            Vector2 newPosition;

            if(hit.collider == null)
            {
                newPosition = playerPosition + new Vector2(distanceFromPlayer * direction, 0);
            }
            else
            {
                float distanceToTeleport = hit.distance - collider.bounds.size.x / 2;
                newPosition = playerPosition + new Vector2(distanceToTeleport * direction, 0);
            }

            RaycastHit2D hitFloor = Physics2D.Raycast(newPosition, Vector2.down, float.MaxValue, 1 << LayerMask.NameToLayer(Values.Layers.Default));

            if (hitFloor.collider != null)
            {
                newPosition += new Vector2(0, hitFloor.distance * Vector2.down.y);
            }

            rigidbody.MovePosition(newPosition);

            mover.LookAtPlayer(newPosition);
        }

        public override void Enter()
        {
            base.Enter();

            TeleportBehindPlayer();

            lastTimeUsed = Time.time;

            Finish();
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }
    }
}
