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
                //BEN_CORRECTION : Classe de UI s'occupant de logique applicative. La gestion du "isGamePaused"
                //                 devrait au moins se faire dans "GameController", et non pas ici.
                //
                //                 Mieux encore, il aurait été plus pratique d'avoir un composant dont le seul but
                //                 est de gérer si le jeu est en pause où non. Je sais déjà ce que vous allez me
                //                 dire : « Oui, mais faire une classe juste pour une variable, c'est pas overkill ? ».
                //
                //                 En fait, non seulement cette classe doit savoir si le jeu est en pause ou non, mais
                //                 il doit aussi assigner le bon "TimeScale" et redémarrer le jeu si l'on passe du jeu
                //                 au "MainMenu" et vice versa.
                //
                //                 Bref, je considère la responsabilité assez complexe pour qu'elle ait sa propre classe.
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


