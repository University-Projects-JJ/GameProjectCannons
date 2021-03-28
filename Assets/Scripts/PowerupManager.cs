using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {
	public static PowerupManager instance;
	public float POWERUP_CHANCE = 0.6f;
	private int POWERUP_HEALTH = 250;
	private int POWERUP_DOUBLE_DAMAGE_COUNT = 3;
	public enum POWERUP_TYPES { HEAL, AMMO, SHIELD };
	public GameObject prefabHealth, prefabAmmo, prefabShield;
	public Transform powerupParent, powerUpSpawner;


	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	// Start is called before the first frame update
	void Start() {

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

	public void spawnPowerup() {
		float spawnChance = Random.Range(0f, 1.0f);
		Debug.Log(spawnChance);
		if (spawnChance <= POWERUP_CHANCE) {
			GameObject powerup;

			// use shields as default;
			GameObject prefab = prefabShield;
			POWERUP_TYPES type = POWERUP_TYPES.SHIELD;

			int random = Random.Range(0, 3);
			float positionX = Random.Range(-10, 10);

			// if 0 => already shield
			if (random == 1) {
				prefab = prefabAmmo;
				type = POWERUP_TYPES.AMMO;
			}
			if (random == 2) {
				prefab = prefabHealth;
				type = POWERUP_TYPES.HEAL;
			}
			powerup = Instantiate(prefab, new Vector3(positionX, powerUpSpawner.position.y, powerUpSpawner.position.z), powerUpSpawner.rotation);
			powerup.GetComponent<PowerUpScript>().powerupType = type;
		}
	}
}
