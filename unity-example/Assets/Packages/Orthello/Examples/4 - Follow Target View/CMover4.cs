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
// Mover behaviour class
// ------------------------------------------------------------------------
using UnityEngine;
using System.Collections;


public class CMover4 : MonoBehaviour {

    // mover waypoint
    Vector2[] wayPoints = { 
           new Vector2(-729.00f,866.00f), new Vector2(-204.00f,866.00f),new Vector2(-204.00f, -540.00f), 
           new Vector2(247.00f ,-540.00f), new Vector2(247.00f,866.00f), new Vector2(757.00f,866.00f),
           new Vector2(757.00f, -861.00f), new Vector2(-729.00f,-861.00f) };

    bool rotating = false;                  // rotating indicator
    OTSprite sprite;                        // mover sprite class
    float speed = 50;                       // movement speed pixels / second
    float rotationSpeed = 2;                // time to complete rotation of 90 degrees
    int wayPoint = 0;                       // current waypoint
    Vector2 towardsPoint = Vector2.zero;    // next waypoint

    float towardsRotation = 0;              // next rotation
    float fromRotation = 0;                 // current rotation
    float rotationTime = 0;                 // rotation progress
    float rotationAngle = 0;                // rotation delta

    // set mover at or between (randomDist) a specific waypoint
    void SetPosition(int wayPoint, bool randomDist)
    {
        // set current waypoint
        this.wayPoint = wayPoint;
        // get position x/y of current waypoint
        Vector2 fromPoint = wayPoints[wayPoint];

        // determine next waypoint that we will be moving towards
        if ((wayPoint+1)<wayPoints.Length)
            towardsPoint = wayPoints[wayPoint+1];
        else
            towardsPoint = wayPoints[0];

        // check if we have to position randomly between the 2 waypoints
        if (randomDist)
        {
            // get distance vector
            Vector2 dist = towardsPoint - fromPoint;
            // randomize distance 0-1
            dist = dist * Random.value;
            // increase starting position
            fromPoint += dist;
        }
        // set mover sprite position
        sprite.position = fromPoint;
        // rotate (heading) mover sprite towards next waypoint
        sprite.RotateTowards(towardsPoint);
    }

	// Use this for initialization
	void Start () {
        // get this mover sprite class
        sprite = GetComponent<OTSprite>();
        // hookup mouse enter and mouse exit delegates
        sprite.onMouseEnterOT = _OnMouseEnter;
        // we will use the 'default' Unity3d OnMouseExit() callback handler , this will be
        // explained at the callback function
        // so no ... sprite.onMouseExitOT = _OnMouseExit;

        // determine random speed
        speed = 150 + Random.value * 1050;
        // calculate rotation speed (related to movement speed)
        rotationSpeed = 1 / (speed / 350);
        // set this mover at a random position bewteen 2 random waypoints
        SetPosition((int)Mathf.Floor(Random.value * wayPoints.Length), true);
	}

    // Mouse enter delegate handler
    // !IMPORTANT : Because the OnMouseEnter() is the default call back function on a 
    // behaviour script when it has a collider ( so always .. ) you have to give 
    // this delegate another name to evade a compiler error. 
    // In this example .. we could have used the default handler as well. And would be even
    // better as we do not need the owner object here. We will do that with the OnMouseExit
    // handler
    
    public void _OnMouseEnter(OTObject owner)
    {
        owner.gameObject.renderer.material.SetColor("_TintColor", new Color(1, 0, 0));
    }

    // Mouse exit delegate handler
    // !IMPORTANT : Because the OnMouseExit() is the default call back function on a 
    // behaviour script when it has a collider ( so always .. ) you must give your
    // Orthello OnMouseExit(OTObject owner) another name if you would like to use that.
    // In this case we dont need the OTObject so we use the default.
    
    public void OnMouseExit()
    {
        gameObject.renderer.material.SetColor("_TintColor", new Color(.5f, .5f, .5f));
    }

	// Update is called once per frame
	void Update () {

        // we do not want to update our proto-type (should it be active)
        if (name == "mover-prototype") return;

        if (rotating)
        {
            // we are rotating so increase rotating time
            rotationTime += Time.deltaTime;
            if (rotationTime >= rotationSpeed)
            {
                // we are rotating long enough so stop rotating 
                rotating = false;
                // set mover to current waypoint
                SetPosition(wayPoint, false);
            }
            else
              // rotate sprite towards waypoint
              sprite.rotation = fromRotation + rotationAngle * (rotationTime / rotationSpeed);			
        }
        else
        {
            // we are moving so determine the movement vector 
            Vector2 towards = towardsPoint - sprite.position;
            // update mover sprite position
            sprite.position += towards.normalized * speed * Time.deltaTime;
            // determine new movement vector
            Vector2 result = towardsPoint - sprite.position;
									
            // if the normalized vectors do not match we have passed the next waypoint 
            if (!Vector2.Equals(result.normalized, towards.normalized))
            {
                // set our sprite on the next waypoint
                sprite.position = towardsPoint;
                // indicate that we will start rotating
                rotating = true;
                // increment our current waypoint and check if we have to go back to the first
                wayPoint++;
                if (wayPoint == wayPoints.Length) wayPoint = 0;
                // determine next wayoint
                if ((wayPoint + 1) < wayPoints.Length)
                    towardsPoint = wayPoints[wayPoint + 1];
                else
                    towardsPoint = wayPoints[0];
				
                // keep current rotation value
                fromRotation = sprite.rotation;
                // determine next rotation value by rotating towards next waypoint
                sprite.RotateTowards(towardsPoint);
                towardsRotation = sprite.rotation;
                // back to current rotation value, because we will be rotating from that angle
                sprite.rotation = fromRotation;
                // intialize rotation variables
                rotationTime = 0;
                // determine rotation delta angle
				rotationAngle = towardsRotation - fromRotation;
				if (Mathf.Abs(rotationAngle)>90.1f)
				{
					if (rotationAngle<0) rotationAngle = 90;
					  else rotationAngle = -90;
				}
            }
        }
    }
}
