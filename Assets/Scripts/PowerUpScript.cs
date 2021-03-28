using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {
	private float POWER_HEALTH = 250;
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	void healPlayer(GameObject player) {
		player.GetComponent<PlayerScript>().playerHealth += 250;
	}

	void restoreDefenses(Transform defensesParent) {
		foreach (Transform child in defensesParent) {
			GameObject defense = child.gameObject;
			defense.GetComponent<HealthScript>().health = 200;
			defense.SetActive(true);
		}
	}
}
