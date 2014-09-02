using UnityEngine;
using System.Collections;

namespace ST
{
    /// <summary>
    /// Generic Helper for all un-sorted/un-refactored utility functions
    /// </summary>
    public class MiscHelper
    {
        /// <summary>
        /// Returns whether the supplied LayerMask contains the supplied layer.
        /// </summary>
        public static bool MaskContainsLayer (LayerMask mask, int layer)
        {
            return ((1 << layer) & mask.value) != 0;
        }   
    }
}