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
// Main Example 4 Demo class
// ------------------------------------------------------------------------

/*
    private var initialized:boolean = false;               // intialization indicator
    private var zooming:boolean = false;                   // zooming indicator
    private var zoomSpeed:Number = 4f;                   // how fast do we zoom in/out
    private var zoomMin:Number = -2.5f;                  // Zoomed out value
    private var zoomMax:Number = -1f;                    // Zoomed in value

    // Create movers
    function CreateObjects()
    {
        // lets create 10 mover sprites
        for (var i:int = 0; i < 10; i++)
        {
            var mov:JSMover4 = OT.CreateSprite("mover").GetComponent("JSMover4");
            var s:OTSprite = mov.GetComponent("OTSprite");
            // tell sprite class to deliver callbacks
            s.InitCallBacks(this);
        }
    }

    // Input handler
    public function OnInput(owner:OTObject)
    {
        // check if we clicked the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // Link View target object for movement and rotation 
            OT.view.movementTarget = owner.gameObject;
            OT.view.rotationTarget = owner.gameObject;
            // We want to zoom in on that target
            zooming = true;
        }
    }

    // Application initialization
    function Initialize()
    {
        // Change GUIText color to black
        var txt:TextMesh = (GameObject.Find("info") as GameObject).GetComponent("TextMesh");
        txt.renderer.material.color = Color.black;
        // create out objects
        CreateObjects();
        // Initialize our view
        InitView();
        // indicate that we have initialized
        initialized = true;
    }

    // Initialize view
    function InitView()
    {
        // no rotation for this view
        OT.view.rotation = 0;
        // position x/y to 0/0
        OT.view.position = Vector2.zero;
    }
	
	// Update is called once per frame
	function Update () {
        // Only go on if Orthello was started correctly
        if (!OT.isValid) return;

        // Lets initialize our application once.
        if (!initialized)
            Initialize();

        // Check if the right mouse button was pressed
        if (Input.GetMouseButtonDown(1))
        {
            // Drop our view follow target
            OT.view.movementTarget = null;
            OT.view.rotationTarget = null;
            // Set our view to its initial state
            InitView();
            // Lets start zooming out
            zooming = true;
        }

        if (zooming)
        {
            // we are zooming in or out
            if (OT.view.movementTarget != null)
            {
                // zooming in
                OT.view.zoom += zoomSpeed * Time.deltaTime;
                // cap zooming at max
                if (OT.view.zoom > zoomMax)
                {
                    OT.view.zoom = zoomMax;
                    zooming = false;
                }
            }
            else
            {
                // zooming out
                OT.view.zoom -= zoomSpeed * Time.deltaTime;
                // cap zooming at min
                if (OT.view.zoom < zoomMin)
                {
                    OT.view.zoom = zoomMin;
                    zooming = false;
                }
            }
        }
	}
	
*/