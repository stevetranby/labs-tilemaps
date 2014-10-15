using UnityEngine;
using System.Collections;

public class MultiResolution : MonoBehaviour {
		
	public enum MRSystemType { InSheet, UsingResources };
	public enum ArtResolution { small, normal, hd }
	
	public MRSystemType systemType = MRSystemType.InSheet;
	public ArtResolution artResolution = ArtResolution.normal;
		
	
	// Use this for initialization
	void Start () {				
		InitResolutionData();			
		TweenUp(null);
	}
		
	void InitResolutionData()
	{
		if (systemType == MRSystemType.UsingResources)
			// lets set the right Resources sub folder so Orthello can get all images with the same name
			// as the OT sprite or sheet or atlas texture name's from it.
			switch(artResolution)
			{
				case ArtResolution.hd :
					OT.textureResourceFolder = "big";
					break;
				case ArtResolution.normal :
					OT.textureResourceFolder = "";
					break;
				case ArtResolution.small :
					OT.textureResourceFolder = "small";
					break;
			}		
		else
			// set the right sizefactor so the right sheet size texture can be set
			// from trhe sheet's or atlasses
			switch(artResolution)
			{
				case ArtResolution.hd :
					OT.sizeFactor = 2;
					break;
				case ArtResolution.normal :
					OT.sizeFactor = 1;
					break;
				case ArtResolution.small :
					OT.sizeFactor = .5f;
					break;
			}		
	}
		
	
	float time = 0;
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time >= 0.6f)
		{
			time -= 0.6f;
			if (OT.Sprite("sprite-atlas").frameIndex<OT.Sprite("sprite-atlas").spriteContainer.frameCount-1)
				OT.Sprite("sprite-atlas").frameIndex++;
			else
				OT.Sprite("sprite-atlas").frameIndex=0;
		}
	}
	
	void TweenUp(OTTween tween)
	{
		new OTTween(OT.Sprite("sprite-background"), 15, OTEasing.SineInOut).
			TweenAdd("position",new Vector2(0,4000)).
				onTweenFinish = TweenDown;
			
	}
	void TweenDown(OTTween tween)
	{
		new OTTween(OT.Sprite("sprite-background"), 15, OTEasing.SineInOut).
			TweenAdd("position",new Vector2(0,-4000)).
				onTweenFinish = TweenUp;			
	}
	
	void OnGUI()
	{		
		GUI.skin.button.normal.textColor = (artResolution == ArtResolution.small)?Color.yellow:Color.white;		
		GUI.skin.button.hover.textColor = GUI.skin.button.normal.textColor;
		if (GUI.Button(new Rect(2,2,150,30),"small"))
		{
			artResolution = ArtResolution.small;
			InitResolutionData();			
		}
		GUI.skin.button.normal.textColor = (artResolution == ArtResolution.normal)?Color.yellow:Color.white;		
		GUI.skin.button.hover.textColor = GUI.skin.button.normal.textColor;
		if (GUI.Button(new Rect(200,2,150,30),"normal"))
		{
			artResolution = ArtResolution.normal;
			InitResolutionData();			
		}
		GUI.skin.button.normal.textColor = (artResolution == ArtResolution.hd)?Color.yellow:Color.white;		
		GUI.skin.button.hover.textColor = GUI.skin.button.normal.textColor;
		if (GUI.Button(new Rect(400,2,150,30),"HD"))
		{
			artResolution = ArtResolution.hd;
			InitResolutionData();			
		}
	}
}
