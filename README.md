Tilemaps
========

Various example code and research for working with orthogonal, isometric, hexagonal tilemaps.

Some of the ideas currently in the Unity-Isometric example were developed based on the Voxel rendering in Unity tutorial series.
http://studentgamedev.blogspot.com/p/unity3d-c-voxel-and-procedural-mesh.html

Note: unfinished, ideas currently in comments will be fleshed out into tutorials or posts.

BUGS (to be fixed before features)
==================================
- chunk size should be >= mapZ, zOrdering/positioning is off otherwise (didn't notice, always testing with 1 chunk in map's data z-axis)
- Entity is trying to moving on diamond isometric, but map is in staggered format
- Refactor the chunking code to actually be suitable for tilemap game, right now it's mostly a test case for efficient instantiation and rendering

TODO
====
- Comment TODOs, move any necessary here if action cannot be taken effectively
- Look into various components on Asset Store for doing any or all tasks of any example.
- Test different approaches regarding code-only, unity-editor-only, and hybrid
- Test using unity's Sprite systems, and without.
- Test using cocos2d-js both WebGL and Canvas renderers
- Test performance of map sizes, chunk sizes, and chunk layout
- Discuss options for hit testing both using physics, colliders, and raycasts, as well as using general algorithms and pure math instead whether by choice or necessity.
- Discuss rendering options regarding Meshes vs GameObjects/Sprites, pros/cons, gotchas
- Discuss making generic map interface that can have plugin concrete classes for any time whether orthogonal, isometrcic, diametric, nav mesh behaving as grid, etc

- Look into whether nav mesh discussion makes sense in this context. It's much better suited for games architected in 3D, even if the camera perspective is 2D or fixed.

Open Assets
===========
- http://opengameart.org/sites/default/files/goblin_shaman.png
- http://www.lostgarden.com/2007/05/dancs-miraculously-flexible-game.html
- http://opengameart.org/content/isometric-tiles-cubes

Resources (Either as research or for reference writing tilemap tutorials)
=========

Iso Tile Picking
- http://www.xnaresources.com/default.asp?page=Tutorial:TileEngineSeries:7

Isometric "2D" rendering using actual 3D cubes
- mapping top,front-left,front-right faces with correct UV coordinates
- http://www.gamedev.net/topic/629496-dynamic-objects-in-isometric-map-drawing-algorythms/

Hex
- http://www.settworks.com/pages/tools/hexkit

Behavior
- http://angryant.com/

Pathfinding, Following, Steering, etc
- Open Steer
- AStarPathfinder Aron ?Garber?
- Unity's Nav Mesh system

Mini Maps Examples
- http://learningsc2.com/wp-content/uploads/2011/06/Minimap-300x275.jpg
- http://www.wsgf.org/f/u/imagecache/node-gallery-display/contrib/dr/119/ingame_16x9.jpg
- http://cdn3.raywenderlich.com/wp-content/uploads/2013/02/tilegame_minimap.png
- http://www.sfml-dev.org/tutorials/2.0/images/graphics-view-minimap.png
