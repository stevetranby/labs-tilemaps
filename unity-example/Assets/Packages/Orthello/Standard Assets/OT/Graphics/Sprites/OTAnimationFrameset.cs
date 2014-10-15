using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// Will store animation information about a sequence of <see cref="OTContainer" /> frames.
/// </summary>
[System.Serializable]
public class OTAnimationFrameset
{
    /// <summary>
    /// Animation frameset name
    /// </summary>
    public string name;
    /// <summary>
    /// Animation frameset container
    /// </summary>
    public OTContainer container;
    /// <summary>
    /// Frameset container start frame, used if frameType is BY_FRAMENUMBER
    /// </summary>
    public int startFrame;
    /// <summary>
    /// Frameset container end frame, used if frameType is BY_FRAMENUMBER
    /// </summary>
    public int endFrame;
    /// <summary>
    /// Set of frame names, used if frameType is BY_NAME 
    /// </summary>
    public string[] frameNames;
    /// <summary>
    /// Get frame names from container using this mask
    /// </summary>
    /// <description>
    /// Can be the name prefix or a regular expression
    /// For example a value of '^(?i:(idle))' will get all 
    /// frames that start with 'idle' and ignoring case.
    /// When using a regular string value, the mask will
    /// be validated as a case insensitive prefix
    /// </description>
    public string frameNameMask;
    /// <summary>
    /// If true, frame names will be sorted
    /// </summary>
    public bool sortFrameNames = true;
   /// <summary>
    /// Frameset (start to end) play count
    /// </summary>
    public int playCount = 1;
    /// <summary>
    /// Ping pong indicator
    /// </summary>
    /// <remarks>
    /// By setting pingPong to true you indicate that this animation frameset has to bounce
    /// back after the animation reaches this frameset's last frame. The end and start frame
    /// of this animation frameset's will be only displayed once. 
    /// </remarks>
    public bool pingPong = false;
    /// <summary>
    /// Animation frameset duration.
    /// </summary>
    /// <remarks>
    /// This duration is used if only this animation's frameset is played and
    /// this setting has a value bigger than 0. If the singleDuration is 0 
    /// the animation's fps  setting will be used to calculate the actual duration.
    /// </remarks>
    public float singleDuration = 0f;

    /// <summary>
    /// Get the list of frame numbers for this frameset 
    /// </summary>
    public int[] frameNumbers
    {
        get
        {
            int totalFrames = frameCount;

            if ((container == null) || (totalFrames == 0))
            {
                return new int[] { };
            }

            int[] frames = new int[totalFrames];
            int start, end, direction;
			
			if (frameNameMask!="" && container!=null)
			{
				Regex regex = null;
				
				try
				{
					if ((frameNameMask[0] == '^' || frameNameMask[0] == '[' || frameNameMask[0] == '('))
						regex = new Regex( @frameNameMask );
				}
				catch(System.Exception)
				{
					regex = null;
				}
				
				List<string> names = new List<string>();
				for (int f=0; f<container.frameCount; f++)
				{
					OTContainer.Frame fr = container.GetFrame(f);	
					try
					{
						if ((regex!=null && regex.Match(fr.name).Success) || (fr.name.ToLower().StartsWith(frameNameMask.ToLower())))
							names.Add(fr.name);															
					}
					catch(System.Exception)
					{
					}
				}
				frameNames = names.ToArray();
			}
			
            if (frameNames!=null && frameNames.Length > 0)
            {								
				if (sortFrameNames) 
					System.Array.Sort(frameNames);
								
                start = 0;
                end = frameNames.Length - 1;
                direction = 1;
            }
            else
            {
                start = (startFrame <= endFrame) ? startFrame : endFrame;
                end = (startFrame <= endFrame) ? endFrame : startFrame;
                direction = (startFrame <= endFrame) ? 1 : -1;
            }
            int numFrames = (end - start) + 1;

            // Calculate a single play's worth of frames (this includes a ping pong)
            int frameIndex = 0;
			
			if (frames.Length<numFrames)
				System.Array.Resize<int>(ref frames,numFrames);
			
            for (int i = 0; i < numFrames; ++i)
            {				
                if (frameNames != null && frameNames.Length>0)
                    frames[i] = container.GetFrameIndex(frameNames[frameIndex]);
                else
                    frames[i] = start + frameIndex;

                frameIndex += direction;
            }

            // Since we don't repeat the start or end frames, then we have to have more than 2 frames for this 
            // to work.
            if ((numFrames > 2) && pingPong)
            {
                System.Array.Copy(frames, 1, frames, numFrames, numFrames - 2);
                System.Array.Reverse(frames, numFrames, numFrames - 2);
                numFrames = numFrames + (numFrames - 2);
            }
		
            // Now repeat that array copy for playCount times
			if (playCount>1 && playCount<=10)
			{				
	            for (int i = 1; i < playCount; i++)
	                System.Array.Copy(frames, 0, frames, i * numFrames, numFrames);
			}
			else
				if (playCount>1)
					Debug.LogWarning("AnimationFrameset "+name+" is set to played more than 10 times!");

            return frames;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int baseFrameCount
    {
        get
        {
            int c;
            if (frameNames!=null && frameNames.Length > 0)
                c = frameNames.Length;
            else
            {
                if (endFrame > startFrame)
                {
                    c = (endFrame - startFrame + 1);
                }
                else
                {
                    c = (startFrame - endFrame + 1);
                }
            }
            return c;
        }
    }

    /// <summary>
    /// Frame count per play 
    /// </summary>
    public int frameCountPerPlay
    {
        get
        {
            int c = baseFrameCount;
            if ((c > 2) && pingPong)
            {
                c *= 2;
                c -= 2;
            }
            return c;
        }
    }
	
	int _frameCount = -1;
    /// <summary>
    /// Number of animation frames
    /// </summary>
    public int frameCount
    {
        get
        {
			if (Application.isPlaying)
			{
				if (_frameCount==-1)
					_frameCount = frameCountPerPlay * playCount;
				return _frameCount;
			}
            return frameCountPerPlay * playCount;
        }
    }

    
    [HideInInspector]
    public int startIndex = 0;
	 
    [HideInInspector]
    public string _containerName = "";
	
}   