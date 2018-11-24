using UnityEditor;
using UnityEngine;

namespace Game.Controller
{
    [CreateAssetMenu]
    public class Level : ScriptableObject
    {
        [SerializeField] private int numberCollectables;
        [SerializeField] private int _expectedTime;
        [SerializeField] private SceneAsset scene;
        [SerializeField] private Level nextLevel;


        public int NumberCollectables => numberCollectables;

        public int ExpectedTime => _expectedTime;

        public SceneAsset Scene => scene;

        public Level NextLevel => nextLevel;
    }
}