using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour {
	private float health = 100;
	private float damage = 25;
	private float speed = 4.0f;
	private Vector3 distance, direction;
	public GameObject playerTarget;
	public GameObject currentTarget;
	public NavMeshAgent targetNavAgent;
	private Rigidbody zombieRB;
	private Animator zombieAnimator;
	// Start is called before the first frame update
	void Start() {
		zombieRB = gameObject.GetComponent<Rigidbody>();
		zombieAnimator = gameObject.GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update() {
		if (playerTarget != null && targetNavAgent != null) {
			moveToTarget();
		}
	}

	void FixedUpdate() {

	}

	void moveToTarget() {
		if (targetNavAgent.enabled) {
			if (zombieRB.velocity.magnitude != 0 || zombieRB.angularVelocity.magnitude != 0) {
				zombieRB.velocity = new Vector3(0, 0, 0);
				zombieRB.angularVelocity = new Vector3(0, 0, 0);
			}
			targetNavAgent.SetDestination(playerTarget.transform.position);
			distance = playerTarget.transform.position - this.transform.position;
			// if (distance.magnitude < 3.1) {
			// 	zombieAnimator.SetBool("canAttack", true);
			// }
			// else {
			// 	zombieAnimator.SetBool("canAttack", false);
			// }
		}
	}

	public IEnumerator fallDown() {
		targetNavAgent.enabled = false;
		zombieAnimator.SetBool("isHit", true);
		yield return new WaitForSeconds(5);
		zombieAnimator.SetBool("isHit", false);
		targetNavAgent.enabled = true;
	}

	void OnCollisionEnter(Collision collision) {
		GameObject target = collision.gameObject;

		if (target.tag == "Fence") {
			targetNavAgent.enabled = false;
			zombieAnimator.SetBool("canAttack", true);
			currentTarget = target;
			StartCoroutine(hitFence());
		}
	}

	IEnumerator hitFence() {
		while (currentTarget != null) {
			currentTarget.GetComponent<HealthScript>().TakeDamage(damage);
			yield return new WaitForSeconds(1);
		}
		// if fence has been destroyed => go back to chasing player

		targetNavAgent.enabled = true;
		zombieAnimator.SetBool("canAttack", false);
	}

}
