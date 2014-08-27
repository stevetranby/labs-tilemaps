using UnityEngine;
using System;
using System.Collections;

namespace ST
{
    /// <summary>
    /// This class is used to manage user interaction with the map
    /// </summary>
    public class ModifyTerrain : MonoBehaviour
    {
        Map map;
        GameObject cameraGO;
        TileCoord selectedTile = null;
        public float cameraPanSpeed = 7f;
        System.Random rnd = new System.Random ();

        void Start ()
        {
            this.map = gameObject.GetComponent ("Map") as Map;
            cameraGO = GameObject.FindGameObjectWithTag ("MainCamera");

            if (this.map.addFogOfWarRandomly) {
                StartCoroutine ("RemoveActiveSightPeriodically");
            }
        }
    
        IEnumerator RemoveActiveSightPeriodically ()
        {
            // start coroutine for check to fake hiding area of "FoW"
            // periodically randomly turn off visibility for an area
            while (true) {       
                // change a random tile in a random chunk
                if (map != null && map.tileIds != null) {
                    int x = rnd.Next () % map.tileIds.GetLength (0);
                    int y = rnd.Next () % map.tileIds.GetLength (1);
                    int radius = map.chunkSize / 3;
                    for (int r = -radius; r < radius; r++) {
                        for (int c = -radius; c < radius; c++) {
                            int tx = x + c;
                            int ty = y + r;
                            if (Mathf.Abs (c) + Mathf.Abs (r) < radius) {
                                SetBlockSeenAt (tx, ty, false, false);
                            }
                        }
                    }
                    // TODO: can we improve UpdateChunk by changing verts/uv/colors directly on mesh?
                    // don't physically update until last since expensive
                    SetBlockSeenAt (x, y, false, true);
                }
                yield return new WaitForSeconds (5.0f);
            }
        }
    
        public void Update ()
        {
            // TODO: refactor into InputHandler class           

            // Tutorial Behavior
//      if(Input.GetMouseButtonDown(0)){
//          ReplaceBlockCursor(2);
//      }
//      
//      if(Input.GetMouseButtonDown(1)){
//          AddBlockCursor(3);
//      }

            if (Input.GetMouseButtonDown (0)) {
                Debug.Log ("Selecting Tile");

                if (selectedTile != null) {
                    // reset color
                    //SetTileColor(selectedTile, Color32(255,255,255,255));
                    selectedTile = null;
                }

                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                RaycastHit hit;            
                if (Physics.Raycast (ray, out hit)) {  
                    Vector3 position = hit.point;
                    TileCoord hitTile = this.map.tileFromWorld (position);
                    selectedTile = hitTile;
                    //SetTileColor(selectedTile, Color32(0,64,255,255));
                }
            }

            if (Input.GetMouseButtonDown (1)) {
                Debug.Log ("smooth neighbor tiles");
                // cause neighbors to match height
                SmoothBlockCursor ();
            } else {
                // check if moved
                // TODO: add selection cursor hovering over map
            }

            if (Input.GetKey (KeyCode.A)) {
                var p = this.cameraGO.transform.localPosition;
                p.x -= this.map.tUnit * cameraPanSpeed * cameraGO.camera.orthographicSize;
                if (p.x > 0)
                    this.cameraGO.transform.localPosition = p;
            }
            if (Input.GetKey (KeyCode.D)) {
                var p = this.cameraGO.transform.localPosition;
                p.x += this.map.tUnit * cameraPanSpeed * cameraGO.camera.orthographicSize;
                if (p.x < this.map.tUnit * this.map.tileIds.GetLength (0) * this.map.tileMapSize.x)
                    this.cameraGO.transform.localPosition = p;
            }
            if (Input.GetKey (KeyCode.S)) {
                var p = this.cameraGO.transform.localPosition;
                p.y -= this.map.tUnit * cameraPanSpeed * cameraGO.camera.orthographicSize;
                if (p.y > 0)
                    this.cameraGO.transform.localPosition = p;
            }
            if (Input.GetKey (KeyCode.W)) {
                var p = this.cameraGO.transform.localPosition;
                p.y += this.map.tUnit * cameraPanSpeed * cameraGO.camera.orthographicSize;
                if (p.y < this.map.tUnit * this.map.tileIds.GetLength (1) * this.map.tileMapSize.y * 0.5f)
                    this.cameraGO.transform.localPosition = p;
            }


            if (Input.GetKey (KeyCode.Q)) {
                this.cameraGO.camera.orthographicSize += map.tUnit * cameraPanSpeed;
            }
            if (Input.GetKey (KeyCode.E)) {
                this.cameraGO.camera.orthographicSize -= map.tUnit * cameraPanSpeed;
            }
        }

