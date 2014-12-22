using UnityEngine;
using System.Collections;

public class SimpleMovementScript : MonoBehaviour {

	public float speed = 0.03f;
	public float maxdistance= 1f;
	public float mindistance= 0.1f;
	private int direction = -1;
	private Vector3 initPos;
	// Use this for initialization
	void Start () {
		initPos = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float distance = initPos.z - transform.position.z;

		if (distance > maxdistance)
			direction = 1;
		else if (distance < mindistance)
			direction = -1;

		transform.Translate(0,0,1*speed*direction, Space.World );
	}
}
