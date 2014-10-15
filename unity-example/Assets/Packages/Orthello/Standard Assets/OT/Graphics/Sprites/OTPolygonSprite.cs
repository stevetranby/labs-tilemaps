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
public class OTPolygonSprite : OTSprite {	

	public Vector2[] _points;
	Vector2[] points_;
	public Vector2[] points
	{
		get
		{
			return _points;
		}	
		set
		{
			_points = value;
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
				
	public bool _filled = true;
	bool filled_ = true;
	/// <summary>
	/// When false, the inner vertical lines will not be displayed
	/// </summary>
	public bool filled
	{
		get
		{
			return _filled;
		}
		set
		{
			_filled = value;
			CheckSettings();
			Update();
		}
	}

	[HideInInspector]
	public Color _fillColor = Color.white;
	// Color fillColor_ = Color.white;
	/// <summary>
	/// When false, the outer horizontal lines will not be displayed
	/// </summary>
	public Color fillColor
	{
		get
		{
			return _fillColor;
		}
		set
		{
			_fillColor = value;
			CheckSettings();
			Update();
		}
	}
			 			
	bool pointsChanged
	{
		get
		{
			if (points.Length!=points_.Length)
				return true;
			for (int i=0; i<points.Length; i++)
			{
				if (points[i].Equals(points_[i]))
					return true;
			}
			return false;
		}
	}
	
	protected override void CheckSettings ()
	{
		base.CheckSettings ();
					
		if (lineThickness_!=_lineThickness || filled!=filled_ || pointsChanged)
		{			
			if (_lineThickness<=0)
				_lineThickness = 1;										
			filled_ = _filled;			
			points_ = (Vector2[]) points.Clone();
			meshDirty = true;
		}
				
	}
	
	protected override string GetTypeName ()
	{
        return "PolygonSprite";
	}	
	
	// Use this for initialization
	new protected void Awake () {		
		lineThickness_ = _lineThickness;
		filled_ = filled;
		//fillColor_ = fillColor;		
		points_ = (Vector2[])points.Clone();
		base.Awake();	
	}
	
	protected override void HandleUV ()
	{		
	}
	
	void AdjustUV(Mesh mesh, Vector2[] mesh_uv)
	{
	}	
				
	Vector3[] Points(int idx)
	{		
		Matrix4x4 m = new Matrix4x4();
		Vector2 ps = points[idx];
		Vector2 pp = (idx==0)?points[points.Length-1]:points[idx-1];
		Vector2 pn = (idx==points.Length-1)?points[0]:points[idx+1];
		
		Vector2 ds = (ps-pp).normalized * (lineThickness/2);
		m.SetTRS(Vector3.zero,Quaternion.Euler(0,0,-90),Vector3.one);
		Vector2 dsn = m.MultiplyPoint3x4(ds);
			
		Vector2 dn = (pn-ps).normalized * (lineThickness/2);
		m.SetTRS(Vector3.zero,Quaternion.Euler(0,0,90),Vector3.one);
		Vector2 dnn = m.MultiplyPoint3x4(dn);
				
		Vector2 p1,p2;
		if (idx==0 && !filled)
		{
			p1 = ps - dnn;
			p2 = ps + dnn;
		}
		else
		if (idx==points.Length-1 && !filled)
		{
			p1 = ps + dsn;
			p2 = ps - dsn;
		}
		else
		{
			try
			{
				Vector2 pss = ps + dsn;
				Vector2 pse = pss + ds;
				Vector2 pns = ps - dnn;
				Vector2 pne = pns + dn;

				p1 = OTHelper.LineIntersectionPoint(pse,pss,pne,pns);
				
				pss = ps - dsn;
				pse = pss + ds;
		
				pns = ps + dnn;
				pne = pns + dn;
				
				p2 = OTHelper.LineIntersectionPoint(pss,pse,pns,pne);
			}
			catch
			{
				// lines are parallel			
				p1 = ps + dsn;
				p2 = ps - dsn;
			}
		}
				
		return new Vector3[]
		{
			p1,
			p2
		};
		
	}	
	
	protected override Mesh GetMesh ()
	{
		Mesh mesh = base.GetMesh();
		if (points.Length<2)
			return mesh;
			
		mesh.triangles = new int[]{};
		
				
		List<Vector3> vertices = new List<Vector3>();		
		
		int wp = points.Length;
		for (int i=0; i<wp; i++)
			vertices.AddRange(Points(i));
				
		mesh.vertices = vertices.ToArray();		
		int[] triangles = new int[(vertices.Count/2) * 6];
		int idx = 0;
		for (int i=0; i<vertices.Count/2; i++)
		{
			if (!filled && (i*2)+1==vertices.Count-1)
				break;
			
			
			triangles[idx++] = (i*2);
			triangles[idx++] = (i*2)+1;
			if ((i*2)+1==vertices.Count-1)
				triangles[idx++] = 1;			
			else
				triangles[idx++] = (i*2)+3;			
			triangles[idx++] = (i*2);									
			if ((i*2)+1==vertices.Count-1)
			{
				triangles[idx++] = 1;			
				triangles[idx++] = 0;			
			}
			else
			{
				triangles[idx++] = (i*2)+3;
				triangles[idx++] = (i*2)+2;								
			}
		}
						
		mesh.triangles = triangles;

		//AdjustUV(mesh,mesh_uv);	
		return mesh;
	}
	
	
					
}
