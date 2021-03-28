using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretController : MonoBehaviour {
	public float mouseSensitivity = 10.0f;
	float xAxisClamp = 0, yAxisClamp = 0;
	private float MAXCLAMP_XDOWN = 50, MAXCLAMP_XUP = -20, MAXCLAMP_Y = 45;
	public GameObject turret;
	public GameObject bulletPrefab, bulletSpawner1, bulletSpawner2;
	private float bulletForce = 0;
	private int shootFromSpawner = 1;

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
		if (Input.GetButton("Fire1")) {
			bulletForce = (bulletForce + 1.5f) % 100;
			forcePowerBar.fillAmount = bulletForce / 100.0f;
		}
		if (Input.GetButtonUp("Fire1")) {
			ShootBullets(bulletForce);
			bulletForce = 0;
			forcePowerBar.fillAmount = 0;
		}
	}

	void ShootBullets(float bulletForce) {
		Vector3 spawnPosition = shootFromSpawner == 1 ? bulletSpawner1.transform.position : bulletSpawner2.transform.position;
		Quaternion spawnRotation = shootFromSpawner == 1 ? bulletSpawner1.transform.rotation : bulletSpawner2.transform.rotation;
		shootFromSpawner = 3 - shootFromSpawner; // switches between 1 and 2

		GameObject bullet = Instantiate(bulletPrefab, spawnPosition, spawnRotation);
		bullet.GetComponent<Rigidbody>().AddForce(turret.transform.forward * bulletForce * 35);
	}

	void RotateCamera() {
		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		float rotX = mouseX * mouseSensitivity;
		float rotY = mouseY * mouseSensitivity;

		xAxisClamp -= rotY;
		yAxisClamp += rotX;

		Vector3 rotateTurret = turret.transform.rotation.eulerAngles;

		rotateTurret.x -= rotY;
		rotateTurret.z = 0;
		rotateTurret.y += rotX;

		// clamp x axis
		if (xAxisClamp > MAXCLAMP_XDOWN) {
			xAxisClamp = MAXCLAMP_XDOWN;
			rotateTurret.x = MAXCLAMP_XDOWN;
		}
		if (xAxisClamp < MAXCLAMP_XUP) {
			xAxisClamp = MAXCLAMP_XUP;
			rotateTurret.x = 360 + MAXCLAMP_XUP;
		}

		if (yAxisClamp < 0) {
			yAxisClamp = 0;
			rotateTurret.y = 136.799f;
		}

		if (yAxisClamp > 90f) {
			yAxisClamp = 90f;
			rotateTurret.y = 226.799f;
		}

		turret.transform.rotation = Quaternion.Euler(rotateTurret);
	}
}
