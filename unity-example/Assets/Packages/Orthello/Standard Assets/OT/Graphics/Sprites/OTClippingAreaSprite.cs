using UnityEngine;
using System.Collections;

/// <summary>
/// Use this sprite to create a clipping area on your screen. 
/// </summary>
/// <remarks>
/// The clipped area will only show game objects and sprites that have a specific layer id (int).
/// You can set clipping on and off just by setting the clipLayer to a value 16+ or 0 to clear. 
/// </remarks>
public class OTClippingAreaSprite : OTSprite {	
	public int _clipLayer = 16;
	
	/// <summary>
	/// Gets or sets the inner margin of the clip area. 
	/// </summary>
	public int clipMargin = 3;
	bool clipped = false;	

	
	// Use this for initialization
	new protected void Awake () {
		passiveControl = true;
		base.Awake();	
	}
	
	/// <summary>
	/// Gets or sets the clip layer id 
	/// </summary>
	public int clipLayer
	{
		get
		{
			return _clipLayer;			
		}
		set
		{
			_clipLayer = value;
			Update();
		}
	}
		
	
	// Use this for initialization
	new protected void Start () {
		base.Start();	
	}
		
	protected override string GetTypeName ()
	{
        return "ClippingArea";
	}
	
	
	public override void PassiveUpdate()
	{
		if ((clipped && !worldRect.Equals(lastRect)) || dirtyChecks)
			Update();
	}

	/// <summary>
	/// The last known clipping rectangle in world coordinates
	/// </summary>	
	public Rect clipRect
	{
		get
		{
			return lastClipRect;
		}
	}
	
	Rect lastRect;
	Rect lastClipRect;
	int  clipLayer_ = 0;
	
	Camera _clipCamera = null;
	/// <summary>
	/// Gets the clipping camera.
	/// </summary>
	public Camera clipCamera
	{
		get
		{
			return _clipCamera;
		}
	}
	
	Vector3 baseVector;
	Vector2 _camOffset = Vector2.zero;
	/// <summary>
	/// Gets or sets the camera offset to scroll the contents of the view area
	/// </summary>
	public Vector2 cameraOffset
	{
		get
		{
			return _camOffset;
		}
		set
		{
			_camOffset = value;
			clipCamera.transform.position = baseVector;
			clipCamera.transform.Translate(cameraOffset);
		}
	}
	
	// Update is called once per frame
	new protected void Update () {
		base.Update();		
		if (OT.isValid && !isInvalid && Application.isPlaying)
		{
			
			if (_clipLayer!=clipLayer_)
			{
				clipLayer_ = clipLayer;
				if (clipped)
				{
					OT.UnClip(gameObject);
					lastRect = worldRect;
				}
				clipped = false;
			}
			
			if (!clipped && clipLayer>0)
			{
				Rect clipRect = worldRect;
				clipRect.xMin += clipMargin;
				clipRect.yMin += clipMargin;
				clipRect.width -= clipMargin;
				clipRect.height -= clipMargin;							

				GameObject[]  excludes = new GameObject[]{ };
				if (clipMargin>0)
					excludes = new GameObject[]{ gameObject };
									
				_clipCamera = OT.Clip(clipRect,gameObject,clipLayer, excludes);
				baseVector = clipCamera.transform.position;
				clipped = true;
				
				lastRect = worldRect;
				lastClipRect = clipRect;
			}
			else
			if (clipped)
			{
				if (!worldRect.Equals(lastRect))
				{
					Rect clipRect = worldRect;
					clipRect.xMin += clipMargin;
					clipRect.yMin += clipMargin;
					clipRect.width -= clipMargin;
					clipRect.height -= clipMargin;							
					OT.ClipMove(gameObject,clipRect);
					baseVector = _clipCamera.transform.position;
					lastRect = worldRect;
					clipCamera.transform.Translate(offset);
				}
			}
		}
	}
}
