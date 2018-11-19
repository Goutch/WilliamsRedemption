using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Puzzle;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Death
{
    class FloorManager : MonoBehaviour
    {
        [SerializeField] private FloorTile[] floors;

        private int nextFloorUp = 0;

        public void ShiftFloors()
        {
            floors[nextFloorUp].MoveUp();
            if(floors[nextFloorUp].IsAtInitialPosition)
            {
                FloorManager_onDestinationReached();
            }
            else
            {
                floors[nextFloorUp].onDestinationReached += FloorManager_onDestinationReached;
            }
        }

        private void FloorManager_onDestinationReached()
        {
            floors[nextFloorUp].onDestinationReached -= FloorManager_onDestinationReached;

            for (int i = 0; i < floors.Length; ++i)
            {
                if (i != nextFloorUp)
                    floors[i].MoveDown();
            }

            nextFloorUp = ++nextFloorUp % floors.Length;
        }

        public void MoveAllFloorsUp()
        {
            for(int i = 0; i < floors.Length; i++)
            {
                floors[i].MoveUpAllChild();
            }
        }
    }
}
