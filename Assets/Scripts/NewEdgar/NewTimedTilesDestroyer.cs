using Harmony;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Edgar
{
    class NewTimedTilesDestroyer : MonoBehaviour
    {
        [SerializeField] private List<Vector3Int> cellToDestroy;
        [SerializeField] private float delayBeforeDestruction;
        private Tilemap platforms;
        private LightController lightController;

        public void Init(List<Vector3Int> cellToDestroy, float delayBeforeDestruction)
        {
            lightController = GameObject.FindGameObjectWithTag(R.S.Tag.LightManager).GetComponent<LightController>();
            platforms = GameObject.FindGameObjectWithTag(R.S.Tag.Plateforme).GetComponent<Tilemap>();

            this.cellToDestroy = cellToDestroy;
            this.delayBeforeDestruction = delayBeforeDestruction;
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
                platforms.SetTile(cellPos, null);
            }

            lightController.UpdateLightAtEndOfFrame();

            Destroy(gameObject);
        }
    }
}


