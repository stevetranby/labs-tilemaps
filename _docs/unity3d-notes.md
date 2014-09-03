:steve

Unity3D Notes
----------------------

- prob should go with unity pro, but double check the costs associated
- start by researching useful assets and plugins and components from unity store

## What's new in 4.3
http://unity3d.com/unity/whats-new/unity-4.3

## Using Source Control
- obviously could use unity pro with asset server:  http://docs.unity3d.com/Documentation/Manual/AssetServer.html
- store code/config/assets in a separate repository, clone to working unity directory
- check for differences when building on Win/OSX/Linux
- one possibility is to store as file.ext.osx / file.ext.win (and then sym link or just copy the platform file to file.ext)
- Plastic SCM could be an option (caters more to binary files)
- http://docs.unity3d.com/Documentation/Manual/ExternalVersionControlSystemSupport.html

### Ignore Folders/Files
- obviously standard platform for C#/UnityScript/Win/OSX (e.g. ".DS_store")
- Library folder changes a ton, so setup project and ignore that folder
- possibly store heavy binary files (assets) in another repo, or even dropbox
- store all "final" assets used in the game of course
- look at using placeholders to limit number of times large binaries change
* may need to go and change "hidden meta files" to "visible meta files
- http://stackoverflow.com/questions/18225126/how-to-use-git-for-unity3d-source-control
- http://stackoverflow.com/a/21725152/415 (same as previous, linked to specific answer)

## Separate Asset process from Code/Script process and source control
- some advice, like SC:Mobile it makes sense to keep art development separate. manually, or automated clone with scripts by one dev into the project /assets/ folder once art is deemed ready. This doesn't mean that it must never change in the future, just that we should deem art files as ready with limited frequency. An even better idea is to use temp art in the same format, resolution, size, etc. Then swap in when it's ready in a similar manner that hopefully prevents a lot of incremental updates.
- see: http://www.bitsalive.com/unity-3d-and-version-control/

* Assets can continue to use dropbox or if desired or another solution as determined by the art team.

## Separate Core "game engine" from Scripts
- store scripts say in ./project/scripts/{paths,to,all,lua,json,scripts,config,etc}
- store unity project in ./project/unity/{Assets,Library,etc}
- create two repositories one for each of the two paths above
- probably use above remove library directory method
- check how well git or another dvcs works with ./project/unity/ repo
- compress ./project/unity/ and store checkpoints for backup purposes

## Sprite Packing
- for later on in polishing, but good to get workflow down now
- figure out texture formats (pixelated, but small as possible, even for desktop)
- asset serialization? I've read force text, but others use mixed
- sprite packing should be used to produce fewer texture atlases

## 2D code-first framework similar in design to cocos2d
https://github.com/MattRix/Futile

## The game event loop and core architecture info
http://docs.unity3d.com/Documentation/Manual/ExecutionOrder.html

## Unity Tutorials
- https://unity3d.com/learn/tutorials/modules

## Social Services
- http://prime31.com/

## Scripting Utility Classes or Libraries/Frameworks
https://github.com/mortennobel/UnityUtils

## Animation and Animator State Machine
- http://docs.unity3d.com/Manual/StateMachineBasics.html
- http://www.codeproject.com/Articles/813160/Mastering-Unity-D-Game-Development-AI-and-State-ma

## Asset Store
- NGUI http://www.tasharen.com/?page_id=140
- Sprite Manager http://www.anbsoft.com/middleware/sm2/
- A*: http://www.arongranberg.com/astar/
- RAIN AI: http://rivaltheory.com/rain/
--> Behavior Trees & navigation
- Simple Path: http://u3d.as/content/alex-kring/simple-path/1QM

# Unity Answers of possible Usefulness
http://forum.unity3d.com/threads/223030-Best-Easiest-way-to-change-color-of-certain-pixels-in-a-single-Sprite
http://forum.unity3d.com/threads/212307-How-to-change-sprite-image-from-script
http://forum.unity3d.com/threads/82265-Pixel-Perfect-and-Clean-Textures
