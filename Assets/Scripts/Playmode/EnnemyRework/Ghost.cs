using Light;
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

        public override void Init(GameObject enemyControllerObject)
        {
            rootMover = enemyControllerObject.GetComponent<RootMover>();
            collider = enemyControllerObject.GetComponent<BoxCollider2D>();
            collider.isTrigger = true;

            enemyControllerObject.GetComponent<Rigidbody2D>().isKinematic = true;
            spriteRenderer = enemyControllerObject.GetComponent<SpriteRenderer>();
        }

        public override void Act()
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

        private void DestroyEnnemyObject(bool Inlight)
        {
            if(Inlight)
            Destroy(rootMover.gameObject);
        }
        
    }
    
}

