using UnityEngine;
using System.Collections;

public class OTAnimatingSpritePassive : MonoBehaviour {
	
	OTAnimatingSprite sprite;
	// Use this for initialization
	void Start () {
		sprite = GetComponent<OTAnimatingSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		if (sprite.passive && !sprite.enabled)
			sprite.ForceUpdate();
	}
}
