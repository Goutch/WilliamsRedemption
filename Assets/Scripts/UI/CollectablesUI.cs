using UnityEngine;

namespace Game.UI
{
    public class CollectablesUI:MonoBehaviour
    {
        [SerializeField] private GameObject collectableUIElementPrefab;
        [SerializeField] private Transform gridLayout;
        private void AddCollectable()
        {
            Instantiate(collectableUIElementPrefab,gridLayout.transform);
        }
    }
}