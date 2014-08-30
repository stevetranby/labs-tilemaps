using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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


		public void Startup() 		
		{
			// Neighbors for Hex Map in Staggered Format (left to right, top to bottom, offset odd rows)
			List<TileCoord> neighborOffsets = new List<TileCoord> ();
			neighborOffsets.Add(new TileCoord(-1,-1));
			neighborOffsets.Add(new TileCoord(-1,0));
			neighborOffsets.Add(new TileCoord(-1,1));
			neighborOffsets.Add(new TileCoord(0,-1));
			neighborOffsets.Add(new TileCoord(0,1));
			neighborOffsets.Add(new TileCoord(1,0));
		}


		// ------------------------------------------------------

		struct MapTile
		{
			// tile coord
			// neighbors
			// fowState (active, inactive, hidden)
			// objects
			//
		}

		public void SetupDefaultMap()
		{		
			// TODO: get from map base class
			int rows = 10;
			int cols = 10;
			for (int r = 0; r < rows; r++) 
			{
//				this.tiles[r] = [];

				for (var c = 0; c < cols; c++) 
				{
					// Old Map Tile
//					var tile = new MapTile();
//					tile.coord = cc.p(r, c);
//					if (r + c > 10 && r + c < 15)
//						tile.revealed = true;
//					this.tiles[r][c] = tile;
				}
			}
			this.RefreshMap();
		}
		                
		public void RefreshMap()
		{
			Color color = new Color32(255,255,0,255);
			const float cos30 = 0.866f;
			const float sin30 = 0.500f;

			// Remove all layer's children
				
			// GameObject "parent" to store onto grid
			GameObject parent = null;

			int rows = 10;
			int cols = 10;
				for (var r = 0; r < rows; r++) 
			{
					for (var c = 0; c < cols; c++) 
				{
					float x = c * (2 * this.tileMapSize.x) + (r % 2) * this.tileMapSize.x;
					float y = r * this.tileMapSize.y;

					//console.log(r,c,x,y);

					// TODO: refactor OLD CODE to use new map format
//					var tile = this.tiles[r][c];
//					if (tile) {
//						if (tile.base) {
//							var tileNode = cc.Sprite.create(tile.base);
//							tileNode.setPosition(cc.p(x, y));
//							this.layerBase.addChild(tileNode);
//						}
//						
//						var fogAsset = tile.revealed ? null : s_unseen;
//						if (fogAsset) {
//							var tileFog = cc.Sprite.create(fogAsset);
//							tileFog.setPosition(cc.p(x, y));
//							this.layerFog.addChild(tileFog);
//						}
//						
//						var tr = this.tileRadius;
//						// Hex Points around "anchorPoint"
//						var pointsTop = [
//						                 cc.p(x - tr * cos30, y + tr * 0.5),
//						                 cc.p(x, y + tr),
//						                 cc.p(x + tr * cos30, y + tr * 0.5),
//						                 cc.p(x + tr * cos30, y - tr * 0.5),
//						                 cc.p(x, y - tr),
//						                 cc.p(x - tr * cos30, y - tr * 0.5),
//						                 ];
//						this.gridNode.drawPoly(pointsTop, cc.BLUE, 1, color);
//						}
//					}
				}				
			}
		}
    }
}