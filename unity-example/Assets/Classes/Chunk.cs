using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ST
{
    /// <summary>
    /// A Chunk is used to improve rendering and decrease instantiation time.
    /// There are added complexities since the map is rendered in chunks and many operations are most 
    /// easily done on chunks, not individual tiles.   
    /// </summary>
    public class Chunk : MonoBehaviour
    {
        public GameObject mapGO;
        public string tilesetFilename = null;
        public Vector2 tilePixelSize = new Vector2(64, 64);
        public Vector2 tileAnchorPoint = new Vector2(0.5f, 0.25f);
        public Vector2 tileBaseSize = new Vector2(1, 1); // how many map base tiles in size NxM

        // Map game objects and components
        private Map map;
        private Mesh mesh;
        private MeshCollider col;

        // Tileset used for rendering mesh (material's main texture
        private Tileset tileset;

        // Mesh data
        private List<Vector3> newVertices = new List<Vector3>();
        private List<int> newTriangles = new List<int>();
        private List<Vector2> newUV = new List<Vector2>();
        private List<Color32> newColors = new List<Color32>();
        private int quadCount; // num quads

        // Chunk info
        public int chunkSize = 16;
        public int chunkX;
        public int chunkY;
        public int chunkZ;

        // needs update
        public bool isDirty = false;
        private System.Random rnd;


        // Use this for initialization
        void Start()
        {   
            // cache components
            map = mapGO.GetComponent("Map") as Map;
            mesh = GetComponent<MeshFilter>().mesh;
            col = GetComponent<MeshCollider>();

            if (this.tilesetFilename != null && tilesetFilename.Length > 2)
            {
                // Load tileset
                Debug.Log("trying to load tileset: " + tilesetFilename);
                this.tileset = new Tileset(map, tilesetFilename, tilePixelSize, tileBaseSize);
                var renderer = GetComponent<MeshRenderer>(); 
                var mat = renderer.materials[0];
                mat.mainTexture = tileset.getTexture();
                mat.shader = Shader.Find("Steve/Mesh");
            }
            else
            {
                var renderer = GetComponent<MeshRenderer>(); 
                var mat = renderer.materials[0];
                if (chunkX == 0 && chunkY == 0 && chunkZ == 0)
                {
                    Debug.Log("getting texture from chunk renderer");
                    Debug.Log(mat.mainTexture);
                }
                this.tileset = new Tileset(map, mat.mainTexture, tilePixelSize, tileBaseSize);
            }

            GenerateMesh();
        }

        byte Tile(int x, int y, int z)
        {
            return map.Tile(x + chunkX, y + chunkY, z + chunkZ);
        }

        byte TileHeight(int x, int y)
        {
            return map.TileHeight(x + chunkX, y + chunkY);
        }

        byte TileSeen(int x, int y)
        {
            return map.TileSeen(x + chunkX, y + chunkY);
        }

        public void GenerateMesh()
        {   
            for (int x=0; x<chunkSize; x++)
            {
                for (int y=0; y<chunkSize; y++)
                {
                    for (int z=0; z<chunkSize; z++)
                    {
                        GenSquare(x, y, z, Tile(x, y, z), TileHeight(x, y));
                    }
                }
            }
            UpdateMesh();
        }

        // TODO: refactor out tx,ty,tz into meaningful vars
        Color32 getColorForTile(int tx, int ty, int tz, float h)
        {
            Color32 c = new Color32(0, 0, 0, 255);
            float heightSegment = h / map.maxTileHeight;
            heightSegment = Mathf.Clamp(heightSegment, 0f, 1f);

            // if visited before
            if (TileSeen(tx, ty) > 0)
            {
                byte alpha = map.baseTileOpacity;
                // if top layer
                if (tz > 6)
                {
                    if (heightSegment < 0.15f)
                    {
                        c = new Color32(128, 128, 255, alpha);
                    }
                    else if (heightSegment < 0.3f)
                    {
                        c = new Color32(180, 160, 64, alpha);
                    }
                    else if (heightSegment < 0.5f)
                    {
                        c = new Color32(140, 160, 64, alpha);
                    }
                    else if (heightSegment < 0.8f)
                    {
                        c = new Color32(140, 255, 128, alpha);
                    }
                    else
                    {
                        c = new Color32(255, 255, 255, alpha);
                    }
                }
                else
                {
                    // bottom layer
                    c = new Color32(180, 160, 128, alpha);
                }
            }
            return c;
        }

        void GenSquare(int tx, int ty, int tz, byte tileid, byte tileHeight)
        {
            if (tileid == 0) 
                return;

            // TODO: move to class property
            float tileHeightStep = 0.6f;

            // get absolute tile coord
            float mapTileX = tx + chunkX;
            float mapTileY = ty + chunkY;

            // alt rows are horz offset by half width for staggered isometric map
            float xOffset = ((ty + chunkY) % 2 == 0) ? map.tUnitOffsets.x * 0.5f : 0f;
            float x = tx * map.tUnitOffsets.x + xOffset;

            // rows are positioned at half-height grid since they overlap in vertical axis 
            float y = ty * map.tUnitOffsets.y * 0.5f;
        
            if (map.relativeLayerHeight)
            {
                y += ((float)tileHeight * tileHeightStep * map.tUnitOffsets.y);           
                // special case offsets depending on map layer (tz)
                if (tz > 6)
                {
                    y += (tz - 6) * map.tUnitOffsets.y * 0.01f;
                }
                else
                {
                    y -= ((6 - tz) * map.tUnitOffsets.y * 0.2f + map.tUnitOffsets.y * 0.5f);
                }
            }
            else
            {
                if (tz > 6)
                {
                    // at tileheight
                    y += ((float)tileHeight * tileHeightStep * map.tUnitOffsets.y);
                    y += (tz - 6) * map.tUnitOffsets.y * 0.01f;
                }
                else
                {
                    int segHeight = Mathf.Min(tz, tileHeight);         
                    y += segHeight * map.tUnitOffsets.y * 0.5f;
                }
            }

            // TODO: this is a hack, should either use a base tile with ground as top tz=6 tile
            //       or otherwise make this generic to any tileset with 1 tile high base tiles and 
            //       no height ground base floor tiles
            // TODO: possibly have different tilesets for ground/floor and base under-ground/floor structure tiles 
            if (tz <= 6)
            {
                if (map.relativeLayerHeight)
                {
                    y -= map.tUnitOffsets.y * 0.55f;
                }
                else
                {
                    y -= map.tUnitOffsets.y * 0.75f;
                }

            }

            // real z is based on ordering tiles should form from top to bottom, right to left
            float z = 50f + mapTileY + mapTileX / map.mapX - (tz + chunkZ) * 0.005f;
            z *= 0.15f; // decrease/increase range of z values (careful about precision)

            // special case offsets depending on map layer (tz)
            if (tz > 6)
            {
                z -= (tz - 6) * 0.05f;
            }
            else
            {
                z += (7 - tz) * 0.02f;
            }        

            // TODO: should refactor above or below into super or sub method

            // tile's bounding rect using halfs to center around anchor point
            float leftHalfW = tileset.tUnitSize.x * tileAnchorPoint.x;
            float rightHalfW = tileset.tUnitSize.x * (1f - tileAnchorPoint.x);
            float botHalfH = tileset.tUnitSize.y * tileAnchorPoint.y;
            float topHalfH = tileset.tUnitSize.y * (1f - tileAnchorPoint.y);
            newVertices.Add(new Vector3(x - leftHalfW, y + topHalfH, z));
            newVertices.Add(new Vector3(x + rightHalfW, y + topHalfH, z));
            newVertices.Add(new Vector3(x + rightHalfW, y - botHalfH, z));
            newVertices.Add(new Vector3(x - leftHalfW, y - botHalfH, z));

            // default order for quad 
            newTriangles.Add(quadCount * 4); //1
            newTriangles.Add(quadCount * 4 + 1); //2
            newTriangles.Add(quadCount * 4 + 2); //3
            newTriangles.Add(quadCount * 4); //1
            newTriangles.Add(quadCount * 4 + 2); //3
            newTriangles.Add(quadCount * 4 + 3); //4
    
            Rect tileRect = tileset.getTileUVRectFromSpecialId(tileid);
            newUV.Add(new Vector2(tileRect.x, tileRect.y + tileRect.height));
            newUV.Add(new Vector2(tileRect.x + tileRect.width, tileRect.y + tileRect.height));
            newUV.Add(new Vector2(tileRect.x + tileRect.width, tileRect.y));
            newUV.Add(new Vector2(tileRect.x, tileRect.y));

            // colors of each vertex for top layers
            float h = (float)tileHeight;
            var c = getColorForTile(tx, ty, tz, h);
            newColors.Add(c);
            newColors.Add(c);
            newColors.Add(c);
            newColors.Add(c);

            if ((chunkX + tx) == 0 && (chunkY + ty) == 0 && (chunkZ + tz) == 0)
            {
//                Debug.Log("x = " + x);
//                Debug.Log("y = " + y);
//                Debug.Log("z = " + z);
//                Debug.Log("tileRect = " + tileRect.x + ", " + tileRect.y + ", " + tileRect.width + ", " + tileRect.height);
//                Debug.Log("h = " + h + " => " + c);
//                Debug.Log("tileid = " + tileid);
//                Debug.Log("txyz = " + tx + ", " + ty + ", " + tz);
            }

            quadCount++; // Add this line
        }
    
        void UpdateMesh()
        {
            // should clear out
            mesh.Clear();

            // it appears can only replace entire vector, not manip single elements 
            mesh.vertices = newVertices.ToArray();
            mesh.uv = newUV.ToArray();
            mesh.triangles = newTriangles.ToArray();
            mesh.colors32 = newColors.ToArray();

            // let unity do its thing
            mesh.Optimize();
            mesh.RecalculateNormals();

            // TODO: does this help or hurt? I would think static would be enabled on all non-moving objects with colliders?
            this.gameObject.isStatic = true;

            // add collider
            col.sharedMesh = null;
            col.sharedMesh = mesh;

            // reset for next re-gen
            newVertices.Clear();
            newUV.Clear();
            newTriangles.Clear();
            newColors.Clear();
            quadCount = 0;
        }

        // not using, could use in place of forced call to generateMesh in map class
        void LateUpdate()
        {
            if (isDirty)
            {
                GenerateMesh();
                isDirty = false;
            }
        }
    }
}