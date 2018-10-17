using UnityEngine;

namespace Enemies
{
    public class BatTrigger:MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                GetComponentInParent<Bat>().OnTriggered();
            }
        }
    }
}