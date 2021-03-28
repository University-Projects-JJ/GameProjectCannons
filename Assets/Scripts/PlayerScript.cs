using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
	public float playerHealth = 1000;
	public Text txtPlayerHealth;
	public Image imgPlayerHealthBar;

	private string currentPowerup;
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
	void FixedUpdate() {
		displayHealth();
	}

	void displayHealth() {
		txtPlayerHealth.text = playerHealth.ToString();
		imgPlayerHealthBar.fillAmount = playerHealth / 1000;
	}
}
