using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OTTouch {	
	/// <summary>
	/// The active.
	/// </summary>
	public static bool active = false;

    
	/// <summary>
	/// Touch gesture delegate.
	/// </summary>
    public delegate void TouchGestureDelegate(OTTouchGesture gesture);

	/// <summary>
	/// delegate that is called when a touch gesture has begun
	/// </summary>
	public static TouchGestureDelegate onGestureBegan = null; 
	/// <summary>
	/// delegate that is called when a touch gesture is running
	/// </summary>
	public static TouchGestureDelegate onGestureRunning = null; 
	/// <summary>
	/// delegate that is called when a touch gesture has ended
	/// </summary>
	public static TouchGestureDelegate onGestureEnded = null; 
		
	static bool _isRotating = false;
	/// <summary>
	/// Indicates if a rotation gesture is being performed
	/// </summary>
	public static bool isRotating
	{
		get
		{
			return _isRotating;
		}
	}
			
	static bool _isPinching = false;
	/// <summary>
	/// Indicates if a pinch gesture is being performed
	/// </summary>
	public static bool isPinching
	{
		get
		{
			return _isPinching;
		}
	}

	static bool _isPanning = false;
	/// <summary>
	/// Indicates if a pan gesture is being performed
	/// </summary>
	public static bool isPanning
	{
		get
		{
			return _isPanning;
		}
	}

	static bool _isSwiping = false;
	/// <summary>
	/// Indicates if a swipe gesture is being performed
	/// </summary>
	public static bool isSwiping
	{
		get
		{
			return _isSwiping;
		}
	}

	static bool _isPressing = false;
	/// <summary>
	/// Indicates if a pressing gesture is being performed
	/// </summary>
	public static bool isPressing
	{
		get
		{
			return _isPressing;
		}
	}

	static bool _isTapping = false;
	/// <summary>
	/// Indicates if a tapping gesture is being performed
	/// </summary>
	public static bool isTapping
	{
		get
		{
			return _isTapping;
		}
	}
			
	static List<OTTouchFinger> fingers = new List<OTTouchFinger>();
	static List<OTTouchGesture> gestures = new List<OTTouchGesture>();
	static List<int> fingerChecks = new List<int>();
	
	static void GetFingers()
	{
		if (OT.mobile)
		{
			fingerChecks.Clear();
			int i;
			for (i=0; i<Input.touches.Length; i++)
				fingerChecks.Add(Input.touches[i].fingerId);

			// remove fingers that have gone
			i=0;
			while (i<fingers.Count)
			{
				if (!fingerChecks.Contains(fingers[i].fingerId))
				{
					OTTouchFinger finger = fingers[i];
					OTTouchFinger.lookup.Remove(finger.fingerId);
					if (finger.gesture!=null)
						finger.gesture.RemoveFinger(finger);					
					fingers.RemoveAt(i);
				}
				else
					i++;
			}			
			// update and add new fingers
			for (i=0; i<Input.touches.Length; i++)
			{
				OTTouchFinger finger = OTTouchFinger.Lookup(Input.touches[i]);
				if (finger==null)
				{
					fingers.Add(new OTTouchFinger(Input.touches[i]));
					gestures.Add(new OTTouchGesture(fingers[fingers.Count-1]));
				}
				else
					finger.Assign(Input.touches[i]);				
			}
		}
				
		for (int i=0; i<fingers.Count; i++)
			fingers[i].Update();
		
	}
			
	public static void Update()
	{		
		if (active)
		{
			GetFingers();
			// check the current gestures
			int i=0;			
			while (i<gestures.Count)
			{
				gestures[i].Update();
				switch(gestures[i].phase)
				{
					case OTTouchGesture.Phase.Began:
						gestures[i].Begin();
						if (onGestureBegan!=null)
							onGestureBegan(gestures[i]);
						gestures[i].phase = OTTouchGesture.Phase.Running;
						i++;
					break;					
					case OTTouchGesture.Phase.Running:
						if (onGestureRunning!=null)
							onGestureRunning(gestures[i]);
						i++;
					break;					
					case OTTouchGesture.Phase.Ended:
						if (onGestureEnded!=null)
							onGestureEnded(gestures[i]);
						gestures.RemoveAt(i);
					break;					
				}
			}						
		}
	}
		
}

public class OTTouchGesture
{
	public enum Type { Tap, Press, Swipe, Pan, Pinch, Rotate };
	public enum Phase { Began, Running, Ended };
	public Type type;
	public Phase phase = Phase.Began;
	
	public OTObject target;
	
	public float duration = 0;
	
	List<OTTouchFinger> startFingers = new List<OTTouchFinger>();
	List<OTTouchFinger> fingers = new List<OTTouchFinger>();
	
	public OTTouchGesture(OTTouchFinger finger)
	{
		type = Type.Press;
		fingers.Add(finger);		
		finger.gesture = this;
	}
	
	public void Begin()
	{
		startFingers.Clear();
		for (int i=0; i<fingers.Count; i++)
			startFingers.Add(fingers[i].Clone());
	}
	
	public void RemoveFinger(OTTouchFinger finger)
	{
		if (fingers.Contains(finger))
		{
			fingers.Remove(finger);
			finger.gesture = null;
		}
	}
	
	public void AddFinger(OTTouchFinger finger)
	{
		if (!fingers.Contains(finger))
		{
			fingers.Add(finger);
			finger.gesture = this;
		}
	}
	
	public void Update()
	{
		if (fingers.Count==0)
			phase = Phase.Ended;
		
		if (phase == Phase.Running)
			duration += Time.deltaTime;
		
	}
	
}

public class OTTouchFinger
{
	public int fingerId;
	Vector2 position = Vector2.zero;
	Vector2 deltaPosition = Vector2.zero;
	float deltaTime = 0;
	int tapCount = 0;
	TouchPhase phase = TouchPhase.Stationary;
	public OTTouchGesture gesture = null;
	public float time = 0;
	
	static public Dictionary<int, OTTouchFinger> lookup = new Dictionary<int, OTTouchFinger>();
	
	public static OTTouchFinger Lookup(Touch touch)
	{
		if (lookup.ContainsKey(touch.fingerId))
			return lookup[touch.fingerId];
		else
			return null;
	}
	
	public OTTouchFinger Clone()
	{
		return new OTTouchFinger(this);
	}
	
	public void Assign(Touch touch)
	{
		fingerId = touch.fingerId;
		position = touch.position;
		deltaPosition = touch.deltaPosition;
		deltaTime = touch.deltaTime;
		tapCount = touch.tapCount;
		phase = touch.phase;
	}
	
	public void Assign(OTTouchFinger finger)
	{
		fingerId = finger.fingerId;
		position = finger.position;
		deltaPosition = finger.deltaPosition;
		deltaTime = finger.deltaTime;
		tapCount = finger.tapCount;
		phase = finger.phase;
	}
	
	public OTTouchFinger(int id, Vector2 position)
	{
		fingerId = id;
		this.position = position;
		lookup.Add(id,this);
	}
	
	public OTTouchFinger(Touch touch)
	{
		Assign(touch);
		lookup.Add(touch.fingerId, this);
	}
	
	public OTTouchFinger(OTTouchFinger finger)
	{
		Assign(finger);
	}
	
	public void Update()
	{
		time += Time.deltaTime;
	}
	
}