using Game.Entity.Enemies.Attack;
using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Edgar
{
    [RequireComponent(typeof(RootMover))]
    public class ConcentrationOfPower : Capacity
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.PlasmaConcentration + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private int numberOfParticules;
        [SerializeField] private GameObject particulesPrefab;
        [SerializeField] private float radiusOfRingWhereParticulesSpawn;
        [SerializeField] private float particulesSpeed;
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;
        [SerializeField] private float delayBetweenEachParticuleSpawn;

        private const float EQUALITY_DISTANCE_SENSIBILITY = 1f;

        private RootMover rootMover;

        private float lastTimeUsed;
        private GameObject[] particules;
        private bool allParticulesSpawned;

        private void Awake()
        {
            if (capacityUsableAtStart)
                lastTimeUsed = -cooldown;

            rootMover = GetComponent<RootMover>();
        }

        public override void Act()
        {
            rootMover.LookAtPlayer();

            UpdateParticulesState();
        }

        private void UpdateParticulesState()
        {
            int numberOfParticulesLeft = 0;

            for (int i = 0; i < particules.Length; ++i)
            {
                if (particules[i] != null)
                {
                    if (Vector2.Distance(particules[i].transform.position, transform.position) < EQUALITY_DISTANCE_SENSIBILITY)
                    {
                        Destroy(particules[i].gameObject);
                    }
                    else
                    {
                        ++numberOfParticulesLeft;
                    }
                }
            }

            if (numberOfParticulesLeft == 0 && allParticulesSpawned)
                Finish();
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }
        public override void Enter()
        {
            base.Enter();

            StartCoroutine(CreateParticules());
            animator.SetTrigger(Values.AnimationParameters.Edgar.PlasmaConcentration);

            allParticulesSpawned = false;
            lastTimeUsed = Time.time;
        }

        private IEnumerator CreateParticules()
        {
            particules = new GameObject[numberOfParticules];

            for (int i = 0; i < numberOfParticules; ++i)
            {
                Vector3 position = GetParticulePosition();

                particules[i] = Instantiate(particulesPrefab, position, BossDirection(position));
                particules[i].GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Enemy);

                ProjectileController projectileController = particules[i].GetComponent<ProjectileController>();
                projectileController.Speed = particulesSpeed;
                projectileController.DestroyOnPlatformsCollision = false;

                yield return new WaitForSeconds(delayBetweenEachParticuleSpawn);
            }

            allParticulesSpawned = true;
        }

        private Vector3 GetParticulePosition()
        {
            float randomAngle = Random.Range(0,360);
            Vector2 randomAngleVector = new Vector2(Mathf.Cos(randomAngle) * radiusOfRingWhereParticulesSpawn, Mathf.Sin(randomAngle) * radiusOfRingWhereParticulesSpawn);
            Vector3 position = transform.position + new Vector3(randomAngleVector.x, randomAngleVector.y);

            return position;
        }

        private Quaternion BossDirection(Vector3 objectPosition)
        {
            Vector2 dir = transform.position - objectPosition;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion direction = Quaternion.AngleAxis(angle, Vector3.forward);
            return direction;
        }
    }
}

