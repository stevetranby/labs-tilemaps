using UnityEngine;
using System.Collections;

namespace ST
{
    public class OGAGame : MonoBehaviour
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
        public GameObject selectedTile;
        public int floorTileWidth;
        public int floorTileHeight;
        public int wallTileWidth;
        public int wallTileHeight;
        public bool showTileGround;
        public float playerFeetOffset;
        private OGAMap map;
        private OGAEntity player;
        private int selectedTileIndex = 1;
        private int selectedWallIndex = 1;
        private Rect guiPanelRect = new Rect (10, 10, 120, 200);
        private bool mouseIsDown = false;

        enum Mode
        {
            MovePlayer,
            PlaceTile,
            ResetTile,
            RaiseTile,
            LowerTile,
            PlaceWall
    }
        ;

        private Mode mode = Mode.MovePlayer;

        // Use this for initialization
        void Start ()
        {
            map = new OGAMap ();
            map.initFloor (floorTileset, floorTileWidth, floorTileHeight, showTileGround);
            // TODO: implement wall placement as collision objects
            map.initWalls (wallTileset, wallTileWidth, wallTileHeight);
            map.generateMap (floorLayer);

            player = new OGAEntity (map, playerFeetOffset);
            player.Start (playerGameObject);
        }
    
        // Update is called once per frame
        void Update ()
        {
            var fps = 1.0f / Time.deltaTime;
            if (fps < 2) {
                Debug.Break ();
            }

            player.update ();


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
                    map.changeTileIndex (tileCoord, selectedTileIndex);
                } else if (mode == Mode.PlaceWall) {
                    // TODO: map.changeWallIndex (tileCoord, 0);
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
                    player.setGoalTile (tileCoord);
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
        }

        void OnGUI ()
        {
            // Make a background box
            GUI.Box (guiPanelRect, "Loader Menu");

            if (GUI.Button (new Rect (10, 40, 100, 24), "Move Player")) {
                mode = Mode.MovePlayer;
            }

            if (GUI.Button (new Rect (10, 70, 100, 24), "Place Tile")) {
                mode = Mode.PlaceTile;
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
            
            if (GUI.Button (new Rect (10, 160, 100, 24), "Place Wall")) {
                mode = Mode.PlaceWall;
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
                var sr = selectedTile.GetComponent<SpriteRenderer> ();
                sr.sprite = map.spriteCreateForTileIndex (selectedTileIndex);
            }
        }
    }

}
