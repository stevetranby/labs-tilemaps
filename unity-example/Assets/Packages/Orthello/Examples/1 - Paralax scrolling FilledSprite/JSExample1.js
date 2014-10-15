// ------------------------------------------------------------------------
// Orthello 2D Framework Example Source Code 
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
// Example 1
// Using Mutiple FilledSprite(s) to create
// a paralax scrolling background effect.
// ------------------------------------------------------------------------
// Main Example 1 Demo class 
// ------------------------------------------------------------------------

/*
	var initialized = false;
    var scrolling = false;

    // This function will resize the a FilledSprite ( provided by name )
    // to match the current view (resolution).
    function  Resize(spriteName:String)
    {
		// IMPORTANT : if you would assign a value with the declaration like:  
		//    var sprite = OT.ObjectByName(spriteName);
		// It will render you a compile error because sprite will be strongly typed to an OTObject.
		// So .... first declare a var so it is initialized as an JS object
        var sprite:OTFilledSprite;
        // lookup the FilledSprite object
		sprite = OT.ObjectByName(spriteName) as OTFilledSprite;
        if (sprite != null)
        {
            // we found our FilledSprite so set the size
            sprite.size = new Vector2(Screen.width, Screen.height);
        }
    }

    // application initialization.
    // This method is called from the Update() function so we can be sure that
    // all Orthello objects have been started.
    function Initialize()
    {
        // resize filled sprites to match screen size
        Resize("filled 1");
        Resize("filled 2");
        Resize("filled 3");
        Resize("filled 4");
        // set initialized notifier to true so we only initialize once.
        initialized = true;
    }

    // Set scroll speed for a specific FilledSprite providing its name
    function SetScroll(spriteName: String, scrollSpeed: Vector2)
    {		
		// IMPORTANT : if you would assign a value with the declaration like:  
		//    var sprite = OT.ObjectByName(spriteName);
		// It will render you a compile error because sprite will be strongly typed to an OTObject.
		// So .... first declare a var so it is initialized as an JS object
		var sprite : OTFilledSprite;		
		// get filled sprite
		sprite = OT.ObjectByName(spriteName) as OTFilledSprite;
        // set scroll speed
 		if (sprite != null)
			sprite.scrollSpeed = scrollSpeed;		
    }
	
	// Update is called once per frame
	function Update () {
        // Only go on if Orthello is valid.
        if (!OT.isValid) return;
        // check if we have to initialize
        if (!initialized)
            Initialize();

        // only scroll when left mouse button pressed
        if (Input.GetMouseButton(0))
        {
            // get delta position relative to screen center
            var delta = (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0));
            delta = new Vector2(delta.x / Screen.width, delta.y / Screen.height);
            // set scroll speed of layers - the more backwards the less scroll
            SetScroll("filled 1", delta * 60);
            SetScroll("filled 2", delta * 50);
            SetScroll("filled 3", delta * 40);
            SetScroll("filled 4", delta * 30);
            scrolling = true;
        }
        else
        {
            if (scrolling)
            {
                // stop scrolling
                SetScroll("filled 1", Vector2.zero);
                SetScroll("filled 2", Vector2.zero);
                SetScroll("filled 3", Vector2.zero);
                SetScroll("filled 4", Vector2.zero);
                scrolling = false;
            }
        }
	}
	
*/