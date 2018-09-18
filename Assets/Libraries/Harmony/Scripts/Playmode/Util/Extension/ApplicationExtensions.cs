using UnityEditor;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour l'application.
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Fourni le chemin vers le dossier des données de l'application. Il est uniquement possible de lire dans ce dossier.
        /// </summary>
        public static string ApplicationDataPath => Application.streamingAssetsPath;

        /// <summary>
        /// Fourni le chemin vers le dossier des sauvegardes de l'application. Il est possible d'écrire et de lire dans ce dossier.
        /// </summary>
        public static string PersistentDataPath => Application.persistentDataPath;

        /// <summary>
        /// Ferme immédiatement l'application. Si l'application est dans l'éditeur, arrête l'éditeur à la place.
        /// </summary>
        public static void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}