// ------------------------------------------------------------------------
// Orthello 2D Framework Example Source Code
// (C)opyright 2011 - WyrmTale Games - http://www.wyrmtale.com
// ------------------------------------------------------------------------
// More info http://www.wyrmtale.com/orthello
// ------------------------------------------------------------------------
// Example 2 - Star behaviour class
// ------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class CStar2 : MonoBehaviour {

    OTSprite sprite;                    // This star's sprite class
    Vector2 speed =                     // Star movement speed / second
        new Vector2(150, 150);          
    float rotation = 90;                // Star rotation speed / second 

    Color startColor =
        new Color(0.5f, 1f, 0.5f);    // Star's tint color
    Color stayColor = 
        new Color(1f, 1f, 1f);     // Star's tint color when overlapping

	// Use this for initialization
	void Start () {
        // get this star's sprite class
        sprite = GetComponent<OTSprite>();
        // Set this sprite's stay/exit/collision delegates
        // HINT : We could use sprite.InitCallBacks(this) as well.
        // but because delegates are the C# way we will use this technique
        sprite.onStay = OnStay;
        sprite.onExit = OnExit;
        sprite.onCollision = OnCollision;
        // Create a random speed for this star
        speed = new Vector2(150 + 150 * Random.value, 150 + 150 * Random.value);
        // Set star's color
        sprite.tintColor = startColor;
        // register the start material so we can use it later for assignment
        OT.RegisterMaterial("Star-start", new Material(sprite.material));
        var m = new Material(sprite.material);
        // register the material so we can use it later for assignment
        m.SetColor("_EmisColor", stayColor);
        OT.RegisterMaterial("Star-stay", m);
    }
	
	// Update is called once per frame
	void Update () {
        // adjust this star's position
        sprite.position += speed * Time.deltaTime;
        // adjust this star's rotation
        sprite.rotation  += (rotation * Time.deltaTime);
    }

    // OnStay delegate is called when star enters (overlaps) another 'collidable' object
    // !IMPORTANT - This sprite's collidable setting has to be true otherwide
    // collision delegates will not be called
    
    public void OnStay(OTObject owner)
    {
        // check if we entered another star and adjust color if we did
        if (owner.collisionObject.name.IndexOf("star") == 0)
            sprite.material = OT.LookupMaterial("Star-stay");
    }
    // OnExit delegate is called when star no longer overlaps another 'collidable' object
    
    public void OnExit(OTObject owner)
    {
        // check if we have left another star and adjust color if we did
        if (owner.collisionObject.name.IndexOf("star") == 0)
            sprite.material = OT.LookupMaterial("Star-start");
    }
    // OnCollision delegate is called when star collides with another 'collidable' object
    // HINT - OnEnter and OnCollision delegates are called exactly at the same time, the only
    // difference is their naming convention
    
    public void OnCollision(OTObject owner)
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

}
