Procedural Generation
----------------------

## Noise Algorithms
Noise Types - perlin, classic, fractal, simplex
https://github.com/ashima/webgl-noise

## Rougelike Dungeon Generators
http://kuoi.org/~kamikaze/GameDesign/art07_rogue_dungeon.php
http://pcg.wikidot.com/pcg-algorithm:dungeon-generation

## Map Generation (terrain, biomes, rivers, etc)
http://www.pixelenvy.ca/wa/river.html


Navigation/Pathfinding
----------------------

- canonical resource: http://theory.stanford.edu/~amitp/GameProgramming/
- http://www.gamedev.net/page/resources/_/technical/artificial-intelligence/

## Heirarchical
- for map sizes greater than 100x100 tiles
- possibly use only course nav mesh with sterring for fine-grained nav
- break any path into two parts starting from start and goal nodes (meet in middle)

## Nav Meshes
- potentially excessive for small tile grids
- calculate on map generation
- with pregenerated maps can tweak the m

## AStar A* (standard)
- easily written, but plenty of other's to use or start from
- 2d maps, can easily move to higher-dimensional grids

### Temporal extension (extra dimension for time to prevent future collisions)
- http://allseeing-i.com/ASIPathFinder
- based on PDF whitepaper

### AStar w/ Jump Point Search
- need to allow diagonal movement
- can be many times faster
- http://harablog.wordpress.com/2011/09/07/jump-point-search/

### AStar3D CSharp
- http://roy-t.nl/index.php/2011/09/24/another-faster-version-of-a-2d3d-in-c/


Path Following
--------------

## Collision Avoidance
- door ways? tricky, temporal extension can help, steering behaviors for queuing can help or work
- entities should ask other entities to move out of the way (possibly same team only)
- entities should be defined as whether can pass through other objects
- possibly shrink bounding boxes so they may appear to pass through/by closely, but still allowed

## Steering Behaviors
- works best when objects can move in full 360 directions
- can you use "underneath" for 2d Ortho/Isometric?
- http://opensteer.sourceforge.net/
- http://www.red3d.com/cwr/steer/
- how best to use with grid based movement (4 iso directions)?
- probably just snap the 2d/3d vector to specific quadrants
- make sure oscillations don't occur (snap NE->NW->NE each frame)

* http://roy-t.nl/index.php/2009/12/29/swarm-like-collision-avoidance-path-finding-and-smooth-following/

--------------

## Misc Isometrics
- http://mhframework.blogspot.co.uk/2013/09/old-vs-new-tile-map-systems.html
- http://clintbellanger.net/articles/isometric_math/

### Paper on isometric staggered with game architecture
http://fivedots.coe.psu.ac.th/~ad/jg/ch064/ch6-4.pdf

### Staggered
http://gamedev.stackexchange.com/questions/45103/staggered-isometric-map-calculate-map-coordinates-for-point-on-screen
