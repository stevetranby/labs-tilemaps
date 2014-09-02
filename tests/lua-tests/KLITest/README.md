KLITest
=======

This is a demo showing one way to use KopiLuaInterface within Unity to
allow user scripting.

Please see http://gfootweb.webspace.virginmedia.com/LuaDemo/ for more 
information and a live instance of the demo.

Assemblies
==========

The project includes precompiled assemblies for the Lua libraries - if you
want to use them in your own projects, read the README file in the Assets/DLL 
directory, and pay attention to the COPYRIGHT file too.

Scenes
======

Controller.unity
----------------

This is the scene that the WebPlayer runs.

Within the editor it's more awkward to get custom Lua code in, but if you
select the "controller" object in the heirarchy then you can use the inspector
to enter some code and press the "Apply" button in the inspector to submit the
new code.

The way Lua is integrated here is far too open for most real world purposes -
there's no sandbox around the Lua code, it can bind to any loaded .NET assembly
and call whatever methods it likes.  Certainly for game purposes you'd want to
disable most of that, and be selective about what you let users access.  Still,
it's really neat that LuaInterface lets us do so much.

KLITest.unity
-------------

This scene is just a test really - there's a sphere in the scene from startup,
which changes colour to red during initialization and later to green if Lua
seems to be working properly.  It also has a UnityScriptTest component
attached, which checks (barely) that everything is working fine from
UnityScript too.

This isn't how I really test KopiLua - the KopiLua repo on my GitHub has much
better test suites.

License
=======

As I understand it, KopiLua and LuaInterface are both licensed under the 
MIT public license.  See the COPYRIGHT file for full details.

All KLITest-specific code should be considered public domain - you can do 
whatever you like with it.

Contact
=======

You can contact me as gfoot on the Unity forums, or george.foot@gmail.com.

