using UnityEngine;

namespace Game.Entity.Enemies
{
    public class BatTrigger:MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Values.Tags.Player))
            {
                GetComponentInParent<Bat>().OnTriggered();
            }
        }
    }
}