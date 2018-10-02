using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlasmaGroundController : MonoBehaviour {
    [SerializeField] private GameObject explosion;
    [SerializeField] private int yOffSetSpawnPlateform;
    [SerializeField] private Tile spawnTile;
    [SerializeField] private float scaleSpeed;
    private Tilemap plateforms;
    private Vector2 explosionSize;
    private Rigidbody2D rb;
    private new Collider2D collider;
    private float scale = 1;
    private float size;
    private bool grounded = false;
    private bool hitWall = false;
    private float maxWidth = 5;
    private float rayCastHitWallPenetration = 0.32f;
    private LightController lightController;
    private float delayBeforeDestructionOfPlatforms;
    private float directionX;


    private void Awake()
    {
        plateforms = GameObject.FindGameObjectWithTag("Plateforme").GetComponent<Tilemap>();
        size = GetComponent<SpriteRenderer>().size.x;
        explosionSize = GetComponent<SpriteRenderer>().size;
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y);
        lightController = GameObject.FindGameObjectWithTag("LightManager").GetComponent<LightController>();
    }

    public void Init(float delayBeforeDestructionOfPlatforms)
    {
        this.delayBeforeDestructionOfPlatforms = delayBeforeDestructionOfPlatforms;
        directionX = Mathf.Sign(transform.position.x - PlayerController.instance.transform.position.x);
    }

    private void Update()
    {
        if (grounded && !hitWall)
        {
            goForward();
        }

        RaycastHit2D hit = Physics2D.Linecast(transform.localPosition - directionX * new Vector3(scale * (size / 2) - rayCastHitWallPenetration, -0.02f * directionX), transform.localPosition - directionX * new Vector3(scale * (size / 2), -0.02f * directionX), 1 << LayerMask.NameToLayer("TransparentFX") | 1 << LayerMask.NameToLayer("Default"));
        Debug.DrawLine(transform.localPosition - directionX * new Vector3(scale * (size / 2) - rayCastHitWallPenetration, -0.02f * directionX), transform.localPosition - directionX * new Vector3(scale * (size / 2), -0.02f * directionX), Color.blue);
        if (hit.collider != null)
        {
            OnWallCollision();
        }
    }

    private void goForward()
    {
        if(transform.localScale.x * size < maxWidth)
        {
            transform.localScale = new Vector3(transform.localScale.x + (scaleSpeed * Time.deltaTime), transform.localScale.y);
            scale += scaleSpeed * Time.deltaTime;
        }

        rb.MovePosition(new Vector3(transform.localPosition.x - directionX * scaleSpeed * Time.deltaTime * (size / 2), transform.localPosition.y));
        Debug.DrawLine(transform.localPosition, transform.localPosition - new Vector3(0, 1), Color.green);
        Debug.DrawLine(transform.localPosition - directionX * new Vector3(scale * (size / 2), 0), transform.localPosition - directionX * new Vector3(scale * (size / 2), 1), Color.red);
    }

    private void OnWallCollision()
    {
        hitWall = true;
        Instantiate(explosion, transform.localPosition - directionX * new Vector3(scale * (size / 2) - explosionSize.x/2, - directionX * explosionSize.y/2), Quaternion.identity);

        Vector3Int cellPos = plateforms.LocalToCell(transform.localPosition - directionX * new Vector3(scale * (size / 2) - explosionSize.x / 2, - directionX * explosionSize.y / 2));
        cellPos += new Vector3Int(0, yOffSetSpawnPlateform, 0);

        List<Vector3Int> platformsPosition = new List<Vector3Int>();

        if (plateforms.GetTile(cellPos) == null)
        {
            platformsPosition.Add(cellPos);
        }

        if (plateforms.GetTile(cellPos + new Vector3Int(1, 0, 0)) == null)
        {
            platformsPosition.Add(cellPos + new Vector3Int(1, 0, 0));
        }

        if (plateforms.GetTile(cellPos + new Vector3Int(-1, 0, 0)) == null)
        {
            platformsPosition.Add(cellPos + new Vector3Int(-1, 0, 0));
        }

        foreach (Vector3Int postionCell in platformsPosition)
            plateforms.SetTile(postionCell, spawnTile);

        lightController.UpdateLightAtEndOfFrame();

        GameObject plateformsDestroyer = new GameObject();
        TimedPlateformeDestroyer timedPlatformsDestroyer = plateformsDestroyer.AddComponent<TimedPlateformeDestroyer>();
        timedPlatformsDestroyer.Init(platformsPosition, plateforms, delayBeforeDestructionOfPlatforms, lightController);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!grounded && collision.tag.Contains("Plateforme"))
        {
            grounded = true;
            rb.constraints = rb.constraints | RigidbodyConstraints2D.FreezePositionY;
            if(PlayerController.instance.CurrentController.Collider.IsTouching(collider))
            {
                OnWallCollision();
            }
        }
    }
}
