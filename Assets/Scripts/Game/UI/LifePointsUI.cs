using Game.Controller;
using Game.Entity;
using Game.Entity.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class LifePointsUI : MonoBehaviour
    {
        [SerializeField] private GameObject lifePointPrefab;
        private GameObject[] lifePointsImages;
        private PlayerController playerController;
        private Health playerHealth;
        private GameController gameController;


        public void InitLifePoints()
        {
            if (lifePointsImages != null)
                foreach (var image in lifePointsImages)
                {
                    Destroy(image);
                }

            gameController = GameObject.FindGameObjectWithTag(Values.GameObject.GameController)
                .GetComponent<GameController>();
            playerController = PlayerController.instance;
            playerHealth = playerController.GetComponent<Health>();
            playerHealth.OnHealthChange += OnHealthChange;
            lifePointsImages = new GameObject[playerHealth.MaxHealth];
            for (int i = 0; i < lifePointsImages.Length; i++)
            {
                lifePointsImages[i] =
                    Instantiate(lifePointPrefab, transform.position, Quaternion.identity, this.transform);
            }
        }


        public void OnHealthChange(GameObject gameObject)
        {
            if (playerHealth.HealthPoints >= 0)
            {
                for (int i = 0; i < playerHealth.MaxHealth; i++)
                {
                    if (i <= playerHealth.HealthPoints)
                    {
                        lifePointsImages[playerHealth.HealthPoints].SetActive(false);
                    }
                }
            }
        }
    }
}