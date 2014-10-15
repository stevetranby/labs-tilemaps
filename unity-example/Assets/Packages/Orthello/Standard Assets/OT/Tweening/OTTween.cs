using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Use this class to tween properties of objects
/// </summary>
public class OTTween
{
    /// <summary>
    /// Tween notification delegate
    /// </summary>
    /// <param name="tween">Tween for this notification</param>
    public delegate void TweenDelegate(OTTween tween);

	/// <summary>
    /// Will be fired when a tween has finished.
    /// </summary>
    public TweenDelegate onTweenFinish;
	
	/// <summary>
    /// Will be fired when a ping pong tween has reach the ping (1st) position
    /// </summary>
    public TweenDelegate onTweenPing;
	
	/// <summary>
    /// Will be fired when a ping pong tween has reach the pong (2nd-reversed) position
    /// </summary>
    public TweenDelegate onTweenPong;
	
	/// <summary>
    /// Will be fired when a tween has been played once.
    /// </summary>
    /// <remarks>
    /// will be called for each playCount or after each play cycle when looping.
    /// </remarks>
    public TweenDelegate onTweenPlayed;

    /// <summary>
    /// Will be true when a tween is running
    /// </summary>
    public bool isRunning
    {
        get
        {
            return _running;
        }
    }
    /// <summary>
    /// Will be true when a tween is pauzed
    /// </summary>
    public bool isPauzed
    {
        get
        {
            return _pauzed;
        }
    }
	
	/// <summary>
	/// Gets the target object for this tween
	/// </summary>
	public object target
	{
		get
		{
			return _target;
		}		
	}

	bool _pingPong = false;
	/// <summary>
	/// Creating a ping pong (auto reversing) tween if this is true
	/// </summary>
	public bool pingPong
	{
		get
		{
			return _pingPong;
		}
		set
		{
			_pingPong = value;			
		}
	}
		
	int _playCount = 1;
	/// <summary>
	/// The number of times this tween has to be runned
	/// </summary>
	/// <remarks>
	/// A value of -1 will create a looping tween. You can also use playLoop = true.
	/// </remarks>
	public int playCount
	{
		get
		{
			return _playCount;
		}
		set
		{
			_playCount = value;
		}
	}
	
	int _played = 0;
	/// <summary>
	/// Gets the number of times the tween has been played
	/// </summary>
	public int played {
		get {
			return _played;
		}
	}	

	bool _restarted = false;
	/// <summary>
	/// Indicates that the tween was restarted
	/// </summary>
	public bool restarted {
		get {
			return _restarted;
		}
	}
	
	int _oldPlayCount = 1;
	/// <summary>
	/// Create a looping tween if true
	/// </summary>
	/// <remarks>
	/// The playCount will be set to -1
	/// </remarks>
	public bool playLoop
	{
		get
		{
			return _playCount == -1;
		}
		set
		{
			if (value)
				_oldPlayCount = _playCount;
			_playCount = (value)?-1:_oldPlayCount;
		}
	}
				
    List<string> vars = new List<string>();
    List<object> addValues = new List<object>();
    List<OTEase> easings = new List<OTEase>();
    List<OTEase> pongEasings = new List<OTEase>();
    List<object> startValues = new List<object>();
    List<object> fromValues = new List<object>();
    List<object> toValues = new List<object>();
    List<FieldInfo> fields = new List<FieldInfo>();
    List<PropertyInfo> props = new List<PropertyInfo>();
    List<Component> callBackTargets = new List<Component>();				

    OTEase easing;
    object _target;
    float duration;
    float time = 0;
    float waitTime = 0;
    bool _running = false;
    bool _doStop = false;
	
	/// <summary>
	/// True if this tween is waiting to start
	/// </summary>
	public bool isWaiting
	{
		get
		{
			return waitTime>0;
		}
	}

