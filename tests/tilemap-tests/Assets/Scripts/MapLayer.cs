using UnityEngine;
using System;

namespace ST
{
    public class MapLayer
    {
        // parent
        private Map map;
        public string name;
        public Texture2D tileset;
        public Size tileSize = new Size (32f, 16f);
        public Size textureTileSize = new Size (32f, 16f);
        public int cols = 24;
        public int rows = 24;
        public float tileHeightStep = 3.0f;
        public int maxTileSteps = 8;
        public bool showTileGround = false;
        public float zOffset = 0;

        // TODO: refactor to tile class and private with accessors
        private GameObject[,] tiles;
        private GameObject[,] tilesGround;

        public MapLayer (Map map, string name, Texture2D tileset, Size tileSize, Size texTileSize, bool showGround)
        {
            this.name = name;
            this.map = map;
            this.tileset = tileset;
            this.tileSize = tileSize;
            this.textureTileSize = texTileSize;
        
            int cols = map.cols;
            int rows = map.rows;
            tiles = new GameObject[cols, rows];
            if (this.showTileGround)
                tilesGround = new GameObject[cols, rows];
        }

        public GameObject createTile (GameObject parentNode, TileCoord coord, int tileIndex)
        {
            GameObject tile = new GameObject ();
            var sr = tile.AddComponent<SpriteRenderer> ();
            sr.sprite = spriteCreateForTileIndex (tileIndex);
            // attach as child
            tile.transform.parent = parentNode.transform;
            
            var xy = map.posForTile (coord);
            var z = zOrderForTile (coord);
            var yoff = map.heightForTile (coord);
            
            //Debug.Log ("refresh height yoff = " + yoff);
            tile.transform.localPosition = new Vector3 (xy.x, xy.y + yoff, z);

            tiles [coord.c, coord.r] = tile;

            return tile;
        }

        public void changeTileColor (TileCoord coord, Color color)
        {
            if (! map.validTileCoord (coord))
                return;

            GameObject tile = tiles [coord.c, coord.r];
            //Debug.Log (coord);
            //Debug.Log (color);
            if (tile) {
                var sr = tile.GetComponent<SpriteRenderer> ();
                sr.material.color = color;
            }
        }

        // z order in Unity is + into the screen 
        public float zOrderForTile (TileCoord coord)
        {
            if (! map.validTileCoord (coord)) 
                return 0;
            
            // greater y = greater z (top center to lower left should decrease z)
            // greater tile x = greater z (top center to lower right)
            float h = map.heightForTile (coord);
            float zHeightOffset = h / (float)maxTileSteps * 0.5f;
            return zOffset - (coord.r + coord.c) - zHeightOffset;

            //return zOffset - (coord.r + coord.c);
        }

        public void refreshTile (TileCoord coord)
        {
            if (! map.validTileCoord (coord))
                return;
            
            if (map.heightForTile (coord) >= 0) {
                
                var tile = tiles [coord.c, coord.r];
                if (null != tile) {
                    var xy = map.posForTile (coord);
                    var z = zOrderForTile (coord);
                    var yoff = map.heightForTile (coord);

                    //Debug.Log ("refresh height yoff = " + yoff);
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
                        float groundHeight = map.heightForTile (coord);
                        child.transform.localPosition = new Vector3 (0, -groundHeight, 0);
                        child.transform.localScale = new Vector3 (tileSize.width * 0.95f, groundHeight * 2f + tileSize.height * 0.15f, 1);
                    }
                }
            }
        }

        public Sprite spriteCreateForTileIndex (int index)
        {
            Vector2 pivot = new Vector2 (0.5f, 0.0f);
            Rect rect = rectForTileIndex (index);
            return Sprite.Create (tileset, rect, pivot, 1f);
        }
        
        // map tile coord to rect in texture
        // row - starts 0 at top increasing down (like in Tiled)
        public Rect rectForTile (TileCoord coord)
        {   
            return new Rect (coord.c * textureTileSize.width, tileset.height - (coord.r * textureTileSize.height), textureTileSize.width, textureTileSize.height);
        }
        
        public Rect rectForTileIndex (int index)
        {
            int colPerRow = (int)(tileset.width / textureTileSize.width);
            int row = index / colPerRow;
            int col = index - row * colPerRow;
            return rectForTile (new TileCoord (col, row + 1));
        }

        public void changeTileIndex (GameObject parentNode, TileCoord coord, int tileIndex)
        {
            if (! map.validTileCoord (coord))
                return;
            
            GameObject tile = tiles [coord.c, coord.r];
            if (null == tile && tileIndex >= 0) {
                tile = createTile (parentNode, coord, tileIndex);
            } else {
                if (tileIndex >= 0) {
                    var sr = tile.GetComponent<SpriteRenderer> ();
                    //sr.sprite.textureRect = rectForTileIndex(tileIndex);
                    sr.sprite = spriteCreateForTileIndex (tileIndex);
                } else {
                    GameObject.Destroy (tile);
                    tiles [coord.c, coord.r] = null;
                }
            }
        }

        // base position of tile (doesn't include height)
        // 0,0 => halfcols, fullrows
        public Vector3 posForTile (TileCoord coord)
        {
            var mh = rows;
            var mw = cols;
            var th = tileSize.height;
            var tw = tileSize.width;
            float x = tw / 2f * (mw + coord.c - coord.r);
            float y = th / 2f * ((mh * 2f - coord.c - coord.r) - 1f);
            return new Vector3 (x, y, 0); 
        }

        // base position of tile (doesn't include height)
        // 0,0 => halfcols, fullrows
        public Vector3 posForTile (Vector2 coordFloat)
        {
            var mh = rows;
            var mw = cols;
            var th = tileSize.height;
            var tw = tileSize.width;
            // coord is in "row,col" format that maps to "x,y"
            float x = tw / 2f * (mw + coordFloat.y - coordFloat.x);
            float y = th / 2f * ((mh * 2f - coordFloat.y - coordFloat.x) - 1f);
            return new Vector3 (x, y, 0); 
        }
        
        // calculating the tile coordinates from world location
        public TileCoord tileForPos (Vector2 pos)
        {
            var x = pos.x;
            var y = pos.y;
            var mh = rows;
            var mw = cols;
            var th = tileSize.height;
            var tw = tileSize.width;
            var isox = Mathf.FloorToInt (mh - y / th + x / tw - mw / 2);// - 1/2),
            var isoy = Mathf.FloorToInt (mh - y / th - x / tw + mw / 2 + 1 / 2); // - 3/2)
            return new TileCoord (isox, isoy, 0);
        }

        public bool tileExists (TileCoord coord)
        {
            return (tiles [coord.c, coord.r] != null);
        }
    }
}

