using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	
	OTSprite sprite;
	public Color[] arrowColors;
			
	// Use this for initialization
	void Start () {
		sprite = GetComponent<OTSprite>();
		if (arrowColors!=null && arrowColors.Length>0)
			sprite.tintColor = arrowColors[(int)Mathf.Floor(Random.value * (arrowColors.Length-0.1f))];
		StartTween(null);
	}
	
	float t = 0;
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime;
		if (t>20)
			OT.DestroyObject(sprite.transform.parent.gameObject);
	}
	
	public void OnEnable()
	{
		t = 0;
	}
	
	void StartTween(OTTween tween)
	{
		sprite.size = new Vector2(0.4f,0.4f);
		sprite.alpha = 0.75f;
		new OTTween(sprite,1.5f,OTEasing.SineOut).
			Tween("size",Vector2.one).
			Tween("alpha",0).onTweenFinish = StartTween;		
	}
}
