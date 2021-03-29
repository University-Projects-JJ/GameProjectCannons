using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleScript : MonoBehaviour {
	public int health, MAX_HEALTH;
	public Image imgHealth;
	public Text txtHealth;
	public GameObject belongsToPlayer;
	// public GameObject destroyedModel;


	// Update is called once per frame
	void Update() {
		displayHealth();
	}
	void displayHealth() {
		if (imgHealth != null)
			imgHealth.fillAmount = (float)(health * 1.0f) / MAX_HEALTH;
		if (txtHealth != null)
			txtHealth.text = health.ToString();
	}

	void OnCollisionEnter(Collision collision) {
		GameObject target = collision.gameObject;
		if (gameObject.tag == "Fence" && target.tag == "Fence") {
			// don't collide
		}
	}

	// Start is called before the first frame update
	public void TakeDamage(int damage) {

		health -= damage;
		if (health <= 0) {
			if (gameObject.tag == "Fence")
				destroyFence();
			else if (gameObject.tag == "Player") {
				// end game
			}
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

	// disables fence
	public void destroyFence() {
		gameObject.SetActive(false);
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
