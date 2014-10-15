using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OTTweenController : OTController
{

    public List<OTTween> tweens = new List<OTTween>();

    public OTTweenController(string name)
        : base(null, name)
    {
    }

    public OTTweenController()
        : base()
    {
    }

    public void Add(OTTween tween)
    {
      tweens.Add(tween);
    }
		
    public void Clear(bool runningTweens, bool waitingTweens)
    {
		
        if(!runningTweens && !waitingTweens)
			return;
				
                //if both are true we want everything gone
        if(runningTweens && waitingTweens)
        {
            tweens.Clear();
            return;
        }
		
        if(runningTweens)
        {
            //remove all tweens that are currently running
            for(int i=0;i<tweens.Count;i++)
                if(tweens[i].isRunning)
                    tweens.RemoveAt(i);
        }
        else
        {
            //remove all tweens that are currently waiting to start
            for(int i=0;i<tweens.Count;i++)
                if(tweens[i].isWaiting)
                    tweens.RemoveAt(i);
        }
    }	

    protected override void Update()
    {
        base.Update();

       	int t = 0;
		while (t<tweens.Count)
		{
			if (tweens[t].Update(deltaTime))
			{
				if (!tweens[t].restarted)
					tweens.Remove(tweens[t]);
			}
			else
				t++;
		}
    }

}
