using UnityEngine;

namespace Game.Controller
{
    [CreateAssetMenu]
    public class Achievement:ScriptableObject
    {
        [SerializeField] private int scoreValue;
        [SerializeField] private AchievementType type;
        [SerializeField] private string requirement;
        [SerializeField] private string accomplishment;
        public int ScoreValue => scoreValue;

        public AchievementType Type => type;

        public string Requirement => requirement;
        public string Accomplishment => accomplishment;

        public enum AchievementType
        {
            Level1,
            Level2,
            Level3,
            AllGame,
        }
    }
}