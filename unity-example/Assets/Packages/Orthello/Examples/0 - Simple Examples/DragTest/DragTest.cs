using UnityEngine;
using System.Collections;

public class DragTest : MonoBehaviour {
	
	OTSprite sprite;
	DragTestMain main;
	
	// Use this for initialization
	void Start () {
		
		main = Camera.main.GetComponent<DragTestMain>();
		
		GameObject.Find("DragStart").renderer.enabled = false;
		GameObject.Find("Dragging").renderer.enabled = false;
		GameObject.Find("DragEnd").renderer.enabled = false;				

		sprite = GetComponent<OTSprite>();
		// mark this sprite to be draggable
		sprite.draggable = true;
		// hookup our drag events
		sprite.onDragStart = DragStart;
		sprite.onDragEnd = DragEnd;
		sprite.onDragging = Dragging;
		sprite.onReceiveDrop = ReceiveDrop;
		
		// when we drag we will drag the sprite at depth -50
		// so the sprite will always be ontop when dragging
		// it will snap back into its original position on release
		sprite.dragDepth = -50;
		
		// when we drag we will drag the sprite at an alpha value 
		// of 0.5f
		sprite.dragAlpha = .5f;
				
		// set the sprite world boundary
		sprite.BoundBy(GameObject.Find("back").GetComponent<OTObject>());
		
	}
	
	bool ended = false;
	float endTimer = 0;
	
	// Update is called once per frame
	void Update () {
		if (ended)
		{
			endTimer+=Time.deltaTime;
			if (endTimer>2)
			{
				ended = false;
				endTimer = 0;
				GameObject.Find("DragStart").renderer.enabled = false;
				GameObject.Find("Dragging").renderer.enabled = false;
				GameObject.Find("DragEnd").renderer.enabled = false;				
			}
		}
	}
	
	
	void DragStart(OTObject owner)
	{
			
		GameObject.Find("DragStart").renderer.enabled = true;
		GameObject.Find("Dragging").renderer.enabled = false;
		GameObject.Find("DragEnd").renderer.enabled = false;				
		ended = false;
		endTimer = 0;
		
		GameObject.Find("DragStart").GetComponent<TextMesh>().text = "start dragging "+owner.name;
		OTDebug.Message("start dragging "+owner.name);
		
	}
	void DragEnd(OTObject owner)
	{
		GameObject.Find("DragEnd").renderer.enabled = true;		
		
		if (main.onlyOnPurple)
		{
			if (owner.dropTarget==null || owner.dropTarget.name!="draggable sprite purple")
			{
				// invalidate this drop by setting target to null
				owner.dropTarget = null;
				return;
			}			
		}		
		
		if (owner.dropTarget!=null)
		{
			GameObject.Find("DragEnd").GetComponent<TextMesh>().text = 
				"Drag Ended - dropped on "+owner.dropTarget.name;
		}
		else
			GameObject.Find("DragEnd").GetComponent<TextMesh>().text = 
				"Drag Ended";

		OTDebug.Message(GameObject.Find("DragEnd").GetComponent<TextMesh>().text);
		
		ended = true;
	}
	void Dragging(OTObject owner)
	{
		GameObject.Find("Dragging").renderer.enabled = true;		
		GameObject.Find("Dragging").GetComponent<TextMesh>().text = 
			"Dragging "+owner.name+" at "+((OT.mobile?""+sprite.dragTouch.position:""+Input.mousePosition))+" -- "+OTDragObject.dragObjects.Count;
						
	}
	void ReceiveDrop(OTObject owner)
	{		
		if (main.lockPurple && sprite.name=="draggable sprite purple")
		{
			// invalidate this drop by setting target to null
			owner.dropTarget = null;
			return;
		}		
				
		GameObject.Find("Dragging").GetComponent<TextMesh>().text = 
			owner.name+" received drop from "+owner.dropTarget.name;
		OTDebug.Message(GameObject.Find("Dragging").GetComponent<TextMesh>().text);						
	}
			
}
