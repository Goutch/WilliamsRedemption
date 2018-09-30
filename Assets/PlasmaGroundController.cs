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
    private FacingSideLeftRight direction;
    private Rigidbody2D rb;
    private new Collider2D collider;
    private float scale = 1;
    private float size;
    private bool grounded = false;
    private bool hitWall = false;
    private float maxWidth = 5;
    private float rayCastHitWallPenetration = 0.32f;
    private LightController lightController;


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

    private void Update()
    {
        if (grounded && !hitWall)
        {
            goForward();
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.localPosition - new Vector3(scale * (size / 2) - rayCastHitWallPenetration, -0.02f), new Vector2(-1, 0), rayCastHitWallPenetration, 1 << LayerMask.NameToLayer("TransparentFX") | 1 << LayerMask.NameToLayer("Default"));
        Debug.DrawLine(transform.localPosition - new Vector3(scale * (size / 2) - rayCastHitWallPenetration, -0.02f), transform.localPosition - new Vector3(scale * (size / 2), -0.02f), Color.blue);
        if (hit.collider != null)
        {
            OnWallCollision();
            Debug.Log("Hit");
        }
    }

    private void goForward()
    {
        if(transform.localScale.x * size < maxWidth)
        {
            transform.localScale = new Vector3(transform.localScale.x + (scaleSpeed * Time.deltaTime), transform.localScale.y);
            scale += scaleSpeed * Time.deltaTime;
        }

        rb.MovePosition(new Vector3(transform.localPosition.x - scaleSpeed * Time.deltaTime * (size / 2), transform.localPosition.y));
        Debug.DrawLine(transform.localPosition, transform.localPosition - new Vector3(0, 1), Color.green);
        Debug.DrawLine(transform.localPosition - new Vector3(scale * (size / 2), 0), transform.localPosition - new Vector3(scale * (size / 2), 1), Color.red);
    }

    private void OnWallCollision()
    {
        hitWall = true;
        Instantiate(explosion, transform.localPosition - new Vector3(scale * (size / 2) - explosionSize.x/2, -explosionSize.y/2), Quaternion.identity);

        Vector3Int cellPos = plateforms.LocalToCell(transform.localPosition - new Vector3(scale * (size / 2) - explosionSize.x / 2, -explosionSize.y / 2));
        cellPos += new Vector3Int(0, yOffSetSpawnPlateform, 0);

        Vector3Int[] platformsPosition = new Vector3Int[] {
            cellPos,
            cellPos + new Vector3Int(1, 0, 0),
            cellPos + new Vector3Int(-1, 0, 0)
        };

        plateforms.SetTile(platformsPosition[0], spawnTile);
        plateforms.SetTile(platformsPosition[1], spawnTile);
        plateforms.SetTile(platformsPosition[2], spawnTile);

        lightController.UpdateLightAtEndOfFrame();

        GameObject plateformsDestroyer = new GameObject();
        TimedPlateformeDestroyer timedPlatformsDestroyer = plateformsDestroyer.AddComponent<TimedPlateformeDestroyer>();
        timedPlatformsDestroyer.Init(platformsPosition, plateforms, 1, lightController);

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
