using Game.Entity.Player;
using Game.Puzzle.Light;
using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class Ghost : Enemy
    {
        [SerializeField] private float damageKnockBackForce = 1;

        [SerializeField] private float disapearTimeLimitBeforeDesSpawn = 5;
        private RootMover rootMover;
        private Rigidbody2D rigidBody;
        private LightSensor playerLightSensor;
        private bool isPlayerReaper = false;
        private bool aprearing;
        private bool disapearing;

        public bool IsPlayerReaper
        {
            get { return isPlayerReaper; }
            set { isPlayerReaper = value; }
        }

        private void Start()
        {
            rootMover = GetComponent<RootMover>();
            rigidBody = GetComponent<Rigidbody2D>();
            playerLightSensor = PlayerController.instance.GetComponent<LightSensor>();
            playerLightSensor.OnLightExpositionChange += OnPlayerStateChange;
        }

        private void FixedUpdate()
        {
            if (playerLightSensor.InLight == false)
            {
                UpdateDirection();
                rootMover.FlyToward(PlayerController.instance.transform.position);
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }

        private void OnPlayerStateChange(bool isInLight)
        {
            IsPlayerReaper = !isInLight;
            if (IsPlayerReaper)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
                StartCoroutine(DisapearRoutine());
            }
        }

        private void OnDisable()
        {
            playerLightSensor.OnLightExpositionChange -= OnPlayerStateChange;
        }

        protected override void Init()
        {
            
        }

        protected override void OnHit(HitStimulus other)
        {
            if (other.DamageSource == HitStimulus.DamageSourceType.Reaper)
            {
                Vector2 kockBackDir = (this.transform.position - other.transform.position);
                base.OnHit(other);
                rigidBody.AddForce(kockBackDir.normalized * damageKnockBackForce, ForceMode2D.Impulse);
            }
        }

        private IEnumerator DisapearRoutine()
        {
            float disapearTime = Time.time;
            while (Time.time < disapearTime + disapearTimeLimitBeforeDesSpawn)
            {
                //if player is william 
                if (!isPlayerReaper)
                    yield return null;
                else
                {
                    //if he is reaper reapear
                    yield break;
                }
            }
            
            Destroy(this.gameObject);
            
        }
        
    }
}