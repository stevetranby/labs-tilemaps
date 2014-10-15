// ------------------------------------------------------------------------
// Orthello 2D Framework Example Source Code
// (C)opyright 2011 - WyrmTale Games - http://www.wyrmtale.com
// ------------------------------------------------------------------------
// More info http://www.wyrmtale.com/orthello
// ------------------------------------------------------------------------
// Using Mutiple FilledSprite(s) to create
// a paralax scrolling background effect.
// ------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
// Main Example 1 Demo class 

public class CExample1 : MonoBehaviour
{

    bool initialized = false;           // initialization notifier
    bool scrolling = false;             // scrolling notifier

    // This method will resize the a FilledSprite ( provided by name )
    // to match the current view (resolution).
    void Resize(string spriteName)
    {
        // Lookup the FilledSprite using its name.
        OTObject sprite = OT.ObjectByName(spriteName);
        if (sprite != null)
        {
            // We found the sprite so lets size it to match the screen's resolution
            // We will assume the OTView.zoom factor is set to zero (no zooming)
            sprite.size = new Vector2(Screen.width, Screen.height);
        }
    }

    // application initialization.
    // This method is called from the Update() function so we can be sure that
    // all Orthello objects have been started.
    void Initialize()
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
    void SetScroll(string spriteName, Vector2 scrollSpeed)
    {
        // lookup sprite
        OTObject sprite = OT.ObjectByName(spriteName);
        // set scroll speed
        if (sprite != null)
            (sprite as OTFilledSprite).scrollSpeed = scrollSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        // Only go on if Orthello is valid.
        if (!OT.isValid) return;

        // check if we have to initialize
        if (!initialized)
            Initialize();

        // only scroll when left mouse button pressed
        if (Input.GetMouseButton(0))
        {
            // get delta position relative to screen center
            Vector2 delta = (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0));
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
            // check if we are scrolling
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
}
