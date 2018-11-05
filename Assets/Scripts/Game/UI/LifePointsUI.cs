
using Game.Entity;
using Game.Entity.Player;
using UnityEngine;

namespace Game.UI
{
    public class LifePointsUI : MonoBehaviour
    {
        //BEN_REVIEW : Espacement vectical à revoir. Vous savez c'est quoi un retour de chariot n'est-ce-pas ?
        [SerializeField] private GameObject lifePointPrefab;
        public static LifePointsUI instance;
        private GameObject[] lifePointsImages;
        private PlayerController playerController; //BEN_CORRECION : Attribut inutile. Pourrait être une variable dans la fonction "Start".
        private Health playerHealth;
        private void Start()
        {
            //BEN_CORRECTION : Propreté du code.
            if (instance == null)
                instance = this;
            else
            {
                //BEN_REVIEW : Au moins mettre un "Debug.LogWarning" dans le cas où cela arrive.
                //             Parce que, c'est pas sensé arriver...
                Destroy(this.gameObject);
            }

            playerController = PlayerController.instance;
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
            //BEN_CORRECTION : Et que va t-il se passer si votre personnage passe de 4 points de vie à 2 d'un seul coup ?
            //
            //                 Prenez le temps d'y réfléchir...
            if (playerHealth.HealthPoints >= 0)
            {
                lifePointsImages[playerHealth.HealthPoints].SetActive(false);
            }

        }

    }
}

