using Light;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Playmode.EnnemyRework
{
    public class Ghost : Enemy
    {
        [SerializeField] private int range;
        private Vector2 disapearPos;
        private RootMover rootMover;
        private Collider2D collider;
        private SpriteRenderer spriteRenderer;
        private LightSensor lightSensor;


        protected override void Init()
        {
            rootMover = GetComponent<RootMover>();
            collider = GetComponent<BoxCollider2D>();
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
                rootMover.FlyToward(PlayerController.instance.transform.position);
            }
            else
            {
                disapearPos = rootMover.transform.position;
                spriteRenderer.enabled = false;
            }
        }

    }
}