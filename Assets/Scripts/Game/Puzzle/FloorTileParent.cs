using Game.Entity.Player;
using Harmony;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Puzzle
{
    public class FloorTileParent : IFloorTile
    {
        private IFloorTile[] initialChild;

        private void Awake()
        {
            initialChild = new IFloorTile[transform.childCount];

            for (int j = 0; j < transform.childCount; ++j)
            {
                initialChild[j] = transform.GetChild(j).GetComponent<IFloorTile>();
            }
        }

        public override void MoveUp()
        {
            foreach (IFloorTile floorTile in initialChild)
            {
                floorTile.MoveUp();
            }
        }

        public override void MoveDown()
        {
            foreach (IFloorTile floorTile in initialChild)
            {
                floorTile.MoveDown();
            }
        }
    }
}