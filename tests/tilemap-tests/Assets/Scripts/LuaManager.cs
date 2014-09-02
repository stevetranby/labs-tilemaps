using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using System.IO;
using System.Text;

public class MyLog
{
	public void Log (string msg)
	{
		Debug.Log ("msg: " + msg);
	}
}

// TODO: get config info or path info of current app, look for folder /_Modding/ in Assets
public class LuaManager : MonoBehaviour
{
	Lua L;
	string extScript;

	string dirName = "/Lua/";

	void Start ()
	{
		Debug.Log (Application.dataPath);
		FileInfo theSourceFile = null;
		TextReader reader = null;  // NOTE: TextReader, superclass of StreamReader and StringReader
		
		// Read from plain text file if it exists
		string filename = "test.lua";
		theSourceFile = new FileInfo (Application.dataPath + dirName + filename);
		if (theSourceFile != null && theSourceFile.Exists) {
			reader = theSourceFile.OpenText ();  // returns StreamReader
		} else {
			// try to read from Resources instead
			TextAsset data = (TextAsset)Resources.Load (dirName + filename, typeof(TextAsset));
			reader = new StringReader (data.text);  // returns StringReader
		}

		// read script into 'extScript' var
		if (reader == null) {
			Debug.Log ("test.lua not found or not readable");
		} else {
			// Read each line from the file/resource
			string line = "";
			List<string> scriptLines = new List<string> ();
			while ((line = reader.ReadLine()) != null) {
				scriptLines.Add (line);
			}
			scriptLines.Add ("");
			extScript = string.Join ("\n", scriptLines.ToArray ());
			Debug.Log ("extScript.length = " + extScript.Length);
		}

		L = new Lua ();
			
		// register a c# class method with lua to have access in lua scripts
		MyLog log = new MyLog ();
		L.RegisterFunction ("dlog", log, log.GetType ().GetMethod ("Log"));
	}
	
	void Update ()
	{
		if (L != null) {
			// call script, run script, reload script, etc
		}
	}
}
