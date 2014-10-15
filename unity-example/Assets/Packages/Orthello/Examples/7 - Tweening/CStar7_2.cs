// ------------------------------------------------------------------------
// Orthello 2D Framework Example Source Code
// (C)opyright 2011 - WyrmTale Games - http://www.wyrmtale.com
// ------------------------------------------------------------------------
// More info http://www.wyrmtale.com/orthello
// ------------------------------------------------------------------------
// Example 7
// Tweening example
// ------------------------------------------------------------------------
// Star 2 class
// ------------------------------------------------------------------------
using UnityEngine;
using System.Collections;


public class CStar7_2 : MonoBehaviour
{

    OTTween runningTween = null;
    OTTween tween = null;
    void OnMouseEnter()
    {
        if ((tween!=null && tween.isRunning) || runningTween != null)
            return;

        tween = new OTTween(GetComponent<OTSprite>(), 2f, OTEasing.Linear).
            Tween("size", new Vector2(250, 250)).
            Tween("alpha", 1 , 0 , OTEasing.Linear);

        tween.onTweenFinish = TweenFinish;
        new OTTween(GetComponent<OTSprite>(), 1f, OTEasing.Linear).
            Tween("tintColor", new Color(0.5f + Random.value * 0.5f, 0.5f + Random.value * 0.5f, 0.5f + Random.value * 0.5f), OTEasing.StrongOut);
    }

    void TweenFinish(OTTween tween)
    {
        if (tween != runningTween)
        {
            runningTween = new OTTween(GetComponent<OTSprite>(), .5f, OTEasing.Linear).
                Tween("size", new Vector2(1, 1), new Vector2(50, 50)).
                Tween("alpha", 0, 1, OTEasing.Linear);
            new OTTween(GetComponent<OTSprite>(), .1f, OTEasing.Linear).
                Tween("tintColor", new Color(0.3f, 0.3f, 0.3f), OTEasing.StrongOut);
            runningTween.onTweenFinish = TweenFinish;
        }
        else
            runningTween = null;        
    }

}
