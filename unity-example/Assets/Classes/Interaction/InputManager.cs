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

        private Vector3 prevMousePosition;

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

        /// <summary>
        /// Template for notes on reacting to directional control input for movement.
        /// 4 or 8 directions - DPAD, WASD, Arrows, Analog Sticks clamped 
        /// 2-Axis - one axis for rotation, other for movement
        /// 2-Axis Weapon - one axis for movement, other for rotational aiming of weapon
        /// </summary>
        void CheckInputDirectionalMovement()
        {
            float dx = Input.GetAxis("Horizontal");
            float dy = Input.GetAxis("Vertical");            
            Vector2 heading = new Vector2(dx, dy);
            heading.Normalize();
                      
            // Using standard, simple, or custom character controller
            //characterController.Move(heading);
        }

        void CheckInputMouseMovement()
        {
            // TODO: make sure either tUnits works instead or definitely calulate based on device screen size and of course fps or deltatime
            //float scalerX = 100f / Screen.width;
            float dx = Input.mousePosition.x - prevMousePosition.x;
            float dy = Input.mousePosition.y - prevMousePosition.y;
        }

        /// <summary>
        /// Template for notes on reacting to touch input for movement.
        /// Tracking movement by moving toward touch based on vector between touch and active player character or unit
        /// </summary>
        void CheckInputTouchMovement()
        {
            //check for touches
            if (Input.touchCount > 0)
            {
                // TODO: get main camera
                Camera camera = null;
                Vector2 touchPosition = Input.GetTouch(0).position;
                Vector3 touchWorldPosition = camera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 15));

                // TODO: need access to currently active/selected character (player, unit, entity, controllable NPC, etc)
                GameObject currentCharacterGO = null;
                Vector3 characterPosition = currentCharacterGO.transform.position;

                //vector math says point to get to - current position = heading.
                float dx = touchWorldPosition.x - characterPosition.x;
                float dy = touchWorldPosition.y - characterPosition.y;
                Vector2 heading = new Vector2(dx, dy);
                heading.Normalize();
            }
        }
    }
   
}