using UnityEditor;
using UnityEngine;

namespace Game.Controller
{
    [CreateAssetMenu]
    public class Level : ScriptableObject
    {
        [SerializeField] private int numberCollectables;
        [SerializeField] private int _expectedTime;
        [SerializeField] private string sceneName;
        [SerializeField] private Level nextLevel;

        [SerializeField] private AudioClip[] levelMusics;


        public int NumberCollectables => numberCollectables;

        public int ExpectedTime => _expectedTime;

        public string Scene => sceneName;

        public Level NextLevel => nextLevel;

        public AudioClip[] LevelMusics => levelMusics;
    }
}