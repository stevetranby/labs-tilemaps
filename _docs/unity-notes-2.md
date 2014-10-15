Notes from May 2014
======================

Can Unity's 3D pathfinding be used while also implementing tile-based world or maps. The world and map should be in 3D. So the map data, or position data, including movement, physics, particle effects, and simulation should all be in 3D (or 4D+ if time or other component(s) are included). The only thing to determine is how to best marry the 2D unity projection with either the Unity A*/Physics/GameObject setup or another similar 3D setup created fully in code.

rambling notes below
----------------------

Everything in the world could have 3d tile coord, and 3d world position. These should then be used to map, if necessary, to the 2D projected view. Obviously the main Unity component that many use is Aron's A* Pathfinder.

*** the only decision to make is what parts of the system can be made invisible or more directly use the other. Can possibly project the info/data of the physical 3d world that is created to test pathfinding onto the 2D plane or directly onto 3d objects by using that 3d data as the direct source to the renderer or mesh components?

### List of resources including Amit's
- http://pavina.net/pathfinding.html

### Show's how 2d map with height just has different costs from tile to tile
- https://www.93i.de/products/software/gamlib-ai

### Reminder
Pathfinding is just for selecting waypoints or the ideal path to get to a goal or target, and Local Steering and Collision Avoidance should be used to avoid obstacles that move around themselves. See second answer to this question regarding moving obstacles.
- instead of trying to use D*Lite or another dynamic pathfinding system
- http://stackoverflow.com/questions/6003235/pathfinding-with-dynamic-obstacles?lq=1

## A* 3D by Roy
- 3d A* post: http://roy-t.nl/index.php/2009/07/07/new-version-a-pathfinding-in-3d/
- unity code: http://forum.unity3d.com/threads/92359-3d-Astar-pathfinding
- blog: http://roy-t.nl/index.php/tag/pathfinding/
- video of sample based on it https://www.youtube.com/watch?v=4OCMCr69BYw

# Basic Aspects of Graph Pathfinding (Search)

- tile grids have a nav graph where each tile is a node
- neighbors are adjacent tiles
- travel to a neighbor costs can include sum of:
   * movement costs (walking over water/fire is higher cost than flat ground)
   * height diff costs (cost could be height diff between neighbor tiles)
   * entity costs (if an entity is present on a tile it's cost may be higher)

## A* Pathfinding
- it's just one of many shortest path graph algorithm
- it can be used for grids, nav meshes, flow charts, travel between airports, etc
- often used with tile grids, but neighbor tiles are still just nodes in a graph and any node that can be reached from an adjacent node becomes a weighted cost edge in the graph
- 3D just uses a 3d heuristic (manhattan dist is just standard right triangle distance A^2+B^2=C^2 where A,B,C are either 2d vectors or 3d vectors)
- however, may be fine to get by with 2d graph if only 1 walkable tile at any coord regardless of height
- what about bridges?

## Unity 3D Physics and Pathfinding

- it's only hybrid in the sense that one would fully use Unity's GUI for development
- otherwise a 3D world is already necessary (even if just simple "height" for z)
- mapping 3D world to 2D projection is not a problem
[rewrite] - using Unity should make this less complex?
- can even have 3D (for 2D) or 4D (for 3D) to add a time dimension to pathfinder (collision avoidance)
- there's no issue having a 3D world that "maps" onto the 2D tile grid

## Mobile 2.5d Physics (simplified 3d)
- how bullet projectiles work in mobile, but without gravity
- how kickback crew works in mobile, with gravity (as a hidden projectile)
- how the flamethrower particle system now works in mobile test

## Pathfinding questions for Different Map Types
- Will there be height differences?
- It's small enough where single layer pathfinding is fine
- Going to use steering or follow path exactly?
- How about two crew on same tile? Sub tile grid so can move a little to the left/right of another crew and pass by
- doors? should they prevent pathfinding when shut? or only if locked?
- could precalculate all paths (not including enemies, assume local avoidance for enemies and other crew)
- Is there a need for real 3D nav mesh style paths? (perhaps only for the high-level path)
- can a single tile have more than 1 height? (say under/on a bridge, tunnel under walkable mountain tile, etc)
- does this require "full" 3D with movements in 360^2 any point on sphere direction or just a more complex "2d" graph where the under bridge tile is a neighbor (edges to) of two tiles and the "on bridge" tile is a neighbor (edges to) of two different tiles?
- tile grid graph or nav mesh? or both
- hierarchical pathfinding necessary? (coarse top layer, fine between coarse waypoints)
- how large will maps be?
- pre calculate all paths if small enough? or for the higher-level graph
- cliffs can be accomplished by having accessible neighbors require maximum height difference (could also be a parameter in case some entities can jump 2 height and some 4)

---- for further reading -----

Note that pathfinding is the straight-forward aspect of AI Movement and pathing mechanics, where pathfollowing (node to node, as waypoints with simple seeking, as waypoints with steering), collision avoidance, local avoidance, grouping, etc are all where the complexity occurs based on having that underlying path.

Graph Search (ie: pathfinding)
DFS/BFS (standard tree graph search)
Dijkstra (lowest cost graph search - better for multi goal)
A* (dijkstra w/heuristic to improve search to single goal)

Variant Aspects or Components of A*
- hierarchical pathfinding, usually a coarse-grained layer to find major waypoints, and a fine-grained layer to find the path between each of these waypoints

Other Algorithms based on A* (links at bottom of A* wikipedia)
http://en.wikipedia.org/wiki/A*_search_algorithm

D*Lite (Iterative A* with a dynamic map having things like enemies moving around)
IDA* ()
