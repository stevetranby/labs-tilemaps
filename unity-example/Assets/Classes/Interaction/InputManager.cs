using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
		public enum ToolMode {
            Selection, // normal
            Terrain, // mode to change the map structure, heights, tile types, etc
            MetaEdit // mode to change behavior of map, collisions, triggers, etc
        }

        // modify the map
		public enum TerrainMode {
            Smooth,
            Roughen,
            RaiseTile,
            LowerTile,
            RaiseArea,
            LowerArea
        }

        // TODO: not currently useful, but could be used to determine action when input is used
		public enum SelectionMode {
            Unselected,
            EntitySelected,
            GroupSelected
        }

		public enum MetaEdit {
            PlaceCollisions,
            RemoveCollisions,
            Collisions, // if two inputs for place/remove (e.g. left/right mouse button)
            Triggers,
            Objects // place objects (health pack, etc)
        }

		// Input map
		public enum InputAction {
			CameraLeft,
			CameraRight,
			CameraUp,
			CameraDown,
			CameraZoomIn,
			CameraZoomOut,
			UseItem1,
			UseItem2,
			Jump,
			MoveEntityLeft,
			MoveEntityRight,
			MoveEntityUp,
			MoveEntityDown,
		}

		// KeyCode is key so that can ask input if contains and then get action
		public Dictionary<InputAction, KeyCode> keymap;
		public Dictionary<InputAction, Action<bool>> keymapBehavior;

        //
        void SetupDefaultKeymap ()
        {
			// Default Mapping
			keymap [InputAction.CameraLeft]  = KeyCode.W;
			keymap [InputAction.CameraUp]    = KeyCode.A;
			keymap [InputAction.CameraDown]  = KeyCode.S;
			keymap [InputAction.CameraRight] = KeyCode.D;

			// CAMERA
			keymap [InputAction.CameraZoomOut] = KeyCode.Q;
			keymap [InputAction.CameraZoomIn]  = KeyCode.E;

			// ACTIONS
			keymap [InputAction.UseItem1] = KeyCode.Z;
			keymap [InputAction.UseItem2] = KeyCode.X;

			keymap [InputAction.Jump] = KeyCode.Space;
			keymap [InputAction.MoveEntityUp] = KeyCode.UpArrow;
			keymap [InputAction.MoveEntityDown] = KeyCode.DownArrow;
			keymap [InputAction.MoveEntityLeft] = KeyCode.LeftArrow;
			keymap [InputAction.MoveEntityRight] = KeyCode.RightArrow;
        }
    
		// TODO: slow this update down to 1/10 or so
        void Update ()
        {
			// check for keys, take action if behavior exists
        }
    }
   
}