using UnityEngine;
using System.Collections;

namespace ST
{
	public class OGAMap
	{
		public Texture2D tileset;
		public float tileWidth = 32f;
		public float tileHeight = 16f;
		public float textureTileHeight = 16f;
		public int cols = 24;
		public int rows = 24;
		public float tileHeightStep = 2.0f;
		public int maxTileSteps = 8;
		public bool showTileGround = true;

		// TODO: refactor to tile class and private with accessors
		public GameObject[,] tilesFloor;
		public GameObject[,] tilesGround;
		public GameObject[,] tilesWall;
		public int[,] heightMap;

		public OGAMap ()
		{
		}

		public void initFloor (Texture2D tex, int width, int height, bool showGround)
		{
			this.tileset = tex;
			this.tileWidth = width;
			this.tileHeight = height;
			this.textureTileHeight = height;
			this.showTileGround = showGround;

			// hack, should specify as part of tileset config
			if (width == height)			
				this.tileHeight = height / 2;	
		
		}

		// TODO: revert to same way tile floors are loaded/accessed
		//        
		// NOTE: note that the manner in which you can get the sprites from the 
		//       texture/sprite names in the unity editor is probably not best
		//       when you have them in a specific ordered grid spritesheet
		public void initWalls (Texture2D atlas, int width, int height)
		{
			// Try to load the actual sprites from the texture (named in unity editor e.g. "[spritename]_08"
			// TODO: issues with Unity 4.3.4+
			// http://answers.unity3d.com/questions/586313/load-unity-43-sprites-with-assetbundles.html
			// - looks like below is only pro now with assetbundles
			// http://docs.unity3d.com/Documentation/ScriptReference/BuildPipeline.BuildAssetBundle.html
			// - but then again, probably wasn't the way to go in the first place
			// 
			// Actually it looks like AssetDatabase is editor only, silly people on "Answers"
			// http://answers.unity3d.com/questions/576153/loading-a-sprite-unity-43-in-resource-folder-and-s.html
			//

			
//			string atlasPath;
//			Object[] atlasAssets;
//			
//				atlasPath = AssetDatabase.GetAssetPath(atlas);
//				atlasAssets = AssetDatabase.LoadAllAssetsAtPath(atlasPath);
//				
//				foreach(Object asset in atlasAssets)
//				{
//					if(AssetDatabase.IsSubAsset(asset))
//					{
//						//asset is a sprite.....
//						//do something
//						//add it to a list maybe
//					}
//				}



//			// Another Attempt
//			Sprite[] textures = Resources.LoadAll<Sprite>("Textures");
//			string[] names = new string[textures.Length];
//			
//			for(int ii=0; ii< names.Length; ii++) {
//				names[ii] = textures[ii].name;
//			}
//
//			// here texture name would be "werebear_white_bare_0"
//			Sprite sprite = textures[Array.IndexOf(names, "textureName")];
		}

		public Sprite GetSpriteByName (string spriteName, Sprite[] spriteArray)
		{
			for (int i = 0; i < spriteArray.Length; i++) {
				if (spriteArray [i].name.Equals (spriteName)) {
					return spriteArray [i];
				}
			}
			
			return null;
		}

		public void generateMap (GameObject layer)
		{
			// make sure to generate a random seed, but SAVE the seed for any replay or regeneration
			Random.seed = 234234;

			tilesFloor = new GameObject[cols, rows];
			tilesGround = new GameObject[cols, rows];
			tilesWall = new GameObject[cols, rows];
			heightMap = new int[cols, rows];

			for (int r = 0; r < rows; r++) {
				for (int c = 0; c < cols; c++) {
					int tileIndex = Random.Range (0, 4);

					GameObject tile = new GameObject ();
					var sr = tile.AddComponent<SpriteRenderer> ();
					sr.sprite = spriteCreateForTileIndex (tileIndex);
					tile.transform.parent = layer.transform;

					int h = Random.Range (0, 4);
					heightMap [c, r] = h;

					TileCoord coord = new TileCoord (c, r);
					var xy = posForTile (coord);
					var z = zOrderForTile (coord);
					tile.transform.position = new Vector3 (xy.x, xy.y, z);

					//Debug.Log ("xy = " + xy + ", z = " + z + ", coord = " + coord);
					tilesFloor [c, r] = tile;

					refreshTile (coord);
				}
			}
		}

		public Sprite spriteCreateForTileIndex (int index)
		{
			Vector2 pivot = new Vector2 (0.5f, 0.5f);
			Rect rect = rectForTileIndex (index);

			return Sprite.Create (tileset, rect, pivot, 1f);
		}

		// map tile coord to rect in texture
		// row - starts 0 at top increasing down (like in Tiled)
		public Rect rectForTile (TileCoord coord)
		{	
			return new Rect (coord.c * tileWidth, tileset.height - (coord.r * textureTileHeight), tileWidth, textureTileHeight);
		}

		public Rect rectForTileIndex (int index)
		{
			int colPerRow = (int)(tileset.width / tileWidth);
			int row = index / colPerRow;
			int col = index - row * colPerRow;
			return rectForTile (new TileCoord (col, row + 1));
		}

