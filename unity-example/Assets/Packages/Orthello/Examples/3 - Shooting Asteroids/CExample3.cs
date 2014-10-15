// ------------------------------------------------------------------------
// Orthello 2D Framework Example Source Code
// (C)opyright 2011 - WyrmTale Games - http://www.wyrmtale.com
// ------------------------------------------------------------------------
// More info http://www.wyrmtale.com/orthello
// ------------------------------------------------------------------------
// Example 3
// Using 'collidable' animating sprites and handle collisions
// - asteroid 'full' animation
// - gun : 2 single frameset (idle/shoot) animation
// ------------------------------------------------------------------------
// Main Example 3 Demo class
// ------------------------------------------------------------------------
using UnityEngine;
using System.Collections;


public class CExample3 : MonoBehaviour {

    // sprite prototypes that will be used when creating objects

    OTAnimatingSprite gun;              // gun sprite reference
    bool initialized = false;           // initialization notifier

    int dp = 100;
    // This method will create an asteroid at a random position on screen and with
    // relative min/max (0-1) size. An OTObject can be provided to act as a base to 
    // determine the new size.
    OTAnimatingSprite RandomBlock(Rect r, float min, float max, OTObject o)
    {		
        // Determine random 1-3 asteroid type
        int t = 1 + (int)Mathf.Floor(Random.value * 3);
        // Determine random size modifier (min-max)
        float s = min + Random.value * (max - min);
		OTSprite sprite = null;
        // Create a new asteroid
        switch (t)
        {
            case 1: sprite = OT.CreateSprite("asteroid1");
                break;
            case 2: sprite = OT.CreateSprite("asteroid2");
                break;
            case 3: sprite = OT.CreateSprite("asteroid3");
                break;
        }
		// big blocks start invisible and will be faded.
		if (o == null)
			sprite.alpha = 0;
        if (sprite != null)
        {
            // Set sprite's size
	        if (o != null)
	            sprite.size = o.size * s;
	        else
	            sprite.size = sprite.size * s;
            // Set sprite's random position
            sprite.position = new Vector2(r.xMin + Random.value * r.width, r.yMin + Random.value * r.height);
            // Set sprote's random rotation
            sprite.rotation = Random.value * 360;
            // Set sprite's name
            sprite.depth = dp++;
            if (dp > 750) dp = 100;
        }
		
		// fade in the (big) asteroid
		if (o == null)
			new OTTween(sprite,0.75f,OTEasing.Linear).Wait(Random.value * 1).Tween("alpha",0,1);
		
        return sprite as OTAnimatingSprite;
    }
    

    // Create objects for this application
    void CreateObjectPools()
    {
		//OT.PreFabricate("asteroid1",250);
		//OT.PreFabricate("asteroid2",250);
		//OT.PreFabricate("asteroid3",250);		
    }

    // application initialization
    void Initialize()
    {
        // Get reference to gun animation sprite
        gun = OT.ObjectByName("gun") as OTAnimatingSprite;
		
        // Set gun animation finish delegate
        // HINT : We could use sprite.InitCallBacks(this) as well.
        // but because delegates are the C# way we will use this technique
        gun.onAnimationFinish = OnAnimationFinish;
		
        CreateObjectPools();
        // set our initialization notifier - we only want to initialize once
        initialized = true;
    }

    // This method will explode an asteroid
    
    public void Explode(OTObject o, CBullet3 bullet)
    {
        // Determine how many debree has to be be created
        int blocks = 2 + (int)Mathf.Floor(Random.value * 2);
        // Create debree
        for (int b = 0; b < blocks; b++)
        {
            // Shrink asteroid's rect to act as the random position container
            // for the debree									
            Rect r = new Rect(
                o.rect.x + o.rect.width / 4,
                o.rect.y + o.rect.height / 4,
                o.rect.width / 2,
                o.rect.height / 2);
            // Create a debree that is relatively smaller than the asteroid that was detroyed
            OTAnimatingSprite a = RandomBlock(r, 0.6f, 0.75f, o);
						
            // Add this debree to the bullet telling the bullet to ignore this debree
            // in this update cycle - otherwise the bullet explosions could enter some
            // recursive 'dead' loop creating LOTS of debree
            bullet.AddDebree(a);
            // Recusively explode 2 asteroids if they are big enough, to get a nice
            // exploding debree effect.
            if (b < 2 && a.size.x > 30)
                Explode(a, bullet);
        }
        // Notify that this asteroid has to be destroyed
        OT.DestroyObject(o);		
    }
	

    
	// Update is called once per frame
	void Update () {
		
        // only go one if Orthello is initialized
        if (!OT.isValid) return;

        // We call the application initialization function from Update() once 
        // because we want to be sure that all Orthello objects have been started.
        if (!initialized)
        {
            Initialize();
            return;
        }
        // Rotate the gun animation sprite towards the mouse on screen
        gun.RotateTowards(OT.view.mouseWorldPosition);
        // Rotate our bullet prototype as well so we will instantiate a
        // 'already rotated' bullet when we shoot
		
		
        // check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Create a new bullet
            OTSprite nBullet = OT.CreateSprite("bullet");
            // Set bullet's position at approximately the gun's shooting barrel
            nBullet.position = gun.position + (gun.yVector * (gun.size.y / 2));
        	nBullet.rotation = gun.rotation;
            // Play the gun's shooting animation frameset once
            gun.PlayOnce("shoot");
        }

        //If we have less than 15 objects within Orthello we will create a random asteroid
        if (OT.objectCount <= 15)
           RandomBlock(OT.view.worldRect, 0.6f, 1.2f, null);        
	}

    // The OnAnimationFinish delegate will be called when an animation or animation frameset
    // finishes playing.
    
    public void OnAnimationFinish(OTObject owner)
    {
        if (owner == gun)
        {
            // Because the only animation that finishes will be the gun's 'shoot' animation frameset
            // we know that we have to switch to the gun's looping 'idle' animation frameset
            gun.PlayLoop("idle");
        }
    }
	
}

