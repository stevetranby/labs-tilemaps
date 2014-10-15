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
// Example 6
// Physics example
// ------------------------------------------------------------------------
// Main Example 6 Demo class
// ------------------------------------------------------------------------

/*
function Start()
{
	// set gravity manually
	Physics.gravity = new Vector3(0,-450,0);
}

// rotate a physical static object
function Rotate(name:String)
{
    var o : OTObject  = OT.ObjectByName(name);    
    if (o != null)
        o.rotation += (90 * Time.deltaTime);
}

 // destroy a 'falling' object/sprite as soon as it is out of view
function OnOutOfView(owner:OTObject)
{
    OT.DestroyObject(owner);
}

var it:Number = 0;

// Update is called once per frame
function Update () {
    it += Time.deltaTime;
    if (it > 0.15f)
    {
    	// check each 0.15 seconds if we want to create a 'falling' sprite
        it = 0;
        if (Random.value > 0.65f)
        {
        	// create a 'falling' sprite
            var sp : OTSprite = null;
            var si:Number = 20 + Random.value * 50;
            if (Random.value > 0.5f)
            {
            	// lets create a new block from prototype
                sp = OT.CreateObject("block").GetComponent("OTSprite");
                sp.size = new Vector2(si, 20 + Random.value * 50);
            }
            else
            {
            	// lets create a new star from prototype
                sp = OT.CreateObject("star").GetComponent("OTSprite");
                sp.size = new Vector2(si, si);
            }

            sp.gameObject.rigidbody.mass = si;
            sp.position = new Vector2(-200 + Random.value * 400, 300);
            sp.InitCallBacks(this);            
        }
    }
    Rotate("rot");
    Rotate("rot_s1");
    Rotate("rot_s2");
    Rotate("rot_s3");
}

public function OnCollision(owner: OTObject)
{
	// a collision occured
	// OT.print(owner.name+" collided with "+owner.collisionObject.name+" at "+owner.collision.contacts[0].point);
}
*/