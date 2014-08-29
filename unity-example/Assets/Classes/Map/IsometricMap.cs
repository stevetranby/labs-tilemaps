using UnityEngine;
using System.Collections;

namespace ST
{
    public class IsometricMap : Map
    {
        // MARK - Tile Methods (should refactor into Tile.cs)
        
        public override bool IsTileValid (TileCoord tile)
        {
            return IsTileValid (tile.c, tile.r, tile.h);
        }
        
        public override bool IsTileValid (int x, int y, int z)
        {
            return (x < mapX && x >= 0 && y < mapY && y >= 0 && z < mapZ && z >= 0);
        }
        
        public override byte Tile (int x, int y, int z)
        {
            if (IsTileValid (x, y, z)) {
                return tileIds [x, y, z];
            }
            return 0;
        }
        
        public override byte TileHeight (int x, int y)
        {
            if (IsTileValid (x, y, 0)) {
                return tileHeights [x, y];
            }
            return (byte)0;
        }
        
        public override byte TileSeen (int x, int y)
        {
            if (IsTileValid (x, y, 0)) {
                return tileSeen [x, y];
            }
            return 0;
        }

        public override void SetTileColor (TileCoord tile, Color c)
        {
        }

        // TODO: move collisions into tile data
        public override bool TileIsCollision (TileCoord tile)
        {
            if (IsTileValid (tile)) {
                // cache these for easier reference
                int cols = tileIds.GetLength (0);
                int rows = tileIds.GetLength (1);
                int layerArea = cols * rows;
                int tileMapIndex = tile.h * (layerArea) + tile.r * rows + tile.c;
                bool isWall = tileColliders [tileMapIndex];
                return isWall;
            }
            return true;
        }
        
        // TODO: decide if rename to TileZDepth, TileVertexZ, or other makes more sense
        public override float TileScreenDepth (TileCoord tile)
        {
            // TODO: implement in concreate class
            // REMOVE: isometric staggered for now
            // real z is based on ordering tiles should form from top to bottom, right to left
            float z = 50f + tile.r + tile.c / this.mapX - (tile.h) * 0.005f;
            z *= 0.15f; // decrease/increase range of z values (careful about precision)
            
            // special case offsets depending on map layer (tz)
            if (tile.h > 6)
            {
                z -= (tile.h - 6) * 0.05f;
            }
            else
            {
                z += (7 - tile.h) * 0.02f;
            }    
            return z;
        }
        
        public override float TileYOffsetForHeight(TileCoord tile)
        {
            // TODO: implement in concreate class
            // REMOVE: isometric staggered for now
            // rows are positioned at half-height grid since they overlap in vertical axis 
            byte tileHeight = TileHeight(tile.c, tile.r);
            int ty = tile.r % chunkSize;
            float y = (float)ty * this.tUnitOffsets.y * 0.5f;
            
            if (this.relativeLayerHeight)
            {
                y += ((float)tileHeight * tileHeightStep * this.tUnitOffsets.y);           
                // special case offsets depending on map layer (tz)
                if (tile.h > 6)
                {
                    y += (tile.h - 6) * this.tUnitOffsets.y * 0.01f;
                }
                else
                {
                    y -= ((6 - tile.h) * this.tUnitOffsets.y * 0.2f + this.tUnitOffsets.y * 0.5f);
                }
            }
            else
            {
                if (tile.h > 6)
                {
                    // at tileheight
                    y += ((float)tileHeight * tileHeightStep * this.tUnitOffsets.y);
                    y += (tile.h - 6) * this.tUnitOffsets.y * 0.01f;
                }
                else
                {
                    int segHeight = Mathf.Min(tile.h, tileHeight);         
                    y += segHeight * this.tUnitOffsets.y * 0.5f;
                }
            }
            
            // TODO: this is a hack, should either use a base tile with ground as top tz=6 tile
            //       or otherwise make this generic to any tileset with 1 tile high base tiles and 
            //       no height ground base floor tiles
            // TODO: possibly have different tilesets for ground/floor and base under-ground/floor structure tiles 
            if (tile.h <= 6)
            {
                if (this.relativeLayerHeight)
                {
                    y -= this.tUnitOffsets.y * 0.55f;
                }
                else
                {
                    y -= this.tUnitOffsets.y * 0.75f;
                }
                
            }
            return y;
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