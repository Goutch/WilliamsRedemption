using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Entity.Enemies
{
    public class PathFinder
    {
        //BEN_REVIEW : C'était pas vraiment utile d'avoir une propriété ici. Cela aurait pu être un attribut
        //             private.
        public Tilemap[] Obstacles { get; set; }

        public PathFinder(Tilemap[] obstacles)
        {
            Obstacles = obstacles;
        }

        public bool[,] GetSurrounding(int range, Vector2 myPosition)
        {
            bool[,] surrounding = new bool[range * 2 + 1, range * 2 + 1];

            //BEN_REVIEW : Pourquoi pas une boucle "for each" ?
            for (int i = 0; i < Obstacles.Length; i++)
            {
                //BEN_CORRECTION : Nommage.
                Vector3Int StartPos = Obstacles[i].WorldToCell(myPosition);
                for (int y = -range; y <= range; y++)
                {
                    for (int x = -range; x <= range; x++)
                    {
                        int cellPosX = StartPos.x + x;
                        int cellPosY = StartPos.y + y;
                        surrounding[x + range, y + range] |=
                            Obstacles[i].GetTile(new Vector3Int(cellPosX, cellPosY, 0));
                    }
                }
            }


            return surrounding;
        }
    }
}