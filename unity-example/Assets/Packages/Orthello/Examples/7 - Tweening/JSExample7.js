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
// Main Example 7 Demo class
// ------------------------------------------------------------------------

/*
private var mode:int = 0;
private var time:Number = 0;
private var easeIdx:int = 0;

private var easings:Array =  [
    OTEasing.ElasticOut,
    OTEasing.BounceOut,
    OTEasing.StrongOut,
    OTEasing.StrongIn,
    OTEasing.BackOut,
    OTEasing.BackIn,
    OTEasing.SineOut,
    OTEasing.Linear];

// Update is called once per frame
function Update () {
    if (time == 0)
        GameObject.Find("text-21").GetComponent("TextMesh").text = "Next easing will be : " + easings[easeIdx].GetType().ToString();
    time += Time.deltaTime;
    if (time > 3)
    {
        MoveStars();
        time = 0;
    }
}

function MoveStars()
{
    var idx:int = 1;
    var g:GameObject = GameObject.Find("stars-"+idx);
    while (g!=null)
    {
        new OTTween(g.transform,1.5f,easings[easeIdx]).TweenAdd("position",new Vector3((mode==0)?400:-400,0,0));
        g = GameObject.Find("stars-" + (++idx));
    }
    mode = 1 - mode;
    if (mode == 0)
    {
        easeIdx++;
        if (easeIdx == easings.length)
            easeIdx = 0;
    }
}
*/