using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour {
	private int damage;
	private Vector3 distance, direction;
	public GameObject playerTarget, currentTarget;
	public NavMeshAgent targetNavAgent;
	private Rigidbody zombieRB;
	private Animator zombieAnimator;

	private List<string> canAttackTags = new List<string>() { "Fence", "Player" };
	// Start is called before the first frame update
	void Start() {
		damage = ZombieManager.instance.ZOMBIE_DAMAGE;
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
	void stopVelocity() {
		if (zombieRB.velocity.magnitude != 0 || zombieRB.angularVelocity.magnitude != 0) {
			zombieRB.velocity = new Vector3(0, 0, 0);
			zombieRB.angularVelocity = new Vector3(0, 0, 0);
		}
	}
	void moveToTarget() {
		if (targetNavAgent.enabled) {
			stopVelocity();
			zombieAnimator.SetInteger("speed", 1);
			targetNavAgent.SetDestination(playerTarget.transform.position);
		}
	}

	public IEnumerator fallDown() {

		stopVelocity();
		targetNavAgent.enabled = false;
		zombieAnimator.SetBool("isHit", true);
		yield return new WaitForSeconds(5);
		zombieAnimator.SetBool("isHit", false);
		targetNavAgent.enabled = true;

		zombieAnimator.SetInteger("speed", 1);
	}


	void OnTriggerEnter(Collider collider) {
		GameObject target = collider.gameObject;
		Debug.Log(target.tag);
		startAttacking(target);
	}
	void OnTriggerExit(Collider collider) {
		GameObject target = collider.gameObject;
		stopAttacking(target);
	}
	void OnCollisionEnter(Collision collision) {
		GameObject target = collision.gameObject;
		if (target.tag != "Bullet")
			stopVelocity();
		startAttacking(target);
	}

	void OnCollisionExit(Collision collision) {
		GameObject target = collision.gameObject;
		if (target.tag != "Bullet")
			stopVelocity();
		stopAttacking(target);
	}

	void startAttacking(GameObject target) {
		if (canAttackTags.Contains(target.tag)) {
			targetNavAgent.enabled = false;
			zombieAnimator.SetBool("canAttack", true);
			currentTarget = target;
			StartCoroutine(hitTarget());
			zombieAnimator.SetInteger("speed", 0);
		}
	}

	void stopAttacking(GameObject target) {
		if (canAttackTags.Contains(target.tag)) {
			targetNavAgent.enabled = true;
			zombieAnimator.SetBool("canAttack", false);
			currentTarget = null;
			StopCoroutine("hitTarget");
			zombieAnimator.SetInteger("speed", 0);
		}
	}



	IEnumerator hitTarget() {
		// only attack if current player's turn
		while (currentTarget != null && zombieAnimator.GetBool("canAttack") && GameManager.instance.getCurrentPlayer() == playerTarget) {

			// wait before attacking to give the player a chance to react
			yield return new WaitForSeconds(1);
			if (currentTarget != null)
				currentTarget = currentTarget.GetComponent<ObstacleScript>().TakeDamage(damage);
		}
		// if fence has been destroyed => go back to chasing player
		targetNavAgent.enabled = true;
		zombieAnimator.SetBool("canAttack", false);
	}

	// used at the end of the game to disable zombies
	public void disableZombie() {
		this.enabled = false;
	}

}
