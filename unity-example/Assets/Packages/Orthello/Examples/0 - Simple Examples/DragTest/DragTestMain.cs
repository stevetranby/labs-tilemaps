using UnityEngine;
using System.Collections;

public class DragTestMain : MonoBehaviour {
	
	public bool onlyOnPurple = false;
	public bool lockPurple = false;
	
	// Use this for initialization
	void Start () {		
		// debug mode on
		OT.debug = true;
		// touch with 5 fingers for 0.5 seconds to toggle debug info or a tablet
		OTDebug.touchCount = 5;  
		OTDebug.touchTime = 0.5f;
		// use Control-D to toggle debug info on a pc
		OTDebug.toggleKey = "shift+d";		
	}
		
	void OnGUI()
	{
		// show multidrag selection on mobile device
		onlyOnPurple = GUI.Toggle(new Rect(10,10,200,20),onlyOnPurple, " Drop only on Purple");
		lockPurple = GUI.Toggle(new Rect(10,30,200,20),lockPurple, " Lock Purple, no drops.");
		if (OT.mobile && !OTDebug.showing)
			OT.multiDrag = GUI.Toggle(new Rect(10,50,200,20),OT.multiDrag, " Multi Drag");
	}
}
