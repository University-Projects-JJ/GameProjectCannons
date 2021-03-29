using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretController : MonoBehaviour {
	public float mouseSensitivity = 10.0f;
	public GameObject turret, turretObject, playerCamera;
	public GameObject bulletPrefab, bulletSpawner1, bulletSpawner2;
	private float bulletForce = 0;
	private int shootFromSpawner = 1;
	public bool canShoot = true;

	// UI ELEMENTS
	public Image forcePowerBar;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		Cursor.lockState = CursorLockMode.Locked;
		RotateCamera();

		// shoot a bullet

		if (canShoot) {

			if (Input.GetButton("Fire1")) {
				bulletForce = (bulletForce + 1.5f) % 100;
				forcePowerBar.fillAmount = bulletForce / 100.0f;
			}
			if (Input.GetButtonUp("Fire1")) {
				ShootBullets(bulletForce);
				bulletForce = 0;
				forcePowerBar.fillAmount = 0;
				canShoot = false;
			}
		}
	}


	IEnumerator waitToEndTurn(GameObject bullet) {
		yield return new WaitUntil(() => bullet == null);
		GameManager.instance.SwitchTurns();
	}
	void ShootBullets(float bulletForce) {
		Vector3 spawnPosition = shootFromSpawner == 1 ? bulletSpawner1.transform.position : bulletSpawner2.transform.position;
		Quaternion spawnRotation = shootFromSpawner == 1 ? bulletSpawner1.transform.rotation : bulletSpawner2.transform.rotation;
		shootFromSpawner = 3 - shootFromSpawner; // switches between 1 and 2

		GameObject bullet = Instantiate(bulletPrefab, spawnPosition, spawnRotation);
		bullet.GetComponent<Rigidbody>().AddForce(turret.transform.forward * bulletForce * 35);

		// assign bullet to player
		BulletScript bulletScript = bullet.GetComponent<BulletScript>();
		bulletScript.belongsToPlayer = gameObject.transform.parent.gameObject;
		bulletScript.explosiveDamageMultiplier = GameManager.instance.EXPLOSIVE_DAMAGE_MULTIPLER;

		// get assigned player
		// GameObject player = bullet.GetComponent<BulletScript>().belongsToPlayer;


		// if has double damage bullets
		if (bulletScript.belongsToPlayer.GetComponent<PlayerScript>().doubleDamageBullets > 0) {

			// increase bullet damage for this bullet
			bulletScript.explosiveDamageMultiplier *= 2;

			// enable its particle effect
			bulletScript.particleChild.SetActive(true);

			// decrement player double damage bullets
			bulletScript.belongsToPlayer.GetComponent<PlayerScript>().doubleDamageBullets--;
		}

		gameObject.GetComponent<AudioSource>().Play();

		StartCoroutine(waitToEndTurn(bullet));
	}

	void RotateCamera() {
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		float rotHorizontal = mouseX * mouseSensitivity;
		float rotVertical = mouseY * mouseSensitivity;
		// float rotHorizontal = Input.GetAxis("Horizontal") * mouseSensitivity;
		// float rotVertical = Input.GetAxis("Vertical") * mouseSensitivity;

		turretObject.transform.Rotate(0, rotHorizontal, 0);
		turret.transform.Rotate(-rotVertical, 0, 0);
	}
}
