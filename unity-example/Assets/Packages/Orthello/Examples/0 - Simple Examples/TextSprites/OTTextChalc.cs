using UnityEngine;

public class OTTextChalc : MonoBehaviour {

	public OTSprite window;
	public OTSprite sizer;
	public OTSprite bound;
	
	OTTextSprite text;

	// Use this for initialization
	void Start () {
		text = GetComponent<OTTextSprite>();
		sizer.BoundBy(bound);
		sizer.onDragging = SizerDrag;
		window.dirtyChecks = true;
	}
			
	void SizerDrag(OTObject owner)
	{
		Vector2 s = sizer.position - window.position;
		window.size = new Vector2(s.x-12,(-s.y)+10);	
		(window as OTScale9Sprite).Rebuild();
		text.wordWrap = (int)(window.size.x/text.size.x)-100;
	}
	
}
