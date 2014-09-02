using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using System.IO;
using System.Text;

public class OGALog
{
		public void Log (string msg)
		{
				Debug.Log ("msg: " + msg);
		}
}

// TODO: get config info or path info of current app, look for folder /_Modding/ in Assets
public class OGALuaManager : MonoBehaviour
{
		Lua L;
		string extScript;

		string dirMods = "/_Modding/";

		double angle = 0.0;	
		double angle3 = 0.0;

		void Start ()
		{
				Debug.Log (Application.dataPath);
				FileInfo theSourceFile = null;
				TextReader reader = null;  // NOTE: TextReader, superclass of StreamReader and StringReader

				// Read from plain text file if it exists
				string filename = "test.lua";
				theSourceFile = new FileInfo (Application.dataPath + dirMods + filename);
				if (theSourceFile != null && theSourceFile.Exists) {
						reader = theSourceFile.OpenText ();  // returns StreamReader
				} else {
						// try to read from Resources instead
						TextAsset data = (TextAsset)Resources.Load (dirMods + filename, typeof(TextAsset));
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
				OGALog log = new OGALog ();
				L.RegisterFunction ("dlog", log, log.GetType ().GetMethod ("Log"));

				Debug.Log ("1");
				// run external script to load into lua state machine
				L.DoString (extScript);
				Debug.Log ("2");
				// showing dlog is registered (we already should know from external script's dlog
				L.DoString ("dlog('calling dlog() from lua')");	
				Debug.Log ("3");
				// created myCube1 inside the lua external script, this isn't a smart 
				// example as either should know about the instance in one place or
				// another, or know what th name of the field in a metatable would 
				// be by convention
				//L.DoString ("myCube1 = setup_cube(cube)");
				L.DoString ("myCube2 = create_cube()");
				L.DoString ("myCube3 = create_cube()");
				Debug.Log ("4");
				
				// test some cross script to C# stuff
				Vector3 v = new Vector3 (0f, 0f, 0f);
//				GameObject go = L ["myCube1"] as GameObject;
//				go.transform.position = v;

				GameObject go3 = L ["myCube3"] as GameObject;
				v.x -= 2.0f;
				go3.transform.position = v;
				go3.renderer.material.color = new Color (1.0f, 1.0f, 0.2f);

				Debug.Log ("5");
				GameObject go2 = L ["myCube2"] as GameObject;
				v.x += 4.0f;
				go2.transform.position = v;
				go2.renderer.material.color = new Color (1.0f, 1.0f, 0.2f);
				//L.DoString ("myCube2.renderer.material.color = luanet.UnityEngine.Color (1.0f, 0.2f, 0.2f)");
				Debug.Log ("6");
		}
	
		void Update ()
		{
				if (L != null) {

//						// Can't currently use transform.Rotate in kopilua		
//						L.DoString ("update_cube_rotation(myCube1)");

						// rotate 3x for cube #1, 2x for cube #2, and 1x for cube #3
						// running lua commands from C# into the current lua 
						angle += 1.0;
						L.DoString ("update_cube_rotation(myCube1, " + angle + ")");

						GameObject go2 = L ["myCube2"] as GameObject;
						go2.transform.Rotate (Vector3.right, 2.0f);

						angle3 += 3.0;
						L.DoString ("update_cube_rotation(myCube3, " + angle3 + ")");
				}
		}
}