        public void ReplaceBlockCenter (float range, byte block)
        {
            //Replaces the block directly in front of the player
        
            Ray ray = new Ray (cameraGO.transform.position, cameraGO.transform.forward);
            RaycastHit hit;
        
            if (Physics.Raycast (ray, out hit)) {
            
                if (hit.distance < range) {
                    ReplaceBlockAt (hit, block);
                }
            }
        }
    
        public void AddBlockCenter (float range, byte block)
        {
            //Adds the block specified directly in front of the player

            Ray ray = new Ray (cameraGO.transform.position, cameraGO.transform.forward);
            RaycastHit hit;
        
            if (Physics.Raycast (ray, out hit)) {       
                if (hit.distance < range) {
                    AddBlockAt (hit, block);
                }
                Debug.DrawLine (ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
            }
        }
    
        public void ReplaceBlockCursor (byte block)
        {
            //Replaces the block specified where the mouse cursor is pointing
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit)) {
                ReplaceBlockAt (hit, block);
                Debug.DrawLine (ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
            }
        }
    
        public void AddBlockCursor (byte block)
        {
            //Adds the block specified where the mouse cursor is pointing
        
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
        
            if (Physics.Raycast (ray, out hit)) {
            
                AddBlockAt (hit, block);
                Debug.DrawLine (ray.origin, ray.origin + (ray.direction * hit.distance),
                           Color.green, 2);
            }
        }
    
