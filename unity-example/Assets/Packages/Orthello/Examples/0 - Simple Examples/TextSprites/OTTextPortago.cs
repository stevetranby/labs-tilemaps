using UnityEngine;
using System.Collections;

public class OTTextPortago : MonoBehaviour {
	
	OTTextSprite text;
	
	// Use this for initialization
	void Start () {
		text = GetComponent<OTTextSprite>();
	}
	
	int tick = 0;
	// Update is called once per frame
	void Update () {
		text.text = "Another text to test this. \n"+(tick++);
	}
}
