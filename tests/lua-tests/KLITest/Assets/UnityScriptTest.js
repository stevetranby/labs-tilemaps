#pragma strict

import LuaInterface;

function Start () {
    var L = Lua();
    L.DoString("x = 'KopiLuaInterface works from UnityScript too'");
    Debug.Log(L["x"]);
}