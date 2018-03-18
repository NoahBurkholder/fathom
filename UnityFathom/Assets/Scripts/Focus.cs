using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focus : MonoBehaviour {

	GameObject player;
	Player playerScript;

	public float scale;
	float x, y;

	CircleCollider2D col;

	// Mostly for reference objects.
	void Start () {

		player = GameObject.Find ("Player");
		playerScript = player.GetComponent<Player> ();

		col = GetComponent<CircleCollider2D> ();
		//scale = col.radius * 2; // Helper for size of orthographic scale.
		x = transform.position.x;
		y = transform.position.y;
	}

	// Keep focus on self if player is within this focal trigger.
	void OnTriggerStay2D (Collider2D c) {
		if (c.gameObject.Equals (player)) {
			playerScript.newFocus (this);
		}
	}

	// Reset focus on player exit.
	void OnTriggerExit2D (Collider2D c) {
		if (c.gameObject.Equals (player)) {
			playerScript.resetFocus ();
		}
	}
}
