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

        [SerializeField] private AudioClip levelMusic;

        [SerializeField] private int numberSoundTrackPerlevel;
        /*[SerializeField] private AudioClip levelMusic1;
        [SerializeField] private AudioClip levelMusic2;*/


        public int NumberCollectables => numberCollectables;

        public int ExpectedTime => _expectedTime;

        public string Scene => sceneName;

        public Level NextLevel => nextLevel;

        public AudioClip LevelMusic => levelMusic;

        public int NumberSoundTrackPerlevel => numberSoundTrackPerlevel;

        /*public AudioClip LevelMusic1 => levelMusic1;
        
        public AudioClip LevelMusic2 => levelMusic2;*/
    }
}