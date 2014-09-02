[Steve's Notes]

# Lua Integration with Lua

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
