using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Xml;
#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
using System.Xml.Linq;
#endif

public static class OTExtensions  {
		
	public static bool ColliderRaycast(this GameObject g, Ray r, out object hit, float distance)
	{
		if (g.collider!=null)
		{
			RaycastHit h;
			bool found = g.collider.Raycast(r, out h, distance);
			hit = h;
			return found;
		}
		
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
#else			
		else
		if (g.collider2D!=null)
		{
			RaycastHit2D[] allHits = Physics2D.RaycastAll(r.origin, r.direction);
			foreach(RaycastHit2D hit2D in allHits)
			{
				if (hit2D.collider == g.collider2D)
				{
					hit = hit2D;
					return true;
				}
			}
		}
#endif
		hit = null;
		return false;

	}
	
	
#if (UNITY_METRO || UNITY_WP8) && !UNITY_EDITOR
	public static List<XElement> SelectElements(this XDocument xdoc, string path)	
	{
		string[] pa = path.Split('/');
		XElement el = null;
		int pi=0;
		for (int p=0; p<pa.Length-1; p++)
		{
			if (pa[p] == "") 
				continue;
			if (pi==0)
				el = xdoc.Element(pa[p]);
			else
				el = el.Element(pa[p]);
			pi++;
			if (el == null)
				return new List<XElement>();
		}		
		return new List<XElement>(el.Elements(pa[pa.Length-1]));		
	}
	
	public static List<XElement> SelectElements(this XElement xel, string path)	
	{
		string[] pa = path.Split('/');
		XElement el = xel;
		for (int p=0; p<pa.Length-1; p++)
		{
			if (pa[p] == "") 
				continue;
			el = el.Element(pa[p]);
			if (el == null)
				return new List<XElement>();
		}		
		return new List<XElement>(el.Elements(pa[pa.Length-1]));				
	}

#endif

#if UNITY_METRO && !UNITY_EDITOR
	public static bool IsSubclassOf(this System.Type type, System.Type pType)
    {
        TypeInfo info = type.GetTypeInfo();
		return info.IsSubclassOf(pType);
	}
	
    public static FieldInfo GetField(this System.Type type, string name)
    {
        TypeInfo info = type.GetTypeInfo();
		List<FieldInfo> fields = new List<FieldInfo>(info.DeclaredFields);
		
		for (int i=0; i<fields.Count; i++)
		{
			if (fields[i].Name == name)
				return fields[i];
		}
		return null;
	}
		
    public static PropertyInfo GetProperty(this System.Type type, string name)
    {
        TypeInfo info = type.GetTypeInfo();
		List<PropertyInfo> props = new List<PropertyInfo>(info.DeclaredProperties);
		
		for (int i=0; i<props.Count; i++)
		{
			if (props[i].Name == name)
				return props[i];
		}
		return null;
	}
	
    public static MethodInfo GetMethod(this System.Type type, string name, System.Type[] types)
    {
        TypeInfo info = type.GetTypeInfo();
		List<MethodInfo> methods = new List<MethodInfo>(info.DeclaredMethods);
		
		for (int i=0; i<methods.Count; i++)
		{
			if (methods[i].Name == name)
			{
        		ParameterInfo[] pars = methods[i].GetParameters();
				int pi = 0;
				bool isOk = true;
        		foreach (ParameterInfo p in pars) 
        		{
            		if (p.ParameterType != types[pi])
					{
						isOk = false;
						break;	
					}
					pi++;
        		}				
				if (isOk)
					return methods[i];
			}
		}
		return null;
	}
					
    public static MethodInfo GetMethod(this System.Type type, string name)
    {
        TypeInfo info = type.GetTypeInfo();
		List<MethodInfo> methods = new List<MethodInfo>(info.DeclaredMethods);
		
		for (int i=0; i<methods.Count; i++)
		{
			if (methods[i].Name == name)
				return methods[i];
		}
		return null;
	}
#endif
	
}
