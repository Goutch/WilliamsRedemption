using UnityEngine;

namespace Game.UI
{
    public class PauseUI : MonoBehaviour
    {

        private bool isGamePaused = false;

        public void OnPressKeyPause()
        {
            if (isGamePaused)
            {
                isGamePaused = false;
                Time.timeScale = 1.0f;
                GameObject.Find(Values.GameObject.PanelPause).SetActive(false);
            }
            else
            {
                isGamePaused = true;
                Time.timeScale = 0f;
                GameObject.Find(Values.GameObject.PanelPause).SetActive(true);
            }
        }
    }
}


