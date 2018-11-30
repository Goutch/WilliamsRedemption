using Game.Entity.Player;
using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class Bat : Enemy
    {
        [SerializeField] private Vector2 exponentialFonction;
        [SerializeField] private float fonctionYOffSet = .32f;

        [Header("Sound")] [SerializeField] private AudioClip batSound;
        [SerializeField] private GameObject soundToPlayPrefab;

        private RootMover rootMover;

        private bool isTriggered;

        protected IEnumerator Fly()
        {
            rootMover.LookAtPlayer();

            int direction = (transform.rotation.y == 1 ? -1 : 1);
            while (true)
            {
                exponentialFonction.x += 1 * Time.deltaTime;
                exponentialFonction.y = Mathf.Pow(exponentialFonction.x, 2) + fonctionYOffSet;

                rootMover.FlyToward(
                    new Vector2(exponentialFonction.x * direction + transform.position.x,
                        exponentialFonction.y + transform.position.y));

                //BEN_REVIEW : Pourquoi pas au Update ? Surtout que "FlyToward" utilise "Time.deltaTime" et non pas
                //             "Time.fixedDeltaTime".
                //
                //             Pour attendre au prochain "Update", retourne "null".
                yield return new WaitForFixedUpdate(); 
            }
        }

        protected override void Init()
        {
            rootMover = GetComponent<RootMover>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.root.CompareTag(Values.Tags.Player) && !isTriggered)
            {
                isTriggered = true;
                animator.SetTrigger(Values.AnimationParameters.Enemy.Fly);

                StartCoroutine(Fly());
                SoundCaller.CallSound(batSound, soundToPlayPrefab, gameObject, true);
                Destroy(this.gameObject, 10);
            }
        }
    }
}