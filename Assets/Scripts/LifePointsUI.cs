using UnityEngine;
using UnityEngine.UI;


public class LifePointsUI : MonoBehaviour
{
    [SerializeField] private GameObject lifePointPrefab;
    public static LifePointsUI instance;
    private GameObject[] lifePointsImages;
    private PlayerController playerController;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }
        playerController = transform.root.GetComponent<PlayerController>();
        lifePointsImages = new GameObject[playerController.NbPlayerLives];
        for (int i = 0; i <lifePointsImages.Length; i++)
        {
            lifePointsImages[i] = Instantiate(lifePointPrefab, transform.position, Quaternion.identity,this.transform);
        }

    }

    public void UpdateLP()
    {
        lifePointsImages[playerController.NbPlayerLivesLeft].SetActive(false);
    }

}