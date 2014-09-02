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

public class KLITest : MonoBehaviour
{
    Lua L;
    string[] extScript;
    double elapsedTime;

    void loadScript()
    {
        Debug.Log (Application.dataPath);
        FileInfo theSourceFile = null;
        TextReader reader = null;  // NOTE: TextReader, superclass of StreamReader and StringReader
        
        // Read from plain text file if it exists
        string filename = "test.lua";
        theSourceFile = new FileInfo (Application.dataPath + "/../../" + filename);
        if (theSourceFile != null && theSourceFile.Exists) {
            reader = theSourceFile.OpenText ();  // returns StreamReader
        } else {
            // try to read from Resources instead
            TextAsset puzdata = (TextAsset)Resources.Load ("puzzles", typeof(TextAsset));
            reader = new StringReader (puzdata.text);  // returns StringReader
        }
        
        if (reader == null) {
            Debug.Log ("puzzles.txt not found or not readable");
        } else {
            // Read each line from the file/resource
            string line = "";
            List<string> scriptLines = new List<string> ();
            while ((line = reader.ReadLine()) != null) {
                scriptLines.Add (line);
                //Debug.Log ("-->" + line);
            }
            scriptLines.Add ("");
            extScript = scriptLines.ToArray ();
            //Debug.Log ("extScript.length = " + extScript.Length);
        }
    }

    void setupLua()
    {
        L = new Lua ();
        
        gameObject.renderer.material.color = new Color (1.0f, 0.8f, 0.2f);
        
        L.DoString ("r = 0");
        L.DoString ("g = 1");
        L.DoString ("b = 0.2");
        
        float r = (float)(double)L ["r"];
        float g = (float)(double)L ["g"];
        float b = (float)(double)L ["b"];
        
        gameObject.renderer.material.color = new Color (r, g, b);
        
        Debug.Log ("1");
        
        L ["go"] = gameObject;
        
        Debug.Log ("2");
        
        Vector3 v = gameObject.transform.position;
        v.y += 1.0f;
        L ["v"] = v;
        
        Vector3 w = gameObject.transform.position;
        w.y += 1.0f;
        w.x += 1.0f;
        
        Debug.Log ("3");
        
        L.DoString ("go.transform.position = v");
        
        Debug.Log ("4");
        
        string[] script = {
            "UnityEngine = luanet.UnityEngine",
            "cube = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube)",
            "cube.transform.position = v",
            ""
        };
        
        foreach (var line in script) {
            L.DoString (line);
        }
        
        MyLog log = new MyLog ();
        L.RegisterFunction ("dlog", log, log.GetType ().GetMethod ("Log"));
        L.DoString ("dlog('steve')");
        foreach (var line in extScript) {
            L.DoString (line);
        }
    }

    void Start ()
    {
        gameObject.renderer.material.color = new Color (1.0f, 0.5f, 0.5f);
        loadScript();
        setupLua ();
    }
    
    void Update ()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 1.0)
        {
            // poor man's reload script, should only do so on change (get filesystem notification??)
            loadScript();         
            elapsedTime = 0;
        }

        L.DoString ("t = UnityEngine.Time.realtimeSinceStartup");
        L.DoString ("q = UnityEngine.Quaternion.AngleAxis(t*50, UnityEngine.Vector3.up)");
        L.DoString ("cube.transform.rotation = q");

        foreach (var line in extScript) {
            L.DoString (line);
        }
    }
}
