using UnityEngine;
using System;

namespace ST
{
    /// <summary>
    /// Map is tile-based. It'll likely become a base class for Isometric, Hexagonal, and Orthogonal map types.
    /// Currently it's data model is meant to be simplistic.
    /// However, it is not bitpacked, nor using a sparse data structure for the top layers where fewer tiles exist.
    /// The main optimization for instantiation and rendering efficiency is using chunks.
    /// 
    /// This implementation is based off the tutorial series on efficient voxel maps with Unity.
    /// http://studentgamedev.blogspot.com/p/unity3d-c-voxel-and-procedural-mesh.html
    /// 
    /// There are options that can be set in Unity editor. The main ones are chunk size, map size, and whether to
    /// render base layers as defined bottom height or not and instead will render with a "flowing" relative base layer height mode
    /// 
    /// * currently this is Isometric staggered map only
    ///
    /// TODO: 
    /// - reintegrate Entity.cs with movement and placement on map with correct z-order based on previous test project(s)
    /// - add "smooth" or "plateau" tool to modifyTerrain, change all neighbor tiles in radius to have same height as tile under mouse
    /// - create Game.cs class to implement large tower defense maps in iso/hex/orthogonal viewpoints
    /// - create Tile.cs to allow for sparse data and bitpacking of collision, seen, height, objects, etc
    /// - redefine the chunk system to support including the bottom layers in a single chunk
    /// - possibly don't use chunks for top layers depending on mouse and input for entity/terrain selection
    /// - create collision data in since removed the wall
    /// - create Chunk.cs for each map type for first iteration (IsoChunk) or create helpers for positioning and z-ordering (IsoUtils.cs)
    /// - first add capability for diamond maps
    /// - define more hotkeys for map/camera manipulation
    /// - add move camera with scroll speed when mouse near edge
    /// - improve map visuals by utilizing the full tileset
    /// - enable the different tile size and UV offsets capabilities for large trees (using a 2nd tileset instance)
    /// 
    /// Possibly port to cocos2d-js to allow for discussion between the two.
    /// Also, look at using our 2013 tests for any useful data model aspects as well as rotating map code
    /// https://dl.dropboxusercontent.com/u/168438/1gam/tutorial-tilemaps/001/index.html
    /// - q,e for rotating map (in Unity it's currently for zoom in/out)
    /// - both staggered and diamond map support
    /// - rotate staggered increases map "canvas" into larger diamond map, rotates, crops
    /// - thus rotating staggered map doesn't look correct since row height is half col width
    /// - ideas on tile info data model instead of separate bitmaps per function (height, seen, id, etc)
    /// https://dl.dropboxusercontent.com/u/168438/1gam/subterra/002/index.html
    /// - for selection, tool modes, cocos2d input handling
    /// </summary>
    public class Map : MonoBehaviour
    {
        public GameObject chunk;
        public Chunk[,,] chunks;
        public bool addFogOfWarRandomly = false;
        public bool relativeLayerHeight = false;
        public byte baseTileOpacity = 255;
        public int chunkSize = 16;
        public int maxTileHeight = 10;

        // TODO: 
        // - should separate the data from the map generator/behavior
        // - bytes should be useful in reducing memory, could have 4 tiles to a byte for FoW(seen)
        // - is bitpacking better than separating into Tile.cs class? where's the cross-over for efficiency
        // - what map sizes does this even matter, create table of map size and memory use
        //   (incl. layer count and percentage of tiles filled for each layer)
        public byte[,,] tileIds;
        public byte[,] tileHeights;
        public byte[,] tileSeen;

        // number of map "tiles" in each axis (z refers to layers currently)
        public int mapX = 10;
        public int mapY = 10;
        public int mapZ = 1;

        // Calcs that shouldn't change, but are defined by the camera/gameObjects in Unity Editor / Project Settings
        // default unity is 100 points per unit (pixels on desktop, need to reference how unity handles retina displays)
        //
        // unity units per pixel
        public float tUnit = 1f / 100f; 
        // size of boundingbox of tile's base for all tiles in map (standard 32x16)
        public Vector2 tileMapSize = new Vector2 (32, 16);
        // unity units for tile position
        public Vector2 tUnitOffsets;
        private Entity testEntity;

        //
        int PerlinNoise (int x, int y, int z, float scale, float height, float power)
        {
            float rValue;
            rValue = Noise.Noise.GetNoise (((double)x) / scale, ((double)y) / scale, ((double)z) / scale);
            rValue *= height;

            if (power != 0) {
                rValue = Mathf.Pow (rValue, power);
            }

            return (int)rValue;
        }

        public byte Tile (int x, int y, int z)
        {

            if (x >= mapX || x < 0 || y >= mapY || y < 0 || z >= mapZ || z < 0) {
                return (byte)0;
            }
            return tileIds [x, y, z];
        }

        public byte TileHeight (int x, int y)
        {

            if (x >= mapX || x < 0 || y >= mapY || y < 0) {
                return (byte)0;
            }
            return tileHeights [x, y];
        }

        public byte TileSeen (int x, int y)
        {
        
            if (x >= mapX || x < 0 || y >= mapY || y < 0) {
                return 0;
            }
            return tileSeen [x, y];
        }

