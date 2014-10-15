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
// Example 7
// Tweening example
// ------------------------------------------------------------------------
// Star 2 class
// ------------------------------------------------------------------------

/*
private var runningTween:OTTween = null;
private var tween:OTTween = null;
function OnMouseEnter()
{
    if ((tween!=null && tween.isRunning) || runningTween != null)
        return;

    tween = new OTTween(GetComponent("OTSprite"), 2f, OTEasing.Linear).
        Tween("size", new Vector2(250, 250)).
        Tween("alpha", 1 , 0 , OTEasing.Linear);

    tween.InitCallBacks(this);
    new OTTween(GetComponent("OTSprite"), 1f, OTEasing.Linear).
        Tween("tintColor", new Color(0.5f + Random.value * 0.5f, 0.5f + Random.value * 0.5f, 0.5f + Random.value * 0.5f), OTEasing.StrongOut);
}

function OnTweenFinish(tween:OTTween)
{
    if (tween != runningTween)
    {
        runningTween = new OTTween(GetComponent("OTSprite"), .5f, OTEasing.Linear).
            Tween("size", new Vector2(1, 1), new Vector2(50, 50)).
            Tween("alpha", 0, 1, OTEasing.Linear);
        new OTTween(GetComponent("OTSprite"), .1f, OTEasing.Linear).
            Tween("tintColor", new Color(0.3f, 0.3f, 0.3f), OTEasing.StrongOut);
    	runningTween.InitCallBacks(this);
    }
    else
        runningTween = null;        
}
*/