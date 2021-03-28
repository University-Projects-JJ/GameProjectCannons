using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

	public float playerHealth = 0;
	public int playerScore = 0;
	public Text txtPlayerHealth, txtPlayerScore;
	public Image imgPlayerHealthBar;
	public GameObject imgListDoubleDamageBullets;

	public int doubleDamageBullets = 0;
	public List<GameObject> defenses;
	public Transform defensesParent;
	// Start is called before the first frame update
	void Start() {
		playerHealth = GameManager.instance.MAX_PLAYER_HEALTH;
		defenses = new List<GameObject>();
		foreach (Transform child in defensesParent) {
			GameObject fence = child.gameObject;
			defenses.Add(fence);
		}
		restoreDefenses();
	}

	// Update is called once per frame
	void Update() {

	}
	void FixedUpdate() {
		displayHealth();
		showDoubleDamageBullets();
		showPlayerScore();
	}

	void displayHealth() {
		txtPlayerHealth.text = playerHealth.ToString();
		imgPlayerHealthBar.fillAmount = playerHealth / 1000;
	}

	public void healPlayer(float health) {
		float newHealth = playerHealth + health;
		float MAX_PLAYER_HEALTH = GameManager.instance.MAX_PLAYER_HEALTH;
		playerHealth = newHealth > MAX_PLAYER_HEALTH ? MAX_PLAYER_HEALTH : newHealth;
	}

	public void enableDoubleDamage(int count) {
		doubleDamageBullets = count;
	}

	public void restoreDefenses() {
		foreach (GameObject fence in defenses) {
			fence.GetComponent<HealthScript>().health = GameManager.instance.MAX_FENCE_HEALTH;
			if (!fence.activeInHierarchy)
				fence.SetActive(true);
		}
	}

	public void showDoubleDamageBullets() {
		for (int i = 1; i <= 3; i++) {
			// color the correct amount of double damabge bullets a player has
			Color color = i > doubleDamageBullets ? new Color(0.08f, 0.08f, 0.08f, 0.48f) : Color.white;
			imgListDoubleDamageBullets.transform.GetChild(i - 1).GetComponent<Image>().color = color;
		}
	}

	public void showPlayerScore() {
		txtPlayerScore.text = "Score: " + playerScore.ToString();
	}
}
