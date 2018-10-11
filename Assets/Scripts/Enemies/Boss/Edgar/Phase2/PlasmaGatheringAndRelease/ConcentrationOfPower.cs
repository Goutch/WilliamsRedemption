using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss;
using Playmode.EnnemyRework;
using UnityEditor;
using Harmony;

namespace Edgar
{
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
        [SerializeField] private State PowerReleaseState;

        private float distanceEqualitySensibility = 1f;
        private float lastTimeUsed;
        private GameObject[] particules;
        private bool allParticulesSpawned;


        private new void Awake()
        {
            base.Awake();
            if (capacityUsableAtStart)
                lastTimeUsed = -cooldown;

#if UNITY_EDITOR
            if (Vector2.Distance(transform.position, (Vector2)transform.position + sizeParticulesSpawn / 2) < rangeWhereParticulesDoNotSpawn)
            {
                Debug.LogError("No particules will be able to spawn. Editor stopped. Modify the range where particules cannot spawn or the size of zone where particlues can spawn.");
                
                if (EditorApplication.isPlaying)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                } 
            }
#endif
        }

        public override void Act()
        {
            float directionX = Mathf.Sign(PlayerController.instance.transform.position.x - transform.position.x);
            if (directionX > 0)
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            else
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);

            int numberOfParticulesLeft = 0;

            for(int i = 0; i < particules.Length; ++i)
            {
                if(particules[i] != null)
                {
                    if (Vector2.Distance(particules[i].transform.position, transform.position) < distanceEqualitySensibility)
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
                Finish(PowerReleaseState);
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }

        protected override void Initialise()
        {
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
                float particulePositionX;
                float particulePositionY;
                Vector3 position;

                do
                {
                    particulePositionX = Random.Range(-sizeParticulesSpawn.x / 2, sizeParticulesSpawn.x / 2) + transform.position.x;
                    particulePositionY = Random.Range(-sizeParticulesSpawn.y / 2, sizeParticulesSpawn.y / 2) + transform.position.y;
                    position = new Vector3(particulePositionX, particulePositionY, 0);
                } while (Vector2.Distance(position, transform.position) < rangeWhereParticulesDoNotSpawn);

                particules[i] = Instantiate(particulesPrefab, position, BossDirection(position));
                particules[i].GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Enemy);
                particules[i].GetComponent<PlasmaController>().Speed = particulesSpeed;

                yield return new WaitForSeconds(delayBetweenEachParticuleSpawn);
            }

            allParticulesSpawned = true;
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

