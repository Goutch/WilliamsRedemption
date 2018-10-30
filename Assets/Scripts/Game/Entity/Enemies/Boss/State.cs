using UnityEngine;

namespace Game.Entity.Enemies.Boss
{
    public delegate void OnStateFinish(State state);

    public abstract class State : MonoBehaviour
    {
        public event OnStateFinish OnStateFinish;

        public abstract void Act();
        public abstract bool CanEnter();

        public virtual void Enter()
        {
            Debug.Log(this);
        }

        public virtual void Finish()
        {
            OnStateFinish?.Invoke(this);
            Debug.Log("Finish: " + this);
        }
    }
}


