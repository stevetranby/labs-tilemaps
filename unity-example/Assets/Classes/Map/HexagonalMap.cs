using UnityEngine;
using System.Collections;

namespace ST
{
    public class HexagonalMap : Map
    {
        public override bool IsTileValid (TileCoord tile)
        {
            return false;
        }
    
        public override bool IsTileValid (int x, int y, int z)
        {
            return false;
        }
    
        public override byte Tile (int x, int y, int z)
        {
            return 0;
        }
    
        public override byte TileHeight (int x, int y)
        {
            return 0;
        }
    
        public override byte TileSeen (int x, int y)
        {
            return 0;
        }
    
        // TODO: move collisions into tile data
        public override bool TileIsCollision (TileCoord tile)
        {
            return false;
        }
    
        public override void SetTileColor (TileCoord tile, Color c)
        {
            return;
        }
    
        // TODO: decide if rename to TileZDepth, TileVertexZ, or other makes more sense
        public override float TileScreenDepth (TileCoord tile)
        {
            return 0f;
        }
    
        public override float TileYOffsetForHeight (TileCoord tile)
        {
            return 0f;
        }

        
        public override TileCoord tileFromWorld (Vector3 world)
        {
            return new TileCoord(0,0,0);
        }
        
        public override Vector3 worldFromTile (TileCoord tile)
        {
            return new Vector3(0,0,0);
        }
    }
}
