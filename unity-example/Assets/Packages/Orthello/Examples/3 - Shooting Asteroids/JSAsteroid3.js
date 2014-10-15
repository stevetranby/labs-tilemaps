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
// to the / (root).
//
// This code was commented to prevent compiling errors when project is
// downloaded and imported using a package.
// ------------------------------------------------------------------------
// Asteroid behaviour class
// ------------------------------------------------------------------------

/*
private var sprite:OTAnimatingSprite;               // this asteroid's sprite class
private var forwardVector:Vector2  = Vector2.zero;      // this asteroid's forward vector
private var sheet1:OTSpriteSheet;


// Use this for initialization
function Start () {
    // get sprite class of this asteroid
    sprite = GetComponent("OTAnimatingSprite");
	sheet1 = OT.ContainerByName("asteroid sheet 1");
}

// Update is called once per frame
function Update () {
    
    // If we did not capture this sprite's formward vector we capture it once
    if (Vector2.Equals(forwardVector, Vector2.zero))
        forwardVector = transform.up;

    // Update asteroid's position
    sprite.position += (forwardVector * 50 * Time.deltaTime);
    // Update asteroid's rotation
    sprite.rotation += 30 * Time.deltaTime;
    // If the asteroid is smaller than 30 pixels lets auto shrink it
    if (sprite.size.x < 50 || sprite.size.y < 50)
    {
		sprite.Stop();
		sprite.depth = 100;
		sprite.frameIndex = 0;
		sprite.rotation += 90 * Time.deltaTime;
		if (sprite.size.x < 10 || sprite.size.y < 10)
		{
			if (sprite.otCollider.enabled)
				sprite.otCollider.enabled = false;
			sprite.spriteContainer = sheet1;
			sprite.size = sprite.size * (1f - (0.99f * Time.deltaTime));
		}
        else
            sprite.size = sprite.size * (1f - (0.95f * Time.deltaTime));
        // If the asteroid is smaller than 2 pixels, destroy it.
        if (sprite.size.x < 2 || sprite.size.y < 2)
        {
			sprite.otCollider.enabled = true;
            OT.DestroyObject(sprite);
        }
    }
    // Destroy the asteroid as ist moves out of view
    if (sprite.outOfView)
    {
		sprite.otCollider.enabled = true;
        OT.DestroyObject(sprite);
    }
}
*/