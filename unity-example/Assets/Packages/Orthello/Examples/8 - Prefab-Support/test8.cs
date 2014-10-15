using UnityEngine;
using System.Collections;


public class test8 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {				
		if (OT.Clicked(GameObject.Find("asteroid-1").GetComponent<OTAnimatingSprite>()))
		{
			GameObject.Find("asteroid-1").GetComponent<OTAnimatingSprite>().Reverse();
		}
	}
}
