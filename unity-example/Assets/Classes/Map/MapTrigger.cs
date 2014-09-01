using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ST
{
    public class MapTrigger : MonoBehaviour
    {    
        // objects we aren't tracking
        Collider[] ignoreColliders;
        GameObject testIgnore;
        List<GameObject> objects;
        bool trackOtherTriggers;
        // layer(s) we're interested in
        LayerMask layerMask;
  
        // Record when objects enter
        public void OnTriggerEnter (Collider other)
        {
            if (! trackOtherTriggers && other.isTrigger) {
                return;
            }
        
            // is this collider in our ignore list?
            foreach (Collider testIgnore in ignoreColliders) {
                if (testIgnore == other) {
                    return;
                }
            }
        
            var go = other.gameObject;       
        
            // if not in mask we're interested in, continue
            if (! MiscHelper.MaskContainsLayer (layerMask, go.layer))
                return;
        
            if (! objects.Contains (go)) {
                objects.Add (go);
            }
        }
    
        // Update when objects leave
        public void OnTriggerExit (Collider other)
        {
            if (!trackOtherTriggers && other.isTrigger)
                return; 
        
            // is this collider in our ignore list?
            foreach (var testIgnore in ignoreColliders) {
                if (testIgnore == other) {
                    return; 
                }
            }
        
            var go = other.gameObject;       
        
            // if not in mask we're interested in, continue
            if (! MiscHelper.MaskContainsLayer (layerMask, go.layer)) {
                return;
            }
        
            objects.Remove (go);
        }
    }
}