using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {
	public static PowerupManager instance;
	public readonly float POWERUP_CHANCE = 0.3f;
	private readonly int POWERUP_HEALTH = 250, POWERUP_DOUBLE_DAMAGE_COUNT = 3;
	public enum POWERUP_TYPES { HEAL, AMMO, SHIELD };
	public GameObject prefabHealth, prefabAmmo, prefabShield;
	public Transform powerupsParent, powerUpSpawnersParent;
	public List<Transform> powerupSpawners;
	public List<bool> powerUpSpawnersAvailable;


	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	// Start is called before the first frame update
	void Start() {
		powerupSpawners = new List<Transform>();

		foreach (Transform powerupSpawner in powerUpSpawnersParent) {
			powerupSpawners.Add(powerupSpawner);
			powerUpSpawnersAvailable.Add(true);
		}
	}

	// Update is called once per frame
	void Update() {

	}

	public void ApplyPowerup(POWERUP_TYPES powerupType, GameObject playerToApply) {
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

	public void clearSpawner(int spawnPosition) {
		powerUpSpawnersAvailable[spawnPosition] = true;
	}
	public void spawnPowerup() {
		int spawnPosition = -1;

		// number of maximum available powerups == number of available and unoccupied spawners
		for (int i = 0; i < powerUpSpawnersAvailable.Count; i++) {
			if (powerUpSpawnersAvailable[i]) {
				spawnPosition = i;
				break;
			}
		}

		if (spawnPosition != -1) {
			float spawnChance = Random.Range(0f, 1.0f);
			if (spawnChance <= POWERUP_CHANCE) {
				GameObject powerup;

				// use shields as default;
				GameObject prefab = prefabShield;
				POWERUP_TYPES type = POWERUP_TYPES.SHIELD;

				int random = Random.Range(0, 3);

				// if 0 => already shield
				if (random == 1) {
					prefab = prefabAmmo;
					type = POWERUP_TYPES.AMMO;
				}
				if (random == 2) {
					prefab = prefabHealth;
					type = POWERUP_TYPES.HEAL;
				}
				Transform powerUpSpawner = powerupSpawners[spawnPosition];
				powerUpSpawnersAvailable[spawnPosition] = false; // block spawner

				powerup = Instantiate(prefab, powerUpSpawner.position, powerUpSpawner.rotation);
				powerup.GetComponent<PowerUpScript>().powerupType = type;
				powerup.GetComponent<PowerUpScript>().spawnPosition = spawnPosition;

				// set to parent
				powerup.transform.SetParent(powerupsParent);
			}
		}
	}

	public IEnumerator disableManager() {
		this.enabled = false;
		yield break;
	}
}
