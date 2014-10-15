using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
/// <summary>
/// Provides functionality to use animating sprites in your scenes.
/// </summary>
public class OTAnimatingSprite : OTSprite
{

    //-----------------------------------------------------------------------------
    // Editor settings
    //-----------------------------------------------------------------------------
    
    public OTAnimation _animation;
    
    public string _animationFrameset = "";
    
    public float _speed = 1;
    
    public bool _looping = true;
    
    public int _numberOfPlays = -1;
    
    public bool _playOnStart = true;
    
    public bool _startAtRandomFrame = false;
    
    public bool _destroyWhenFinished = false;
    /// <summary>
    /// Animation editor preview progress manipulator.
    /// </summary>
    /// <remarks>
    /// This manipulator set the sprite's display frame according to the animation procentual progress value.
    /// By increasing or decreasing (0-100) this editor property, you can preview your current
    /// animation. 
    /// </remarks>
    public int animationPreview = 0;

	
	[HideInInspector]
	public string _animationName = "";

    //-----------------------------------------------------------------------------
    // Delegates
    //-----------------------------------------------------------------------------
    /// <summary>
    /// Animation start delegate.
    /// </summary>
    /// <remarks>
    /// The onAnimationStart delegate will be called when an animation is started.
    /// </remarks>
    public OTObject.ObjectDelegate onAnimationStart = null;
    /// <summary>
    /// Animation finish delegate.
    /// </summary>
    /// <remarks>
    /// The onAnimationFinish delegate will be called when the animation finishes or is stopped.
    /// </remarks>
    public OTObject.ObjectDelegate onAnimationFinish = null;
    /// <summary>
    /// Animation frame progress delegate.
    /// </summary>
    /// <remarks>
    /// The onAnimationFrame delegate will be called when the animation progresses a frame.
    /// </remarks>
    public OTObject.ObjectDelegate onAnimationFrame = null;
    /// <summary>
    /// Animation pauze delegate.
    /// </summary>
    /// <remarks>
    /// The onAnimationPauze delegate will be called when the animation is pauzed.
    /// </remarks>
    public OTObject.ObjectDelegate onAnimationPauze = null;
    /// <summary>
    /// Animation resume delegate.
    /// </summary>
    /// <remarks>
    /// The onAnimationResume delegate will be called when the animation is resumed.
    /// </remarks>
    public OTObject.ObjectDelegate onAnimationResume = null;

    //-----------------------------------------------------------------------------
    // public attributes (get/set)
    //-----------------------------------------------------------------------------
    /// <summary>
    /// This sprite's animation
    /// </summary>
    new public OTAnimation animation
    {
        get
        {
            return _animation;
        }
        set
        {
            _animation = value;
            if (isPlaying)
                waiting = true;
			else
				System.Array.Resize<OTAnimation.Frame>(ref frames,frCount);							
        }
    }
    /// <summary>
    /// Specific animation frameset
    /// </summary>
    /// <remarks>
    /// If not specified (empty) the all animation framesets are played
    /// </remarks>
    public string animationFrameset
    {
        get
        {
            return _animationFrameset;
        }
        set
        {
            _animationFrameset = value;
			animationFramesetLower = value.ToLower();
            if (isPlaying)
                waiting = true;
        }
    }

    /// <summary>
    /// Looping animation indicator
    /// </summary>
    public bool looping
    {
        get
        {
            return _looping;
        }
        set
        {
            _looping = value;
            if (value)
                _numberOfPlays = -1;
            else
                _numberOfPlays = 1;
        }
    }

    /// <summary>
    /// The number of times an animation is played
    /// </summary>
    public int numberOfPlays
    {
        get
        {
            return _numberOfPlays;
        }
        set
        {
            _numberOfPlays = value;
        }
    }

