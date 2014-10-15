// ------------------------------------------------------------------------
// Orthello 2D Framework Example Source Code
// (C)opyright 2011 - WyrmTale Games - http://www.wyrmtale.com
// ------------------------------------------------------------------------
// More info http://www.wyrmtale.com/orthello
// ------------------------------------------------------------------------
// Example 6
// Physics example
// ------------------------------------------------------------------------
// Main Example 6 Demo class
// ------------------------------------------------------------------------
using UnityEngine;
using System.Collections;


public class CExample6 : MonoBehaviour {
	
	void Start()
	{
		// set gravity manually
		Physics.gravity = new Vector3(0,-450,0);
	}
	
    // rotate a physical static object
    void Rotate(string name)
    {
        OTObject o = OT.ObjectByName(name);
        if (o != null)
            o.rotation += (90 * Time.deltaTime);
    }

    // destroy a 'falling' object/sprite as soon as it is out of view
    void DestroyWhenOutOfView(OTObject owner)
    {
        OT.DestroyObject(owner);
    }
    
    float it = 0;   
	// Update is called once per frame
	void Update () {
        it += Time.deltaTime;
        if (it > 0.15f)
        {
            // check each 0.15 seconds if we want to create a 'falling' sprite
            it = 0;
            if (Random.value > 0.65f)
            {
                // create a 'falling' sprite 
                OTSprite sp = null;
                float si = 20 + Random.value * 50;
                if (Random.value > 0.5f)
                {
                    // lets create a new block from prototype
                    sp = OT.CreateObject("block").GetComponent<OTSprite>();
                    sp.size = new Vector2(si, 20 + Random.value * 50);
                }
                else
                {
                    // lets create a new star from prototype
                    sp = OT.CreateObject("star").GetComponent<OTSprite>();
                    sp.size = new Vector2(si, si);
                }

                sp.gameObject.rigidbody.mass = si;
                sp.position = new Vector2(-200 + Random.value * 400, 300);
                sp.onOutOfView = DestroyWhenOutOfView;
				sp.onCollision = CollisionOccured;
            }
        }

        Rotate("rot");
        Rotate("rot_s1");
        Rotate("rot_s2");
        Rotate("rot_s3");
    }
	
	void CollisionOccured(OTObject owner)
	{
		// a collision occured
		//OT.print(owner.name+" collided with "+owner.collisionObject.name+" at "+owner.collision.contacts[0].point);
	}
	
}
