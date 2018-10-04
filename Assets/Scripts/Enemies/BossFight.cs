using UnityEngine;
    public class BossFight: MonoBehaviour
    {
        [SerializeField] private GameObject boss;

        [SerializeField] private DoorScript doorToCloseOnBossFightBegin;
        [SerializeField] private DoorScript doorToOpenOnBossDeath;

        private void Awake()
        {
            boss.SetActive(false);
            boss.GetComponent<Health>().OnDeath += OpendWinDoor;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            boss.SetActive(true);
            doorToCloseOnBossFightBegin?.Close();
            doorToOpenOnBossDeath?.Close();
            Destroy(GetComponent<BoxCollider2D>());
        }

        private void OpendWinDoor()
        {
            doorToOpenOnBossDeath?.Open();
        }
    }
