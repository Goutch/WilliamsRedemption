using UnityEditorInternal;
using UnityEngine;

namespace DefaultNamespace.Playmode
{
    
    public abstract class EnemyData : MonoBehaviour , IEntityData
    {
        [SerializeField] protected int healthPoint;
        [SerializeField] protected float speed;

        public int HealthPoint => healthPoint;

        public float Speed => speed;
    }
}