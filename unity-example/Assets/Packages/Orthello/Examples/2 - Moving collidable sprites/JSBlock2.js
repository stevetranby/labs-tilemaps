// ------------------------------------------------------------------------
// Orthello 2D Framework Example 
// (C)opyright 2011 - WyrmTale Games - http://www.wyrmtale.com
// ------------------------------------------------------------------------
// More info http://www.wyrmtale.com/orthello
// ------------------------------------------------------------------------
// Because Orthello is created as a C# framework the C# classes 
// will only be available as you place them in the /Standard Assets folder.
//
// If you would like to test the JS examples or use the framework in combination
// with Javascript coding, you will have to move the /Orthello/Standard Assets folder
// to the / (root) using the Unity editor.
//
// This code was commented to prevent compiling errors when project is
// downloaded and imported using a package.
// ------------------------------------------------------------------------
// Example 2 - Block behaviour class
// ------------------------------------------------------------------------

/*

private var sprite:OTSprite;            // This block's sprite class
private var colorFade:boolean = false;  // color fade notifier
private var fadeTime:Number;            // fade time counter
private var fadeSpeed:Number = 1;     // fade speed

private var startColor:Color =          // block color
    new Color(0.3f, 0.3f, 0.3f);

// Use this for initialization
function Start () {
    // Get this block's sprite
	sprite = GetComponent("OTSprite");
	// Because Javascript does not support C# delegate we have to notify our sprite class that 
	// we want to receive notification callbacks.
	// If we have initialized for callbacks our (!IMPORTANT->) 'public' declared call back 
	// functions will be asutomaticly called when an event takes place.
	// HINT : This technique can be used within a C# class as well.
	sprite.InitCallBacks(this);
	// Set block's color
    sprite.tintColor = startColor;	
}

// Update is called once per frame
function Update () {
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

// This function will be called when this block is hit.
public function OnCollision(owner:OTObject)
{
    // Set color fading indicator
    colorFade = true;
    // Reset fade time
    fadeTime = 0;
}

*/