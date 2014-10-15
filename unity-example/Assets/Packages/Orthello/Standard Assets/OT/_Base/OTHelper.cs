using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Useful Orthello static helper functions
/// </summary>
public class OTHelper {

	/// <summary>
	/// Get children transforms of a GameObject, ordered by name (as displayed in the scene hierarchy)
	/// </summary>
	public static Transform[] ChildrenOrderedByName(Transform parent)
	{
		List<Transform> res = new List<Transform>();
		if (parent!=null && parent.childCount>0)
			foreach(Transform child in parent.transform)
				res.Add(child);
		
		if (res.Count>0)
			res.Sort(delegate(Transform a , Transform b)
			{
				string sa = a.name.Replace("-","_").ToLower();
				string sb = b.name.Replace("-","_").ToLower();
				return string.Compare(sa, sb);
			});
				
		return res.ToArray();		
	}
	
	/// <summary>
	/// Sets the layer of the childrens of the provided parent
	/// </summary>
	public static void ChildrenSetLayer(GameObject parent, int layer, List<GameObject> exclude)
	{
		foreach (Transform child in parent.transform)
			if (exclude == null || !exclude.Contains(child.gameObject))
			{
				child.gameObject.layer = layer;
				ChildrenSetLayer(child.gameObject,layer,exclude);
			}
	}
	/// <summary>
	/// Sets the layer of the childrens of the provided parent
	/// </summary>
	public static void ChildrenSetLayer(GameObject parent, int layer)
	{
		ChildrenSetLayer(parent,layer,null);
	}

	/// <summary>
	/// Converts world coordinate based Rectangle to Bounds, using a specifc depth size 
	/// </summary>
	public static Bounds RectToBounds(Rect r, int depthSize)
	{
		bool td = OT.world == OT.World.WorldTopDown2D;
		Vector3 center = new Vector3(r.center.x, td?0:r.center.y, td?r.center.y:0);
		Vector3 size = new Vector3(Mathf.Abs(r.width), td?depthSize:Mathf.Abs(r.height), td?Mathf.Abs(r.height):depthSize);			
		return new Bounds(center, size);		
	}

	/// <summary>
	/// Converts world coordinate based Rectangle to Bounds
	/// </summary>
	public static Bounds RectToBounds(Rect r)
	{
		return RectToBounds(r,3000);
	}

	/// <summary>
	/// Converts a gameobject's parent local point to world coordinate
	/// </summary>
	public static Vector3 WorldPoint(GameObject g, Vector3 point)
	{
		if (g.transform.parent == null)
			return point;
		else
			return g.transform.parent.localToWorldMatrix.MultiplyPoint3x4(point);
	}
	
	/// <summary>
	/// loads a texture from resources
	/// </summary>
	public static Texture2D ResourceTexture(string filename)
	{
		Texture2D tex = Resources.Load(filename, typeof(Texture2D)) as Texture2D;
		return  tex;
	}
	/// <summary>
	/// Get an XmlDataReader from a resource (xml) text asset
	/// </summary>
	public static OTXMLDataReader ResourceXML(string filename)
	{
		TextAsset txt = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
		if (txt!=null)
		{
			try
			{
				return new OTXMLDataReader(filename,txt);
			}
			catch(System.Exception)
			{
			}
		}
		return  null;
	}
	
	/// <summary>
	/// Lightens color to white
	/// </summary>
	public static Color Lighter(Color c, int perc)
	{
		return Color.Lerp(c,Color.white,(float)perc/100);
	}
	
	/// <summary>
	/// Darkens color to black
	/// </summary>
	public static Color Darker(Color c, int perc)
	{
		return Color.Lerp(c,Color.black,(float)perc/100);
	}
	
	/// <summary>
	/// Converts a string with format '(x,y)' or 'x,y' to a Vector2
	/// </summary>
	/// <returns>
	public static Vector2 StringVector2(string vector2)
	{
		string v = vector2;
		if (vector2.IndexOf("(")==0)
			v = v.Substring(1, v.Length-2);
			
		string[] va = v.Split(',');
		if (va.Length == 2)
			return new Vector2((float)System.Convert.ToDouble(va[0]),(float)System.Convert.ToDouble(va[1]));
		
		return Vector2.zero;
	}
	
	/// <summary>
	/// Converts a string with format '(x,y,z)' or 'x,y,z' to a Vector3
	/// </summary>
	/// <returns>
	public static Vector3 StringVector3(string vector3)
	{
		string v = vector3;
		if (vector3.IndexOf("(")==0)
			v = v.Substring(1, v.Length-2);
			
		string[] va = v.Split(',');
		if (va.Length == 3)
			return new Vector3((float)System.Convert.ToDouble(va[0]),(float)System.Convert.ToDouble(va[1]), (float)System.Convert.ToDouble(va[2]));
		
		return Vector3.zero;
	}

	/// <summary>
	/// Calculates the intersection point between 2 lines
	/// </summary>
	/// <returns>
	/// The intersection point.
	/// </returns>
	/// <remarks>
	/// Line 1 is represented by Vector2 ps1 to pe1
	/// Line 2 is represented by Vector2 ps2 to pe2
	/// The method will throw an Exception whe the lines are parallel and do not cross
	/// </remarks>
	public static Vector2 LineIntersectionPoint(Vector2 ps1, Vector2 pe1, Vector2 ps2, Vector2 pe2)
	{
		float A1 = pe1.y-ps1.y; 
		float B1 = ps1.x-pe1.x; 
		float C1 = A1*ps1.x+B1*ps1.y;
		
		float A2 = pe2.y-ps2.y; 
		float B2 = ps2.x-pe2.x; 
		float C2 = A2*ps2.x+B2*ps2.y;
		
		float delta = A1*B2 - A2*B1;
		
		if(delta == 0) 
    		throw new System.Exception("Lines are parallel");
		return new Vector2((B2*C1 - B1*C2)/delta,(A1*C2 - A2*C1)/delta);
	}
	
	

	
}
