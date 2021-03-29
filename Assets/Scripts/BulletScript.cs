using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletScript : MonoBehaviour {
	private int explosionRadius = 6, explosionForce = 1000;
	public AudioClip explosionSound;
	public float explosiveDamageMultiplier;
	public Collider[] colliders;
	private List<string> alwaysAffectedTags = new List<string> { "Zombie", "Obstacle", "Barrel" };
	private List<string> conditionallyAffectedTags = new List<string> { "Fence", "Player" };

	public GameObject belongsToPlayer;
	public GameObject particleChild;
	// Start is called before the first frame update


	// Start is called before the first frame update
	void Start() {
	}

	// Update is called once per frame
	void Update() {
		// if out of bounds

	}
	float calculateDamage(Collider col) {
		float distance = Vector3.Distance(transform.position, col.transform.position);
		float damage = Mathf.Abs(1 - distance / explosionRadius) * 100;
		return damage;
	}

	void dealDamage(Collider col) {
		int damage = (int)Mathf.Ceil(calculateDamage(col) * explosiveDamageMultiplier);
		belongsToPlayer.GetComponent<PlayerScript>().playerScore += damage;
		col.gameObject.GetComponent<ObstacleScript>().TakeDamage(damage);
	}
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag != "Powerup")
			causeExplosion();
	}

	void OnTriggerEnter(Collider collider) {
		GameObject target = collider.gameObject;
		if (target.tag == "Player" && target.GetComponent<ObstacleScript>().belongsToPlayer != belongsToPlayer)
			causeExplosion();
	}

	void causeExplosion() {
		colliders = Physics.OverlapSphere(transform.position, explosionRadius);
		AudioSource.PlayClipAtPoint(explosionSound, gameObject.transform.position);

		foreach (Collider col in colliders) {
			GameObject target = col.gameObject;
			Rigidbody rigidBody = target.GetComponent<Rigidbody>();

			// disable zombies' navMeshAgent so they can take explosive force
			if (target.tag == "Zombie")
				target.GetComponent<NavMeshAgent>().enabled = false;

			// Apply explosive force if not player (player doesn't have rigidBody for obvious reasons)
			if (rigidBody)
				rigidBody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

			// if can take damage => in affected tags
			if (conditionallyAffectedTags.Contains(target.tag)) {
				GameObject targetBelongsToPlayer = target.GetComponent<ObstacleScript>().belongsToPlayer;
				if (targetBelongsToPlayer == belongsToPlayer)
					continue;
				else
					dealDamage(col);
			}

			else if (alwaysAffectedTags.Contains(target.tag)) {

				// if player hits zombie, set the zombie to attack the player
				if (target.tag == "Zombie") {
					target.GetComponent<ZombieScript>().playerTarget = belongsToPlayer;
				}
				// inflict damage based on distance
				dealDamage(col);
			}
			// Destroy bullet after dealing damage
			gameObject.SetActive(false);
			Invoke("destroyBullet", 1);
		}
	}

	void destroyBullet() {
		Destroy(gameObject);
	}

}
