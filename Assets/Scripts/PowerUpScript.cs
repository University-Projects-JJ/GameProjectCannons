using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {

	public PowerupManager.POWERUP_TYPES powerupType;
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	void OnCollisionEnter(Collision collision) {
		// will only collide with bullets
		GameObject player = collision.gameObject.GetComponent<BulletScript>().belongsToPlayer;

		// apply powerup
		PowerupManager.instance.ApplyPowerup(powerupType, player);

		// destroy bullet here
		// Destroy(collision.gameObject);
		Destroy(gameObject);
	}
}
