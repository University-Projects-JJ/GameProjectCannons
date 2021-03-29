using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

	public readonly int PLAYER_TIMER = 60;
	public int playerScore = 0, playerTimer;
	public Text txtPlayerScore, txtPlayerTimer;
	// public Image imgPlayerHealthBar;
	public GameObject imgListDoubleDamageBullets, imgPointerToPlayer;

	public int doubleDamageBullets = 0;
	public List<GameObject> defenses;
	public Transform defensesParent;
	// Start is called before the first frame update
	void Start() {
		// SET UP HEALTH
		int MAX_HEALTH = GameManager.instance.MAX_PLAYER_HEALTH;
		gameObject.GetComponentInChildren<ObstacleScript>().health = MAX_HEALTH;
		gameObject.GetComponentInChildren<ObstacleScript>().MAX_HEALTH = MAX_HEALTH;

		playerTimer = PLAYER_TIMER;

		defenses = new List<GameObject>();
		foreach (Transform child in defensesParent) {
			GameObject fence = child.gameObject;
			defenses.Add(fence);
		}
		restoreDefenses();

		StartCoroutine(decrementPlayerTimer());
	}

	// Update is called once per frame
	void Update() {
		showDoubleDamageBullets();
		showPlayerScore();
	}
	void FixedUpdate() {
	}

	// void displayHealth() {
	// 	int health = gameObject.GetComponentInChildren<HealthScript>().health;
	// 	int MAX_HEALTH = gameObject.GetComponentInChildren<HealthScript>().MAX_HEALTH;
	// 	txtPlayerHealth.text = health.ToString();
	// 	imgPlayerHealthBar.fillAmount = health / MAX_HEALTH;
	// }

	public void healPlayer(int health) {
		int newHealth = gameObject.GetComponentInChildren<ObstacleScript>().health + health;
		int MAX_PLAYER_HEALTH = GameManager.instance.MAX_PLAYER_HEALTH;
		gameObject.GetComponentInChildren<ObstacleScript>().health = newHealth > MAX_PLAYER_HEALTH ? MAX_PLAYER_HEALTH : newHealth;
	}

	public void enableDoubleDamage(int count) {
		doubleDamageBullets = count;
	}

	public void restoreDefenses() {
		foreach (GameObject fence in defenses) {
			fence.GetComponent<ObstacleScript>().health = GameManager.instance.MAX_FENCE_HEALTH;
			fence.GetComponent<ObstacleScript>().MAX_HEALTH = GameManager.instance.MAX_FENCE_HEALTH;
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

	public IEnumerator decrementPlayerTimer() {
		while (playerTimer > 0) {
			yield return new WaitForSeconds(1);
			// decrement timer if player's turn and hasn't shot yet
			if (GameManager.instance.getCurrentPlayer() == gameObject && gameObject.GetComponentInChildren<TurretController>().canShoot) {
				playerTimer--;
				txtPlayerTimer.text = playerTimer.ToString() + "s";
			}
		}
		if (playerTimer <= 0)
			GameManager.instance.endGame("time");
	}
}
