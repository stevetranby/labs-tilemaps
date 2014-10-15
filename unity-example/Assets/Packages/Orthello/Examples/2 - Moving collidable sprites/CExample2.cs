// ------------------------------------------------------------------------
// Orthello 2D Framework Example Source Code
// (C)opyright 2011 - WyrmTale Games - http://www.wyrmtale.com
// ------------------------------------------------------------------------
// Example 2
// Using 'collidable' sprites and handle collisions
// ------------------------------------------------------------------------
// More info http://www.wyrmtale.com/orthello
// ------------------------------------------------------------------------
// Main Example 2 Demo class
// ------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class CExample2 : MonoBehaviour {
	    
    public OTSprite blockPrototype; // prototype to create blocks
    
    public OTSprite starPrototype;  // prototype to create stars

    bool initialized = false;       // initialization indicator

    // Create the objects of this demo using provided prototypes
    void CreateObjects()
    {
        // Find the empty that will act as the block container
        GameObject blocks = GameObject.Find("Blocks");
        if (blocks!=null)
        {
            // Calculate the horizontal number of blocks for the current resolution
            int c = (Screen.width - 20) / 85;
            // Calculate horizontal center spacing
            int s = (Screen.width - (c * 85)) / 2;
            // Create bottom horizontal blocks
            for (int x = 0; x < c; x++)
            {
                // Create a new block
                OTSprite b = (Instantiate(blockPrototype.gameObject) as GameObject).GetComponent<OTSprite>();
                // Set block's position
                b.position = new Vector2(s + 50 + (x * 85)- (Screen.width/2), 20 - (Screen.height/2));
                // Set block's name
                b.name = "bottom" + x;
                // Link to parent
                b.transform.parent = blocks.transform;
            }
            // Create top horizontal blocks
            for (int x = 0; x < c; x++)
            {
                // Create a new block
                OTSprite b = (Instantiate(blockPrototype.gameObject) as GameObject).GetComponent<OTSprite>();
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
            for (int x = 0; x < c; x++)
            {
                // Create a new block
                OTSprite b = (Instantiate(blockPrototype.gameObject) as GameObject).GetComponent<OTSprite>();
                // Rotate this block 90 degrees
                b.rotation = 90;
                // Set block's position
                b.position = new Vector2(20 - (Screen.width / 2), (Screen.height/2) - 40 - s - (x*85) );
                // Set block's name
                b.name = "left" + x;
                // Link block to parent
                b.transform.parent = blocks.transform;				
            }
            // Create right vertical blocks
            for (int x = 0; x < c; x++)
            {
                // Create new block
                OTSprite b = (Instantiate(blockPrototype.gameObject) as GameObject).GetComponent<OTSprite>();
                // Rotate block 90 degrees
                b.rotation = 90;
                // Set block's position
                b.position = new Vector2((Screen.width / 2)-20, (Screen.height / 2) - 40 - s - (x * 85));
                // Set block's name
                b.name = "right" + x;
                // Link block to parent
                b.transform.parent = blocks.transform;
            }
        }

        // Find the empty that will act as the stars container
        GameObject stars = GameObject.Find("Stars");
        if (stars != null)
        {
            // We will create 50 stars
            int c = 50;
            for (int x = 0; x < c; x++)
            {
                // Create a new star
                OTSprite s = (Instantiate(starPrototype.gameObject) as GameObject).GetComponent<OTSprite>();
                // Set star's random position
                s.position =
                    new Vector2(
                        -1 * (Screen.width / 2) + 50 + Random.value * (Screen.width - 100),
                        (Screen.height / 2) - 40 - Random.value * (Screen.height - 80));
                // Set star's name
                s.name = "star" + x;
                s.depth = 100 + x;
                // Link star to parent
                s.transform.parent = stars.transform;
            }
        }
    }

    // Application initialization
    void Initialize()
    {
        // Create all objects for this demo
        CreateObjects();
        // Set initialization notifier - we only need to initialize once.
        initialized = true;
    }

	
	// Update is called once per frame
	void Update () {
        // Only go one when Orthello is initialized
        if (!OT.isValid) return;

        // Call initialization once from this Update() so we can be sure all
        // Orthello objects have been started
        if (!initialized)
            Initialize();
	}
}
