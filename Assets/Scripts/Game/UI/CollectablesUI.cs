using Game.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class CollectablesUI:MonoBehaviour
    {
        [SerializeField] private Text numberText;
        private GameController gameController;
        private void Start()
        {
            GameObject.FindGameObjectWithTag(Values.Tags.GameController).GetComponent<GameController>();
        }

        public void AddCollectable()
        {
            numberText.text=gameController.ToString();
        }
    }
}