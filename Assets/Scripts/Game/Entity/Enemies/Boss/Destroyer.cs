using UnityEngine;

namespace Game.Entity.Enemies.Boss
{
    class Destroyer : MonoBehaviour
    {
        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}