using UnityEngine;
using System.Collections;

namespace ST
{
/// <summary>
/// A Tileset is composed of a texture and information to parse tiles from it.
/// There can be more than one tileset instance that references the same texture 
/// if necessary for multiple different tile sizes.
/// TODO: map/chunk rendering will need to handle this, it might already be taken 
/// care of with valid
/// </summary>
    public class Tileset
    {

        private Map map;
        private string filename;
        private Texture texture;
        private Vector2 texSize;
    
        // tile texture size in uv [0,1)
        private Vector2 uvTileSize;
        // unity units tile size 
        public Vector2 tUnitSize;

        // texture size of tile in pixels (varies depending on tileset, should import from config)
        public Vector2 tileTexSize = new Vector2 (32, 16);

        // TODO: change name of tileCounts
        // how many "map tiles" the tile takes up (1x1, 2x2, 3x2)
        private Vector2 tileCounts;

        // num tiles arranged in single row of texture
        private int tileCols;
        static bool first = true;

        public Tileset (Map map, string filename, Vector2 tilePixelSize, Vector2 tileCounts)
        {
            this.map = map;
            this.filename = filename;
            this.tileTexSize = tilePixelSize;       

//      // load texture from outside resource path? probably use HTTP/URL load with file:/// path
//        // http://docs.unity3d.com/ScriptReference/Application-dataPath.html
//        var pathSeparator = System.IO.Path.DirectorySeparatorChar;
//        var tilesetDir = Application.dataPath + pathSeparator + ".." + pathSeparator + "Resources";
//        var filepath = tilesetDir + pathSeparator + this.filename;      
//        Debug.Log("filepath = " + filepath);
                    
            this.texture = UnityEngine.Resources.Load<Texture2D> ("terrain_1");            
//            Debug.Log("texture = " + texture);

            SetupTexture ();
        }

        public Tileset (Map map, Texture texture, Vector2 tilePixelSize, Vector2 tileCounts)
        {
            this.map = map;
            this.texture = texture;
            this.tileTexSize = tilePixelSize;
            this.tileCols = map.tileIds.GetLength (0);
            SetupTexture ();
        }

        public void SetupTexture ()
        {
            this.texSize = new Vector2 (texture.width, texture.height);
            this.uvTileSize = new Vector2 (tileTexSize.x / texSize.x, tileTexSize.y / texSize.y);
            this.tUnitSize = new Vector2 (tileTexSize.x * map.tUnit, tileTexSize.y * map.tUnit);
            this.tileCols = Mathf.FloorToInt (texSize.x / tileTexSize.x);
//            if (first)
//            {
//                first = false;
//                Debug.Log("tileTexSize = " + tileTexSize.x + ", " + tileTexSize.y);
//                Debug.Log("tileCols = " + tileCols);
//
//                Debug.Log("texSize = " + texSize.x + ", " + texSize.y);
//                Debug.Log("uvTileSize = " + uvTileSize.x + ", " + uvTileSize.y);
//                Debug.Log("tUnitSize = " + tUnitSize.x + ", " + tUnitSize.y);
//            }
        }

        public Texture getTexture ()
        {
            return this.texture;
        }

        /// <summary>
        /// Purely for example to use only selected tiles in generation
        /// </summary>
        /// <returns>The sprite rect from special identifier.</returns>
        /// <param name="tileId">Tile identifier.</param>
        public Rect getTileUVRectFromSpecialId (int tileId)
        {
            switch (tileId) {      
            case 2:
                return getTileUVRect (0, 2);
            case 3:
                return getTileUVRect (1, 4);
            case 4:
                return getTileUVRect (4, 4);
            case 5:
                return getTileUVRect (5, 4);
            }
            return getTileUVRect (4, 5);            
        }

        /// <summary>
        /// Gets the sprite rect based on the index where 0 is top left tile increasing to the right, then down
        /// </summary>
        /// <returns>The sprite rect.</returns>
        /// <param name="index">Index.</param>
        public Rect getTileRectUV (int index)
        {
            // get row/col from index
            int ty = index / tileCols;
            int tx = index - ty * tileCols;
            return getTileUVRect (tx, ty);
        }

        /// <summary>
        /// Gets the sprite rect.
        /// The rect is the tile at grid position from top left
        /// </summary>
        /// <returns>The sprite rect.</returns>
        /// <param name="tx">Column in texture grid of tiles</param>
        /// <param name="ty">Row in texture grid of tiles</param>
        public Rect getTileUVRect (int tx, int ty)
        {
            // TODO: cache yoff
            float yoff = 1f - uvTileSize.y;
            float x = tx * uvTileSize.x;
            float y = yoff - ty * uvTileSize.y; // add 1 to get lower left origin for the given tile
            float w = uvTileSize.x;
            float h = uvTileSize.y;
            return new Rect (x, y, w, h);
        }
    }
}