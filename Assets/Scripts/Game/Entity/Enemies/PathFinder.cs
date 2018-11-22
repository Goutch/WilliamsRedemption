using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Entity.Enemies
{
    public class PathFinder
    {
        public Tilemap Obstacles { get; set; }

        public PathFinder(Tilemap obstacles)
        {
            Obstacles = obstacles;
        }

        public bool[,] GetSurrounding(int range, Vector2 myPosition)
        {
            bool[,] surrounding = new bool[range * 2 + 1, range * 2 + 1];
            Vector3Int StartPos = Obstacles.WorldToCell(myPosition);

            for (int y = -range; y <= range; y++)
            {
                for (int x = -range; x <= range; x++)
                {
                    int cellPosX = StartPos.x + x;
                    int cellPosY = StartPos.y + y;
                    surrounding[x + range, y + range] = Obstacles.GetTile(new Vector3Int(cellPosX, cellPosY, 0));
                }
            }

            return surrounding;
        }


    }
}