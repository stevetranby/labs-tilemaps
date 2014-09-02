using UnityEngine;
using System.Collections;

namespace ST
{
    /// <summary>
    /// Possibly unnecessary
    /// </summary>
    public class AudioUtils
    {
        public static void GetInstance ()
        {
        }

        // cache main audio source, audio clips, etc
        public static void PlaySound (string name, int loopCount, float volume, Vector3 sourcePosition)
        {
        }

        public static void PauseSound (string name)
        {
        }

        public static void StopSound (string name)
        {
        }

        // music
        public static void PlayMusic (string name, int loopCount)
        {
        }

        public static void PauseMusic (string name)
        {
        }

        public static void StopMusic (string name)
        {
        }

        public static void PreloadMusic (string name)
        {
        }

        public static void SeekMusic (float timestamp)
        {
        }

        // current time within music track
        public static float GetMusicTimestamp ()
        {
            return 0f;
        }
    }
}
