using UnityEngine;

namespace Game.Entity.Enemies.Boss
{
    public delegate void OnStateFinish(State state);

    public abstract class State : MonoBehaviour
    {
        public event OnStateFinish OnStateFinish;
        public abstract State GetCurrentState();


        public abstract void Act();
        public abstract bool CanEnter();

        public virtual void Enter()
        {
            Debug.Log(this);
        }

        public virtual void Finish()
        {
            Debug.Log("Finish: " + this);
            OnStateFinish?.Invoke(this);
        }
    }
}


