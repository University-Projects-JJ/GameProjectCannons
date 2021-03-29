using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public readonly int MAX_PLAYER_HEALTH = 1000,
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
		// yield return StartCoroutine(ZombieManager.instance.spawnZombies(5, 0));
		// yield return new WaitForSeconds(1);

		// ZombieManager.instance.setZombiesTarget(testTarget);
	}

	// Update is called once per frame
	void Update() {
	}

	private void startGame() {
		isPlaying = true;
		currentPlayer = Random.Range(1, 3);
		SwitchTurns();
	}

	public void endGame() {
		Debug.Log("Game Ended");
		isPlaying = false;
		StartCoroutine(ZombieManager.instance.disableZombies());
		StartCoroutine(PowerupManager.instance.disableManager());
		displayWinner();
	}

	void displayWinner() {
		panelWinning.SetActive(true);
		txtWinner.text = "PLAYER " + (3 - currentPlayer) + " WON!!!";
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
