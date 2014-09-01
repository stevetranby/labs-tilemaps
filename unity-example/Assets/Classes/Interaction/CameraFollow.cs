using UnityEngine;
using System.Collections;

namespace ST
{
    /// <summary>
    /// Camera follow from unsourced sample code.
    /// 
    /// TODO: 
    /// - test performance
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        // What to follow
        public Transform target;

        // Settings
        public float dampingFactor = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        // Internal Use
        private Transform cachedTransform;
        private float offsetZ;
        private Vector3 lastTargetPosition;
        private Vector3 currentVelocity;
        private Vector3 lookAheadPosition;
            
        void Start ()
        {
            this.cachedTransform = this.GetComponent<Transform> ();
            lastTargetPosition = target.position;
            offsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }
    
        void Update ()
        {
            // only update lookahead pos if accelerating or changed direction
            float dx = (target.position - lastTargetPosition).x;
        
            bool updateLookAheadTarget = Mathf.Abs (dx) > lookAheadMoveThreshold;
        
            if (updateLookAheadTarget) {
                lookAheadPosition = lookAheadFactor * Vector3.right * Mathf.Sign (dx);
            } else {
                lookAheadPosition = Vector3.MoveTowards (lookAheadPosition, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }
        
            Vector3 aheadTargetPosition = target.position + lookAheadPosition + Vector3.forward * offsetZ;
            Vector3 newPosition = Vector3.SmoothDamp (transform.position, aheadTargetPosition, ref currentVelocity, dampingFactor);
        
            this.cachedTransform.position = newPosition;
        
            lastTargetPosition = target.position;
        }
    }
}