	/// <summary>
	/// sets the onTweenFinish Delegate
	/// </summary>
	public OTTween OnFinish(TweenDelegate onTweenFinish)
	{
		this.onTweenFinish = onTweenFinish;
		return this;
	}
	/// <summary>
	/// sets the onTweenPing Delegate
	/// </summary>
	public OTTween OnPing(TweenDelegate onTweenPing)
	{
		this.onTweenPing = onTweenPing;
		return this;
	}
	/// <summary>
	/// sets the onTweenPong Delegate
	/// </summary>
	public OTTween OnPong(TweenDelegate onTweenPong)
	{
		this.onTweenPong = onTweenPong;
		return this;
	}
	/// <summary>
	/// sets the onTweenPlay Delegate
	/// </summary>
	public OTTween OnPlayed(TweenDelegate onTweenPlayed)
	{
		this.onTweenPlayed = onTweenPlayed;
		return this;
	}
	
	/// <summary>
	/// Sets the playCount
	/// </summary>
	public OTTween PlayCount(int count)
	{
		_oldPlayCount = count;
		playCount = count;
		return this;
	}
	
	/// <summary>
	/// Sets the looping state
	/// </summary>
	public OTTween Loop()
	{
		playLoop = true;
		return this;
	}
	/// <summary>
	/// Sets the ping pong state
	/// </summary>
	public OTTween PingPong()
	{
		pingPong = true;
		return this;
	}
	
    static OTTweenController controller = null;

	/// <summary>
	/// Stops all running and/or waiting tweens.
	/// </summary>
    public static void StopAll(bool running = true, bool waiting = true)
    {
        if(controller != null && (running || waiting))
            controller.Clear(running,waiting);
    }	
				
    void CheckController()
    {
        if (controller == null)
        {
            controller = OT.Controller(typeof(OTTweenController)) as OTTweenController;
            if (controller == null)
            {
                controller = new OTTweenController();
                OT.AddController(controller);
            }
        }
        else
        {
            if ((OT.Controller(typeof(OTTweenController)) as OTTweenController) == null)
                OT.AddController(controller);
        }

        if (controller != null && !controller.tweens.Contains(this))
            controller.Add(this);
    }

    /// <summary>
    /// OTTween constructor
    /// </summary>
    /// <param name="target">Object on which to tween properties</param>
    /// <param name="duration">Tween duration</param>
    /// <param name="easing">Tween 'default' easing function</param>
    public OTTween(object target, float duration, OTEase easing)
    {
        this._target = target;
        this.duration = duration;
        this.easing = easing;
        CheckController();
    }

    /// <summary>
    /// OTTween constructor (easing Linear)
    /// </summary>
    /// <param name="target">Object on which to tween properties</param>
    /// <param name="duration">Tween duration</param>
    public OTTween(object target, float duration)
    {
        this._target = target;
        this.duration = duration;
        this.easing = OTEasing.Linear;
        CheckController();
    }

    // -----------------------------------------------------------------
    // class methods
    // -----------------------------------------------------------------
    /// <summary>
    /// Tween has to use callback functions.
    /// </summary>
    /// <param name="target">target class that will receive the callbacks.</param>
    public void InitCallBacks(Component target)
    {
        callBackTargets.Add(target);
    }


    private void SetVar(string name)
    {
        if (target != null)
        {
            FieldInfo field = target.GetType().GetField(name);
            if (field != null)
            {
                fromValues.Add(field.GetValue(target));
                fields.Add(field);
                props.Add(null);
            }
            else
            {
                PropertyInfo prop = target.GetType().GetProperty(name);
                if (prop != null)
                {
                    fromValues.Add(prop.GetValue(target, null));
                    props.Add(prop);
                    fields.Add(null);
                }
                else
                {
					Debug.LogWarning("property or field ["+name+"] could not been found on the target object!");		
                    fromValues.Add(null);
                    fields.Add(null);
                    props.Add(null);
                }
            }
        }		
		startValues.Add(fromValues[fromValues.Count-1]);		
    }

