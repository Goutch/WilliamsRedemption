﻿using Light;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Playmode.EnnemyRework
{
    [CreateAssetMenu(fileName = "Ghost", menuName = "EnnemyStrategy/Ghost", order = 1)]
    public class Ghost : Enemy
    {
        [SerializeField] private int range;
        private Vector2 disapearPos;
        private RootMover rootMover;
        private Collider2D collider;
        private SpriteRenderer spriteRenderer;
        private LightSensor lightSensor;

        private void Awake()
        {
            rootMover = GetComponent<RootMover>();
            collider = GetComponent<BoxCollider2D>();
            collider.isTrigger = true;
            GetComponent<Rigidbody2D>().isKinematic = true;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (Vector2.Distance(PlayerController.instance.transform.position, rootMover.transform.position) > range)
            {
                Destroy(rootMover.gameObject);
            }
            if (PlayerController.instance.CurrentController.GetComponent<ReaperController>())
            {
                spriteRenderer.enabled = true;
                rootMover.FlyToward(PlayerController.instance.transform.position, Speed);
            }
            else
            {
                disapearPos = rootMover.transform.position;
                spriteRenderer.enabled = false;
            }
        }

        public override void ReceiveDamage()
        {
            throw new System.NotImplementedException();
        }
    }
    
}

