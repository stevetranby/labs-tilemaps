using UnityEngine;
using System.Collections;

namespace ST
{
    public class Game : MonoBehaviour
    {
        public AudioClip sfxButton;
        public AudioClip sfxMovePlayer;
        public AudioClip sfxLowerTile;
        public AudioClip sfxRaiseTile;
        public GameObject ship;
        public GameObject floorLayer;
        public Texture2D floorTileset;
        public GameObject wallLayer;
        public Texture2D wallTileset;
        public GameObject playerGameObject;
        public GameObject selectedFloor;
        public GameObject selectedWall;
        public float tileWidth;
        public float tileHeight;
        public bool showTileGround;
        public bool UseAstarPathfinding = false;
        public bool UseSimple2Dastar = false;
        private Map map;
        private Entity player;
        private Entity enemy;
        private int selectedTileIndex = 1;
        private int selectedWallIndex = 1;
        private Rect guiPanelRect = new Rect (10, 10, 120, 200);
        private bool mouseIsDown = false;
        private double changeTarget = 0f;

        // AronGrandberg's AStar Props
        public GameObject astarShipNode;
        public GameObject enemyGameObject;
        public GameObject astarPlayer;
        public GameObject astarEnemy;

        enum Mode
        {
            MovePlayer,
            PlaceTile,
            ResetTile,
            RaiseTile,
            LowerTile,
            PlaceWall
        }

        private Mode mode = Mode.MovePlayer;

        // Use this for initialization
        void Start ()
        {
            if (UseAstarPathfinding) {
                Debug.Log ("using an AStar system");
                if (UseSimple2Dastar) {
                    Debug.Log ("using simple AStar");
                } else {
                    Debug.Log ("using AronGranberg's AStar");
                }
            } else {
                Debug.Log ("NOT using AStar system, simple seek instead");
            }

            map = new Map ();
            map.initFloor (floorTileset, new Size (tileWidth, tileHeight), new Size (32f, 16f), true);
            // TODO: implement wall placement as collision objects
            map.initWalls (wallTileset, new Size (tileWidth, tileHeight), new Size (32f, 64f), false);
            map.UseAstarPathfinding = UseAstarPathfinding;
            map.UseSimple2Dastar = UseSimple2Dastar;
            map.astarShipNode = astarShipNode;
            map.generateMap (floorLayer);

            player = new Entity (map);
            player.Start (playerGameObject);

            if (UseAstarPathfinding) {
                if (UseSimple2Dastar) {
                } else {
                    // hook up enemy object
                    enemy = new Entity (map);
                    enemy.Start (enemyGameObject);

                    // create some walls
                    map.changeWall (new TileCoord (3, 4), 1);
                    map.changeWall (new TileCoord (4, 4), 1);
                    map.changeWall (new TileCoord (5, 4), 1);
                    map.changeWall (new TileCoord (6, 4), 1);
                    map.changeWall (new TileCoord (7, 4), 1);
                    map.changeWall (new TileCoord (3, 5), 1);
                    map.changeWall (new TileCoord (3, 6), 1);
                    map.changeWall (new TileCoord (3, 7), 1);
                    map.changeWall (new TileCoord (3, 8), 1);
                }
            }
        }
    
