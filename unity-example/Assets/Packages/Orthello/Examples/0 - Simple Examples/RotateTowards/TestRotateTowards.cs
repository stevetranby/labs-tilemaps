using UnityEngine;
using System.Collections;

public class TestRotateTowards : MonoBehaviour {
		
	// Update is called once per frame
	void Update () {
		if (!OT.isValid)
			return;
		
		// now totate the 'rotate' sprite towards the 'target' sprite
		OT.Sprite("rotate").RotateTowards(OT.Sprite("target"));		
	}
}
