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
// Example 2 - Star behaviour class
// ------------------------------------------------------------------------

/*
private var sprite:OTSprite;        // This star's sprite class
private var speed:Vector2 =         // Star movement speed / second
    new Vector2(150, 150);
private var rotation:Number = 90;   // Star rotation speed / second 

private var stayColor:Color =       // Star's tint color when overlapping
    new Color(0.95f, 1f, .95f);
private var startColor:Color =      // Star's tint color
    new Color(0.5f, 0.9f, 0.5f);

// Use this for initialization
function Start () {
    // Get this star's sprite class
	sprite = GetComponent("OTSprite");
	// Because Javascript does not support C# delegate we have to notify our sprite class that 
	// we want to receive notification callbacks.
	// If we have initialized for callbacks our (!IMPORTANT->) 'public' declared call back 
	// functions will be asutomaticly called when an event takes place.
	// HINT : This technique can be used within a C# class as well.
	sprite.InitCallBacks(this);
	// Give this star a random speed
	speed = new Vector2(150 + 150 * Random.value, 150 + 150 * Random.value);
	// Set this star's color
	sprite.tintColor = startColor;
	// register the start material so we can use it later for assignment
	OT.RegisterMaterial("Star-start", new Material(sprite.material));
	var m = new Material(sprite.material);
	// register the material so we can use it later for assignment
	m.SetColor("_EmisColor", stayColor);
	OT.RegisterMaterial("Star-stay", m);
}

// Update is called once per frame
function Update () {
    // Update star's position
	sprite.position += speed * Time.deltaTime;
	// Update star's rotation
	sprite.rotation  += (rotation * Time.deltaTime);
}

// OnStay callback function is called when star enters (overlaps) another 'collidable' object
// !IMPORTANT - This sprite's collidable setting has to be true otherwide
// collision delegates will not be called
public function OnStay(owner:OTObject)
{
	if (owner.collisionObject.name.IndexOf("star")==0)
            sprite.material = OT.LookupMaterial("Star-stay");
}
// OnExit callback function is called when star no longer overlaps another 'collidable' object
public function OnExit(owner:OTObject)
{
	if (owner.collisionObject.name.IndexOf("star") == 0)
		 sprite.material = OT.LookupMaterial("Star-start");
}
// OnCollision callback function is called when star collides with another 'collidable' object
// HINT - OnEnter and OnCollision callbacks are called exactly at the same time, the only
// difference is their naming convention
public function OnCollision(owner:OTObject)
{
    // check if we collided with a top block and adjust our speed and rotation accordingly
	if (owner.collisionObject.name.IndexOf("top") == 0 && speed.y > 0)
	{
		speed = new Vector2(speed.x, speed.y * -1);
		if ((speed.x < 0 && rotation > 0) || (speed.x > 0 && rotation < 0))
			rotation *= -1;
	}
	else
	    // check if we collided with a bottom block and adjust our speed and rotation accordingly
		if (owner.collisionObject.name.IndexOf("bottom") == 0 && speed.y < 0)
		{
			speed = new Vector2(speed.x, speed.y * -1);
			if ((speed.x < 0 && rotation < 0) || (speed.x > 0 && rotation > 0))
				rotation *= -1;
		}
		else
		    // check if we collided with a left block and adjust our speed and rotation accordingly
			if (owner.collisionObject.name.IndexOf("left") == 0 && speed.x < 0)
			{
				speed = new Vector2(speed.x * -1, speed.y);
				if ((speed.y < 0 && rotation > 0) || (speed.y > 0 && rotation < 0))
					rotation *= -1;
			}
			else
			    // check if we collided with a right block and adjust our speed and rotation accordingly
				if (owner.collisionObject.name.IndexOf("right") == 0 && speed.x > 0)
				{
					speed = new Vector2(speed.x * -1, speed.y);
					if ((speed.y < 0 && rotation < 0) || (speed.y > 0 && rotation > 0))
						rotation *= -1;
				}
}
*/