// ------------------------------------------------------------------------
// Orthello 2D Framework Example Source Code
// (C)opyright 2011 - WyrmTale Games - http://www.wyrmtale.com
// ------------------------------------------------------------------------
// More info http://www.wyrmtale.com/orthello
// ------------------------------------------------------------------------
// Example 7
// Tweening example
// ------------------------------------------------------------------------
// Star 1 class
// ------------------------------------------------------------------------
using UnityEngine;
using System.Collections;


public class CStar7_1 : MonoBehaviour
{
    OTTween tween;
    void OnMouseEnter()
    {
        if (tween!=null && tween.isRunning)
            tween.Stop();
        tween = new OTTween(GetComponent<OTSprite>(), 1f, OTEasing.ElasticOut).
            Tween("size", new Vector2(80, 80)).
            Tween("tintColor", new Color(0.5f + Random.value * 0.5f, 0.5f + Random.value * 0.5f, 0.5f + Random.value * 0.5f), OTEasing.StrongOut);
    }
    void OnMouseExit()
    {
        if (tween!=null && tween.isRunning)
            tween.Stop();
        tween = new OTTween(GetComponent<OTSprite>(), 1.5f, OTEasing.ElasticOut).
            Tween("size", new Vector2(50, 50)).
            Tween("tintColor", new Color(0.5f,0.5f,0.5f), OTEasing.StrongIn);
    }

}
