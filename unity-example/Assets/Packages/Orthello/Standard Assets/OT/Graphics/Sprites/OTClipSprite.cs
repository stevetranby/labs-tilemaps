using UnityEngine;
using System.Collections;

public class OTClipSprite : OTSprite {	
	
	public Rect _clipRect = new Rect(0,0,1,1);
	public float _clipFactor = 1;
	public bool _onlyClipUV = false;
	
	
	public bool onlyClipUV
	{
		get{
			return _onlyClipUV;			
		}
		set {
			_onlyClipUV = value;
			CheckSettings();
			if (meshDirty)
				Update();
		}
	}
	
	Rect clipRect_;
	/// <summary>
	/// clip the sprite using this 'visible' area Rect(0,0,1,1)
	/// </summary>
	public Rect clipRect
	{
		get{
			return _clipRect;			
		}
		set {
			_clipRect = value;
			CheckSettings();
			if (meshDirty)
				Update();
		}
	}
	
	protected override void CheckSettings ()
	{
		base.CheckSettings ();
		
		if (_clipFactor<0)
			_clipFactor = 0;
		else
		if (_clipFactor>1)
			_clipFactor = 1;	
		
		if (_clipRect.width<0)
			_clipRect.width = 0;
		else
		if (_clipRect.width>1)
			_clipRect.width = 1;
		
		if (_clipRect.height<0)
			_clipRect.height = 0;
		else
		if (_clipRect.height>1)
			_clipRect.height = 1;
		
		if (_clipRect.x < 0)
			_clipRect.x = 0;
		else
		if (_clipRect.x > 1-_clipRect.width)
			_clipRect.x = 1-_clipRect.width;
		
		if (_clipRect.y < 0)
			_clipRect.y = 0;
		else
		if (_clipRect.y > 1-_clipRect.height)
			_clipRect.y = 1-_clipRect.height;
		
		
		
		if (_clipFactor!=clipFactor_ || !_clipRect.Equals(clipRect_) || (_onlyClipUV != onlyClipUV_))
		{			
			meshDirty = true;
			clipRect_ = _clipRect;
			clipFactor_ = _clipFactor;
			onlyClipUV_ = _onlyClipUV;
		}
	}
	
	float clipFactor_ = 1;
	bool onlyClipUV_ = false;
	/// <summary>
	/// Set the size the clipRect Area, 0 to 1
	/// </summary>
	public float clipFactor
	{
		get{
			return _clipFactor;			
		}
		set {
			_clipFactor = value;
			CheckSettings();
			if (meshDirty)
				Update();
		}
	}

	protected override string GetTypeName ()
	{
        return "ClipSprite";
	}	
	
	// Use this for initialization
	new protected void Awake () {
		clipRect_ = _clipRect;
		clipFactor_ = _clipFactor;
		onlyClipUV_ = _onlyClipUV;
		base.Awake();	
	}

	void AdjustUV()
	{
		if (mesh!=null && mesh.uv.Length>0)
		{
			bool rotated = false;
	        if (spriteContainer != null && spriteContainer.isReady && spriteContainer is OTSpriteAtlas)
	        {	
				if (frameIndex<(spriteContainer as OTSpriteAtlas).atlasData.Length)
					rotated = (spriteContainer as OTSpriteAtlas).atlasData[frameIndex].rotated;
			}
					
			Rect uvRect;						
			if (rotated)
				uvRect = new Rect(mesh.uv[3].y,mesh.uv[3].x,mesh.uv[1].y - mesh.uv[3].y, mesh.uv[1].x - mesh.uv[3].x);				
			else
				uvRect = new Rect(mesh.uv[3].x,mesh.uv[3].y,mesh.uv[1].x - mesh.uv[3].x, mesh.uv[1].y - mesh.uv[3].y);				
							
			float _mLeft = uvRect.xMin + (clipRect.xMin * uvRect.width);
			float _mRight = _mLeft + (clipRect.width * uvRect.width);
			float _mBottom = uvRect.yMin + (clipRect.yMin * uvRect.height);
			float _mTop = _mBottom + (clipRect.height * uvRect.height);
																
			if (clipFactor>0)
			{
				if (clipFactor<1)
				 FactorClip(ref _mTop, ref _mLeft, ref _mBottom, ref _mRight);
				
				if (rotated)
					mesh.uv = new Vector2[] { 
			                new Vector2(_mTop, _mLeft),			// topleft
			                new Vector2(_mTop, _mRight),		// topright
			                new Vector2(_mBottom, _mRight),		// botright
			                new Vector2(_mBottom, _mLeft)		// botleft
		            }; 
				else
					mesh.uv = new Vector2[] { 
			                new Vector2(_mLeft, _mTop),			// topleft
			                new Vector2(_mRight, _mTop),		// topright
			                new Vector2(_mRight, _mBottom),		// botright
			                new Vector2(_mLeft, _mBottom)		// botleft
		            }; 
								
			}
			else
			{
				mesh.triangles = new int[] { };
				mesh.uv = new Vector2[] { };
				mesh.colors = new Color[] { };
				mesh.vertices = new Vector3[] { };
			}
		}
	}	
	
	void FactorClip(ref float _mTop, ref float _mLeft, ref float _mBottom, ref float _mRight)
	{
		float mw = (_mRight-_mLeft);
		float fw = mw * clipFactor;
		float dw = mw - fw;
		float mh = (_mTop-_mBottom);
		float fh = mh * clipFactor;
		float dh = mh - fh;
		
		switch(pivot)
		{
			case Pivot.Top:
				_mBottom += dh; 
				break;
			case Pivot.TopLeft:
				_mRight -= dw; _mBottom += dh; 
				break;
			case Pivot.TopRight:
				_mLeft += dw; _mBottom += dh;
				break;
			case Pivot.Left:							
				_mRight -= dw; 
				break;
			case Pivot.Right:							
				_mLeft += dw; 
				break;
			case Pivot.Center:							
				_mLeft += dw/2; _mRight -= dw/2; _mBottom += dh/2; _mTop -= dh/2;
				break;
			case Pivot.Bottom:
				_mTop -= dh; 
				break;
			case Pivot.BottomLeft:
				_mRight -= dw; _mTop -= dh; 
				break;
			case Pivot.BottomRight:
				_mLeft += dw; _mTop -= dh;
				break;
		}
	}

	protected override void HandleUV ()
	{
		base.HandleUV();
		AdjustUV();
	}
	
	protected override Mesh GetMesh ()
	{
		Mesh mesh = base.GetMesh();
				
		if (clipRect.width>0 && clipRect.height>0 && clipFactor>0)
		{
			float _mLeft = mLeft + clipRect.xMin;
			float _mRight = _mLeft + clipRect.width;
			float _mBottom = mBottom + clipRect.yMin;
			float _mTop = _mBottom + clipRect.height;

			
			if (clipFactor<1)
				FactorClip(ref _mTop, ref _mLeft, ref _mBottom, ref _mRight);

			
			if (!onlyClipUV)
				mesh.vertices = new Vector3[] { 
		                new Vector3(_mLeft, _mTop, 0),		// topleft
		                new Vector3(_mRight, _mTop, 0),		// topright
		                new Vector3(_mRight, _mBottom, 0),	// botright
		                new Vector3(_mLeft, _mBottom, 0)		// botleft
		            };        	
			
			
			AdjustUV();
			
			return mesh;
		}
		else
		{
			mesh.triangles = new int[] { };
			mesh.uv = new Vector2[] { };
			mesh.colors = new Color[] { };
			mesh.vertices = new Vector3[] { };
			return mesh;
		}
		
	}
	
	
					
}
