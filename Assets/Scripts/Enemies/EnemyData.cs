using UnityEditorInternal;
using UnityEngine;

namespace DefaultNamespace.Playmode
{
    
    public abstract class EnemyData : MonoBehaviour 
    {
        [SerializeField] protected float speed;

        public float Speed => speed;
    }
}