    private void TweenVar(object fromValue, object toValue, OTEase easing, FieldInfo field, PropertyInfo prop)
    {
        object value = null;
        if (toValue == null || fromValue == null) return;
		
		float dur = duration;
		if (pingPong) dur/=2;
		
        switch (fromValue.GetType().Name.ToLower())
        {
            case "single":
                if (!(toValue is float))
                    toValue = System.Convert.ToSingle(toValue);
                value = easing.ease(time, (float)fromValue, (float)toValue - (float)fromValue, dur);
                break;
            case "double":
                if (!(toValue is double))
                    toValue = System.Convert.ToDouble(toValue);
                value = easing.ease(time, (float)fromValue, (float)toValue - (float)fromValue, dur);
                break;
            case "int":
            case "int32":
                if (!(toValue is int))
                    toValue = System.Convert.ToInt32(toValue);

                value = (int)easing.ease(time, (int)fromValue, (int)toValue - (int)fromValue, dur);
                break;			
            case "vector2":
                Vector2 _toValue2 = (Vector2)toValue;
                Vector2 _fromValue2 = (Vector2)fromValue;
                Vector2 _value2 = Vector2.zero;

                if ((_toValue2 - _fromValue2).x != 0)
                    _value2.x = easing.ease(time, _fromValue2.x, (_toValue2 - _fromValue2).x, dur);
                else
                    _value2.x = _fromValue2.x;

                if ((_toValue2 - _fromValue2).y != 0)
                    _value2.y = easing.ease(time, _fromValue2.y, (_toValue2 - _fromValue2).y, dur);
                else
                    _value2.y = _fromValue2.y;

                value = _value2;
                break;
            case "vector3":
                Vector3 _toValue3 = (Vector3)toValue;
                Vector3 _fromValue3 = (Vector3)fromValue;
                Vector3 _value3 = Vector3.zero;

                if ((_toValue3 - _fromValue3).x != 0)
                    _value3.x = easing.ease(time, _fromValue3.x, (_toValue3 - _fromValue3).x, dur);
                else
                    _value3.x = _fromValue3.x;
			
                if ((_toValue3 - _fromValue3).y != 0)
                    _value3.y = easing.ease(time, _fromValue3.y, (_toValue3 - _fromValue3).y, dur);
                else
                    _value3.y = _fromValue3.y;
			
                if ((_toValue3 - _fromValue3).z != 0)
                    _value3.z = easing.ease(time, _fromValue3.z, (_toValue3 - _fromValue3).z, dur);
                else
                    _value3.z = _fromValue3.z;


                value = _value3;
                break;
            case "color":
                Color _toColor = (Color)toValue;
                Color _fromColor = (Color)fromValue;

                float r = easing.ease(time, _fromColor.r, _toColor.r - _fromColor.r, dur);
                float g = easing.ease(time, _fromColor.g, _toColor.g - _fromColor.g, dur);
                float b = easing.ease(time, _fromColor.b, _toColor.b - _fromColor.b, dur);
                float a = easing.ease(time, _fromColor.a, _toColor.a - _fromColor.a, dur);

                value = new Color(r, g, b, a);
                break;
        }

		try
		{
	        if (field != null)
	            field.SetValue(target, value);
	        else
	            if (prop != null)
	                prop.SetValue(target, value, null);
		}
		catch(System.Exception)
		{
			
			_doStop = true;
			return;
		};

    }

    
    protected bool CallBack(string handler, object[] param)
    {
        for (int t = 0; t < callBackTargets.Count; t++)
        {
            MethodInfo mi = callBackTargets[t].GetType().GetMethod(handler);
            if (mi != null)
            {
                mi.Invoke(callBackTargets[t], param);
                return true;
            }
        }
        return false;
    }

    
	bool _pauzed = false;
	/// <summary>
	/// Pauzes the tween.
	/// </summary>
    public void  Pauze()
	{
		_pauzed = true;	
	}	
	/// <summary>
	/// Resumes a pauzed tween
	/// </summary>
    public void Resume()
	{
		_pauzed = false;	
	}
	
	bool ping = true;
    public bool Update(float deltaTime)
    {
		if (_pauzed)
			return false;
		
        if (_doStop)
        {
            _running = false;
            return true;
        }

        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            if (waitTime > 0) return false;
        }
        if (vars.Count == 0) return false;
        _running = true;
		
		float dur = duration;
		if (pingPong) dur/=2;
		
		_restarted = false;
        time += deltaTime;
        if (time > dur) time = dur;
        for (int v = 0; v < vars.Count; v++)
        {
            OTEase easing = this.easing;
			if (pingPong && !ping && pongEasings[v]!=null)
                easing = pongEasings[v];
			else			
            if (easings[v] != null)
                easing = easings[v];
            TweenVar(fromValues[v], toValues[v], easing, fields[v], props[v]);
        }
						
