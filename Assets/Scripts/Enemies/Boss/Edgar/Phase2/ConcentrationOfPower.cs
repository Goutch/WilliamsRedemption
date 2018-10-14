using System.Collections;
using UnityEngine;
using UnityEditor;
using Harmony;

namespace Playmode.EnnemyRework.Boss.Edgar
{
    [RequireComponent(typeof(RootMover))]
    public class ConcentrationOfPower : Capacity
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.PlasmaConcentration + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private int numberOfParticules;
        [SerializeField] private GameObject particulesPrefab;
        [SerializeField] private Vector2 sizeParticulesSpawn;
        [SerializeField] private float rangeWhereParticulesDoNotSpawn;
        [SerializeField] private float particulesSpeed;
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;
        [SerializeField] private float delayBetweenEachParticuleSpawn;
        [SerializeField] private State powerReleaseState;

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

#if UNITY_EDITOR
            if (Vector2.Distance(transform.position, (Vector2)transform.position + sizeParticulesSpawn / 2) < rangeWhereParticulesDoNotSpawn)
            {
                Debug.LogError("No particules will be able to spawn. Editor stopped. Modify the range where particules cannot spawn or the size of zone where particules can spawn.");
                
                if (EditorApplication.isPlaying)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                } 
            }
#endif
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
                Finish(powerReleaseState);
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
            animator.SetTrigger(R.S.AnimatorParameter.PlasmaConcentration);

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
                particules[i].GetComponent<PlasmaController>().Speed = particulesSpeed;

                yield return new WaitForSeconds(delayBetweenEachParticuleSpawn);
            }

            allParticulesSpawned = true;
        }

        private Vector3 GetParticulePosition()
        {
            Vector3 position;
            do
            {
                float particulePositionX = Random.Range(-sizeParticulesSpawn.x / 2, sizeParticulesSpawn.x / 2) + transform.position.x;
                float particulePositionY = Random.Range(-sizeParticulesSpawn.y / 2, sizeParticulesSpawn.y / 2) + transform.position.y;

                position = new Vector3(particulePositionX, particulePositionY, 0);

            } while (Vector2.Distance(position, transform.position) < rangeWhereParticulesDoNotSpawn);

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