        /// <summary>
        /// Get tile coordinate from world coordinates.
        /// Should effectively be reverse coordinate calculations for positioning each tile in the world      
        /// </summary>
        /// <returns>The from world.</returns>
        /// <param name="world">World.</param>
        public TileCoord tileFromWorld (Vector3 world)
        {
            
            // WHEN CREATING CHUNK (for reference)
            // float px = x * chunkSize * tileMapSize.x * tUnit - 0.5f;
            // float py = y * chunkSize * tileMapSize.y * tUnit * 0.5f;
            
            
            // TODO: cache this data
            // This should be zeroed, but it could change eventually? camera should move, not map
            //var worldOffset = new Vector3(0,0,0);
            
            // where in chunk?
            var tileSize = this.tileMapSize;
//            var chunkSize = this.chunkSize;

            int chunkR = Mathf.RoundToInt (world.y / (tileSize.y * this.tUnit * 0.5f));
            int chunkC = Mathf.RoundToInt (world.x / (tileSize.x * this.tUnit));
//            int tileR = Mathf.RoundToInt(world.y - (chunkR * chunkWorldSize.y));
//            int tileC = Mathf.RoundToInt(world.x - (chunkC * chunkWorldSize.x));
//            int r = chunkR;// * chunkSize + tileR;
//            int c = chunkC;// * chunkSize + tileC;
//            return new TileCoord(c, r);
            if (chunkC < 0 || chunkR < 0 || chunkC >= tileIds.GetLength (0) || chunkR >= tileIds.GetLength (1)) {
                return null;
            }
            return new TileCoord (chunkC, chunkR);
        }

        /// <summary>
        /// TODO:
        /// - check this, it was a quick addition for Entity, likely incorrect/buggy
        /// </summary>
        public Vector3 worldFromTile (TileCoord tile)
        {
            float px = tile.c * chunkSize * tileMapSize.x * tUnit - 0.5f;
            float py = tile.r * chunkSize * tileMapSize.y * tUnit * 0.5f;
            float pz = 0f;

            return new Vector3(px, py, pz);
        }   

    

        /// <summary>
        /// Enter's scene
        /// </summary>
        public void Start ()
        {
            this.tUnitOffsets = new Vector2 (tileMapSize.x * tUnit, tileMapSize.y * tUnit);
            Debug.Log ("tUnitOffsets = " + tUnitOffsets);

            tileIds = new byte[mapX, mapY, mapZ];
            tileHeights = new byte[mapX, mapY];
            tileSeen = new byte[mapX, mapY];

            var rnd = new System.Random ();
            float h = (float)this.maxTileHeight;

            // Simple and stuipd noise map generation
            // TODO: using the tileset let's generate maps similar to this
            //       http://opengameart.org/content/isometric-64x64-outside-tileset
            //
            // TODO: improve, create real layers, create base tiles based on surface type
            // TODO: allow for improved water
            for (int x=0; x < mapX; x++) {
                for (int y=0; y < mapY; y++) {

                    // use noise for height
                    float noiseFactor = PerlinNoise (x, y, 0, 4f * h, h, 0);
                    tileHeights [x, y] = (byte)(noiseFactor);

                    // set all viewable
                    tileSeen [x, y] = 1; 

                    // for now using z as layer at given tile coord
                    for (int z=0; z < mapZ; z++) {
                        if (z <= 6) {
                            // base layer tile
                            tileIds [x, y, z] = (byte)(1);//rnd.Next() % 2 + 1);
                        } else if (z <= 8) {
                            // "grass" layer tile
                            tileIds [x, y, z] = (byte)(2);
                        } else if (rnd.Next () % 15 == 1) {
                            // "object" layer tile
                            tileIds [x, y, z] = (byte)(2);//rnd.Next() % 3 + 3);
                        }
                    }
                }
            }

            int nX = Mathf.Max (Mathf.CeilToInt ((float)mapX / (float)chunkSize), 1);
            int nY = Mathf.Max (Mathf.CeilToInt ((float)mapY / (float)chunkSize), 1);
            int nZ = Mathf.Max (Mathf.CeilToInt ((float)mapZ / (float)chunkSize), 1);
            Debug.Log ("creating chunks[" + nX + ", " + nY + ", " + nZ + "]");
            chunks = new Chunk[nX, nY, nZ];

            //chunk.renderer.materials[0].SetTexture("Element 0", tilesetTexture);

            // TODO: this part of the map generation should or needs to be modified depending on your map and gameplay
            // as you might want to easily change the top layer, but not necessarily the base layers
            // 
            for (int x=0; x<chunks.GetLength(0); x++) {
                for (int y=0; y<chunks.GetLength(1); y++) {
                    // z = 0 is the bottom layers of the map currently
                    // z = length is the top layer of the map currently
                    for (int z=0; z<chunks.GetLength(2); z++) {

                        //Create a temporary Gameobject for the new chunk instead of using chunks[x,y,z]
                        // offset of chunk is #tiles(chunksize) * pixels/unity
                        // y is offset only by half since rows in isometric are spaced half height apart
                        float px = x * chunkSize * tileMapSize.x * tUnit - 0.5f;
                        float py = y * chunkSize * tileMapSize.y * tUnit * 0.5f;
                        float pz = 0.0f;
                        GameObject newChunk = Instantiate (chunk, new Vector3 (px, py, pz), new Quaternion (0, 0, 0, 0)) as GameObject;

                        // We are currently using this map object as the parent to collect all the chunks in the editor hierarchy
                        newChunk.transform.parent = this.gameObject.transform;

                        //Now instead of using a temporary variable for the script assign it
                        //to chunks[x,y,z] and use it instead of the old \"newChunkScript\"
                        chunks [x, y, z] = newChunk.GetComponent ("Chunk") as Chunk;
                        chunks [x, y, z].mapGO = this.gameObject;
                        chunks [x, y, z].tilesetFilename = "terrain_1.png";
                        chunks [x, y, z].chunkSize = chunkSize;
                        chunks [x, y, z].chunkX = x * chunkSize;
                        chunks [x, y, z].chunkY = y * chunkSize;
                        chunks [x, y, z].chunkZ = z * chunkSize;
                    }
                }
            }

            testEntity = new Entity (this);
            testEntity.setTile (new TileCoord(5,5));

        }
    }
}