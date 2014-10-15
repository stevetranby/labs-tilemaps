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
// Bullet behaviour class
// ------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CBullet3 : MonoBehaviour {

    OTSprite sprite;                            // this bullet's sprite class
    CExample3 app;                              // main application class

    int speed = 1000;                           // bullet speed
    float ignoreCollisions = 0;                 // ignore debree collisions time

    List<OTAnimatingSprite> debree =            // created debree list 
        new List<OTAnimatingSprite>();

	// Use this for initialization
	void Start () {
        // Get application main class
        app = Camera.main.GetComponent<CExample3>();
        // Get this bullet's sprite class
        sprite = GetComponent<OTSprite>();
        // Set this sprite's collision delegate
        // HINT : We could use sprite.InitCallBacks(this) as well.
        // but because delegates are the C# way we will use this technique
        sprite.onCollision = OnCollision;
	}
	
	// Update is called once per frame
	void Update () {
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
            debree.Clear();
        }
				
        // Update bullet position
        sprite.position += (Vector2)sprite.yVector	* speed * Time.deltaTime;
        // Destroy bullet as it moves out of view
        if (sprite.outOfView) 
          OT.DestroyObject(sprite);
	}

    // This method will add a debree object to the ignore debree list.
    // We will have to maintain a ignore debree list because if we dont,
    // this bullet can generate a 'recursive' exploding state that will
    // create LOTS and LOTS of debree, totaly hanging this application
    
    public void AddDebree(OTAnimatingSprite debreeObject)
    {
        debree.Add(debreeObject);
    }

    // OnCollision delegate is called when this bullet collides with another 'collidable' object
    // !IMPORTANT - This sprite's collidable setting has to be true otherwide
    // collision delegates will not be called
    
    public void OnCollision(OTObject owner)
    {
        // check if the asteroid we are colliding with is not in our
        // ignore debree list.
        if (debree.IndexOf(owner.collisionObject as OTAnimatingSprite) < 0)
        {
            // We have to ignore debree the following 0.1 seconds
            ignoreCollisions = 0.1f;
            // Lets Explode this asteroid
            app.Explode(owner.collisionObject, this);
        }
    }

}
