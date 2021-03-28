using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {
	// public GameObject destroyedModel;
	public float health;
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}


	// Start is called before the first frame update
	public void TakeDamage(float damage) {
		health -= damage;
		if (health <= 0) {

			if (gameObject.tag == "Fence")
				gameObject.GetComponent<ObstacleScript>().destroyFence();
			else
				Destroy(gameObject);
			// 3 variations
			// 1 - Destroy and replace by a particle system

			// 2 - Changing the texture
			// foreach (Transform lod in transform) {
			// lod.gameObject.GetComponent<MeshRenderer>().material = destroyedMaterial
			// }

			// 3 - Replace it by another asset (different mesh or Ragdoll)
			// StartCoroutine("ReplaceWithDestroyed");

			// Destroy the text
			// Destroy(healthText.transform.parent.gameObject);
		}
		else {
			// if zombie add dying animation for a few seconds before continuing to walk
			if (gameObject.tag == "Zombie") {
				StartCoroutine(gameObject.GetComponent<ZombieScript>().fallDown());
			}
		}
	}

	// IEnumerator ReplaceWithDestroyed() {
	// 	GameObject destroyed = Instantiate(destroyedModel, transform.position, transform.rotation);
	// 	Rigidbody destroyedRB = destroyed.GetComponent<Rigidbody>();

	// 	destroyedRB.velocity = gameObject.GetComponent<Rigidbody>().velocity;
	// 	destroyedRB.angularVelocity = gameObject.GetComponent<Rigidbody>().angularVelocity;

	// 	// wait before destroying the original asset to be able to copy its properties
	// 	yield return new WaitForSeconds(0.1f);
	// 	// Destroy the current Barrel
	// 	Destroy(gameObject);
	// }
}
