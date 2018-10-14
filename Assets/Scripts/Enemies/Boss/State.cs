using UnityEngine;

namespace Playmode.EnnemyRework.Boss
{
    public delegate void OnStateFinish(State state, State nextState);

    public abstract class State : MonoBehaviour
    {
        public event OnStateFinish OnStateFinish;

        public abstract void Act();
        public abstract bool CanEnter();

        public virtual void Enter()
        {
            Debug.Log(this);
        }

        public virtual void Finish(State nextState)
        {
            OnStateFinish?.Invoke(this, nextState);
        }
        public virtual void Finish()
        {
            OnStateFinish?.Invoke(this, null);
        }
    }
}


