using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour {
	private float health = 100;
	private float damage = 5;
	private float speed = 4.0f;
	private Vector3 distance, direction;
	public GameObject target;
	public NavMeshAgent targetNavAgent;
	private Rigidbody zombieRB;
	private Animator zombieAnimator;
	// Start is called before the first frame update
	void Start() {
		zombieRB = gameObject.GetComponent<Rigidbody>();
		// zombieAnimator = gameObject.GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update() {
		if (target != null && targetNavAgent != null)
			moveToTarget();
	}

	void FixedUpdate() {

	}

	void moveToTarget() {
		targetNavAgent.SetDestination(target.transform.position);

		distance = target.transform.position - this.transform.position;
		if (distance.magnitude < 3.1) {
			this.GetComponentInChildren<Animator>().SetBool("canAttack", true);
		}
	}
}
