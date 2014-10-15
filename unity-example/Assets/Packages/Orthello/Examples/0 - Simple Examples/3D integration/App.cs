using UnityEngine;
using System.Collections;

public class App : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	
	float t;
	// Update is called once per frame
	void Update () {
		
		t += Time.deltaTime;
		if (t > 0.5f)
		{
			OT.CreateObject("cube");
			t = 0;
		}
		
	}
}