    /// <summary>
    /// Animation speed
    /// </summary>
    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }

    /// <summary>
    /// Animation will start playing when the sprite is 'started'
    /// </summary>
    public bool playOnStart
    {
        get
        {
            return _playOnStart;
        }
        set
        {
            _playOnStart = value;
        }
    }

    /// <summary>
    /// The animation will start to play at a random frame
    /// </summary>
    public bool startAtRandomFrame
    {
        get
        {
            return _startAtRandomFrame;
        }
        set
        {
            _startAtRandomFrame = value;
        }
    }

    /// <summary>
    /// Sprite will be destroyed automaticly after the animation finishes.
    /// </summary>
    public bool destroyWhenFinished
    {
        get
        {
            return _destroyWhenFinished;
        }
        set
        {
            _destroyWhenFinished = value;
        }
    }

    /// <summary>
    /// Animation is playhing indicator
    /// </summary>
    public bool isPlaying
    {
        get
        {
            return _playing;
        }
    }
    /// <summary>
    /// Indicates that the animation is only playing a specific part
    /// </summary>
    public bool isPlayingPartly
    {
        get
        {
            return (_playing && endFrame != -1);
        }
    }

    /// <summary>
    /// Indicates that the animation is playing forward
    /// </summary>
    public bool isPlayingForward
    {
        get
        {
            return (_playing && direction == 1);
        }
    }

    /// <summary>
    /// Indicates that the animation is playing backward
    /// </summary>
    public bool isPlayingBackward
    {
        get
        {
            return (_playing && direction == -1);
        }
    }
	
	/// <summary>
	/// Reverse the animation. 
	/// </summary>
	public void Reverse()
	{
		if (animation!=null)
		{
			direction *= -1 ;
			time = animation.GetDuration(frameset)-time;
		}		
	}

    //-----------------------------------------------------------------------------
    // private and protected fields
    //-----------------------------------------------------------------------------
    bool waiting = true;
    float time = 0;
    float _time = -1;
    bool _playing = false;
    int direction = 1;
    int startFrame = -1;
    int endFrame = -1;
    float delay = 0;
    float waitTime = 0;
    float endTime = 0;
    int timesPlayed = 0;

    OTAnimationFrameset frameset = null;
    float frDuration;
    int frCount;
	
	
	public override void PassiveUpdate()
	{
		base.PassiveUpdate();
		if (isPlaying)
			Update();
	}
	
	protected new void Awake()
	{
		passiveControl = true;
		if (GetComponent<OTAnimatingSpritePassive>()!=null)
			DestroyImmediate(GetComponent<OTAnimatingSpritePassive>());
		base.Awake();
	}
	
    //-----------------------------------------------------------------------------
    // public methods
    //-----------------------------------------------------------------------------
    
    public override void StartUp()
    {
        if (Application.isPlaying && playOnStart) Play();
        base.StartUp();
    }

    /// <summary>
    /// Plays this sprite's animation.
    /// </summary>
    public void Play()
    {
        this.startFrame = -1;
        this.endFrame = -1;
        this.delay = 0;
        _Play();
    }
    /// <summary>
    /// Plays a frameset of this sprite's animation.
    /// </summary>
    /// <param name="frameSet">Animation frameset to play.</param>
    public void Play(string frameSet)
    {
        this.animationFrameset = frameSet;
        Play();
    }

    /// <summary>
    /// Plays the animation once
    /// </summary>
    public void PlayOnce()
    {
        this.looping = false;
        this.startFrame = -1;
        this.endFrame = -1;
        this.delay = 0;
        _Play();
    }
		
    /// <summary>
    /// Plays a frameset of this sprite's animation once.
    /// </summary>
    /// <param name="frameSet">Animation frameset to play.</param>
    public void PlayOnce(string frameSet)
    {
        this.animationFrameset = frameSet;
        PlayOnce();
    }
	
	
    /// <summary>
    /// Plays this animation looping
    /// </summary>
    public void PlayLoop()
    {
        this.looping = true;
        this.startFrame = -1;
        this.endFrame = -1;
        this.delay = 0;
        _Play();
    }
		
    /// <summary>
    /// Plays a frameset of this sprite's animation looping.
    /// </summary>
    /// <param name="frameSet">Animation frameset to play.</param>
    public void PlayLoop(string frameSet)
    {
		if (!isPlaying || (isPlaying && animationFrameset!=frameSet))
		{
        	this.animationFrameset = frameSet;
			PlayLoop();
		}
    }
    
    /// <summary>
    /// Plays this sprite's animation from a start frame.
    /// </summary>
    /// <param name="startFrame">Animation frame number where to start.</param>
    public void Play(int startFrame)
    {
        this.startFrame = startFrame;
        this.endFrame = -1;
        this.delay = 0;
        _Play();
    }
	
    /// <summary>
    /// Plays this sprite's animation from a start frame, starting after a delay.
    /// </summary>
    /// <param name="startFrame">Animation frame number where to start.</param>
    /// <param name="delay">After this delay in seconds the animation will be started.</param>
    public void Play(int startFrame, float delay)
    {
        this.startFrame = startFrame;
        this.endFrame = -1;
        this.delay = delay;
        _Play();
    }
    /// <summary>
    /// Plays this sprite's animation partly from a start frame to an end frame, starting after a delay.
    /// </summary>
    /// <param name="startFrame">Animation frame number where to start.</param>
    /// <param name="endFrame">Animation frame number where to end.</param>
    /// <param name="delay">After this delay in seconds the animation will be started.</param>
    public void Play(int startFrame, int endFrame, float delay)
    {
        if (startFrame <= endFrame)
        {
            this.startFrame = startFrame;
            this.endFrame = endFrame;
            this.delay = delay;
            _Play();
        }
        else
        {
            this.endFrame = startFrame;
            this.startFrame = endFrame;
            this.delay = delay;
            _PlayBackward();
        }
    }

    /// <summary>
    /// Plays this sprite's animation backward.
    /// </summary>
    public void PlayBackward()
    {
        this.startFrame = -1;
        this.endFrame = -1;
        _PlayBackward();
    }
    /// <summary>
    /// Plays this a frameset of this sprite's animation backward.
    /// </summary>
    /// <param name="frameSet">Animation frameset to play.</param>
    public void PlayBackward(string frameSet)
    {
        this.animationFrameset = frameSet;
        PlayBackward();
    }

    /// <summary>
    /// Plays this animation once backward
    /// </summary>
    public void PlayOnceBackward()
    {
        this.startFrame = -1;
        this.endFrame = -1;
        this.looping = false;
        _PlayBackward();
    }
	
	
    /// <summary>
    /// Plays this a frameset of this sprite's animation backward once.
    /// </summary>
    /// <param name="frameSet">Animation frameset to play.</param>
    public void PlayOnceBackward(string frameSet)
    {
        this.animationFrameset = frameSet;
        PlayOnceBackward();
    }

    /// <summary>
    /// Plays this animation looping backward
    /// </summary>
    public void PlayLoopBackward()
    {
        this.startFrame = -1;
        this.endFrame = -1;
        this.looping = true;
        _PlayBackward();
    }	
	
    /// <summary>
    /// Plays this a frameset of this sprite's animation backward looping.
    /// </summary>
    /// <param name="frameSet">Animation frameset to play.</param>
    public void PlayLoopBackward(string frameSet)
    {
        this.animationFrameset = frameSet;
        PlayLoopBackward();
    }
	
    /// <summary>
    /// Plays this sprite's animation backward starting at the start frame.
    /// </summary>
    /// <param name="startFrame">Animation frame number (from the back) where to start.</param>
    public void PlayBackward(int startFrame)
    {
        this.startFrame = startFrame;
        this.endFrame = -1;
        this.delay = 0;
        _PlayBackward();
    }

    /// <summary>
    /// Plays this sprite's animation backward from a start frame, starting after a delay.
    /// </summary>
    /// <param name="startFrame">Animation frame number (from the back) where to start.</param>
    /// <param name="delay">After this delay in seconds the animation will be started.</param>
    public void PlayBackward(int startFrame, float delay)
    {
        this.startFrame = startFrame;
        this.endFrame = -1;
        this.delay = delay;
        _PlayBackward();
    }
    /// <summary>
    /// Plays this sprite's animation partly backward from a start frame to an end frame, starting after a delay.
    /// </summary>
    /// <param name="startFrame">Animation frame number (from the back) where to start.</param>
    /// <param name="endFrame">Animation frame number (from the back) where to end.</param>
    /// <param name="delay">After this delay in seconds the animation will be started.</param>
    public void PlayBackward(int startFrame, int endFrame, float delay)
    {
        if (startFrame <= endFrame)
        {
            this.startFrame = startFrame;
            this.endFrame = endFrame;
            this.delay = delay;
            _PlayBackward();
        }
        else
        {
            this.endFrame = startFrame;
            this.startFrame = endFrame;
            this.delay = delay;
            _Play();
        }
    }

    /// <summary>
    /// Pauze the current playhing animation.
    /// </summary>
    public void Pauze()
    {
        _playing = false;
        if (onAnimationPauze != null)
            onAnimationPauze(this);
        if (!CallBack("onAnimationPauze", callBackParams))
            CallBack("OnAnimationPauze", callBackParams);
    }

    /// <summary>
    /// Resume the current pauzed animation.
    /// </summary>
    public void Resume()
    {
        _playing = true;
        if (onAnimationResume != null)
            onAnimationResume(this);
        if (!CallBack("onAnimationResume", callBackParams))
            CallBack("OnAnimationResume", callBackParams);
    }

    /// <summary>
    /// Stop the current plagying animation.
    /// </summary>
    public void Stop()
    {
        _playing = false;
        time = 0;
        timesPlayed = 0;
        endFrame = -1;

        if (onAnimationFinish != null)
            onAnimationFinish(this);
        if (!CallBack("onAnimationFinish", callBackParams))
            CallBack("OnAnimationFinish", callBackParams);

        if (destroyWhenFinished)
            OT.DestroyObject(this);
    }

    
    protected override Material InitMaterial()
    {
        if (spriteContainer == null && animation != null) return null;
        Material mat = base.InitMaterial();
        return mat;
    }

    /// <summary>
    /// Shows a specific animation frame.
    /// </summary>
    /// <param name="frameNumber">number of the animation frame to show.</param>
    public void ShowFrame(int frameNumber)
    {
        if (animation != null && animation.isReady)
        {
			InitAnimation();
            if (frameNumber >= 0 && frameNumber < frames.Length)
            {				
				SetAnimationFrame(frames[frameNumber]);	
				time = frameNumber * frDurationDelta;
				frTime = 0;				
                if (isPlaying)
                    Pauze();
            }
            else
                throw (new System.IndexOutOfRangeException("Frame number out of range!"));
        }
    }

    //-----------------------------------------------------------------------------
    // class methods
    //-----------------------------------------------------------------------------
    
    protected override string GetTypeName()
    {
        return "Animating Sprite";
    }

	
    override protected void CheckSettings()
    {
        base.CheckSettings();
        if (Application.isEditor || OT.dirtyChecks || dirtyChecks)
        {
			if (animation!=null)
				_animationName = animation.name;
		}
	}


	string animationFramesetLower = "";
    protected override void Start()
    {
        base.Start();
		
		animationFramesetLower = animationFrameset.ToLower();		
        if (playOnStart)
            Play();
    }

    void _Play()
    {
        direction = 1;
        __Play();
    }

    void _PlayBackward()
    {
        direction = -1;
        __Play();
    }

    void __Play()
    {
        _playing = true;
        waiting = true;
        timesPlayed = 0;		
    }
	
	float frTime = 0;
	float frDurationDelta = 0;
	OTAnimation.Frame[] frames = new OTAnimation.Frame[]{};
	
	void SetAnimationFrame(OTAnimation.Frame animationFrame)
	{
        if (spriteContainer != animationFrame.container || animationFrame.frameIndex != frameIndex)
        {	
			if (passive)
			{
	            spriteContainer = animationFrame.container;
	            frameIndex = animationFrame.frameIndex;
			}
			else
			{
	            _spriteContainer = animationFrame.container;
	            _frameIndex = animationFrame.frameIndex;
			}

            if (onAnimationFrame != null)
                onAnimationFrame(this);
            if (!CallBack("onAnimationFrame", callBackParams))
                CallBack("OnAnimationFrame", callBackParams);

            isDirty = true;
        }
	}
	
	int _animationFrameNumber = -1;
	/// <summary>
	/// Gets the current frame number of the playing animation (frameset)
	/// </summary>
	public int animationFrameNumber
	{
		get
		{
			return _animationFrameNumber;
		}
	}
	
    void UpdateFrame(float deltaTime)
    {		
		if (frames == null || frames.Length == 0)
			return;
		
		if (frTime == 0 || frTime >= frDurationDelta)
		{	
			if (frTime>=frDurationDelta)
				frTime -= frDurationDelta;
			int idx = Mathf.FloorToInt(time/frDurationDelta);
			if (idx>=frames.Length) idx = 0;
	        OTAnimation.Frame animationFrame = frames[idx];
			_animationFrameNumber = idx;
			SetAnimationFrame(animationFrame);	
	        time += (deltaTime * speed);
	        if (endFrame != -1 && time >= endTime)
	        {
	            Stop();
	            return;
	        }	
		}
		else
	        time += (deltaTime * speed);
			
        if (time >= frDuration)
        {
            if (looping)
                time -= frDuration;
            else
            {
                time = 0;
                timesPlayed++;
                if (timesPlayed >= numberOfPlays)
                    Stop();
            }
        }
		frTime += (deltaTime * speed);
		// Debug.Log("frTime ="+frTime+", time = "+time);
    }
	
	void InitAnimation()
	{
        if (animationFrameset != "")
        {
            frameset = animation.GetFrameset(animationFrameset);
			if (frameset!=null)
			{
				framesetName = frameset.name.ToLower();
				animationFramesetLower = animationFrameset.ToLower();
			}
			
            frDuration = animation.GetDuration(frameset);
            frCount = animation.GetFrameCount(frameset);
        }
        else
        {
            frameset = null;
            frDuration = animation.duration;
            frCount = animation.frameCount;
        }
		frDurationDelta = frDuration/frCount;
		frTime = 0;
		
		if (frCount==0) 
			return;

        if (startAtRandomFrame)
            time = frDurationDelta * (Mathf.Floor(frCount * Random.value));
        else
        {
            if (startFrame != -1)
            {
                time = frDurationDelta * startFrame;
                if (endFrame != -1)
                    endTime = frDurationDelta * (endFrame + 1) - 0.001f;

            }
            else
                time = 0;
        }
		
		System.Array.Resize<OTAnimation.Frame>(ref frames,frCount);			
		// cache the animation frames for quicker lookup
		for (int i=0; i<frCount; i++)
			frames[i] = animation.GetFrame(i * frDurationDelta, direction, frameset);
	}
	
	
	void HandleWaiting()
	{
	    if (waitTime >= delay)
	    {
	        waitTime = 0;
	        delay = 0;
			InitAnimation();				
												
	        waiting = false;
	
	        if (onAnimationStart != null)
	            onAnimationStart(this);
	        if (!CallBack("onAnimationStart", new object[] { this }))
	            CallBack("OnAnimationStart", new object[] { this });
							
	    }
	    else
	        waitTime += Time.deltaTime;
		
	}


    // Update is called once per frame
    string framesetName = "";
    protected override void Update()
    {			
		if (animation == null && _animationName!=null)
			animation = OT.AnimationByName(_animationName);
		
		if (animation == null) 
			return;
		
		if (!animation.isReady)
		{
			if (!animation.enabled)
				animation.SendMessage("Update");
			return;
		}
		
        if (spriteContainer != null && !spriteContainer.isReady)
            return;

        if (speed < 0) speed = 0;
        if (Application.isPlaying)		
        {
			if (_playing)			
			{
	            if (waiting)
					HandleWaiting();
				
	            if (!waiting)
	               	UpdateFrame(Time.deltaTime);
			}
        }
        else
			EditorPreview();
		
       	base.Update();
    }
	
	void EditorPreview()
	{
        if (animationPreview < 0) animationPreview = 0;
        else
            if (animationPreview > 100) animationPreview = 100;
        if (!Application.isPlaying && animation != null && animation.isReady)
        {
            if (animationFrameset != "" && (frameset == null || (frameset != null && framesetName != animationFramesetLower)))
            {
                frameset = animation.GetFrameset(animationFrameset);
                frCount = animation.GetFrameCount(frameset);
	            frDuration = animation.GetDuration(frameset);				
				System.Array.Resize<OTAnimation.Frame>(ref frames,0);			
            }
			else
			{
				frCount = animation.frameCount;
				frDuration = animation.duration;
			}
			frDurationDelta = frDuration/frCount;
			

			if (frames.Length == 0)
			{
				System.Array.Resize<OTAnimation.Frame>(ref frames,frCount);			
				// cache the animation frames for quicker lookup
				for (int i=0; i<frCount; i++)
					frames[i] = animation.GetFrame(i * frDurationDelta, direction, frameset);
			}			
					
            frDuration = animation.GetDuration(frameset);
            if (frameset != null && frameset.name == "")
                frameset = null;
            time = ((frDuration / 100) * animationPreview);
            if (time == frDuration) time -= 0.001f;
            if (time != _time)
            {
				frTime = frDurationDelta;
                UpdateFrame(0);
                _time = time;
            }
        }
        else
            if (animation != null && animation.isReady)
            {
                if (spriteContainer == null)
                    spriteContainer = animation.GetFrame(0, 1, null).container;
            }
		
	}
	
}
