using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Goutch2D.Tiles
{
    [CreateAssetMenu(fileName = "NewSmartTile", menuName = "Tiles/SmartTile")]
    public class SmartTile : TileBase
    {
        [SerializeField] private string spriteSheetPath="SmartTiles/";

        private Sprite[] sprites;
        //Sprite indexes

        //   0

        // 1 2 3
        // 4 5 6
        // 7 8 9

        //semiStrips
        //   10 
        //11    12
        //   13 

        //strips
        //vertical and horizontal
        //14 15

        private void OnEnable()
        {
            if (sprites == null||!sprites.Any())
            {
                GetSpritesFromTexture();
            }
        }

        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            if (sprites == null || !sprites.Any())
            {
                GetSpritesFromTexture();
            }

            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    var neighborPositon = new Vector3Int(position.x + i, position.y + j, position.z);
                    var neighborTile = tilemap.GetTile(neighborPositon);
                    if (HasSameTileAssetThan(neighborTile))
                    {
                        //Please note : Here, we do not call "neighborTile.RefreshTile", but
                        //              "tilemap.RefreshTile". Tilemap holds a list of tiles to be refreshed
                        //              by calling "GetTileData".
                        tilemap.RefreshTile(neighborPositon);
                    }
                }
            }
        }

        /// <summary>
        /// Determines what the Tile looks like on the Tilemap (the sprite used).
        /// </summary>
        /// <param name="position">Position of the tile.</param>
        /// <param name="tilemap">Which Tilemap it belongs to.</param>
        /// <param name="tileData">Output. What the Tile must</param>
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            if (!sprites.Any() || sprites == null)
            {
                GetSpritesFromTexture();
            }

            //Basic settings
            tileData.transform = Matrix4x4.identity;
            tileData.color = Color.white;
            tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
            tileData.colliderType = Tile.ColliderType.Sprite;

            //Sprite setting
            var leftNeighbor = tilemap.GetTile(new Vector3Int(position.x - 1, position.y, position.z));
            var rightNeighbor = tilemap.GetTile(new Vector3Int(position.x + 1, position.y, position.z));
            var topNeighbor = tilemap.GetTile(new Vector3Int(position.x, position.y + 1, position.z));
            var bottomNeighbor = tilemap.GetTile(new Vector3Int(position.x, position.y - 1, position.z));

            //Conditions are ordered by "most likely to happen first" for performance purposes.
            //Is center ?
            //   o
            // o t o 
            //   o 
            if (HasSameTileAssetThan(topNeighbor) &&
                HasSameTileAssetThan(leftNeighbor) &&
                HasSameTileAssetThan(rightNeighbor) &&
                HasSameTileAssetThan(bottomNeighbor))
            {
                tileData.sprite = sprites[5];
            }
            //Has no neighbors ?
            //   x
            // x t x 
            //   x 
            else if (HasNotSameTileAssetThan(topNeighbor) &&
                     HasNotSameTileAssetThan(leftNeighbor) &&
                     HasNotSameTileAssetThan(rightNeighbor) &&
                     HasNotSameTileAssetThan(bottomNeighbor))
            {
                tileData.sprite = sprites[0];
            }
            //Is top left corner ?
            //   x 
            // x t o 
            //   o 
            else if (HasNotSameTileAssetThan(topNeighbor) &&
                     HasNotSameTileAssetThan(leftNeighbor) &&
                     HasSameTileAssetThan(rightNeighbor) &&
                     HasSameTileAssetThan(bottomNeighbor))
            {
                tileData.sprite = sprites[1];
            }
            //Is top right corner ?
            //   x 
            // o t x 
            //   o 

            else if (HasNotSameTileAssetThan(topNeighbor) &&
                     HasNotSameTileAssetThan(rightNeighbor) &&
                     HasSameTileAssetThan(leftNeighbor) &&
                     HasSameTileAssetThan(bottomNeighbor))
            {
                tileData.sprite = sprites[3];
            }
            //Is bottom left corner ?
            //   o
            // x t o 
            //   x 
            else if (HasNotSameTileAssetThan(leftNeighbor) &&
                     HasNotSameTileAssetThan(bottomNeighbor) &&
                     HasSameTileAssetThan(rightNeighbor) &&
                     HasSameTileAssetThan(topNeighbor))
            {
                tileData.sprite = sprites[7];
            }
            //Is bottom right corner ?
            //   o
            // o t x 
            //   x 
            else if (HasNotSameTileAssetThan(rightNeighbor) &&
                     HasNotSameTileAssetThan(bottomNeighbor) &&
                     HasSameTileAssetThan(leftNeighbor) &&
                     HasSameTileAssetThan(topNeighbor))
            {
                tileData.sprite = sprites[9];
            }
            //Is top ?
            //   x
            // o t o 
            //   o 
            else if (HasNotSameTileAssetThan(topNeighbor) &&
                     HasSameTileAssetThan(leftNeighbor) &&
                     HasSameTileAssetThan(rightNeighbor) &&
                     HasSameTileAssetThan(bottomNeighbor))
            {
                tileData.sprite = sprites[2];
            }
            //Is bottom ?
            //   o
            // o t o 
            //   x 
            else if (HasNotSameTileAssetThan(bottomNeighbor) &&
                     HasSameTileAssetThan(leftNeighbor) &&
                     HasSameTileAssetThan(rightNeighbor) &&
                     HasSameTileAssetThan(topNeighbor))
            {
                tileData.sprite = sprites[8];
            }
            //Is left ?
            //   o
            // o t x 
            //   o 
            else if (HasNotSameTileAssetThan(leftNeighbor) &&
                     HasSameTileAssetThan(topNeighbor) &&
                     HasSameTileAssetThan(rightNeighbor) &&
                     HasSameTileAssetThan(bottomNeighbor))
            {
                tileData.sprite = sprites[4];
            }
            //Is right ?
            //   o
            // x t o 
            //   o 
            else if (HasNotSameTileAssetThan(rightNeighbor) &&
                     HasSameTileAssetThan(topNeighbor) &&
                     HasSameTileAssetThan(leftNeighbor) &&
                     HasSameTileAssetThan(bottomNeighbor))
            {
                tileData.sprite = sprites[6];
            }
            //Has only neighbors on top ?
            //   0
            // x t x 
            //   x 
            else if (HasSameTileAssetThan(topNeighbor) &&
                     HasNotSameTileAssetThan(leftNeighbor) &&
                     HasNotSameTileAssetThan(rightNeighbor) &&
                     HasNotSameTileAssetThan(bottomNeighbor))
            {
                tileData.sprite = sprites[13];
            }
            //Has only neighbors on bottom ?
            //   x
            // x t x 
            //   o 
            else if (HasSameTileAssetThan(bottomNeighbor) &&
                     HasNotSameTileAssetThan(leftNeighbor) &&
                     HasNotSameTileAssetThan(rightNeighbor) &&
                     HasNotSameTileAssetThan(topNeighbor))
            {
                tileData.sprite = sprites[10];
            }

            //Has only neighbors on right ?
            //   x
            // x t o 
            //   x 
            else if (HasSameTileAssetThan(rightNeighbor) &&
                     HasNotSameTileAssetThan(topNeighbor) &&
                     HasNotSameTileAssetThan(leftNeighbor) &&
                     HasNotSameTileAssetThan(bottomNeighbor))
            {
                tileData.sprite = sprites[11];
            }
            //Has only neighbors on left ?
            //   x
            // o t x 
            //   x 
            else if (HasSameTileAssetThan(leftNeighbor) &&
                     HasNotSameTileAssetThan(bottomNeighbor) &&
                     HasNotSameTileAssetThan(rightNeighbor) &&
                     HasNotSameTileAssetThan(topNeighbor))
            {
                tileData.sprite = sprites[12];
            }
            //Has only neighbors on top and bottom ?
            //   0
            // x t x 
            //   0 
            else if (HasSameTileAssetThan(topNeighbor) &&
                     HasSameTileAssetThan(bottomNeighbor) &&
                     HasNotSameTileAssetThan(leftNeighbor) &&
                     HasNotSameTileAssetThan(rightNeighbor))
            {
                tileData.sprite = sprites[14];
            }
            //Has only neighbors on right and left ?
            //   x
            // o t o 
            //   x 
            else if (HasNotSameTileAssetThan(topNeighbor) &&
                     HasNotSameTileAssetThan(bottomNeighbor) &&
                     HasSameTileAssetThan(leftNeighbor) &&
                     HasSameTileAssetThan(rightNeighbor))
            {
                tileData.sprite = sprites[15];
            }
            //default
            else
            {
                tileData.sprite = sprites[0];
            }
        }

        private bool HasSameTileAssetThan(TileBase otherTile)
        {
            //There's only one instance of each Tile asset. Thus, if it's the same reference,
            //it's the same tile asset.
            return otherTile != null && this == otherTile;
        }

        private bool HasNotSameTileAssetThan(TileBase otherTile)
        {
            //There's only one instance of each Tile asset. Thus, if it's the same reference,
            //it's the same tile asset.
            return otherTile == null || this != otherTile;
        }

        private void GetSpritesFromTexture()
        {
            sprites = Resources.LoadAll<Sprite>(spriteSheetPath)
                .OfType<Sprite>().ToArray();
        }
    }
}