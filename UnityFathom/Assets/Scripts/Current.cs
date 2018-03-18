using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Current : MonoBehaviour {

	GameObject player;
	Rigidbody2D rb;
	public float strength;
	// Mostly for reference objects.
	void Start () {

		player = GameObject.Find ("Player");
		rb = player.GetComponent<Rigidbody2D> ();

	}
	void OnTriggerStay2D (Collider2D c) {
		if (c.gameObject.Equals (player)) {
			rb.AddForce (new Vector2(0, strength));
		}
	}
}
