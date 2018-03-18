using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {


	GameObject lmb, rmb;
	SpriteRenderer lmbSprite, rmbSprite;
	GameObject player;
	Player playerScript;
	Rigidbody2D playerRB;
	int clicks;
	int timer;
	bool lmbComplete, rmbComplete;

	void Start() {
		player = GameObject.Find ("Player");
		playerScript = player.GetComponent<Player> ();
		playerRB = player.GetComponent<Rigidbody2D> ();
		lmb = GameObject.Find ("MouseGraphic");
		lmbSprite = lmb.GetComponent<SpriteRenderer> ();
		rmb = GameObject.Find ("MouseGraphic2");
		rmbSprite = rmb.GetComponent<SpriteRenderer> ();
		timer = 120;
		playerScript.freezeShedding = true;

	}

	readonly float GREY = 0.3f;

	// Update is called once per frame
	void FixedUpdate () {
		float mag = playerRB.velocity.magnitude;

		if (mag > 4.0f) {
			mag = 4f;
			lmbComplete = true;
			playerScript.freezeShedding = false;
		}

		if (!lmbComplete) {
			lmbSprite.color = new Color (1f, 1f, 1f, 1f - (mag * 0.25f));
			rmbSprite.color = new Color (1f, 1f, 1f, 0);
		} else {
			lmbSprite.color = new Color (1f, 1f, 1f, 0);
			if (!rmbComplete) {
				mag = playerScript.rgb.b * 2;
				rmbSprite.color = new Color (1f, 1f, 1f, mag);

				if (mag < 0.1f) {
					rmbComplete = true;
				}
			} else {
				rmbSprite.color = new Color (1f, 1f, 1f, 0);
			}
		}
	}
}
