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
// Star 1 class
// ------------------------------------------------------------------------

/*
private var tween:OTTween;
function OnMouseEnter()
{
    if (tween!=null && tween.isRunning)
        tween.Stop();
    tween = new OTTween(GetComponent("OTSprite"), 1f, OTEasing.ElasticOut).
        Tween("size", new Vector2(80, 80)).
        Tween("tintColor", new Color(0.5f + Random.value * 0.5f, 0.5f + Random.value * 0.5f, 0.5f + Random.value * 0.5f), OTEasing.StrongOut);
}
function OnMouseExit()
{
    if (tween!=null && tween.isRunning)
        tween.Stop();
    tween = new OTTween(GetComponent("OTSprite"), 1.5f, OTEasing.ElasticOut).
        Tween("size", new Vector2(50, 50)).
        Tween("tintColor", new Color(0.5f,0.5f,0.5f), OTEasing.StrongIn);
}
*/