using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieManager : MonoBehaviour {
	public readonly int MAX_ZOMBIE_HEALTH = 100, MAX_ZOMBIE_COUNT = 20,
	ZOMBIE_DAMAGE = 25;
	private readonly float X_LEFT_BOUND = -15, Z_LOWER_BOUND = -20;
	public static ZombieManager instance;
	public GameObject zombiePrefab;
	public Transform zombiesParent;
	private List<GameObject> zombies;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}
	// Start is called before the first frame update
	void Start() {
		zombies = new List<GameObject>();
	}

	// Update is called once per frame
	void Update() {

	}

	// Spawn zombies randomly	IEnumerator spawnZombies(int count, int waitTime = 1) {
	public IEnumerator spawnZombies(int count = 20, int waitTime = 1) {
		if (zombies == null)
			zombies = new List<GameObject>();

		for (int i = 0; i < count; i++) {
			if (zombies.Count < MAX_ZOMBIE_COUNT) {
				float x = Random.Range(X_LEFT_BOUND, X_LEFT_BOUND * -1);
				float z = Random.Range(Z_LOWER_BOUND, Z_LOWER_BOUND * -1);

				GameObject zombie = Instantiate(zombiePrefab, new Vector3(x, 0, z), transform.rotation);
				// assign zombie health
				zombie.GetComponent<ObstacleScript>().health = MAX_ZOMBIE_HEALTH;
				zombie.GetComponent<ObstacleScript>().MAX_HEALTH = MAX_ZOMBIE_HEALTH;
				zombie.transform.SetParent(zombiesParent);
				zombies.Add(zombie);
				yield return new WaitForSeconds(waitTime);
			}
		}
	}

	// not used
	public void setZombiesTarget(GameObject target) {
		foreach (Transform zombie in zombiesParent) {
			GameObject zombieObj = zombie.gameObject;
			zombieObj.GetComponent<ZombieScript>().playerTarget = target;
		}
	}

	// Disable All Zombies
	public IEnumerator disableZombies() {
		foreach (GameObject zombie in zombies)
			zombie.GetComponent<ZombieScript>().disableZombie();
		yield break;
	}
}
