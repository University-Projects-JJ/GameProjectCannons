using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour {
	GameObject belongsToPlayer;
	// Start is called before the first frame update

	void OnCollisionEnter(Collision collision) {
		GameObject target = collision.gameObject;
		if (gameObject.tag == "Fence" && target.tag == "Fence") {
			// don't collide
		}
	}

	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
}
