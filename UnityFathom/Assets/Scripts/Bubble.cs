using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

	Rigidbody2D rb;
	SpriteRenderer sr;
	float collisionTimer;
	float deleteTimer;
	// Secondary mechanic of color-channel-based physical properties.
	public void updateProperties(float buoyancy, float weight, Vector2 velocity, Color color) {
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
		rb.gravityScale = weight - buoyancy - 1;
		rb.AddRelativeForce(velocity);
		collisionTimer = 1.0f;
		deleteTimer = 10f;
	}

	void pop() {
		GameObject.Destroy (gameObject);
	}

	void FixedUpdate() {
		if (collisionTimer > 0) {
			collisionTimer -= Time.deltaTime;
		} else {
			gameObject.layer = LayerMask.NameToLayer ("BubbleOld");
		}
		if (deleteTimer > 0) {
			deleteTimer -= Time.deltaTime;
		} else {
			pop ();
		}
	}

	void OnCollisionEnter2D(Collision2D c) {
		if (c.relativeVelocity.magnitude > 5f) {
				pop ();

		}
	}
}
