using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletScript : MonoBehaviour {
	private int explosionRadius = 10, explosionForce = 1000;
	public AudioClip explosionSound;
	public float explosiveDamageMultiplier = 1.2f;
	public Collider[] colliders;
	private List<string> alwaysAffectedTags = new List<string> { "Zombie", "Obstacle" };
	private List<string> conditionallyAffectedTags = new List<string> { "Fence", "Player" };

	public GameObject belongsToPlayer;
	public GameObject particleChild;
	// Start is called before the first frame update


	// Start is called before the first frame update
	void Start() {
		// always destroy the bullet if not destroyed
		// yield return new WaitForSeconds(10);
		// if (gameObject != null)
		// 	Destroy(gameObject);
	}

	// Update is called once per frame
	void Update() {
		// if out of bounds

	}
	float calculateDamage(Collider col) {
		float distance = Vector3.Distance(transform.position, col.transform.position);
		float damage = (1 - distance / explosionRadius) * 100;
		return damage;
	}

	void dealDamage(Collider col) {
		int damage = (int)Mathf.Ceil(calculateDamage(col) * explosiveDamageMultiplier);
		belongsToPlayer.GetComponent<PlayerScript>().playerScore += damage;
		col.gameObject.GetComponent<HealthScript>().TakeDamage(damage);
	}
	void OnCollisionEnter(Collision collision) {
		colliders = Physics.OverlapSphere(transform.position, explosionRadius);

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
				GameObject targetBelongsToPlayer = target.tag == "Player" ? target : target.GetComponent<ObstacleScript>().belongsToPlayer;

				if (targetBelongsToPlayer == belongsToPlayer)
					continue;
				else
					dealDamage(col);
			}

			else if (alwaysAffectedTags.Contains(target.tag)) {
				// inflict damage based on distance
				dealDamage(col);
				// float damage = calculateDamage(col) * explosiveDamageMultiplier;
				// belongsToPlayer.GetComponent<PlayerScript>().playerScore += (int)Mathf.Ceil(damage);
				// target.GetComponent<HealthScript>().TakeDamage(damage);
			}
		}
		AudioSource.PlayClipAtPoint(explosionSound, gameObject.transform.position);

		// Destroy bullet after dealing damage
		gameObject.SetActive(false);
		Invoke("destroyBullet", 1);
	}

	void destroyBullet() {
		Destroy(gameObject);
	}
}
