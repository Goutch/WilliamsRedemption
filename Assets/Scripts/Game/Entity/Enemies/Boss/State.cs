using Game.Entity.Player;
using UnityEngine;

namespace Game.Entity.Enemies.Boss
{
    public delegate void OnStateFinish(State state);

    public abstract class State : MonoBehaviour
    {
        protected PlayerController player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag(Values.Tags.Player).GetComponent<PlayerController>();
            Init();
        }

        public event OnStateFinish OnStateFinish;
        public abstract State GetCurrentState();

        protected abstract void Init();

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