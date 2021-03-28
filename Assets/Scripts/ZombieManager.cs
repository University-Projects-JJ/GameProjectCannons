using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieManager : MonoBehaviour {
	private float X_LEFT_BOUND = -24;
	private float Z_LOWER_BOUND = -5;
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
		for (int i = 0; i < count; i++) {
			float x = Random.Range(X_LEFT_BOUND, X_LEFT_BOUND * -1);
			float z = Random.Range(Z_LOWER_BOUND, Z_LOWER_BOUND * -1);

			GameObject zombie = Instantiate(zombiePrefab, new Vector3(x, 1.0f, z), transform.rotation);
			// assign zombie health
			zombie.GetComponent<HealthScript>().health = 100;
			zombie.transform.SetParent(zombiesParent);
			yield return new WaitForSeconds(waitTime);
		}
	}

	public void setZombiesTarget(GameObject target) {
		foreach (Transform zombie in zombiesParent) {
			GameObject zombieObj = zombie.gameObject;
			zombieObj.GetComponent<ZombieScript>().playerTarget = target;
		}
	}
}
