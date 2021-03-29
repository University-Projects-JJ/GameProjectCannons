using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public readonly int MAX_PLAYER_HEALTH = 500,
	MAX_FENCE_HEALTH = 200;

	public readonly float EXPLOSIVE_DAMAGE_MULTIPLER = 1.5f;

	public static GameManager instance;

	public GameObject player1, player2;
	private int currentPlayer = 1;
	private bool isPlaying = false;

	public GameObject panelWinning;
	public Text txtWinner;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}
	// Start is called before the first frame update
	void Start() {
		startGame();
	}

	// Update is called once per frame
	void Update() {
	}

	private void startGame() {
		isPlaying = true;
		currentPlayer = Random.Range(1, 3);
		SwitchTurns();
	}

	public void endGame(string reason = "time") {
		isPlaying = false;
		StartCoroutine(ZombieManager.instance.disableZombies());
		StartCoroutine(PowerupManager.instance.disableManager());
		PlayerScript player1Script = player1.GetComponent<PlayerScript>();
		PlayerScript player2Script = player2.GetComponent<PlayerScript>();

		player1Script.StopAllCoroutines();
		player2Script.StopAllCoroutines();

		int winner = 0;
		int player1Score = player1Script.playerScore, player1Health = player1.GetComponentInChildren<ObstacleScript>().health;
		int player2Score = player2Script.playerScore, player2Health = player2.GetComponentInChildren<ObstacleScript>().health;

		if (reason == "time") {
			if (player1Score > player2Score)
				winner = 1;
			else if (player2Score > player1Score)
				winner = 2;
			else {
				if (player1Health > player2Health)
					winner = 1;
				else if (player2Health > player1Health)
					winner = 2;
				else winner = 0;
			}
		}
		else if (reason == "health") {
			winner = player1Health == 0 ? 2 : 1;
		}
		displayWinner(winner);
	}

	void displayWinner(int winner) {
		panelWinning.SetActive(true);
		txtWinner.text = winner == 0 ? "IT'S A DRAW!!!" : "PLAYER " + winner + " WON!!!";
	}

	void enablePlayer(GameObject player, bool isEnabled) {
		TurretController turretController = player.GetComponentInChildren<TurretController>();
		PlayerScript playerScript = player.GetComponent<PlayerScript>();
		turretController.playerCamera.SetActive(isEnabled);
		turretController.enabled = isEnabled;
		turretController.canShoot = isEnabled;
		playerScript.imgListDoubleDamageBullets.SetActive(isEnabled);
		playerScript.imgPointerToPlayer.SetActive(!isEnabled);
	}

	public GameObject getCurrentPlayer() {
		// return opposite because currentPlayer is switched after enabling turn
		return currentPlayer == 2 ? player1 : player2;
	}


	public void SwitchTurns() {
		// if the game is running
		if (isPlaying) {
			// switch player
			if (currentPlayer == 1) {
				enablePlayer(player1, true);
				enablePlayer(player2, false);
			}
			else {
				enablePlayer(player1, false);
				enablePlayer(player2, true);
			}
			currentPlayer = 3 - currentPlayer;

			// run powerup spawner (20%-30% chance usually)
			PowerupManager.instance.spawnPowerup();

			// spawnZombies
			StartCoroutine(ZombieManager.instance.spawnZombies(5, 0));
		}
	}
}
