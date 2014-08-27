Tilemaps
========

Various example code and research for working with orthogonal, isometric, hexagonal tilemaps.

Some of the ideas currently in the Unity-Isometric example were developed based on the Voxel rendering in Unity tutorial series.
http://studentgamedev.blogspot.com/p/unity3d-c-voxel-and-procedural-mesh.html

Note: unfinished, ideas currently in comments will be fleshed out into tutorials or posts.

BUGS (to be fixed before features)
==================================
- chunk size should be >= mapZ, zOrdering/positioning is off otherwise (didn't notice, always testing with 1 chunk in map's data z-axis)

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
