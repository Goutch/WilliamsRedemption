using UnityEngine;
using UnityEngine.SceneManagement;

namespace Harmony
{
    /// <summary>
    /// Charge un World au lancement (Start) du script.
    /// </summary>
    [AddComponentMenu("Game/SceneManagement/LoadInitialWorld")]
    public class LoadInitialWorld : MonoBehaviour
    {
        [SerializeField] private World world;

        private WorldLoader worldLoader;

        private void Awake()
        {
            worldLoader = this.GetComponentInTaggedObjectChildrens<WorldLoader>(R.S.Tag.GlobalComponents);
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (SceneManager.sceneCount == 1)
            {
                worldLoader.LoadWorld(world);
            }
            else
            {
                worldLoader.SetPreloadedWorld(world);
            }
#else
            worldLoader.LoadWorld(world);
#endif
        }
    }
}