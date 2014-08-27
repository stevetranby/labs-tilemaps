using System.Collections;

// Try to implement without need for Monobehavior/UnityEngine
// Create a visual class instead (implementation using this generic data/algorithm)

namespace ST
{
    /// <summary>
    /// Fog of War. 
    /// Would like to show off various ways to code and display fog of war behavior.
    /// 
    ///
    /// Active - Unit is present and active in area
    /// Visited - Unit has seen, but is not present
    /// Hidden - 
    ///
    /// 1. Fog of war 360 around active units
    ///    - simplistic radial tile mask, either fully visible or linear/exponential decreased visibility further from center
    ///    - can light or add effect to tiles/sprites (like XCOM) or use an alpha mask (like AoM)
    ///    - can improve? by moving the center in front of the player and if desired elongate in the facing direction
    /// 2. Fog of war using Line of Sight
    ///    - use shadow map mask and algorithm to create a cone of visibility instead of radial
    ///    - this may not be desired, and an ellipse sented in front of player may be better visually and for gameplay
    /// 3. Fog of war visual effects 
    ///    - mask with cut outs around active unit's visible areas
    ///    - there will likely be components of the game that maybe should always be visible
    /// 4. Fog of war data model
    ///    - ?combine as bitmask with other properties of map like height, weight?
    ///    - coarse map model where 1,3 might refer to a 8x8 grid of tiles
    ///    - the texture to store the mask definitely must be small, shader used to blur and extend it
    /// 5. Fog of war behavior
    ///    - how should Enemy/NPC units behave if not in active area?
    ///    - what properties or behavior should be in this class to allow for AI or player components to gain insight
    ///
    /// probably should just draw anything on top or after fow if it's meant to not be masked, but zorder is where the mask is
    ///
    /// Near Z
    /// [---- unaffected sprites/meshes -----]
    /// [xxxx FOW xxxxxx]
    /// [.... map ......]
    /// Far Z
    ///
    /// </summary>
    public class FogOfWar
    {   
        // If using simple texture+shader as single quad or a few quads over entire map
        UnityEngine.Texture2D alphaMask;
        UnityEngine.Shader fowShader;
        float fowZOrder; 
    }
}