using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
	public float mouseSensitivity = 10.0f;
	float xAxisClamp = 0, yAxisClamp = 0;
	private float MAXCLAMP_XDOWN = 50, MAXCLAMP_XUP = -20, MAXCLAMP_Y = 45;
	public GameObject turret;
	// Start is called before the first frame update
	void Start() {
	}

	// Update is called once per frame
	void Update() {
		// Cursor.lockState = CursorLockMode.Locked;
		RotateCamera();
		// gameObject.transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime, 0);
		// gameObject.transform.Rotate(Input.GetAxis("Vertical") * -rotationSpeed * Time.deltaTime, 0, 0);

		// // we can now take the forward of the child to get the actual forward direction to shoot bullets
		// // cannonObject.transform.forward

		// // shoot a bullet
		// if (Input.GetButtonDown("Fire1")) {
		// 	GameObject bullet = Instantiate(bulletPrefab, bulletSpawner.transform.position,
		// 			bulletSpawner.transform.rotation);
		// 	bullet.GetComponent<Rigidbody>().AddForce(cannonObject.transform.forward * bulletForce);
		// }
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
