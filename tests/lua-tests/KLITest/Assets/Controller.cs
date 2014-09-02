using UnityEngine;
using LuaInterface;

public class Controller : MonoBehaviour
{
	Lua _lua;
	public string Code { get; set; }
	
	void Start()
	{
		_lua = new Lua();
		_lua.DoString("UnityEngine = luanet.UnityEngine");
		_lua.DoString("System = luanet.System");
		_lua["gameObject"] = this;
		
		Code = "function update(dt)\n\nend\n";
		
		DoCode(Code);
	}
	
	void FixedUpdate()
	{
		_lua.DoString(string.Format("update({0})", Time.deltaTime));
	}
	
	public void DoCode(string code)
	{
		_lua.DoString(code);
	}
}
