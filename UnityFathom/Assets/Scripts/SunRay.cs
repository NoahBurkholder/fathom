using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRay : MonoBehaviour {

	GameObject player;
	Player playerScript;

	GameObject gate;
	Gate gateScript;

	SpriteRenderer sr;
	public bool isWhiteGate;

	// Get references, mostly.
	void Start() {

		sr = GetComponent<SpriteRenderer> ();

		player = GameObject.Find ("Player");
		playerScript = player.GetComponent<Player> ();

		gate = transform.parent.gameObject;
		gateScript = gate.GetComponent<Gate> ();

	}
	
	void OnTriggerEnter2D(Collider2D c) { // When collider (presumably player) enters.
		if (isWhiteGate) {
			if (c.gameObject.Equals (player)) { // Confirm is player.
				Color playerRGB = playerScript.rgb; // Access player's color.
				Color lightRGB = sr.color;
				float difference = 0; // New float for summation of difference in channels.
				float playerH, playerS, playerV;
				float lightH, lightS, lightV;
				Color.RGBToHSV (playerRGB, out playerH, out playerS, out playerV);
				Color.RGBToHSV (lightRGB, out lightH, out lightS, out lightV);

				difference += Mathf.Abs (lightS - playerS) * 0.2f; // Add difference between saturation. (Less weight.)
				difference += Mathf.Abs (lightV - playerV) * 1f; // Add difference between brightness. (Less weight.)




				if (difference < 0.1f) { // If summed number is less than 0.3, then you successfully finish the gate.
					playerScript.isLit = true;
					gateScript.shatter (); // Shatter rock.
					playerScript.revokeColor (); // Return player to grey state.

				}
			}
		} else {
			if (c.gameObject.Equals (player)) { // Confirm is player.
				Color playerRGB = playerScript.rgb; // Access player's color.
				Color lightRGB = sr.color;
				float difference = 0; // New float for summation of difference in channels.
				float playerH, playerS, playerV;
				float lightH, lightS, lightV;
				Color.RGBToHSV (playerRGB, out playerH, out playerS, out playerV);
				Color.RGBToHSV (lightRGB, out lightH, out lightS, out lightV);

				difference += Mathf.Abs (lightH - playerH) * 1f; // Add difference between hue.
				difference += Mathf.Abs (lightS - playerS) * 0.2f; // Add difference between saturation. (Less weight.)
				difference += Mathf.Abs (lightV - playerV) * 0.2f; // Add difference between brightness. (Less weight.)

	


				if (difference < 0.1f) { // If summed number is less than 0.3, then you successfully finish the gate.
					gateScript.shatter (); // Shatter rock.
					playerScript.revokeColor (); // Return player to grey state.

				}
			}
		}
	}
}
