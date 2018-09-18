using System.Collections;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Chargeur de "Worlds".
    /// </summary>
    [AddComponentMenu("Game/SceneManagement/WorldLoader")]
    public class WorldLoader : MonoBehaviour
    {
        private bool isLoadingWorld;
        private bool isUnloadingWorld;

        /// <summary>
        /// Se déclanche lors que "WorldLoader" débute le chargement un "World".
        /// </summary>
        /// <remarks>
        /// Vous risquez une fuite de mémoire si vous ne vous désabonnez pas de cet événement lors de la destruction
        /// de l'abonné.
        /// </remarks>
        public event EventHandler OnWorldLoadingStarted;

        /// <summary>
        /// Se déclanche lors que "WorldLoader" a terminé de charger un "World".
        /// </summary>
        /// <remarks>
        /// Vous risquez une fuite de mémoire si vous ne vous désabonnez pas de cet événement lors de la destruction
        /// de l'abonné.
        /// </remarks>
        public event EventHandler OnWorldLoadingEnded;

        /// <summary>
        /// Se déclanche lors que "WorldLoader" débute le déchargement d'un "World".
        /// </summary>
        /// <remarks>
        /// Vous risquez une fuite de mémoire si vous ne vous désabonnez pas de cet événement lors de la destruction
        /// de l'abonné.
        /// </remarks>
        public event EventHandler OnWorldUnloadingStarted;

        /// <summary>
        /// Se déclanche lors que "WorldLoader" a terminé de décharger un "World".
        /// </summary>
        /// <remarks>
        /// Vous risquez une fuite de mémoire si vous ne vous désabonnez pas de cet événement lors de la destruction
        /// de l'abonné.
        /// </remarks>
        public event EventHandler OnWorldUnloadingEnded;

        /// <summary>
        /// Démarre le chargement asynchrone d'un World.
        /// </summary>
        /// <param name="world">World à charger.</param>
        /// <seealso cref="OnWorldLoadingStarted"/>
        /// <seealso cref="OnWorldLoadingEnded"/>
        public void LoadWorld(World world)
        {
            StartCoroutine(GetLoadWorldRoutine(world));
        }

        /// <summary>
        /// Démarre de déchargement asynchrone d'un World.
        /// </summary>
        /// <param name="world">World à décharger.</param>
        /// <seealso cref="OnWorldUnloadingStarted"/>
        /// <seealso cref="OnWorldUnloadingEnded"/>
        public void UnloadWorld(World world)
        {
            StartCoroutine(GetUnloadWorldRoutine(world));
        }

        /// <summary>
        /// Démarre de rechargement asynchrone d'un World.
        /// </summary>
        /// <param name="world">World à recharger.</param>
        /// <seealso cref="OnWorldLoadingStarted"/>
        /// <seealso cref="OnWorldLoadingEnded"/>
        /// <seealso cref="OnWorldUnloadingStarted"/>
        /// <seealso cref="OnWorldUnloadingEnded"/>
        public void ReloadWorld(World world)
        {
            StartCoroutine(GetReloadWorldRoutine(world));
        }

        /// <summary>
        /// Indique si un "World" est en cours de chargement.
        /// </summary>
        /// <returns>Vrai si un "World" est en cours de chargement, faux sinon.</returns>
        public bool IsLoadingWorld()
        {
            return isLoadingWorld;
        }

        /// <summary>
        /// Indique si un "World" est en cours de déchargement.
        /// </summary>
        /// <returns>Vrai si un "World" est en cours de déchargement, faux sinon.</returns>
        public bool IsUnloadingWorld()
        {
            return isUnloadingWorld;
        }

        /// <summary>
        /// Indique si un "World" est en cours de chargement ou de déchargement.
        /// </summary>
        /// <returns>Vrai si un "World" est en cours de chargement ou de déchargement, faux sinon.</returns>
        public bool IsLoadingOrUnloadingWorld()
        {
            return IsLoadingWorld() || IsUnloadingWorld();
        }

#if UNITY_EDITOR

        /// <summary>
        /// NE JAMAIS UTILISER EN DEHORS DE L'ÉDITEUR!!!
        ///
        /// Permet d'indiquer au "WorldLoader" qu'un "World" est déjà chargé.
        /// </summary>
        /// <param name="world">World à considérer comme chargé.</param>
        public void SetPreloadedWorld(World world)
        {
            world.SetPreloadedWorld();
        }

#endif

        private IEnumerator GetLoadWorldRoutine(World world)
        {
            NotifyWorldLoadStart();

            yield return world.GetLoadRoutine();

            NotifyWorldLoadEnd();
        }

        private IEnumerator GetUnloadWorldRoutine(World world)
        {
            NotifyWorldUnloadStart();

            yield return world.GetUnloadRoutine();

            NotifyWorldUnloadEnd();
        }

        private IEnumerator GetReloadWorldRoutine(World world)
        {
            yield return GetUnloadWorldRoutine(world);

            yield return GetLoadWorldRoutine(world);
        }

        private void NotifyWorldLoadStart()
        {
            isLoadingWorld = true;

            if (OnWorldLoadingStarted != null) OnWorldLoadingStarted();
        }

        private void NotifyWorldLoadEnd()
        {
            isLoadingWorld = false;

            if (OnWorldLoadingEnded != null) OnWorldLoadingEnded();
        }

        private void NotifyWorldUnloadStart()
        {
            isUnloadingWorld = true;

            if (OnWorldUnloadingStarted != null) OnWorldUnloadingStarted();
        }

        private void NotifyWorldUnloadEnd()
        {
            isUnloadingWorld = false;

            if (OnWorldUnloadingEnded != null) OnWorldUnloadingEnded();
        }
    }
}