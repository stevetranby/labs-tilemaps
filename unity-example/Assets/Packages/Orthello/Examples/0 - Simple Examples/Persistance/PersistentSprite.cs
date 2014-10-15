using UnityEngine;
using System.Collections;

public class PersistentSprite : MonoBehaviour {
	
	OTObject otObject = null;
		
	// Use this for initialization
	void Start () {
		otObject = GetComponent<OTObject>();		
		otObject.BoundBy(GameObject.Find("Boundary").GetComponent<OTObject>());
		OT.Persist(otObject);
	}
			
	void OnLevelWasLoaded (int level) {
		otObject.BoundBy(GameObject.Find("Boundary").GetComponent<OTObject>());
	}	
			
	void OnGUI()
	{
		GUI.Label(new Rect(10,10,100,20),"DRAG ME AROUND");
		if (Application.loadedLevelName == "level-0")
			if (GUI.Button(new Rect(10,30,100,20),"to Level-1"))
				Application.LoadLevel("level-1");
	}
}
