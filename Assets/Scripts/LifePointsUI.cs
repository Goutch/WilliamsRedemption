using UnityEngine;
using UnityEngine.UI;


public class LifePointsUI : MonoBehaviour
{
    [SerializeField] private GameObject lifePointPrefab;
    public static LifePointsUI instance;
    private GameObject[] lifePointsImages;
    private PlayerController playerController;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }

        playerController = PlayerController.instance;
        lifePointsImages = new GameObject[playerController.NbPlayerLives];
        for (int i = 0; i <lifePointsImages.Length; i++)
        {
            lifePointsImages[i] = Instantiate(lifePointPrefab, transform.position, Quaternion.identity,this.transform);
        }

    }

    public void UpdateLP()
    {
        if (playerController.NbPlayerLivesLeft >= 0)
        {
            lifePointsImages[playerController.NbPlayerLivesLeft].SetActive(false);
        }
        
    }

}