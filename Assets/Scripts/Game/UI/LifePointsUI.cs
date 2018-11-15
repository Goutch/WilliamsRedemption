
using Game.Entity;
using Game.Entity.Player;
using UnityEngine;

namespace Game.UI
{
    public class LifePointsUI : MonoBehaviour
    {
        [SerializeField] private GameObject lifePointPrefab;
        public static LifePointsUI instance;
        private GameObject[] lifePointsImages;
        private PlayerController playerController;
        private Health playerHealth;

        private void Start()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(this.gameObject);
            }
            GameObject objectS = GameObject.FindGameObjectWithTag(Values.Tags.Player);
            playerController = GameObject.FindGameObjectWithTag(Values.Tags.Player).GetComponent<PlayerController>();
            playerHealth = playerController.GetComponent<Health>();
            playerHealth.OnHealthChange += OnHealthChange;
            lifePointsImages = new GameObject[playerHealth.MaxHealth];
            for (int i = 0; i < lifePointsImages.Length; i++)
            {
                lifePointsImages[i] = Instantiate(lifePointPrefab, transform.position, Quaternion.identity, this.transform);
            }

        }

        public void OnHealthChange(GameObject gameObject)
        {
            if (playerHealth.HealthPoints >= 0)
            {
                lifePointsImages[playerHealth.HealthPoints].SetActive(false);
            }

        }

    }
}

