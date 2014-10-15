// ------------------------------------------------------------------------
// Orthello 2D Framework Example Source Code
// (C)opyright 2011 - WyrmTale Games - http://www.wyrmtale.com
// ------------------------------------------------------------------------
// More info http://www.wyrmtale.com/orthello
// ------------------------------------------------------------------------
// Example 2 - Block behaviour class
// ------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class CBlock2 : MonoBehaviour {

    OTSprite sprite;                // This block's sprite class
    bool colorFade = false;         // color fade notifier
    float fadeTime = 0;             // fade time counter
    float fadeSpeed = 1f;         // fade speed
    
    Color startColor = 
        new Color(0.3f, 0.3f, 0.3f); // block color

	// Use this for initialization
	void Start () {
        // Lookup this block's sprite
        sprite = GetComponent<OTSprite>();
        // Set this sprite's collision delegate 
        // HINT : We could use sprite.InitCallBacks(this) as well.
        // but because delegates are the C# way we will use this technique
        sprite.onCollision = OnCollision;
        // Set this block's tinting to the start color
        sprite.tintColor = startColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (colorFade)
        {
            // We are color fading so set block's color to fade time dependend color
            sprite.tintColor = Color.Lerp(Color.white,startColor,(fadeTime / fadeSpeed));
            // Incement fade time
            fadeTime += Time.deltaTime;
            if (fadeTime >= fadeSpeed)
                // We have faded long enough
                colorFade = false;
        }
	}

    // This method will be called when this block is hit.
    
    public void OnCollision(OTObject owner)
    {
        // Set color fading indicator
        colorFade = true;
        // Reset fade time
        fadeTime = 0;
    }

}
