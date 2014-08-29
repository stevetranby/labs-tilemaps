using UnityEngine;
using System.Collections;

namespace ST
{
    /// <summary>
    /// Mini map.
    /// 
    /// TODO:
    /// - look at asset store options 
    ///   http://u3d.as/content/kolmich-creations/kgfmap-system-minimap-/3bf
    /// - possibly use GUI system (shows examples for drawing units)
    ///   http://wiki.unity3d.com/index.php?title=Radar
    /// - follow entity around, or follow player's chosen current location in map
    /// - should have mask pulled from FoW data
    /// 
    /// Types of Mini Maps
    /// - static, fog of war, shows bounding primative showing bounds of current main camera 
    /// - "scrolling" top-down small viewport into part of the 3D map (more useful in 3d, or large maps)
    /// - radar with unit blips, but no actual map
    /// - map represenation, not a camera view, with player location or units or bounding primative
    /// 
    /// 2nd camera??
    /// - works okay, but it will require deciding on what functionality is desired
    /// - it should show rect of main camera viewport
    /// - it should show player units
    /// - it should show enemy units in active areas
    /// </summary>
    public class MiniMap : MonoBehaviour
    {
        public bool usingCameraForMiniMap = true;
        // For GUI texture mini map
        public RenderTexture texture = null;
        //public Rect viewPortRect = null;

        // TODO: should make private and set this.gameObject equivalent to cache gameObject
        public Camera minimapCamera;

        void Start ()
        {
            // if texture or other flag is set use texture
            if (minimapCamera != null) {
                SetupCameraMiniMap ();
            } else {
                usingCameraForMiniMap = false;
                SetupTextureMiniMap ();
            }
        }

        void Update ()
        {
            if (usingCameraForMiniMap) {
                UpdateCameraMiniMap ();
            } else {
                UpdateTextureMiniMap ();
            }
        }

        /// <summary>
        /// TODO:
        /// - mask camera and support overlay "frame" to separate with a "border"
        ///   http://answers.unity3d.com/questions/757631/masked-advanced-minimap.html
        /// </summary>
        void SetupCameraMiniMap ()
        {
            // find 2nd camera
            // set camera's viewport to say 0.8,0.8,0.2,0.2
            // wrap inside GUI texture for frame

            // for now this is setup in Unity Editor           
        }

        void UpdateCameraMiniMap ()
        {
            // test to change position of main camera by click in minimap
            if (Input.GetMouseButtonDown (0) && minimapCamera.pixelRect.Contains (Input.mousePosition)) {            
                RaycastHit hit;
                Debug.Log ("Step1");
                Ray ray = minimapCamera.ScreenPointToRay (Input.mousePosition);
                Debug.Log ("Step2");
                if (Physics.Raycast (ray, out hit)/* && hit.transform.name=="MinimapBackground"*/) {
                    minimapCamera.transform.position = hit.point;
                    Debug.Log ("Step3");
                    // hit.point contains the point where the ray hits the
                    // object named "MinimapBackground"
                    Debug.Log (hit.point);
                }
                //minimapCamera.transform.position = Vector3.Lerp(minimapCamera.transform.position, hit.point, 0.1f);
            }
        }

        void SetupTextureMiniMap ()
        {
            
            //            // duplicate the original texture and assign to the material
            //            var texture : Texture2D = Instantiate(renderer.material.mainTexture);
            //            renderer.material.mainTexture = texture;
            //            
            //            // colors used to tint the first 3 mip levels
            //            var colors = new Color[3];
            //            colors[0] = Color.red;
            //            colors[1] = Color.green;
            //            colors[2] = Color.blue;
            //
            //            // TODO: we should remove mipmap support for the mini map, unless we're going to scale it
            //            var mipCount = Mathf.Min( 3, texture.mipmapCount );
            //            
            //            // initial setup or test minimap
            //            for( var mip = 0; mip < mipCount; ++mip ) {
            //                for( var i = 0; i < cols.Length; ++i ) {
            //                    cols[i] = ;
            //                }
            //                texture.SetPixels( cols, mip );
            //            }
            //            
            //            // actually apply all SetPixels, don't recalculate mip levels
            //            texture.Apply( false );
        }

        void UpdateTextureMiniMap ()
        {
            // TODO: update minimap's pixel per tile bitmap (or pixel per chunk)
        }
    }              
}