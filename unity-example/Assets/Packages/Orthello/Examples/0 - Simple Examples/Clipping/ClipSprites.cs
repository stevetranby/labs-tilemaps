using UnityEngine;
using System.Collections;

public class ClipSprites : MonoBehaviour {

	// keep movehandle because we are going to de-activate it.
	public OTSprite moveHandle;
	
	// Use this for initialization
	void Start () {
		// hide the movehandle and cliparea sprite
		moveHandle.Deactivate();		
		// set clip sprites to initial state
		SetClipFactor(0);
	}
	
	bool clipArea = false;
	float clipFactor = 0;
	void SetClipFactor(float _factor)
	{
		clipFactor = _factor;
		// loop all sprites and change the overlay sprites
		foreach (Transform t in GameObject.Find("sprites").transform)
		{
			// get the overlay sprite
			OTSprite overlay = t.GetComponent<OTSprite>().Sprite("overlay");
			if (overlay!=null)
				(overlay as OTClipSprite).clipFactor = clipFactor;
		}		
	}	
	
	void HandleClipArea()
	{
		if (clipArea)
		{
			moveHandle.Activate();
			// set the clip layer so a camera is created
			(moveHandle.Sprite("clipArea") as OTClippingAreaSprite).clipLayer = 16;
			// set all children of 'sprites' to layer 16
			OTHelper.ChildrenSetLayer(GameObject.Find("sprites"),16);
		}
		else
		{
			// set all children of 'sprites' to layer 0 = default
			OTHelper.ChildrenSetLayer(GameObject.Find("sprites"),0);
			// set the clipLayer to 0 so the clipping camera is removed
			(moveHandle.Sprite("clipArea") as OTClippingAreaSprite).clipLayer = 0;
			// hide the movehandle and cliparea sprite
			moveHandle.Deactivate();
		}
	}
	
	
	void OnGUI()
	{
		// clip factor mode
		GUI.Label(new Rect(10,10,100,20),"Clip factor");
		float _factor = GUI.HorizontalSlider(new Rect(100,15,300,20), clipFactor, 0,1 );
		if (_factor!=clipFactor)
		{
			// clipfactor changed
			SetClipFactor(_factor);
		}									
		if (GUI.Button(new Rect(10,40,150,20),(clipArea)?"Unclip Area":"Clip Area"))			
		{
			clipArea = !clipArea;
			HandleClipArea();
		}
	}
	
}
