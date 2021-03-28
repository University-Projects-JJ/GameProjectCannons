using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public float MAX_PLAYER_HEALTH = 1000;
	public float MAX_FENCE_HEALTH = 200;
	public static GameManager instance;
	public GameObject testTarget;

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}
	// Start is called before the first frame update
	IEnumerator Start() {
		// yield return StartCoroutine(ZombieManager.instance.spawnZombies(5, 0));
		yield return new WaitForSeconds(1);
		// ZombieManager.instance.setZombiesTarget(testTarget);
	}

	// Update is called once per frame
	void Update() {

	}

}