        // TODO: need to fix tile coord from world coord and 
        public void SmoothBlockCursor ()
        {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;            
            if (Physics.Raycast (ray, out hit)) {                
                // TODO: We need to account for heights in tiles, prob in tileFromWorld()

                // get block at (raycasthit)
                Vector3 position = hit.point;
                Debug.Log (position);
//                position += (hit.normal * -0.5f);

                // get block at (position)
                // need to get tile coord from world coord
                var tile = this.map.tileFromWorld (position);
                int x = tile.c;
                int y = tile.r;
//                int z = Mathf.RoundToInt(position.z);

                Debug.Log ("smoothing neighbor tiles around " + x + ", " + y);

                if (x < 0 || y < 0) 
                    return;

                // update neighbors
                byte h = this.map.tileHeights [x, y];
                int radius = 7;
                for (int r = -radius; r < radius; r++) {
                    for (int c = -radius; c < radius; c++) {
                        int tx = x + c;
                        int ty = y + r;
                        int cols = this.map.tileIds.GetLength (0);
                        int rows = this.map.tileIds.GetLength (1);
                        if ((Mathf.Abs (c) + Mathf.Abs (r)) < radius && tx >= 0 && ty >= 0 & tx < cols && ty < rows) {
                            this.map.tileHeights [tx, ty] = h;
                        }
                    }
                }

                // TODO: still seems to not update adjacent chunks sometimes? curious....
                int zMax = map.chunks.GetLength (2);
                for (int z = 0; z < zMax; ++z) {
                    UpdateChunkAt (x, y - 1, z);
                    UpdateChunkAt (x, y, z);
                    UpdateChunkAt (x, y + 1, z);
                    UpdateChunkAt (x - 1, y - 1, z);
                    UpdateChunkAt (x - 1, y, z);
                    UpdateChunkAt (x - 1, y + 1, z);
                    UpdateChunkAt (x + 1, y - 1, z);
                    UpdateChunkAt (x + 1, y, z);
                    UpdateChunkAt (x + 1, y + 1, z);
                }

                Debug.DrawLine (ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
            }
        }

        public void ReplaceBlockAt (RaycastHit hit, byte block)
        {
            //removes a block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
            Vector3 position = hit.point;
            position += (hit.normal * -0.5f);
            SetBlockAt (position, block);
        }
    
        public void AddBlockAt (RaycastHit hit, byte block)
        {
            //adds the specified block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
            Vector3 position = hit.point;
            position += (hit.normal * 0.5f);
        
            SetBlockAt (position, block);
        }
    
        public void SetBlockAt (Vector3 position, byte block)
        {
            SetBlockAt (position, block, false);
        }
                
        public void SetBlockAt (Vector3 position, byte block, bool update)
        {
            //sets the specified block at these coordinates
            int x = Mathf.RoundToInt (position.x);
            int y = Mathf.RoundToInt (position.y);
            int z = Mathf.RoundToInt (position.z);
            SetBlockAt (x, y, z, block, update);
        }
    
        public void SetBlockAt (int x, int y, int z, byte block, bool update)
        {
            if (x > 0 && y > 0 && z > 0 
                && x < map.tileIds.GetLength (0)
                && y < map.tileIds.GetLength (1)
                && z < map.tileIds.GetLength (2)) {
                //adds the specified block at these coordinates
                //print("Adding: " + x + ", " + y + ", " + z);
                map.tileIds [x, y, z] = block;
                if (update)
                    UpdateChunkAt (x, y, z);
            } else {
                //Debug.Log("failed hit test");
            }
        }
    
        public void SetBlockSeenAt (int x, int y, bool seen, bool update)
        {
            if (x > 0 && y > 0 
                && x < map.tileIds.GetLength (0)
                && y < map.tileIds.GetLength (1)) {
                // TODO: this should allow for variability (either -1,0,1 for inactive, unseen, active, or 0,1,2, or 0-N for time delayed "fade")
                map.tileSeen [x, y] = seen ? (byte)1 : (byte)0;
                if (update) {
                    int z = map.chunks.GetLength (2) - 1;
                    UpdateChunkAt (x, y, z);
                }
            } else {
                //Debug.Log("failed hit test");
            }
        }
    
        public void UpdateChunkAt (int x, int y, int z)
        {
            //Updates the chunk containing this block
        
            int updateX = Mathf.FloorToInt (x / map.chunkSize);
            int updateY = Mathf.FloorToInt (y / map.chunkSize);
            int updateZ = Mathf.FloorToInt (z / map.chunkSize);
        
            //print("Updating: " + updateX + ", " + updateY + ", " + updateZ);
  
//            // let's just force update directly for now
//            map.chunks[updateX, updateY, updateZ].GenerateMesh();

            // A better method to regenerate after all normal updates occur
            if (updateX < 0 || updateY < 0 || updateZ < 0
                || updateX >= map.chunks.GetLength (0) 
                || updateY >= map.chunks.GetLength (1)
                || updateZ >= map.chunks.GetLength (2)) {
                return;
            }

            map.chunks [updateX, updateY, updateZ].isDirty = true;


            // TODO: fix so this part works correctly, or test that it is working now
//            // Update Neighbors if tile is on edge of chunk
//              if(x-(map.chunkSize*updateX)==0 && updateX!=0){
//                  map.chunks[updateX-1,updateY, updateZ].isDirty=true;
//              }
//              
//              if(x-(map.chunkSize*updateX)==15 && updateX!=map.chunks.GetLength(0)-1){
//                  map.chunks[updateX+1,updateY, updateZ].isDirty=true;
//              }
//              
//              if(y-(map.chunkSize*updateY)==0 && updateY!=0){
//                  map.chunks[updateX,updateY-1, updateZ].isDirty=true;
//              }
//              
//              if(y-(map.chunkSize*updateY)==15 && updateY!=map.chunks.GetLength(1)-1){
//                  map.chunks[updateX,updateY+1, updateZ].isDirty=true;
//              }
//              
//              if(z-(map.chunkSize*updateZ)==0 && updateZ!=0){
//                  map.chunks[updateX,updateY, updateZ-1].isDirty=true;
//              }
//              
//              if(z-(map.chunkSize*updateZ)==15 && updateZ!=map.chunks.GetLength(2)-1){
//                  map.chunks[updateX,updateY, updateZ+1].isDirty=true;
//              }
        }
    }
}