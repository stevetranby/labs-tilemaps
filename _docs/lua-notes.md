# Unity Lua Integration

## KPI LuaInterface [link needed]

### Setup
- Drop the KPILua folder into the Assets/ folder.
- .mdb and .pdb are the metadata and symbols for debugging purposes
* This should contain KopiLua.dll, KopiLuaDll.dll, KopiLuaInterface.dll

### Other Resources
- http://lua-users.org/wiki/LuaImplementations
- https://github.com/xebecnan/UniLua ("pure" implementation)
- http://www.pixelcrushers.com/dialogue_system/manual/html/lua.html

### Notes
- Need to test if this works on Android/iOS/Web builds.
- Currently runs on OSX and Win7/Win8

### TODO
- reload lua files on file system file changed notification??
- reload lua files on button click
- actually load and call the lua file when spawning new entity


## Learning/Reference
http://learnxinyminutes.com/docs/lua/
http://www.lua.org/
http://www.lua.org/manual/5.2/
http://lua-users.org/wiki/LuaImplementations

## Unity3D Lua Interface (Kopi Lua)
- https://github.com/NLua/KopiLua
- https://github.com/gfoot/kopiluainterface
- http://www.gfootweb.webspace.virginmedia.com/LuaDemo/

## C/C++ Lua Interface

### LuaBind or ToLua++?

## Scripting
- looks like they use a more namespaced method where each lua file has a unique name given to the module, which is more like AMD/Require javascript modules. This is different than the freeform lua scripting done in starbound.

### Entire Game in Lua?

*Love2D*
- simplified framework, but may cater just fine to simple 2D game
- should be able to easily bind a 'C' plugin if necessary for simulation perf
- sample OSX .App package: ./SCG_Love2D_Test_OSX/

*Lua bindings for Cocos2d-x*
- game written in lua could be used for prototyping
- only move back into C++ parts that need to be optimized (profile first)
- scripting could be easier or a mess since you would want to restrict access to features and would have to write an aspect of code in lua or c++ to handle the access to various methods, data, capabilities.

*Redpoint: 3D Engine to write games in Lua*
https://code.google.com/p/redpoint/

### Actual Notes for Implementing Scripting

*Global Lua State:*
- beware of threading issues
- create uniquely named metatables based on object unique ids
- reference "self" object (starbound) or similar to isolate entity instances

*Local Lua State:*
- could create a new lua state for each object instance
- don't want to create more than 1000 lua states (number unknown, it's very lightweight, but would require testing memory/perf loads)

### Games With interesting wikis or source
- http://docs.cryengine.com/display/SDKDOC4/Script+Usage
- http://wiki.roblox.com/index.php/Scripting

### StarBound Scripting Info
- favors data, JSON, over lua for the most part
- limit available functionality, like most game scripting
- makes writing behaviors easier instead of abusing data formats
- still uses data heavily in form of .json

*sample tutorial:* http://community.playstarbound.com/index.php?threads/lua-mod-tutorial-1-simple-spawner.44035/
