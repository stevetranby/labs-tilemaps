using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {
	
	public OTSprite destination;
	
	NavMeshAgent na;
	OTSprite sprite;
		
	// Use this for initialization
	void Start () {
		sprite = GetComponent<OTSprite>();
		na = GetComponent<NavMeshAgent>();
		na.updateRotation = false;
	}
	
	// Update is called once per frame
	void Update () {
		na.destination = destination.gameObject.transform.position;
		sprite.RotateTowards(new Vector2(na.steeringTarget.x,na.steeringTarget.z));				
	}
}
