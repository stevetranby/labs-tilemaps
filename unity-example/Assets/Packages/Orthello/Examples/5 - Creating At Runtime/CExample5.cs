// ------------------------------------------------------------------------
// Orthello 2D Framework Example Source Code
// (C)opyright 2011 - WyrmTale Games - http://www.wyrmtale.com
// ------------------------------------------------------------------------
// More info http://www.wyrmtale.com/orthello
// ------------------------------------------------------------------------
// Example 5
// Creation of objects by code
// ------------------------------------------------------------------------
// Main Example 5 Demo class
// ------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CExample5 : MonoBehaviour {

    
    public Texture greenStarsAnimation;     // texture with rotating star frames
    
    public Texture walkingManAnimation;     // texture with walking man frames
    
    public Texture whiteTile;               // Just a white wyrmtale tile
    
    public GameObject stage1;               // stage1 menu itmem
    
    public GameObject stage2;               // stage2 menu itmem

    bool initialized = false;           // initialization indicator
    int appMode = 1;                    // application mode ( 1 = stars, 2 = walking man )

    OTAnimatingSprite star;             // center star animating sprite, used as prototype
    List<OTAnimatingSprite> stars =     // moving stars
        new List<OTAnimatingSprite>();
    OTAnimatingSprite man;              // walking man animating sprite
    OTFilledSprite back;                // background
    float starSpeed = 0.25f;            // star creation frequency
    float starTime = 0;                 // star creation wait time

    TextMesh stagemesh1;                // textmesh for stage 1 menu item
    TextMesh stagemesh2;                // textmesh for stage 1 menu item
   
    // Create all objects fo    r the star stage by code
    void AnimatingGreenStarStage()
    {
        // To create a scrolling background, first create a filled sprite object
        back = OT.CreateObject(OTObjectType.FilledSprite).GetComponent<OTFilledSprite>();
        // Set the image to the tile texture
        back.image = whiteTile;
        // Set material reference to 'custom' blue material
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
        OTSpriteSheet sheet = OT.CreateObject(OTObjectType.SpriteSheet).GetComponent<OTSpriteSheet>();
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
        OTAnimation animation = OT.CreateObject(OTObjectType.Animation).GetComponent<OTAnimation>();
        // This animation will use only one frameset with all animating star frames. 
        OTAnimationFrameset frameset = new OTAnimationFrameset();
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
        animation.framesets = new OTAnimationFrameset[] { frameset };
        // Set the duration of this animation
        // HINT : By using the OTAnimationSprite.speed setting one can speed up or slow down a
        // running animation without changing the OTAnimation.fps or .duration setting.
        animation.duration = .95f;
        // Lets give our animation a name
        animation.name = "star-animation";

        // To finally get the animatiing star on screen we will create our animation sprite object
        star = OT.CreateObject(OTObjectType.AnimatingSprite).GetComponent<OTAnimatingSprite>();
        // Link up the animation
        star.animation = animation;
        // Lets start at a random frame
        star.startAtRandomFrame = true;
        // Lets auto-start this animation
        star.playOnStart = true;       
		// start invisible and make visible when containers are ready.
		star.visible = false;
		star.name = "star";
		star.spriteContainer = sheet;

        // INFO : This animation 'star' will be center (0,0) positioned and act as a prototype
        // to create more moveing and animating (additive) 'stars'.
    }

    // Create an walking man animation frameset with 15 images
    // INFO : Because our walking man sprite sheet contains 8 direction
    // animations of 15 frames. We will put these into seperate animation
    // framesets, so they can be played quickly when needed.
    OTAnimationFrameset WalkingFrameset(string name, int row, OTContainer sheet)
    {
        // Create a new frameset
        OTAnimationFrameset frameset = new OTAnimationFrameset();
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
    void WalkingBlueManStage()
    {
        // To create the background lets create a filled sprite object
        back = OT.CreateObject(OTObjectType.FilledSprite).GetComponent<OTFilledSprite>();
        // Set the image to our wyrmtale tile
        back.image = whiteTile;
        // But this all the way back so all other objects will be located in front.
        back.depth = 1000;
        // Set material reference to 'custom' green material - check OT material references
        back.materialReference = "green";
        // Set the size to match the screen resolution.
        back.size = new Vector2(Screen.width, Screen.height);
        // Set the fill image size to 50 x 50 pixels
        back.fillSize = new Vector2(50, 50);

        // To create the walking man animation we first will have to create a sprite sheet
        OTSpriteSheet sheet = OT.CreateObject(OTObjectType.SpriteSheet).GetComponent<OTSpriteSheet>();
        // Link our walking man frames
        sheet.texture = walkingManAnimation;
        // specify the number or column and rows (frames) of this container
        sheet.framesXY = new Vector2(15, 8);

        // The next step is to create our animation object that will hold all
        // animation framesets (8 directions of walking) for our walking man
        OTAnimation animation = OT.CreateObject(OTObjectType.Animation).GetComponent<OTAnimation>();
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
        man = OT.CreateObject(OTObjectType.AnimatingSprite).GetComponent<OTAnimatingSprite>();
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
		// start invisible and make visible when containers are ready.
		man.visible = false;
		man.spriteContainer = sheet;

        // INFO : In this class Update() method, we will check the location of
        // the mouse pointer and play the corresponding direction animation
        // as we will set the right scroll speed for our background.
    }

    // (Re)Create our objects
    void CreateObjects()
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
    void Initialize()
    {
        // Create all application objects 
        CreateObjects();
        // Get TextMesh objects to color menu's on hover later
        stagemesh1 = stage1.GetComponent<TextMesh>();
        stagemesh2 = stage2.GetComponent<TextMesh>();
        // indicate that we have initialized
        initialized = true;
    }

	// Update is called once per frame
	void Update () {
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
                    OTAnimatingSprite newStar = OT.CreateSprite(star) as OTAnimatingSprite;
                    // Put this star in our active stars list
                    stars.Add(newStar);
                    // Give it a random size
                    newStar.size *= (0.3f + Random.value * 2.5f);
                    // Give it a random position on the right border of the screen
                    newStar.position = new Vector2((Screen.width / 2) + newStar.size.x / 2,
                       ((Screen.height / 2) * -1) + newStar.size.y/2 + Random.value * (Screen.height - newStar.size.y));
                    // Calculate the depth (smaller stars to the back, bigger to the front)
                    newStar.depth = (int)((1 / newStar.size.x)*100);
                    // Set material to additive
                    newStar.additive = true;
                    newStar.frameIndex = 0;
                }

                // Lets loop all active stars
                // HINT : Because we will be adjusting (removing) items as they get out of view,
                // we better not use a for() loop. While() is the better way for this.
                int s=0;
                while (s<stars.Count)
                {
                    // get next active star
                    OTAnimatingSprite dStar = stars[s];
                    // increase its position
                    dStar.position += new Vector2(stars[s].size.x * 3 * Time.deltaTime * -1, 0);
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
				man.visible = true;
                // we are in the walking man stage so calculate a normalized vector from
                // our man to the mouse pointer
                Vector2 mouseVector = (OT.view.mouseWorldPosition - man.position).normalized;
                // The Atan2 will give you a -3.141 to 3.141 range depending of your vector x/y values
                float angle = Mathf.Atan2(mouseVector.x, mouseVector.y);
                // Play the right frameset dependent on the angle
                float part = 6.2f / 8;
				string anim = "";
                if (angle > -1 * (part / 2) && angle <= (part / 2)) anim = "up";
                else
                    if (angle > -3 * (part / 2) && angle <= -1 * (part / 2)) anim = "upLeft";
                    else
                        if (angle > -5 * (part / 2) && angle <= -3 * (part / 2)) anim = "left";
                        else
                            if (angle > -7 * (part / 2) && angle <= -5 * (part / 2)) anim = "downLeft";
                            else
                                if (angle > (part / 2) && angle <= 3 * (part / 2)) anim = "upRight";
                                else
                                    if (angle > 3 * (part / 2) && angle <= 5 * (part / 2)) anim = "right";
                                    else
                                        if (angle > 5 * (part / 2) && angle <= 7 * (part / 2)) anim = "downRight";
                                        else
                                            anim = "down";
				man.PlayLoop(anim);
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

}
