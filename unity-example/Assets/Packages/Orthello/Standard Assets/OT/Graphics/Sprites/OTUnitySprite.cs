using UnityEngine;
using System.Collections;

/// <summary>
/// Provides functionality to use unity 2d sprites (SpriteRenderer) within the Orthello framework
/// </summary>
public class OTUnitySprite : OTObject {
	
	protected override string GetTypeName()
	{
		return "UnitySprite";
	}	
	
	#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
	#else
	
	new void Awake()
	{
		base.Awake();
		if (GetComponent<SpriteRenderer>()==null)
		{
			otRenderer = gameObject.AddComponent<SpriteRenderer>();
			if (otRenderer!=null)
			{
				otRenderer.sortingLayerID = 0;
				otRenderer.sortingOrder = 0;
			}
		}
	}
	
	#endif
	
}

