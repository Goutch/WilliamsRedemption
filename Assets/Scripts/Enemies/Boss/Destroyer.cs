using UnityEngine;

namespace Boss
{
    class Destroyer : MonoBehaviour
    {
        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
