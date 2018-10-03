using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

class TimedPlateformeDestroyer : MonoBehaviour 
{
    [SerializeField] private List<Vector3Int> cellToDestroy;
    [SerializeField] private float delayBeforeDestruction;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private LightController lightController;

    public void Init(List<Vector3Int> cellToDestroy, Tilemap tilemap, float delayBeforeDestruction, LightController lightController)
    {
        this.cellToDestroy = cellToDestroy;
        this.tilemap = tilemap;
        this.delayBeforeDestruction = delayBeforeDestruction;
        this.lightController = lightController;
    }

    private void Start()
    {
        StartCoroutine(DestroyTiles());
    }

    IEnumerator DestroyTiles()
    {
        yield return new WaitForSeconds(delayBeforeDestruction);

        foreach (Vector3Int cellPos in cellToDestroy)
        {
            tilemap.SetTile(cellPos, null);
        }

        lightController.UpdateLightAtEndOfFrame();

        Destroy(gameObject);
    }
}