        // Update is called once per frame
        void Update ()
        {
            var fps = 1.0f / Time.deltaTime;
            if (fps < 2) {
                Debug.Break ();
            }

            if (Input.GetMouseButton (0)) {
                // get screen coord
                var mouse = Input.mousePosition;
                var p = Camera.main.ScreenToWorldPoint (mouse);

                //Debug.Log ("mouse = " + mouse + ", p = " + p + ", guipanelrect = " + guiPanelRect);

                var tileCoord = map.tileForPos (p);
                //Debug.Log ("tilecoord = " + tileCoord.c + ", " + tileCoord.r + ", seltileidx = " + selectedTileIndex);

                if (guiPanelRect.Contains (new Vector2 (mouse.x, Screen.height - mouse.y))) {      
                    Debug.Log ("protecting GUI");
                } else if (mode == Mode.PlaceTile) {
                    map.changeFloor (tileCoord, selectedTileIndex);
                } else if (mode == Mode.PlaceWall) {
                    map.changeWall (tileCoord, selectedWallIndex);
                }
            }

            // only below once every time mouse is pressed and released
            if (mouseIsDown) {
                mouseIsDown = Input.GetMouseButton (0);
            } else if (Input.GetMouseButton (0)) {
                mouseIsDown = true;

                var mouse = Input.mousePosition;
                var p = Camera.main.ScreenToWorldPoint (mouse);
                var tileCoord = map.tileForPos (p);
        
                if (guiPanelRect.Contains (new Vector2 (mouse.x, Screen.height - mouse.y))) {      
                    Debug.Log ("protecting GUI");
                } else if (mode == Mode.MovePlayer) {

                    // set target tile
                    
                    if (map.UseAstarPathfinding) {
                        if (map.UseSimple2Dastar) {
                            player.setGoalTile (tileCoord);
                        } else {
                            // use the actual A* plugin
                            var ai = astarPlayer.GetComponent<TestAISeekPlayer> ();
                            ai.targetPosition = new Vector3 (tileCoord.r, ai.targetPosition.y, tileCoord.c);

                            Debug.Log ("target updated to " + ai.targetPosition);
                            ai.updatePath ();
                        }
                    } else {
                        player.setGoalTile (tileCoord);
                    }

                    audio.PlayOneShot (sfxMovePlayer);
                    //player.setTile (tileCoord);
                } else if (mode == Mode.RaiseTile) {
                    map.raiseTile (tileCoord);
                    audio.PlayOneShot (sfxRaiseTile);
                } else if (mode == Mode.LowerTile) {
                    map.lowerTile (tileCoord);
                    audio.PlayOneShot (sfxLowerTile);
                }
            }


            if (UseAstarPathfinding) {
                if (UseSimple2Dastar) {
                    // simple 2d astar
                    // use entity update
                    if (null != player) {
                        player.update ();
                    }
                } else {

                    // TODO: should probably just get the 3D astar path from the A* system and set the waypoints
                    //       in a similar manner to how simple astar works
                    // map players and enemy objects
                    {
                        // NOTE: unity 3D coords are mapped using floor plane (XZ in this case)
                        var astarPos = astarPlayer.transform.localPosition;

                        int r = (int)Mathf.Floor (astarPos.x + 0.5f);
                        int c = (int)Mathf.Floor (astarPos.z + 0.5f);

//                      float rFrac = astarPos.x - (float)r;
//                      float cFrac = astarPos.z - (float)c;
//
//                      // NOTE: pos in 2D tilemap uses x,y as position in 2D projection
//                      //var pos = map.posForTile(new TileCoord(c,r));                 
//                      pos.y += rFrac;
//                      pos.x += cFrac;
//                      Debug.Log("player tile = {" + r + ", " + c + "}, pos = " + pos + ", rfrac = " + rFrac + ", cfrac = " + cFrac);

                        var pos = map.posForTile (new Vector2 (astarPos.x, astarPos.z));                    
                        // remove offset from unity
                        pos.x += 0.5f;
                        pos.y += 0.5f;
                        //Debug.Log("player astarpos = " + astarPos + ", pos = " + pos);
                                
                        player.curTile = new TileCoord (c, r);
                        player.setPosition (new Vector3 (pos.x, pos.y));
                    }

                    // enemies
                    {
                        changeTarget -= Time.deltaTime;
                        if (changeTarget < 0) {
                            // range of seconds for how often to change 
                            changeTarget = Random.Range (6.0f, 10.0f);
                            TileCoord tileCoord;

                            // higher chance we'll move toward random tile instead of player
                            if (Random.Range (0, 10) > 3) {
                                // go to random location
                                int r = Random.Range (0, map.rows);
                                int c = Random.Range (0, map.cols);
                                tileCoord = new TileCoord (c, r);                           
                            } else {
                                tileCoord = player.curTile;
                            }

                            // use the actual A* plugin
                            var ai = astarEnemy.GetComponent<TestAISeekPlayer> ();
                            ai.targetPosition = new Vector3 (tileCoord.r, ai.targetPosition.y, tileCoord.c);
                            
                            Debug.Log ("enemy target updated to " + ai.targetPosition);
                            ai.updatePath ();
                        } else {                
                            var astarPos = astarEnemy.transform.localPosition;
                            int r = (int)Mathf.Floor (astarPos.x + 0.5f);
                            int c = (int)Mathf.Floor (astarPos.z + 0.5f);
                            var pos = map.posForTile (new Vector2 (astarPos.x, astarPos.z));                    
                            pos.x += 0.5f;
                            pos.y += 0.5f;
                            enemy.curTile = new TileCoord (c, r);
                            enemy.setPosition (new Vector3 (pos.x, pos.y));
                        }
                    }
                }
            } else {
                // not using astar pathfinding
                if (null != player)
                    player.update ();
            }
        }

        void OnGUI ()
        {
            // Make a background box
            GUI.Box (guiPanelRect, "Loader Menu");

            if (GUI.Button (new Rect (10, 40, 100, 24), "Move Player")) {
                mode = Mode.MovePlayer;
            }

            if (GUI.Button (new Rect (10, 70, 80, 24), "Place Tile")) {
                mode = Mode.PlaceTile;
            } 
            if (GUI.Button (new Rect (90, 70, 80, 24), "Remv Tile")) {
                mode = Mode.PlaceTile;
                selectedTileIndex = -1;
            }

            // change tile
            if (GUI.Button (new Rect (10, 100, 45, 24), "<<")) {
                selectedTileIndex--;
                mode = Mode.PlaceTile;
            }
            if (GUI.Button (new Rect (50, 100, 45, 24), ">>")) {
                selectedTileIndex++;
                mode = Mode.PlaceTile;
            } 
        
            // raise/lower tile
            if (GUI.Button (new Rect (10, 130, 45, 24), "↑")) {
                mode = Mode.RaiseTile;
            }
            if (GUI.Button (new Rect (50, 130, 45, 24), "↓")) {
                mode = Mode.LowerTile;
            } 
            
            if (GUI.Button (new Rect (10, 160, 80, 24), "Place Wall")) {
                mode = Mode.PlaceWall;
            }
            if (GUI.Button (new Rect (90, 160, 80, 24), "Remv Wall")) {
                mode = Mode.PlaceWall;
                selectedWallIndex = -1;
            }
            
            // change tile
            if (GUI.Button (new Rect (10, 190, 45, 24), "<<")) {
                selectedWallIndex--;
                mode = Mode.PlaceWall;
            }
            if (GUI.Button (new Rect (50, 190, 45, 24), ">>")) {
                selectedWallIndex++;
                mode = Mode.PlaceWall;
            } 

            if (selectedTileIndex >= 0) {
                if (map != null) {
                    var sr = selectedFloor.GetComponent<SpriteRenderer> ();
                    sr.sprite = map.floorSpriteForTileIndex (selectedTileIndex);
                }
            }

            //Debug.Log ("selectedWallIndex = " + selectedWallIndex);
            if (selectedWallIndex >= 0) {
                if (map != null) {
                    var sr = selectedWall.GetComponent<SpriteRenderer> ();
                    sr.sprite = map.wallSpriteForTileIndex (selectedWallIndex);
                }
            }
        }
    }

}
