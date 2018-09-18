namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour le temps.
    /// </summary>
    public static class TimeExtensions
    {
        private static float previousTimeScale = 1;

        /// <summary>
        /// Redémarre l'écoulement du temps dans le jeu.
        /// </summary>
        public static void Resume()
        {
            UnityEngine.Time.timeScale = previousTimeScale;
        }

        /// <summary>
        /// Arrête l'écoulement du temps dans le jeu.
        /// </summary>
        public static void Pause()
        {
            previousTimeScale = UnityEngine.Time.timeScale;
            UnityEngine.Time.timeScale = 0;
        }

        /// <summary>
        /// Indique si le temps s'écoule ou non dans le jeu.
        /// </summary>
        /// <returns>Vrai le temps ne s'écoule, faux sinon.</returns>
        public static bool IsRunning()
        {
            return UnityEngine.Time.timeScale > 0;
        }

        /// <summary>
        /// Indique si le temps s'écoule ou non dans le jeu.
        /// </summary>
        /// <returns>Vrai le temps ne s'écoule pas, faux sinon.</returns>
        public static bool IsPaused()
        {
            return !IsRunning();
        }
    }
}