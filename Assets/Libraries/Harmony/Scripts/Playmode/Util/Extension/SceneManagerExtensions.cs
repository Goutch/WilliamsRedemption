using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour le gestionaire de scènes.
    /// </summary>
    public static class SceneManagerExtensions
    {
        /// <summary>
        /// Indique si une scène donnée est chargée ou non.
        /// </summary>
        /// <param name="sceneName">Nom de la scène.</param>
        /// <returns>Vrai si la scène est chargée (ou en cours de chargement), faux sinon.</returns>
        public static bool IsSceneLoaded(string sceneName)
        {
            return SceneManager.GetSceneByName(sceneName).isLoaded;
        }

        /// <summary>
        /// Retourne la liste de toutes les scènes chargées.
        /// </summary>
        /// <returns>Liste des noms de toutes les scènes actuellement chargées ou en cours de chargement.</returns>
        public static IList<string> GetLoadedScenes()
        {
            var scenes = new List<string>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                scenes.Add(SceneManager.GetSceneAt(i).name);
            }

            return scenes;
        }
    }
}