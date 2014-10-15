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
// Example 2
// Using 'collidable' sprites and handle collisions
// ------------------------------------------------------------------------
// Main Example 2 Demo
// ------------------------------------------------------------------------

/*
private var blockPrototype:OTSprite;     // prototype to create blocks
private var starPrototype:OTSprite;      // prototype to create stars

var initialized:boolean = false;        // intialization notifier

// Create the objects of this demo using provided prototypes
function CreateObjects()
{
    // Find the empty that will act as the block container
	var blocks:GameObject = GameObject.Find("Blocks");
	if (blocks!=null)
	{
		// Declare local variables
		var x:int = 0;
		var b :OTSprite;
	    // Calculate the horizontal number of blocks for the current resolution
		var c:int = (Screen.width - 20) / 85;
		// Calculate horizontal center spacing
		var s:int = (Screen.width - (c * 85)) / 2;
        // Create bottom horizontal blocks		
		for (x= 0; x < c; x++)
		{
		    // Create a new block
			b = (Instantiate(blockPrototype.gameObject) as GameObject).GetComponent("OTSprite");
			// Set block position
		    b.position = new Vector2(s + 50 + (x * 85)- (Screen.width/2), 20 - (Screen.height/2));
		    // Set block name
			b.name = "bottom"+x;
			// Link to parent
			b.transform.parent = blocks.transform;
		}
        // Create top horizontal blocks		
		for (x = 0; x < c; x++)
		{
		    // Create a new block
			b = (Instantiate(blockPrototype.gameObject) as GameObject).GetComponent("OTSprite");
            // Set block's position			
		    b.position = new Vector2(s + 50 + (x * 85) - (Screen.width / 2), Screen.height - 20 - (Screen.height / 2));
		    // Set block's name
			b.name = "top" + x;
			// Link to parent
			b.transform.parent = blocks.transform;
		}
        // Calculate the vertical number of blocks for the current resolution
		c = (Screen.height - 50) / 85;
        // Calculate vertical center spacing		
		s = (Screen.height - (c * 85)) / 2;
        // Create left vertical blocks		
		for (x = 0; x < c; x++)
		{
		    // Create a new block
			b = (Instantiate(blockPrototype.gameObject) as GameObject).GetComponent("OTSprite");
			// Rotate by 90 degrees
		    b.rotation = 90;
		    // Set block's position
			b.position = new Vector2(20 - (Screen.width / 2), (Screen.height/2) - 40 - s - (x*85) );
			// Set block's name
			b.name = "left" + x;
			// Link to parent
			b.transform.parent = blocks.transform;
		}
        // Create right vertical blocks		
		for (x = 0; x < c; x++)
		{
		    // Create new block
			b = (Instantiate(blockPrototype.gameObject) as GameObject).GetComponent("OTSprite");
			// Rotate by 90 degrees
			b.rotation = 90;
			// Set block's position
			b.position = new Vector2((Screen.width / 2)-20, (Screen.height / 2) - 40 - s - (x * 85));
			// Set block's name
			b.name = "right" + x;
			// Link to parent
			b.transform.parent = blocks.transform;
		}
	}

    // Find the empty that will act as the stars container
	var stars:GameObject = GameObject.Find("Stars");
	if (stars != null)
	{
	    // We will create 50 stars
		c = 50;
		var st : OTSprite;
		for (x = 0; x < c; x++)
		{
		    // Create a new star
			st = (Instantiate(starPrototype.gameObject) as GameObject).GetComponent("OTSprite");
			// Set star's random position
			st.position =
				new Vector2(
					-1 * (Screen.width / 2) + 50 + Random.value * (Screen.width - 100),
					(Screen.height / 2) - 40 - Random.value * (Screen.height - 80));
		    // Set star's name
			st.name = "star" + x;
			st.depth = 100+x;
			// Link to parent
			st.transform.parent = stars.transform;
		}
	}
}

// Application initialization
function Initialize()
{

	blockPrototype = GameObject.Find("proto-block").GetComponent("OTSprite");
	starPrototype = GameObject.Find("proto-star").GetComponent("OTSprite");
	
	CreateObjects();
	// set initialization notifier - We only need to initialize once
	initialized = true;
}

// Update is called once per frame
function Update () {
    // We will call the application initialization function once from the Update() function
    // because we will be sure that all Orthello objects have been started.
	if (!initialized)
		Initialize();
}
*/