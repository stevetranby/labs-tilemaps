using System.Collections;
using System.Collections.Generic;
//TODO: can we not use this. 
//using UnityEngine;

namespace ST 
{
    /// <summary>
    /// Game State and Controller
    /// 
    /// Example Game will be simple RTS
    /// 
    /// TODO:
    /// - Should probably have a root game object, unless only one map exists at all times
    /// - and then keep data and Monobehavior separate if possible
    /// - Can we only have one map?
    /// - Should we show support for multiple maps? 
    /// - Maybe we could have multiple "maps" for loading portions of massive maps 
    /// - Do we want to show how to stream tilemaps from file(s) or server?
    /// 
    /// Open Royalty Free Asset Resources:
    /// - http://opengameart.org/content/isometric-64x64-medieval-building-tileset
    /// </summary>
    public class Game {
             
        private Map map;
        private List<Map> maps;
        private EntityManager entityManager;

        void Start () {
            //currentMap = new Map();
            //currentMap = GameObject.Find("GameMap");
        }
        
        void Update () {
        
        }
    }
}