using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {
	private int POWERUP_HEALTH = 250;
	private int POWERUP_DOUBLE_DAMAGE_COUNT = 3;
	public enum POWERUP_TYPES { HEAL, AMMO, SHIELD };
	public POWERUP_TYPES powerupType;
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
		ApplyPowerup(player);

		// destroy bullet here
		// Destroy(collision.gameObject);
		Destroy(gameObject);
	}

	void ApplyPowerup(GameObject playerToApply) {
		PlayerScript player = playerToApply.GetComponent<PlayerScript>();
		if (powerupType == POWERUP_TYPES.HEAL) {
			player.healPlayer(POWERUP_HEALTH);
		}

		if (powerupType == POWERUP_TYPES.AMMO) {
			player.enableDoubleDamage(POWERUP_DOUBLE_DAMAGE_COUNT);
		}

		if (powerupType == POWERUP_TYPES.SHIELD) {
			player.restoreDefenses();
		}
	}

	void restoreDefenses(Transform defensesParent) {
		foreach (Transform child in defensesParent) {
			GameObject defense = child.gameObject;
			defense.GetComponent<HealthScript>().health = 200;
			defense.SetActive(true);
		}
	}
}
