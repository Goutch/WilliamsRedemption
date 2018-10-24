using Harmony;
using UnityEngine;

public class BossFight: MonoBehaviour
{
    [SerializeField] private GameObject boss;

    [SerializeField] private DoorScript doorToCloseOnBossFightBegin;
    [SerializeField] private DoorScript doorToOpenOnBossDeath;

    private Collider2D bossArea;
    private CameraController cameraController;

    private void Awake()
    {
        boss.SetActive(false);
        boss.GetComponent<Health>().OnDeath += OnBossDead;
        bossArea = GetComponent<Collider2D>();

        cameraController = GameObject.FindGameObjectWithTag(Values.Tags.MainCamera).GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        boss.SetActive(true);
        doorToCloseOnBossFightBegin?.Close();
        doorToOpenOnBossDeath?.Close();

        cameraController.FixePoint(bossArea.bounds.center, bossArea.bounds.size.x / 3);

        Destroy(GetComponent<BoxCollider2D>());
    }

    private void OnBossDead(GameObject gameObject)
    {
        doorToOpenOnBossDeath?.Open();
        cameraController.ResumeFollow();
    }
}