		// base position of tile (doesn't include height)
		// 0,0 => halfcols, fullrows
		public Vector3 posForTile (TileCoord coord)
		{
//			float mapHeight = (rows + cols) * tileHeight/2f;
//			float x = (coord.c - coord.r) * tileWidth / 2f;
//			float y = mapHeight - ((coord.r + coord.c) * tileHeight / 2f);

			var mh = rows;
			var mw = cols;
			var th = tileHeight;
			var tw = tileWidth;
			float x = tw / 2f * (mw + coord.c - coord.r);
			float y = th / 2f * ((mh * 2f - coord.c - coord.r) - 1f);
		
			return new Vector3 (x, y, 0); 
		}
	
		// calculating the tile coordinates from world location
		public TileCoord tileForPos (Vector2 pos)
		{
//			float mapHeight = (rows + cols) * tileHeight/2f;
//			float inverseY = mapHeight - pos.y;
//			int tileX = (int)(pos.x / tileWidth / 2f + inverseY / tileHeight / 2f);
//			int tileY = (int)(inverseY / tileHeight / 2f - (pos.x / tileWidth / 2f));

			var x = pos.x;
			var y = pos.y;
			var mh = rows;
			var mw = cols;
			var th = tileHeight;
			var tw = tileWidth;
			var isox = Mathf.FloorToInt (mh - y / th + x / tw - mw / 2);// - 1/2),
			var isoy = Mathf.FloorToInt (mh - y / th - x / tw + mw / 2 + 1 / 2); // - 3/2)
			return new TileCoord (isox, isoy, 0);
		}

		public bool validTileCoord (TileCoord coord)
		{
			if (coord.c < 0 || coord.c > cols - 1 || coord.r < 0 || coord.r > rows - 1)
				return false;
			return true;
		}

		public void changeTileIndex (TileCoord coord, int tileIndex)
		{
			if (! validTileCoord (coord))
				return;

			GameObject tile = tilesFloor [coord.c, coord.r];

			if (tile) {
				var sr = tile.GetComponent<SpriteRenderer> ();
				//sr.sprite.textureRect = rectForTileIndex(tileIndex);
				sr.sprite = spriteCreateForTileIndex (tileIndex);
			} else {
				Debug.LogError ("Couldn't find tile for coord " + coord.c + ", " + coord.r);
			}
		}

		public void changeTileColor (TileCoord coord, Color color)
		{
			if (! validTileCoord (coord))
				return;
			
			GameObject tile = tilesFloor [coord.c, coord.r];
			
			if (tile) {
				var sr = tile.GetComponent<SpriteRenderer> ();
				sr.material.color = color;
			}

		}
	
		// z order in Unity is + into the screen 
		public float zOrderForTile (TileCoord coord)
		{
			if (! validTileCoord (coord)) 
				return 0;
		                 
			// greater y = greater z (top center to lower left should decrease z)
			// greater tile x = greater z (top center to lower right)
			return -(coord.r + coord.c);
		}

		public float heightForTile (TileCoord coord)
		{
			if (! validTileCoord (coord))
				return 0;

			return (float)(heightMap [coord.c, coord.r]) * tileHeightStep;
		}

		public void raiseTile (TileCoord coord)
		{
			if (! validTileCoord (coord))
				return;
			int h = heightMap [coord.c, coord.r];
			h++;
			heightMap [coord.c, coord.r] = h > maxTileSteps ? maxTileSteps : h;
			refreshTile (coord);
			Debug.Log ("raising tile @ " + coord + " to height = " + heightMap [coord.c, coord.r]);
		}

		public void lowerTile (TileCoord coord)
		{
			if (! validTileCoord (coord))
				return;
			int h = heightMap [coord.c, coord.r];
			h--;
			heightMap [coord.c, coord.r] = h < 0 ? 0 : h;
			refreshTile (coord);
			
			Debug.Log ("lowering tile @ " + coord + " to height = " + heightMap [coord.c, coord.r]);
		}

		public void refreshTile (TileCoord coord)
		{
			if (! validTileCoord (coord))
				return;

			if (heightForTile (coord) >= 0) {

				var tile = tilesFloor [coord.c, coord.r];
				var xy = posForTile (coord);
				var z = zOrderForTile (coord);
				var yoff = heightForTile (coord);
				tile.transform.localPosition = new Vector3 (xy.x, xy.y + yoff, z);

				// TODO: need to figure out how to anchor the child to (0,1f)
				// ground underneath
				if (showTileGround) {
					// find child
					GameObject child = tilesGround [coord.c, coord.r];
					if (! child) {
						child = GameObject.CreatePrimitive (PrimitiveType.Quad);
						child.renderer.material.color = new Color (0.35f, 0.35f, 0.25f);
						child.transform.parent = tile.transform;
						tilesGround [coord.c, coord.r] = child;
					}
					float groundHeight = heightForTile (coord);
					child.transform.localPosition = new Vector3 (0, -groundHeight, 0);
					child.transform.localScale = new Vector3 (tileWidth * 0.95f, groundHeight * 2f + tileHeight * 0.15f, 1);
				}
			}
		}


	}
}