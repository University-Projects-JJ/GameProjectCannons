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
	public GameObject TakeDamage(int damage) {

		health -= damage;
		if (health <= 0) {
			health = 0;

			if (gameObject.tag == "Fence")
				destroyFence();
			else if (gameObject.tag == "Player") {
				// end game
				GameManager.instance.endGame("health");
			}
			else
				Destroy(gameObject);

			return null;
		}
		else {
			// if zombie add dying animation for a few seconds before continuing to walk
			if (gameObject.tag == "Zombie") {
				ZombieScript zombieScript = gameObject.GetComponent<ZombieScript>();
				StartCoroutine(zombieScript.fallDown());
			}
			return gameObject;
		}
	}

	// disables fence
	public void destroyFence() {
		gameObject.SetActive(false);
	}
}