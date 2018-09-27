using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    public class PathFinder:MonoBehaviour
    {
        [SerializeField] private Tilemap obstacles;
        public static PathFinder instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(this.gameObject);
            }
        }
        public bool[,] GetSurrounding(int range,Vector2 myPosition)
        {
            bool[,] surrounding=new bool[range*2+1,range*2+1];
            Vector3Int StartPos=obstacles.WorldToCell(myPosition);
            for (int x = range; x <= +range; x++)
            {
                for (int y = -range; y <= +range; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        surrounding[x, y] = true;
                        continue;
                    }
                    
                    int cellPosX = StartPos.x + x;
                    int cellPosY = StartPos.y + y;
                    surrounding[x+range,y+range]=obstacles.GetTile(new Vector3Int(cellPosX,cellPosY,0));
                }
            }

            return surrounding;
        }

        
    }
}