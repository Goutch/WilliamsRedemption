using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Harmony
{
    /// <summary>
    /// Un "World" est un lot de scènes. Utilisez <see cref="WorldLoader"/> pour charger et décharger
    /// des "Worlds".
    /// </summary>
    /// <remarks>
    /// La scène principale (Main scene) est toujours chargée et ne peut faire partie d'un World.
    /// </remarks>
    [CreateAssetMenu(fileName = "New World", menuName = "Game/Scene Management/World")]
    public class World : ScriptableObject
    {
        [SerializeField] private R.E.Scene[] scenes = { };
        [SerializeField] private R.E.Scene activeScene = R.E.Scene.None;
        [SerializeField] private bool isDeveloppement = false;
#if UNITY_EDITOR
        [SerializeField] private R.E.Scene mainScene = R.E.Scene.None;
#endif

        /// <summary>
        /// Se déclanche après que ce "World" aie été chargé complètement.
        /// </summary>
        /// <remarks>
        /// Vous risquez une fuite de mémoire si vous ne vous désabonnez pas de cet événement lors de la destruction
        /// de l'abonné.
        /// </remarks>
        public event EventHandler OnWorldLoaded;

        /// <summary>
        /// Se déclanche avant que ce "World" soi déchargé.
        /// </summary>
        /// <remarks>
        /// Vous risquez une fuite de mémoire si vous ne vous désabonnez pas de cet événement lors de la destruction
        /// de l'abonné.
        /// </remarks>
        public event EventHandler OnWorldUnloading;

        /// <summary>
        /// Scènes du "World". Toutes les scènes du world sont chargées en même temps.
        /// </summary>
        public IEnumerable<R.E.Scene> Scenes => scenes;

        /// <summary>
        /// Scène du "World" du est active à son lancement. La Scène active est celle où les nouveaux
        /// GameObjects sont créés par défaut.
        /// </summary>
        public R.E.Scene ActiveScene => activeScene;

        /// <summary>
        /// Indique si ce "World" n'est utilisé qu'à des fins de développement. Si c'est le cas,
        /// ce "World" ne sera pas inclus dans le "build" final.
        /// </summary>
        public bool IsDeveloppement => isDeveloppement;
#if UNITY_EDITOR

        /// <summary>
        /// Scène à considérer comme "MainScene" lors du lancement de ce "World" dans l'éditeur.
        /// </summary>
        public R.E.Scene MainScene => mainScene;
#endif

        /// <summary>
        /// Indique si ce "World" est chargé ou non.
        /// </summary>
        public bool IsLoaded { get; set; } = false;

        /// <summary>
        /// Retourne la routine de chargement de ce "World".
        /// </summary>
        /// <returns>Routine de chargement du "World".</returns>
        public IEnumerator GetLoadRoutine()
        {
            yield return GetLoadRoutineInternal();

            SetActiveScene();

            NotifyWorldLoaded();
        }

        private IEnumerator GetLoadRoutineInternal()
        {
            foreach (var scene in scenes)
            {
                var sceneName = R.S.Scene.ToString(scene);
                if (!SceneManagerExtensions.IsSceneLoaded(sceneName))
                {
                    yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                }
                else
                {
                    Debug.LogWarning("Problem while loading World \"" + name + "\" : scene named \"" + sceneName +
                                     "\" is already loaded.");
                }
            }
        }

        private void SetActiveScene()
        {
            if (activeScene != R.E.Scene.None)
            {
                var sceneName = R.S.Scene.ToString(activeScene);
                var sceneToMakeActive = SceneManager.GetSceneByName(sceneName);
                if (sceneToMakeActive.isLoaded)
                {
                    SceneManager.SetActiveScene(sceneToMakeActive);
                }
                else
                {
                    Debug.LogWarning("Problem while making the scene named \"" + sceneName +
                                     "\" the active scene : it is not loaded. It does not seems to belongs to the " +
                                     "World named \"" + name + "\".");
                }
            }
        }

        /// <summary>
        /// Retourne la routine de déchargement de ce "World".
        /// </summary>
        /// <returns>Routine de déchargement du "World".</returns>
        public IEnumerator GetUnloadRoutine()
        {
            NotifyWorldUnloading();

            yield return GetUnloadRoutineInternal();
        }

        private IEnumerator GetUnloadRoutineInternal()
        {
            foreach (var scene in scenes)
            {
                var sceneName = R.S.Scene.ToString(scene);
                if (SceneManagerExtensions.IsSceneLoaded(sceneName))
                {
                    yield return SceneManager.UnloadSceneAsync(sceneName);
                }
                else
                {
                    Debug.LogWarning("Problem while unloading World \"" + name + "\" : scene named \"" + sceneName +
                                     "\" is not loaded.");
                }
            }
        }

#if UNITY_EDITOR

        /// <summary>
        /// NE JAMAIS UTILISER EN DEHORS DE L'ÉDITEUR!!!
        ///
        /// Permet d'indiquer au "World" qu'il est déjà chargé.
        /// </summary>
        public void SetPreloadedWorld()
        {
            SetActiveScene();
        }

#endif

        private void NotifyWorldLoaded()
        {
            IsLoaded = true;

            if (OnWorldLoaded != null) OnWorldLoaded();
        }

        private void NotifyWorldUnloading()
        {
            IsLoaded = false;

            if (OnWorldUnloading != null) OnWorldUnloading();
        }
    }
}