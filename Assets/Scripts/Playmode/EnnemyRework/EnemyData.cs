using UnityEditorInternal;
using UnityEngine;

namespace DefaultNamespace.Playmode
{
    
    public class EnemyData : ScriptableObject
    {
        [SerializeField] protected Sprite sprite;
        [SerializeField] protected Animator ennemyAnimator;
        [SerializeField] protected int healthPoint;
        [SerializeField] protected float speed;
        
        public Sprite Sprite => sprite;

        public Animator EnnemyAnimator => ennemyAnimator;

        public int HealthPoint => healthPoint;

        public float Speed => speed;
    }
}