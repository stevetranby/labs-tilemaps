using UnityEngine;
using System.Collections;

namespace ST
{
    /// <summary>
    /// Input manager.
    /// 
    /// TODO:
    /// - move all keypresses here
    /// - create mappings for various inputs (keys -> actions, mouse buttons -> actions, drag/hover -> actions)
    /// - setup modes for UI interaction 
    /// - make sure if too much is added here for a "game editing" mode, refactor into multiple classes
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        enum ToolMode {
            Selection, // normal
            Terrain, // mode to change the map structure, heights, tile types, etc
            MetaEdit // mode to change behavior of map, collisions, triggers, etc
        }

        // modify the map
        enum TerrainMode {
            Smooth,
            Roughen,
            RaiseTile,
            LowerTile,
            RaiseArea,
            LowerArea
        }

        // TODO: not currently useful, but could be used to determine action when input is used
        enum SelectionMode {
            Unselected,
            EntitySelected,
            GroupSelected
        }

        enum MetaEdit {
            PlaceCollisions,
            RemoveCollisions,
            Collisions, // if two inputs for place/remove (e.g. left/right mouse button)
            Triggers,
            Objects // place objects (health pack, etc)
        }

        //
        void Start ()
        {
    
        }
    
        void Update ()
        {
    
        }
    }
   
}