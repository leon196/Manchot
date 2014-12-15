
using UnityEngine;
using System.Collections;

public class collidescript : MonoBehaviour {

	public string collidetag= "sphere";
	// The Controller GameObject which has the network script attached
	public GameObject controller;


	void Start(){
		if (controller == null)
			print ("Error, no Controller Object attached");
	}

	void OnCollisionEnter ( Collision collision) {

		GameObject other = collision.gameObject;
		if (other.CompareTag (collidetag)) {
			//Send Message To Controller Object 
			controller.SendMessage("SendSerialMessage", "1");
		}
	}

	void OnTriggerEnter (Collider other) {
		// other gameObject needs a rigidbody attached

		if (other.gameObject.CompareTag (collidetag)) {
			//Debug.Log("sending message to arduino");
			controller.SendMessage("SendSerialMessage", "1");
		}
	}
}