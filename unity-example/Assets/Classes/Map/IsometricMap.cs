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

		public byte TileCost (TileCoord tile)
		{
			const byte MAX_TILE_COST = 255;
			return MAX_TILE_COST;
		}

        // TODO: move collisions into tile data
		public override bool TileIsCollision (TileCoord tile)
        {
            return true;
        }
        
        // TODO: decide if rename to TileZDepth, TileVertexZ, or other makes more sense
        public override float TileScreenDepth (TileCoord tile)
        {
            return 0f;
        }
        
        public override float TileYOffsetForHeight(TileCoord tile)
        {
            return 0f;
        }

        public override TileCoord tileFromWorld (Vector3 world)
        {
//			var ret = cc.PointZero();
//				
//				pos = cc.POINT_PIXELS_TO_POINTS( pos );
//				
//				var tw = mapLayer.getMapTileSize().width;
//				var th = mapLayer.getMapTileSize().height;
//				var mw = mapLayer.getLayerSize().width;
//				var mh = mapLayer.getLayerSize().height;
//				
//				var x = pos.x;
//				var y = pos.y;
//				
//				var isox = Math.floor(mh - y/th + x/tw - mw/2);// - 1/2),
//				var isoy = Math.floor(mh - y/th - x/tw + mw/2 + 1/2); // - 3/2)
//				
//				ret = cc.p(isox, isoy);
			
            return new TileCoord(0,0,0);
        }
        
        public override Vector3 worldFromTile (TileCoord tile)
        {
//            var zOrder = (r + c) >> 0;
//            var x = tw * this.cols / 2 + ((c - r) * tw / 2);
//            var y = th * this.rows - ((c + r) * th / 2);
//            var z = tileInfo.height;

            return new Vector3(0,0,0);
        }

        public void SetupDefaultMap()
        {
            // NOTE: "chunks" or the mesh could be for each row of the map
//            var tileRowNode = this.isoLayer.getChildByTag(100 + zOrder);
//            if (!tileRowNode) {
//                tileRowNode = cc.DrawNode.create();
//                tileRowNode.setPosition(cc.p(0, y));
//                this.isoLayer.addChild(tileRowNode, zOrder, 100 + zOrder);
//            }
        }

        public void RotateMapCW() 
        {
        }

        public void RotateMapCCW() 
        {
        }
    }
}