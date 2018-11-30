using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Puzzle;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Death
{
    //BEN_REVIEW : Aurait pu (dû ?) être dans son propre dossier avec tout ce qui est "FloorMachinChouette".
    //BEN_CORRECTION : Nommage pas clair. C'est un "ShiftingFloor", ça devrait donc s'appeller ainsi.
    class FloorManager : MonoBehaviour
    {
        [SerializeField] private IFloorTile[] floors;
        [SerializeField] private float delayBeforeMoveDown;

        private int nextFloorUp = 0;

        public void ShiftFloors()
        {
            floors[nextFloorUp].MoveUp();

            StartCoroutine(MoveDownExcept(nextFloorUp));

            nextFloorUp = ++nextFloorUp % floors.Length;
        }

        //BEN_CORRECTION : Cette fonction assume qu'il y a exactement 2 floors à bouger.
        //                 Conception pas assez souple pouvant causer des bogues.
        public IEnumerator MoveDownExcept(int index)
        {
            yield return new WaitForSeconds(delayBeforeMoveDown);
            for (int i = 0; i < floors.Length; ++i)
            {
                if (i != index)
                    floors[i].MoveDown();
            }
        }

        public void MoveAllFloorsUp()
        {
            for (int i = 0; i < floors.Length; i++)
            {
                floors[i].MoveUp();
            }
        }
    }
}