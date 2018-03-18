using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex : MonoBehaviour {
	SpriteRenderer sr;
	float opacity;

	void Start() {
		sr = GetComponent<SpriteRenderer> ();
		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, opacity);
	}

	public bool isFading;

	// Fade-out of absorption cone sprite.
	void FixedUpdate() {
			if (opacity > 0) {
				opacity -= 0.05f;
			} else {
				opacity = 0;
			}
		Color opaque = new Color (sr.color.r, sr.color.g, sr.color.b, opacity);
		sr.color = opaque;

	}


	// Set correct orientation.
	public void setAngle(Vector3 angle) {
		transform.localEulerAngles = new Vector3(0, 0, ((angle.z) * Mathf.Rad2Deg) - 90);
	}
	// Fade-in of absorption cone sprite.
	public void setColor(Color c) {
		if (opacity < 1f) {
			opacity += 0.1f;

		} else {
			opacity = 1f;
		}
		Color opaque = new Color (c.r, c.g, c.b, opacity);
		sr.color = opaque;
	}
}