        if (time == dur)
        {
			if (_pingPong)
			{
				time = 0;
		        for (int v = 0; v < vars.Count; v++)
		        {
					object t = toValues[v];
					toValues[v] = fromValues[v];
					fromValues[v] = t;
		        }
				
				if (ping)
				{
					ping = false;
		            if (onTweenPing != null)
		                onTweenPing(this);
		            if (!CallBack("onTweenPing", new object[] { this }))
		                CallBack("OnTweenPing", new object[] { this });			
					time = 0;
					return false;
				}
				else
				{
					ping = true;
		            if (onTweenPong != null)
		                onTweenPong(this);
		            if (!CallBack("onTweenPong", new object[] { this }))
		                CallBack("OnTweenPong", new object[] { this });			
				}
				
			}
			_played++;
			if (playCount>0)
				--playCount;
            if (onTweenPlayed != null)
                onTweenPlayed(this);
            if (!CallBack("onTweenPlayed", new object[] { this }))
                CallBack("OnTweenPlayed", new object[] { this });			
			
			if (playCount==0)
			{												
	            _running = false;
	            if (onTweenFinish != null)
	                onTweenFinish(this);
	            if (!CallBack("onTweenFinish", new object[] { this }))
	                CallBack("OnTweenFinish", new object[] { this });
            	return true;
			}
			else
			{
				if (!pingPong)
				{
					// reset add tween start values to new values
			        for (int v = 0; v < vars.Count; v++)
			        {
						object t = addValues[v];
						if (t!=null)
						{
							fromValues[v] = toValues[v];
							AddValue(v, t);
						}
			        }
				}
			}
			
			time = 0;			
			return false;
				
        }
        else
            return false;
    }
	
	
	public OTTween Restart()
	{
		CheckController();
		time = 0;
		_played = 0;
		_playCount = _oldPlayCount;
		_restarted = true;
		// reset add tween start values to new values
        for (int v = 0; v < vars.Count; v++)
        {
			object t = addValues[v];
			if (t!=null)
			{
				fromValues[v] = startValues[v];
				AddValue(v, t);
			}
        }	
		return this;
	}

    /// <summary>
    /// Sets the wait time (start delay) for this tween 
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    public OTTween Wait(float waitTime)
    {
        this.waitTime = waitTime;
        return this;
    }

    /// <summary>
    /// Tween a 'public' property of the tween's target object
    /// </summary>
    /// <param name="var">Property name</param>
    /// <param name="fromValue">From value</param>
    /// <param name="toValue">To value</param>
    /// <param name="easing">Easing function</param>
    /// <param name="pongEasing">Easing when 'ponging'</param>
    /// <returns>Tween object</returns>
    public OTTween Tween(string var, object fromValue, object toValue, OTEase easing, OTEase pongEasing)
    {
        vars.Add(var);
		addValues.Add(null);
        SetVar(var);
        if (fromValue != null)
        {
            object fv = fromValues[fromValues.Count - 1];
            if (fv != null)
            {
                if (fv is float && fromValue is int)
                    fromValue = System.Convert.ToSingle(fromValue);
                else
                    if (fv is double && fromValue is int)
                        fromValue = System.Convert.ToDouble(fromValue);
            }
            fromValues[fromValues.Count - 1] = fromValue;
        }
        toValues.Add(toValue);
        easings.Add(easing);
        pongEasings.Add(pongEasing);
        return this;
    }

    /// <summary>
    /// Tween a 'public' property of the tween's target object
    /// </summary>
    /// <param name="var">Property name</param>
    /// <param name="fromValue">From value</param>
    /// <param name="toValue">To value</param>
    /// <param name="easing">Easing function</param>
    /// <returns>Tween object</returns>
    public OTTween Tween(string var, object fromValue, object toValue, OTEase easing)
    {
        return Tween(var, fromValue, toValue, easing, null);
    }
    /// <summary>
    /// Tween a 'public' property of the tween's target object
    /// </summary>
    /// <param name="var">Property name</param>
    /// <param name="fromValue">From value</param>
    /// <param name="toValue">To value</param>
    /// <returns>Tween object</returns>
    public OTTween Tween(string var, object fromValue, object toValue)
    {
        return Tween(var, fromValue, toValue, null, null);
    }
    /// <summary>
    /// Tween a 'public' property of the tween's target object
    /// </summary>
    /// <param name="var">Property name</param>
    /// <param name="toValue">To value</param>
    /// <param name="easing">Easing function</param>
    /// <param name="pongEasing">Easing when 'ponging'</param>
    /// <returns>Tween object</returns>
    public OTTween Tween(string var, object toValue, OTEase easing, OTEase pongEasing)
    {
        return Tween(var, null, toValue, easing, pongEasing);
    }
    /// <summary>
    /// Tween a 'public' property of the tween's target object
    /// </summary>
    /// <param name="var">Property name</param>
    /// <param name="toValue">To value</param>
    /// <param name="easing">Easing function</param>
    /// <returns>Tween object</returns>
    public OTTween Tween(string var, object toValue, OTEase easing)
    {
        return Tween(var, null, toValue, easing, null);
    }
    /// <summary>
    /// Tween a 'public' property of the tween's target object
    /// </summary>
    /// <param name="var">Property name</param>
    /// <param name="toValue">To value</param>
    /// <returns>Tween object</returns>
    public OTTween Tween(string var, object toValue)
    {
        return Tween(var, null, toValue, null, null);
    }

	
	void AddValue(int idx, object addValue)
	{
        object fromValue = fromValues[idx];				
        if (fromValue is int)
        {
            try
            {
                addValue = System.Convert.ToInt32(addValue);
            }
            catch (System.Exception)
            {
                addValue = 0;
            }
        }
        else
            if (fromValue is float)
            {
                try
                {
                    addValue = System.Convert.ToSingle(addValue);
                }
                catch (System.Exception)
                {
                    addValue = 0.0f;
                }
            }
            else
                if (fromValue is double)
                {
                    try
                    {
                        addValue = System.Convert.ToDouble(addValue);
                    }
                    catch (System.Exception)
                    {
                        addValue = 0.0;
                    }
                }

        switch (fromValue.GetType().Name.ToLower())
        {
            case "single": toValues[idx]=((float)fromValue + (float)addValue); break;
            case "double": toValues[idx]=((double)fromValue + (double)addValue); break;
            case "int": toValues[idx]=((int)fromValue + (int)addValue); break;
            case "int32": toValues[idx]=((int)fromValue + (int)addValue); break;
            case "vector2": toValues[idx]=((Vector2)fromValue + (Vector2)addValue); break;
            case "vector3": toValues[idx]=((Vector3)fromValue + (Vector3)addValue); break;
            default: toValues[idx]=(null); break;
        }	
	}
	
    /// <summary>
    /// Tween a 'public' property, adding a value, of the tween's target object.
    /// </summary>
    /// <param name="var">Property name</param>
    /// <param name="addValue">Value to add</param>
    /// <param name="easing">Easing function</param>
    /// <param name="pongEasing">Easing when 'ponging'</param>
    /// <returns>Tween object</returns>
    public OTTween TweenAdd(string var, object addValue, OTEase easing, OTEase pongEasing)
    {
        vars.Add(var);
		addValues.Add(addValue);
        SetVar(var);
        easings.Add(easing);
        pongEasings.Add(pongEasing);
		toValues.Add(null);
		AddValue(fromValues.Count-1, addValue);
        return this;
    }
    /// <summary>
    /// Tween a 'public' property, adding a value, of the tween's target object.
    /// </summary>
    /// <param name="var">Property name</param>
    /// <param name="addValue">Value to add</param>
    /// <param name="easing">Easing function</param>
    /// <returns>Tween object</returns>
    public OTTween TweenAdd(string var, object addValue, OTEase easing)
    {
        return TweenAdd(var, addValue, easing, null);
    }
    /// <summary>
    /// Tween a 'public' property, adding a value, of the tween's target object.
    /// </summary>
    /// <param name="var">Property name</param>
    /// <param name="addValue">Value to add</param>
    /// <returns>Tween object</returns>
    public OTTween TweenAdd(string var, object addValue)
    {
        return TweenAdd(var, addValue, null, null);
    }

    /// <summary>
    /// Stop this tween.
    /// </summary>
    public void Stop()
    {
        if (isRunning)
            _doStop = true;
    }

}
