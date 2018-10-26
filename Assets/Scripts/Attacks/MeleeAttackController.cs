
using UnityEngine;


class MeleeAttackController : MonoBehaviour
{
    [SerializeField] private float delayBeforeDestruction;


    private void Awake()
    {
        Destroy(this.gameObject,delayBeforeDestruction);
    }

}
