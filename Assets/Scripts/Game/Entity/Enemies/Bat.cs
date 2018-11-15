using Game.Entity.Player;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class Bat : Enemy
    {
        [SerializeField] private Vector2 exponentialFonction;
        [SerializeField] private float fonctionYOffSet = .32f;

        private RootMover rootMover;

        private int direction = 1;
        private bool isTriggered;



        protected void Fly()
        {
            exponentialFonction.x += 1 * Time.deltaTime;
            exponentialFonction.y = Mathf.Pow(exponentialFonction.x, 2) + fonctionYOffSet;
            rootMover.FlyToward(
                new Vector2(exponentialFonction.x * direction + transform.position.x,
                    exponentialFonction.y + transform.position.y));
        }

        public void OnTriggered()
        {
            if (!isTriggered)
            {
                isTriggered = true;
                animator.SetTrigger(Values.AnimationParameters.Enemy.Fly);
                Destroy(this.gameObject, 10);
                direction = player.transform.position.x - transform.root.position.x > 0
                    ? 1
                    : -1;
                if (direction == 1)
                    spriteRenderer.flipX = false;
                else
                    spriteRenderer.flipX = true;
            }
        }

        protected override void Init()
        {
            rootMover = GetComponent<RootMover>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (isTriggered)
            {
                Fly();
            }
        }
    }
}

