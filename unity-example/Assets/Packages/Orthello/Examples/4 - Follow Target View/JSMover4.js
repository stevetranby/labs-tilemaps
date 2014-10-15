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
// Mover behaviour class
// ------------------------------------------------------------------------

/*
    // mover waypoint
    private var wayPoints:Array  = [ new Vector2(-729.00f,866.00f), new Vector2(-204.00f,866.00f),new Vector2(-204.00f, -540.00f), 
           new Vector2(247.00f ,-540.00f), new Vector2(247.00f,866.00f), new Vector2(757.00f,866.00f),
           new Vector2(757.00f, -861.00f), new Vector2(-729.00f,-861.00f) ];

    private var rotating:boolean = false;               // rotating indicator
    private var sprite:OTSprite;                        // mover sprite class
    private var speed:Number = 50;                      // movement speed pixels / second
    private var rotationSpeed:Number = 2;               // time to complete rotation of 90 degrees
    private var wayPoint:int = 0;                       // current waypoint
    private var towardsPoint:Vector2 = Vector2.zero;    // next waypoint

    private var towardsRotation:Number = 0;             // next rotation
    private var fromRotation:Number = 0;                // current rotation
    private var rotationTime:Number = 0;                // rotation progress
    private var rotationAngle:Number = 0;               // rotation delta
	

    // set mover at or between (randomDist) a specific waypoint
    function SetPosition(wayPoint:int , randomDist:boolean)
    {
        // set current waypoint
        this.wayPoint = wayPoint;
        // get position x/y of current waypoint
        var fromPoint:Vector2 = wayPoints[wayPoint];

        // determine next waypoint that we will be moving towards
        if ((wayPoint+1)<wayPoints.length)
            towardsPoint = wayPoints[wayPoint+1];
        else
            towardsPoint = wayPoints[0];

        // check if we have to position randomly between the 2 waypoints
        if (randomDist)
        {
            // get distance vector
            var dist:Vector2 = towardsPoint - fromPoint;
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
	function Start () {
        // get this mover sprite class
        sprite = GetComponent("OTSprite");
        // notify sprite class to deliver callbacks
        sprite.InitCallBacks(this);
        // we will use the 'default' Unity3d OnMouseExit() callback handler , this will be
        // explained at the callback function
        // so no ... sprite.onMouseExit = _OnMouseExit;

        // determine random speed
        speed = 150 + Random.value * 1050;
        // calculate rotation speed (related to movement speed)
        rotationSpeed = 1 / (speed / 350);
        // set this mover at a random position bewteen 2 random waypoints
        SetPosition(Mathf.Floor(Random.value * wayPoints.length), true);
	}

    // Orthello Mouse enter callback handler
    public function OnMouseEnterOT(owner:OTObject)
    {
        owner.gameObject.renderer.material.SetColor("_TintColor", new Color(1, 0, 0));
    }

    // 'default' Mouse exit delegate handler
    public function OnMouseExit()
    {
        gameObject.renderer.material.SetColor("_TintColor", new Color(.5f, .5f, .5f));
    }

	// Update is called once per frame
	function Update () {

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
            var towards:Vector2 = towardsPoint - sprite.position;
            // update mover sprite position
            sprite.position += towards.normalized * speed * Time.deltaTime;
            // determine new movement vector
            var result:Vector2 = towardsPoint - sprite.position;

            // if the normalized vectors do not match we have passed the next waypoint 
            if (!Vector2.Equals(result.normalized, towards.normalized))
            {
                // set our sprite on the next waypoint
                sprite.position = towardsPoint;
                // indicate that we will start rotating
                rotating = true;
                // increment our current waypoint and check if we have to go back to the first
                wayPoint++;
                if (wayPoint == wayPoints.length) wayPoint = 0;
                // determine next wayoint
                if ((wayPoint + 1) < wayPoints.length)
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
*/