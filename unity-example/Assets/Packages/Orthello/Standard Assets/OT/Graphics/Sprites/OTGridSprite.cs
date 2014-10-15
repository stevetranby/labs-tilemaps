using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// A sprite that can create a grid of lines.
/// </summary>
/// <remarks>
/// The mesh will be generated using the provided cell sizes
/// so a localscale of 1,1,1 will present you  with the grid with the specified dimensions
/// </remarks>
public class OTGridSprite : OTSprite {	
	
	public IVector2 _gridXY = new IVector2(3,3);	
	IVector2 gridXY_ = new IVector2(3,3);
	/// <summary>
	/// Gets or sets the dimension of the grid in number cells XY
	/// </summary>
	public IVector2 gridXY
	{
		get
		{
			return _gridXY;
		}
		set
		{
			_gridXY = value;
			CheckSettings();						
			Update();
		}
	}
	
	public Vector2 _cellXY = new Vector2(50,50);	
	Vector2 cellXY_ = new Vector2(50,50);
	/// <summary>
	/// Gets or sets the size of a grid cell
	/// </summary>
	public Vector2 cellXY
	{
		get
		{
			return _cellXY;
		}
		set
		{
			_cellXY = value;
			CheckSettings();
			Update();
		}
	}
	
	public float _lineThickness = 4;
	float lineThickness_ = 4;
	/// <summary>
	/// When false, the inner vertical lines will not be displayed
	/// </summary>
	public float lineThickness
	{
		get
		{
			return _lineThickness;
		}
		set
		{
			_lineThickness = value;
			CheckSettings();			
			Update();
		}
	}
			
	public bool _innerHorizontal = true;
	bool innerHorizontal_ = true;
	/// <summary>
	/// When false, the inner horizontal lines will not be displayed
	/// </summary>
	public bool innerHorizontal
	{
		get
		{
			return _innerHorizontal;
		}
		set
		{
			_innerHorizontal = value;
			CheckSettings();
			Update();
		}
	}
	
	public bool _innerVertical = true;
	bool innerVertical_ = true;
	/// <summary>
	/// When false, the inner vertical lines will not be displayed
	/// </summary>
	public bool innerVertical
	{
		get
		{
			return _innerVertical;
		}
		set
		{
			_innerVertical = value;
			CheckSettings();
			Update();
		}
	}
	
	
	public bool _outerHorizontal = true;
	bool outerHorizontal_ = true;
	/// <summary>
	/// When false, the outer horizontal lines will not be displayed
	/// </summary>
	public bool outerHorizontal
	{
		get
		{
			return _outerHorizontal;
		}
		set
		{
			_outerHorizontal = value;
			CheckSettings();
			Update();
		}
	}
	
	public bool _outerVertical = true;
	bool outerVertical_ = true;
	/// <summary>
	/// When false, the outer horizontal lines will not be displayed
	/// </summary>
	public bool outerVertical
	{
		get
		{
			return _outerVertical;
		}
		set
		{
			_outerVertical = value;
			CheckSettings();
			Update();
		}
	}
	
		 			
	protected override void CheckSettings ()
	{
		base.CheckSettings ();
					
		if (!gridXY.Equals(gridXY_) || !cellXY.Equals(cellXY_) || lineThickness_!=_lineThickness ||
			innerHorizontal!=innerHorizontal_ || innerVertical!=innerVertical_ || 
			outerHorizontal!=outerHorizontal_ || outerVertical!=outerVertical_ )
		{			
			if (_gridXY.x<1)
				_gridXY.x = 1;
			if (_gridXY.y<1)
				_gridXY.y = 1;
			gridXY_ = _gridXY.Clone();

			if (_cellXY.x<=0)
				_cellXY.x = 1f;
			if (_cellXY.y<1)
				_cellXY.y = 1;
			cellXY_ = _cellXY;

			if (_lineThickness<=0)
				_lineThickness = 1;							
			lineThickness_ = _lineThickness;
			
			innerHorizontal_ = innerHorizontal;
			innerVertical_ = innerVertical;
			outerHorizontal_ = outerHorizontal;
			outerVertical_ = outerVertical;
			
			
			meshDirty = true;
		}
				
	}
	
	protected override string GetTypeName ()
	{
        return "GridSprite";
	}	
	
	// Use this for initialization
	new protected void Awake () {
		
		gridXY_ = gridXY.Clone();
		cellXY_ = _cellXY;
		innerHorizontal_ = innerHorizontal;
		innerVertical_ = innerVertical;
		outerHorizontal_ = outerHorizontal;
		outerVertical_ = outerVertical;
		lineThickness_ = _lineThickness;
		base.Awake();	
	}
	
	protected override void HandleUV ()
	{		
	}
	
	void AdjustUV(Mesh mesh, Vector2[] mesh_uv)
	{
	}	
	
	Vector3[] VLine(int h)
	{
		float wi = gridXY.x * cellXY.x;
		float hg = gridXY.y * cellXY.y;
		wi += lineThickness;
		hg += lineThickness;
		float sx = (-wi/2);
		float sy = (-hg/2);
		
		sx+= h * cellXY.x;
		
		return new Vector3[] 
		  {
			new Vector3(sx,sy,0),
			new Vector3(sx,sy+hg,0),
			new Vector3(sx+lineThickness,sy+hg,0),
			new Vector3(sx+lineThickness,sy,0)
		  };		
		
	}	

	Vector3[] HLine(int v)
	{
		float wi = gridXY.x * cellXY.x;
		float hg = gridXY.y * cellXY.y;
		wi += lineThickness;
		hg += lineThickness;
		float sx = (-wi/2);
		float sy = (-hg/2);
		
		sy += v * cellXY.y;
		
		return new Vector3[] 
		  {
			new Vector3(sx,sy,0),
			new Vector3(sx,sy+lineThickness,0),
			new Vector3(sx+wi,sy+lineThickness,0),
			new Vector3(sx+wi,sy,0)
		  };		
		
	}
	
	protected override Mesh GetMesh ()
	{
		Mesh mesh = base.GetMesh();
			
		mesh.triangles = new int[]{};
		
		List<Vector3> vertices = new List<Vector3>();
		for (int x=0; x<gridXY.x; x++)
		{
			if (x>0 && innerVertical)
				vertices.AddRange(VLine(x));
			else
				if (outerVertical)
					vertices.AddRange(VLine(0));
			
			if (x == gridXY.x-1 && outerVertical)
			  vertices.AddRange(VLine(gridXY.x));
		}
		
		for (int y=0; y<gridXY.y; y++)
		{
			if (y>0 && innerHorizontal)
				vertices.AddRange(HLine(y));
			else
				if (outerHorizontal)
					vertices.AddRange(HLine(0));
			
			if (y == gridXY.y-1 && outerHorizontal)
			  vertices.AddRange(HLine(gridXY.y));
		}
		
		mesh.vertices = vertices.ToArray();
		
		int[] triangles = new int[(vertices.Count/4) * 6];
		int idx = 0;
		for (int i=0; i< vertices.Count/4; i++)
		{
			triangles[idx++] = (i*4);
			triangles[idx++] = (i*4)+1;
			triangles[idx++] = (i*4)+2;
			triangles[idx++] = (i*4);
			triangles[idx++] = (i*4)+2;
			triangles[idx++] = (i*4)+3;
		}
		
		mesh.triangles = triangles;

		//AdjustUV(mesh,mesh_uv);	
		return mesh;
	}
	
	
					
}
