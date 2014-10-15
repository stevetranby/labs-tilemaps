// ------------------------------------------------------------------------
// Orthello 2D Framework Example Source Code
// (C)opyright 2011 - WyrmTale Games - http://www.wyrmtale.com
// ------------------------------------------------------------------------
// More info http://www.wyrmtale.com/orthello
// ------------------------------------------------------------------------
// Example 4
// Let the view follow a target
// - OTView object linking/unlinking
// - OnInput handling
// - OnMouseEnter/OnMouseExit handling
// ------------------------------------------------------------------------
// Main Example 4 Demo class
// ------------------------------------------------------------------------
using UnityEngine;
using System.Collections;


public class CExample4 : MonoBehaviour {

    
    public CMover4 moverPrototype = null;   // movesprite prototype

    bool initialized = false;               // intialization indicator
    bool zooming = false;                   // zooming indicator
    float zoomSpeed = 4f;                   // how fast do we zoom in/out
    float zoomMin = -2.5f;                  // Zoomed out value
    float zoomMax = -1f;                    // Zoomed in value

    // Create movers
    void CreateObjects()
    {
        // lets create 10 mover sprites
        for (int i = 0; i < 10; i++)
        {
            OTSprite s = OT.CreateSprite("mover");
            // hook up onInput delegate to handle clicking on a mover
            s.onInput = OnInput;
        }
    }

    // Input handler
    
    public void OnInput(OTObject owner)
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
    void Initialize()
    {
        // Change GUIText color to black
        TextMesh txt = (GameObject.Find("info") as GameObject).GetComponent<TextMesh>();
		txt.renderer.material.color = Color.black;
        // create out objects
        CreateObjects();
        // Initialize our view
        InitView();
        // indicate that we have initialized
        initialized = true;
    }

    // Initialize view
    void InitView()
    {
        // no rotation for this view
        OT.view.rotation = 0;
        // position x/y to 0/0
        OT.view.position = Vector2.zero;
    }
	
	// Update is called once per frame
	void Update () {
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
}
