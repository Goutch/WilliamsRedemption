using Game.Controller;
using Game.Entity;
using Game.Entity.Player;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class LifePointsUI : MonoBehaviour
    {
        [SerializeField] private GameObject lifePointPrefab;
        [SerializeField] private GameObject lifePointsParent;

        private GameObject[] lifePointsImages;
        private PlayerController playerController;
        private Health playerHealth;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += InitLifePoints;
        }

        public void InitLifePoints(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != Values.Scenes.Menu && scene.name != Values.Scenes.Main)
            {
                if (lifePointsImages != null)
                    foreach (var image in lifePointsImages)
                    {
                        Destroy(image);
                    }

                playerController = GameObject.FindGameObjectWithTag(Values.Tags.Player)
                    .GetComponent<PlayerController>();
                playerHealth = playerController.GetComponent<Health>();
                playerHealth.OnHealthChange += OnHealthChange;
                lifePointsImages = new GameObject[playerHealth.MaxHealth];
                for (int i = 0; i < lifePointsImages.Length; i++)
                {
                    lifePointsImages[i] =
                        Instantiate(lifePointPrefab, transform.position, Quaternion.identity,
                            lifePointsParent.transform);
                }
            }
        }

        public void UpdateHealth()
        {
            if (playerHealth.HealthPoints >= 0)
            {
                for (int i = 0; i < playerHealth.MaxHealth; i++)
                {
                    if (i < playerHealth.HealthPoints)
                    {
                        lifePointsImages[i].SetActive(true);
                    }
                    else
                    {
                        lifePointsImages[i].SetActive(false);
                    }
                }
            }
        }

        public void OnHealthChange(GameObject receiver, GameObject attacker)
        {
            UpdateHealth();
        }
    }
}