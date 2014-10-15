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
// Example 5
// Creation of objects by code
// ------------------------------------------------------------------------
// Main Example 5 Demo class
// ------------------------------------------------------------------------

    public var greenStarsAnimation:Texture;     // texture with rotating star frames
    public var walkingManAnimation:Texture;     // texture with walking man frames
    public var whiteTile:Texture;               // Just a white wyrmtale tile
	public var stage1:GameObject;				// stage1 menu item game object	
	public var stage2:GameObject;				// stage2 menu item game object
	
/*	                
    private var initialized:boolean = false;    // initialization indicator
    private var appMode:int = 1;                // application mode ( 1 = stars, 2 = walking man )

    private var star:OTAnimatingSprite;         // center star animating sprite, used as prototype
    private var stars:Array =                   // moving stars
        new Array();
    private var man:OTAnimatingSprite;          // walking man animating sprite
    private var back:OTFilledSprite;            // background
    private var starSpeed:float = 0.25f;        // star creation frequency
    private var starTime:float= 0;              // star creation wait time
	
	private var stagemesh1:TextMesh;		// Textmesh for stage 1 menuItem
	private var stagemesh2:TextMesh;		// Textmesh for stage 2 menuItem
   
    // Create all objects for the star stage by code
    function AnimatingGreenStarStage()
    {
        // To create a scrolling background, first create a filled sprite object
        back = OT.CreateObject(OTObjectType.FilledSprite).GetComponent("OTFilledSprite");
        back.position = new Vector2(0,0);
        // Set the image to the tile texture
        back.image = whiteTile;
        // Set the 'custom' material for this background
        back.materialReference = "blue";
        // Set size to match the screen resolution so it will fill the entire screen
        back.size = new Vector2(Screen.width,Screen.height);
        // Set the display depth all the way back so everything else will come on top
        back.depth = 1000;
        // Set fill image size to 50 x 50 pixels
        back.fillSize = new Vector2(50, 50);
        // Set scroll speed so we will scroll horizontally 20 px / second
        back.scrollSpeed = new Vector2(20, 0);

        // To create the animating star we first have to create the sprite sheet object that will 
        // hold our texture/animation frames
        var sheet:OTSpriteSheet = OT.CreateObject(OTObjectType.SpriteSheet).GetComponent("OTSpriteSheet");
	    //  Give our sheet a name
	    sheet.name = "mySheet";
        // Assign texture
        sheet.texture = greenStarsAnimation;
        // Specify how many columns and rows we have (frames)
        sheet.framesXY = new Vector2(25, 3);
        // Because we have some overhead space to the right and bottom of our texture we have to
        // specify the original texture size and the original frame size so that the right texture 
        // scale and offsetting can be calculated.
        sheet.sheetSize = new Vector2(2048, 256);
        sheet.frameSize = new Vector2(80, 80);

        // Next thing is to create the animation that our animating sprite will use
        var animation:OTAnimation = OT.CreateObject(OTObjectType.Animation).GetComponent("OTAnimation");
        // This animation will use only one frameset with all animating star frames. 
        var frameset:OTAnimationFrameset = new OTAnimationFrameset();
        // Link up the sprite sheet to this animation's frameset
        frameset.container = sheet;
        // Frameset animation will start at frame index 0
        frameset.startFrame = 0;
        // Frameset animation will end at frame index 69
        frameset.endFrame = 69;
        // Assign this frameset to the animation.
        // HINT : by asigning more than one frameset it is possible to create an animation that will span
        // across mutiple framesets
        // HINT : Because it is possible (and very easy) to play a specific frameset of an animation
        // One could pack a lot of animations into one animation object.
        animation.framesets = [ frameset ];
        // Set the duration of this animation
        // HINT : By using the OTAnimationSprite.speed setting one can speed up or slow down a
        // running animation without changing the OTAnimation.fps or .duration setting.
        animation.duration = .95f;
        // Lets give our animation a name
        animation.name = "star-animation";

        // To finally get the animatiing star on screen we will create our animation sprite object
        star = OT.CreateObject(OTObjectType.AnimatingSprite).GetComponent("OTAnimatingSprite");
        // Link up the animation
        star.animation = animation;
        // Lets start at a random frame
        star.startAtRandomFrame = true;
        // Lets auto-start this animation
        star.playOnStart = true;  
        star.visible = false;     
        star.name = "star";
        // assign the sheet to the sprite as well to avoid a material blinking error
		star.spriteContainer = sheet;
        
        // INFO : This animation 'star' will be center (0,0) positioned and act as a prototype
        // to create more moveing and animating (additive) 'stars'.
    }

    // Create an walking man animation frameset with 15 images
    // INFO : Because our walking man sprite sheet contains 8 direction
    // animations of 15 frames. We will put these into seperate animation
    // framesets, so they can be played quickly when needed.
    function WalkingFrameset(name:String, row:int, sheet:OTContainer )
    {
        // Create a new frameset
        var frameset:OTAnimationFrameset = new OTAnimationFrameset();
        // Give this frameset a name for later reference
        frameset.name = name;
        // Link our provided sprite sheet
        frameset.container = sheet;
        // Set the correct start frame
        frameset.startFrame = (row - 1) * 15;
        // Set the correct end frame
        frameset.endFrame = ((row - 1) * 15) + 14;
        // Set this frameset's animation duration that will only
        // be used when the frameset is played as a single animation
        frameset.singleDuration = 0.95f;
        // Return this new frameset
        return frameset;
    }

    // Create all objects for the 'walking man' stage
    function WalkingBlueManStage()
    {
        // To create the background lets create a filled sprite object
        back = OT.CreateObject(OTObjectType.FilledSprite).GetComponent("OTFilledSprite");
        back.position = new Vector2(0,0);
        // Set the image to our wyrmtale tile
        back.image = whiteTile;
        // Set the 'custom' material for this background
        back.materialReference = "green";
        // But this all the way back so all other objects will be located in front.
        back.depth = 1000;
        // Set the size to match the screen resolution.
        back.size = new Vector2(Screen.width, Screen.height);
        // Set the fill image size to 50 x 50 pixels
        back.fillSize = new Vector2(50, 50);

        // To create the walking man animation we first will have to create a sprite sheet
        var sheet:OTSpriteSheet = OT.CreateObject(OTObjectType.SpriteSheet).GetComponent("OTSpriteSheet");
        // Link our walking man frames
        sheet.texture = walkingManAnimation;
        // specify the number or column and rows (frames) of this container
        sheet.framesXY = new Vector2(15, 8);

        // The next step is to create our animation object that will hold all
        // animation framesets (8 directions of walking) for our walking man
        var animation:OTAnimation = OT.CreateObject(OTObjectType.Animation).GetComponent("OTAnimation");
        // Initialize our animation framesets so it can hold 8 framesets
        animation.framesets = new OTAnimationFrameset[8];
        // Add the 8 direction framesets
        animation.framesets[0] = WalkingFrameset("down",1, sheet);
        animation.framesets[1] = WalkingFrameset("downLeft", 2, sheet);
        animation.framesets[2] = WalkingFrameset("left", 3, sheet);
        animation.framesets[3] = WalkingFrameset("upLeft", 4, sheet);
        animation.framesets[4] = WalkingFrameset("up", 5, sheet);
        animation.framesets[5] = WalkingFrameset("upRight", 6, sheet);
        animation.framesets[6] = WalkingFrameset("right", 7, sheet);
        animation.framesets[7] = WalkingFrameset("downRight", 8, sheet);
        // Give our animation a name
        animation.name = "walking-animation";

        // To put our walking man on screen we create an animting sprite object
        man = OT.CreateObject(OTObjectType.AnimatingSprite).GetComponent("OTAnimatingSprite");
        // Set the size of our walking man
        man.size = new Vector2(40, 65);
        // Link our animation
        man.animation = animation;
        // Lets play a single frameset .. we start with 'down'
        man.animationFrameset = "down";
        // Auto-start to play this animation frameset
        man.playOnStart = true;
        // Give our sprite a name
        man.name = "man";
        // assign the sheet to the sprite as well to avoid a material blinking error
        man.spriteContainer = sheet;

        // INFO : In this class Update() method, we will check the location of
        // the mouse pointer and play the corresponding direction animation
        // as we will set the right scroll speed for our background.
    }

    // (Re)Create our objects
    function CreateObjects()
    {
        OT.objectPooling = false;    
        // Destroy all objects, containers and animations
        OT.DestroyAll();
        // Clear our active stars list
        stars.Clear();
        // check what appMode we are in and create all objects for the 
        // corresponding stage
        switch (appMode)
        {
            case 1:
                AnimatingGreenStarStage();
                break;
            case 2:
                WalkingBlueManStage();
                break;
        }
    }

    // Application initialization
    function Initialize()
    {
        // Create all application objects 
        CreateObjects();
        // Get TextMesh objects to color menu's on hover later
        stagemesh1 = stage1.GetComponent("TextMesh");
        stagemesh2 = stage2.GetComponent("TextMesh");		
        // indicate that we have initialized
        initialized = true;
    }

	// Update is called once per frame
	function Update () {
        // only go on if Orthello is ready
	    if (!OT.isValid) return;
        // Initialize application once
        if (!initialized)
            Initialize();

		// if containers that were created are not ready yet, lets hold.
		if (!OT.ContainersReady())
			return;

        switch (appMode)
        {
            case 1:
            	star.visible = true;
                // we are in the star stage so increase star creation wait time
                starTime += Time.deltaTime;
                // check if we may create a new star
                if (starTime > starSpeed)
                {
                    // Lets create one, reset the wait time
                    starTime = 0;
                    // Create a copy of out animating star
                    var newStar:OTAnimatingSprite = OT.CreateSprite(star) as OTAnimatingSprite;
                    // Put this star in our active stars list
                    stars.Add(newStar);
                    // Give it a random size
                    newStar.size *= (0.3f + Random.value * 2.5f);
                    // Give it a random position on the right border of the screen
                    newStar.position = new Vector2((Screen.width / 2) + newStar.size.x / 2,
                       ((Screen.height / 2) * -1) + newStar.size.y/2 + Random.value * (Screen.height - newStar.size.y));
                    // Calculate the depth (smaller stars to the back, bigger to the front)
                    newStar.depth = ((1 / newStar.size.x)*100);
                    // Set material to additive
                    newStar.additive = true;
                    newStar.frameIndex = 0;
                }

                // Lets loop all active stars
                // HINT : Because we will be adjusting (removing) items as they get out of view,
                // we better not use a for() loop. While() is the better way for this.
                var s:int=0;
                while (s<stars.Count)
                {
                    // get next active star
                    var dStar:OTAnimatingSprite = stars[s];
                    // increase its position
                    dStar.position += new Vector2(dStar.size.x * 3 * Time.deltaTime * -1, 0);
                    // If the star gets out of view we will remove and destroy it
                    if (dStar.outOfView)
                    {
                        // remove from active stars list
                        stars.Remove(dStar);
                        // destroy this object
                        OT.DestroyObject(dStar);
                        // no need to increment iterator as we just removed the current element
                    }
                    else
                        s++; // increment iterator
                }
                break;
            case 2:
                // we are in the walking man stage so calculate a normalized vector from
                // our man to the mouse pointer
                var mouseVector:Vector2 = (OT.view.mouseWorldPosition - man.position).normalized;
                // The Atan2 will give you a -3.141 to 3.141 range depending of your vector x/y values
                var angle:float = Mathf.Atan2(mouseVector.x, mouseVector.y);
                // Play the right frameset dependent on the angle
                var part:float = 6.2 / 8;
                if (angle > -1 * (part / 2) && angle <= (part / 2)) man.PlayLoop("up");
                else
                    if (angle > -3 * (part / 2) && angle <= -1 * (part / 2)) man.PlayLoop("upLeft");
                    else
                        if (angle > -5 * (part / 2) && angle <= -3 * (part / 2)) man.PlayLoop("left");
                        else
                            if (angle > -7 * (part / 2) && angle <= -5 * (part / 2)) man.PlayLoop("downLeft");
                            else
                                if (angle > (part / 2) && angle <= 3 * (part / 2)) man.PlayLoop("upRight");
                                else
                                    if (angle > 3 * (part / 2) && angle <= 5 * (part / 2)) man.PlayLoop("right");
                                    else
                                        if (angle > 5 * (part / 2) && angle <= 7 * (part / 2)) man.PlayLoop("downRight");
                                        else
                                            man.PlayLoop("down");
                // adjust background scroll speed related to our mouse vector
                back.scrollSpeed = mouseVector * 10;
                break;
        }
		
        // color menu red when we are hovering over an item
        if (OT.Over(stage1))
            stagemesh1.renderer.material.color = new Color(1, .3f, .3f);
        else
            stagemesh1.renderer.material.color = new Color(1, 1, 1);

        if (OT.Over(stage2))
            stagemesh2.renderer.material.color = new Color(1, .3f, .3f);
        else
            stagemesh2.renderer.material.color = new Color(1, 1, 1);


        //check if we want to change stage
        if (OT.Clicked(stage1))
        {
            appMode = 1;
            CreateObjects();
        }
        else
            if (OT.Clicked(stage2))
            {
                appMode = 2;
                CreateObjects();
            }		
		
	}
*/