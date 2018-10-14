using UnityEngine;

namespace Playmode.EnnemyRework.Boss
{
    class Destroyer : MonoBehaviour
    {
        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
