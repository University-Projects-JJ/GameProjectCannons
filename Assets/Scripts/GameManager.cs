using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public int MAX_PLAYER_HEALTH = 1000;
	public int MAX_FENCE_HEALTH = 200;

	public static GameManager instance;

	public GameObject testTarget;

	public GameObject player1, player2;
	private int currentPlayer = 1;
	private bool isPlaying = false;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}
	// Start is called before the first frame update
	IEnumerator Start() {
		isPlaying = true;
		currentPlayer = Random.Range(1, 3);
		SwitchTurns();
		// yield return StartCoroutine(ZombieManager.instance.spawnZombies(5, 0));
		yield return new WaitForSeconds(1);

		// ZombieManager.instance.setZombiesTarget(testTarget);
	}

	// Update is called once per frame
	void Update() {
	}

	void enablePlayer(GameObject player, bool isEnabled) {
		TurretController turretController = player.GetComponentInChildren<TurretController>();
		turretController.camera.SetActive(isEnabled);
		turretController.enabled = isEnabled;
		turretController.canShoot = isEnabled;
		player.GetComponent<PlayerScript>().imgListDoubleDamageBullets.SetActive(isEnabled);
	}


	public void SwitchTurns() {
		if (currentPlayer == 1) {
			enablePlayer(player1, true);
			enablePlayer(player2, false);
		}
		else {
			enablePlayer(player1, false);
			enablePlayer(player2, true);
		}
		currentPlayer = 3 - currentPlayer;
		PowerupManager.instance.spawnPowerup();
	}

}
