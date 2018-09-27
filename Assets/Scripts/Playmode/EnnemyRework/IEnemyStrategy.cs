using Harmony;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public interface IEnemyStrategy
    {
        void Init(GameObject enemyControllerObject);
        void Act();
    }
}
