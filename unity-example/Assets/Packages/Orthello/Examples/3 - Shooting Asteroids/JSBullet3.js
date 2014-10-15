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
// Bullet behaviour class
// ------------------------------------------------------------------------

/*
private var sprite:OTSprite;                            // this bullet's sprite class
private var app:JSExample3;                              // main application class

private var speed:int = 1000;                           // bullet speed
private var ignoreCollisions:Number = 0;                // ignore debree collisions time

private var debree:Array = new Array();                 // created debree list 

// Use this for initialization
function Start () {
    // Get application main class
    app = Camera.main.GetComponent("JSExample3");
    // Get this bullet's sprite class
    sprite = GetComponent("OTSprite");
	// Because Javascript does not support C# delegate we have to notify our sprite class that 
	// we want to receive notification callbacks.
	// If we have initialized for callbacks our (!IMPORTANT->) 'public' declared call back 
	// functions will be asutomaticly called when an event takes place.
	// HINT : This technique can be used within a C# class as well.
    sprite.InitCallBacks(this);
}

// Update is called once per frame
function Update () {
    // Check if we have to ignore colliding with created debree
    if (ignoreCollisions > 0)
    {
        // increase ignore time
        ignoreCollisions -= Time.deltaTime;
        if (ignoreCollisions < 0) ignoreCollisions = 0;
    }
    else
    {
        // we no longer have to ignore the created debree so
        // lets clear the debree list.
        debree = new Array();
    }
    // Update bullet position
    sprite.position += sprite.yVector * speed * Time.deltaTime;
    // Destroy bullet as it moves out of view
    if (sprite.outOfView) 
      OT.DestroyObject(sprite);
}

// This method will add a debree object to the ignore debree list.
// We will have to maintain a ignore debree list because if we dont,
// this bullet can generate a 'recursive' exploding state that will
// create LOTS and LOTS of debree, totaly hanging this application
public function AddDebree(debreeObject:OTAnimatingSprite)
{
    debree.push(debreeObject);
}

// OnCollision callback function  is called when this bullet collides with another 'collidable' object
// !IMPORTANT - This sprite's collidable setting has to be true otherwide
// collision delegates will not be called
public function OnCollision(owner:OTObject)
{
    // check if the asteroid we are colliding with is not in our
    // ignore debree list.		
	var found:boolean = false;
	for (var i:int = 0; i<debree.length; i++)
		if (debree[i] == owner.collisionObject)
		{
			found = true;
			break;
		}
	
    if (!found)
    {
        // We have to ignore debree the following 0.1 seconds
        ignoreCollisions = 0.1f;
        // Lets Explode this asteroid
        app.Explode(owner.collisionObject, this);
    }
}
*/