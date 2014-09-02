using UnityEngine;
using System.Collections;
using Pathfinding;

public class TestAISeekPlayer : MonoBehaviour {
	//The point to move to
	public Vector3 targetPosition;
	
	private Seeker seeker;
	private CharacterController controller;
	
	//The calculated path
	public Path path;
	
	//The AI's speed per second
	public float speed = 100;
	
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	
	public void Start () {
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
	}

	public void updatePath() {
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		seeker.StartPath (transform.position, targetPosition, OnPathComplete);
	}

	public void OnDisable () {
		seeker.pathCallback -= OnPathComplete;
	}
	
	public void OnPathComplete (Path p) {
		Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
		if (!p.error) {
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;

			// TODO: combine any waypoints in straight line to use the last waypoint before turn
		}
	}

	public void FixedUpdate () {
		if (path == null) {
			//We have no path to move after yet
			return;
		}
		
		if (currentWaypoint >= path.vectorPath.Count) {
			//Debug.Log ("End Of Path Reached");
			return;
		}

		// TODO: use next waypoint to find if the current speed movement would overshoot the waypoint
		//       and if it does then just move exactly to waypoint


		//Direction to the next waypoint
		Vector3 cur = transform.position;
		Vector3 next = path.vectorPath[currentWaypoint];
		next.y = cur.y;

		Vector3 dir = (next-cur).normalized;
		float speedDist = speed * Time.fixedDeltaTime;
		dir *= speedDist;

//		// NOTE: we don't want gravity by default, so use Move() instead
//		dir *= speedDist;
//		controller.SimpleMove (dir);


		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		float dist = Vector3.Distance (cur, next);

		// Clamp to next waypoint in case we're too close that we overshoot
		float mag = Mathf.Clamp((float)dist, nextWaypointDistance * 0.2f, nextWaypointDistance * 0.8f);

		//Debug.Log("dist = " + dist);
		if (dist < nextWaypointDistance) {
			// move to next waypoint
			controller.Move (path.vectorPath[currentWaypoint] - transform.position);
			currentWaypoint++;
			return;
		} else {
			controller.Move(dir);
		}
	}
} 