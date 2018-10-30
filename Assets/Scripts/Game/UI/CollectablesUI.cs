using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class CollectablesUI:MonoBehaviour
    {
        [SerializeField] private Text numberText;
        private int numberOfCollectable=0;
        public void AddCollectable()
        {
            numberOfCollectable++;
            numberText.text=numberOfCollectable.ToString();
        }
    }
}