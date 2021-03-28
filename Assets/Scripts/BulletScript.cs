using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletScript : MonoBehaviour {
	private int explosionRadius = 10, explosionForce = 1000;
	public float explosiveDamageMultiplier = 1.5f;
	public Collider[] colliders;
	public List<string> affectedTags = new List<string> { "Zombie", "Obstacle" };
	// Start is called before the first frame update


	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		// if out of bounds
		if (gameObject.transform.position.y < 0)
			Destroy(gameObject);
	}
	float calculateDamage(Collider col) {
		float distance = Vector3.Distance(transform.position, col.transform.position);
		float damage = (1 - distance / explosionRadius) * 100;
		return damage;
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
			if (affectedTags.Contains(target.tag)) {
				// inflict damage based on distance
				float damage = calculateDamage(col);
				target.GetComponent<HealthScript>().TakeDamage(damage); // SUGGESTION *explosiveMultiplier
			}

			// Destroy bullet when after dealing damage
			Destroy(gameObject);
		}
	}
}
