using Game.Entity.Player;
using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class Bat : Enemy
    {     
        [SerializeField] private AudioClip batSound;
        [SerializeField] private Vector2 exponentialFonction;
        [SerializeField] private float fonctionYOffSet = .32f;
        [SerializeField] private GameObject soundToPlayPrefab;
        
        private RootMover rootMover;

        private bool isTriggered;
        private GameObject soundToPlay;

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
            if(collision.transform.root.CompareTag(Values.Tags.Player) && !isTriggered)
            {
                isTriggered = true;
                animator.SetTrigger(Values.AnimationParameters.Enemy.Fly);

                StartCoroutine(Fly());
                UseSound();
                Destroy(this.gameObject, 10);
            }
        }
        private void UseSound()
        {
            soundToPlay=Instantiate(soundToPlayPrefab,this.transform.position,Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(batSound, true, gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }
}

