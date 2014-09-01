using System.Collections;
using UnityEngine;

namespace ST
{
    /// <summary>
    /// Line of sight utilities, including field of view (shadow casting).
    ///
    /// http://www.roguebasin.com/index.php?title=Field_of_Vision
    ///
    /// Permissive "Supercover" Algorithm
    /// http://lifc.univ-fcomte.fr/~dedu/projects/bresenham/index.html
    /// 
    /// libfov - lots of options and flags to determine how fov behaves
    /// https://code.google.com/p/libfov/
    /// 
    /// C# tutorial for shadow casting
    /// http://blogs.msdn.com/b/ericlippert/archive/2011/12/12/shadowcasting-in-c-part-one.aspx
    ///
    /// Height map with LoS hiding based on terrain height
    /// http://williamedwardscoder.tumblr.com/post/13269950091/a-while-ago-i-was-playing-with-computing
    ///
    /// Various shadow types based on line collisions
    /// http://www.roguebasin.com/index.php?title=Comparative_study_of_field_of_view_algorithms_for_2D_grid_based_worlds
    /// 
    /// The algorithm(s) are generic for the actual line of sight and field of view
    /// the specific map type will need custom additions since it and its tile properties will vary
    ///
    /// 
    /// # Bookmarks
    /// http://unity3d.com/learn/tutorials/projects/stealth/enemy-sight
    /// 
    /// TODO:
    /// - cache parent as entity
    /// - possibly split into static methods and instance methods/data
    /// </summary>

    public class SightComponent : MonoBehaviour
    {
        // TODO: 
        int visionTileRadius = 5;
        float visionRadius = 5f;

        // TODO: Create Interface for line of sight and field of view to allow for inserting custom implementation
        // Either implement your own Bresenham's line algorithm or use a C# open source class/library

        // permissive - allows for a wider line for testing, "see through" small obstacles or corners
        // TODO: possibly create as extension method on a Tile class or Entity
        bool TileVisibleFromTile (Map map, TileCoord tile1, TileCoord tile2, bool permissive)
        {
            // TODO: pass in position instead or convert tile to position
            var pos1 = map.worldFromTile(tile1);
            var pos2 = map.worldFromTile(tile2); 
            var layerMask = LayerMask.NameToLayer("entities");
            if (! Physics.Linecast (pos1, pos2, layerMask)) 
            {
                // if colliders are working, possibly use Unity's system instead
                // target is in sight
            }

            bool notAtDestination = true;
            while (notAtDestination) {
                // step through line algorithm or shadow map
                // if collision, break out early
                // NOTE: collision might want to use a cost or a permissivity property of tile 
                // if not collision, yield/generate/next on algorithm and continue
                notAtDestination = false;
            }
            return false;
        }

        // Move to Entity.cs
        // Use the line of sight algorithm to check, but first make sure they're facing the entity
        // This prevents player from being able to see enemies not in sight line
        // Again this is specific to a game's design and desired gameplay
        // Can also use this for enemies to detect if they should attack or take evasive action, for example.         
        bool Entity_CanSeeEntity (Map map, Entity otherEntity)
        {   
            // NOTE: Always check the least expensive things first and bail :D

            // is enemy close enough?
            Vector3 myPosition = new Vector3 (0, 0, 0); //this.position;
            Vector3 otherPosition = new Vector3 (0, 0, 0); //otherEntity.position;
            float dist = Vector3.Distance (myPosition, otherPosition);
            float visibilityRadius = 10f; // this.visibilityRadius
            if (dist > visibilityRadius) {
                return false;
            }

            // TODO: first check if enemy is in active area
            if (map.TileSeen (otherEntity.curTile.c, otherEntity.curTile.r) > 0) {
                return false;
            }

            Vector3 myDirectionVector = new Vector3 (0, 0, 0); //this.directionVector;
            Vector3 directionVectorToEnemy = new Vector3 (0, 0, 0); // vectorFromPositions(this.position, otherEntity.position);

            // TODO: should check angle between, but can technically use dotproduct result
            // since dot product is 0 at 90 degree angle diff and 1.0 at 0 degree angle diff (with normalized vectors)
            const float dotProductFOV90 = 0.71f; // cos(45deg) 45 left or 45 right
            //const float dotProductFOV120 = 0.87f; // cos(30deg)
            //const float dotProductFOV60 = 0.50f; // cos(60deg)
            // need UnityEngine for Vector3 dotproduct :(
            var dotProduct = Vector3.Dot (myDirectionVector.normalized, directionVectorToEnemy.normalized);
            if (dotProduct < dotProductFOV90) {
                return false;
            }

            // or should probably use Unity's capabilities and use angle method directly instead
            // (it prob uses dot product internally)
            if (Vector3.Angle (myDirectionVector, directionVectorToEnemy) > 45) {
                // I'm looking at entity, let's see if anything is in between us and the entity
                var tile1 = new TileCoord (0,0); //this.currentTile;
                var tile2 = new TileCoord (0,0); //otherEntity.currentTile;
                if (TileVisibleFromTile (map, tile1, tile2, false)) {
                    return true;
                }
            }

            return false;
        }
    }
}