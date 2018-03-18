using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	SpriteRenderer sr;
	public Sprite deadSprite;

	readonly float GREY = 0.4f; // 50 / 255
	readonly float BLACK = 0;
	float shedSpeed = 0.0007f; // Rate of shedding.
	public float lifeForce;
	public bool isDead, isHealed;

	public GameObject dialogueObject;
	Subtitle dialogue;
	bool wantsHelp;

	public Color characterRGB;
	public Color lifeColor;

	float dR, dG, dB;
	public float flare;
	float currentFlare;
	public float flareR, flareG, flareB;
	float currentFlareR, currentFlareG, currentFlareB;

	GameObject player;
	GameObject vortex;
	Vortex vortexScript;
	Player playerScript;


	GameObject glowObject;
	Light lt;

	public bool isReady;

	// Initial stuff.
	void Start () {
		sr = GetComponent<SpriteRenderer> ();

		dialogue = dialogueObject.GetComponent<Subtitle> ();

		player = GameObject.Find ("Player");
		playerScript = player.GetComponent<Player> ();

		vortex = GameObject.Find ("Vortex").gameObject;
		vortexScript = vortex.GetComponent<Vortex> ();

		characterRGB = sr.color;
		lifeColor = sr.color;
		lifeForce = ((lifeColor.r + lifeColor.g + lifeColor.b) / (characterRGB.r + characterRGB.g + characterRGB.b));
		lifeForce = Mathf.Sqrt (lifeForce);
		currentFlare = (flare * lifeForce);
		currentFlareR = (flareR * lifeForce);
		currentFlareG = (flareG * lifeForce);
		currentFlareB = (flareB * lifeForce);

		sr.color = new Color(lifeColor.r + (currentFlareR), lifeColor.g +  + (currentFlareG), lifeColor.b +  + (currentFlareB));
		lt = transform.Find ("Glow").GetComponent<Light> ();
		lt.color = lifeColor;
		lt.intensity = ((lifeColor.r + lifeColor.g + lifeColor.b)*currentFlare)-1.174f;


	}
	// Distributes color changes through helper variables and the renderer/light.
	public void setColor(Color c) {
		lifeColor = checkColor (c);

		lifeForce = ((lifeColor.r + lifeColor.g + lifeColor.b) / (characterRGB.r + characterRGB.g + characterRGB.b));
		lifeForce = Mathf.Sqrt (lifeForce);
		currentFlare = (flare * lifeForce);
		currentFlareR = (flareR * lifeForce);
		currentFlareG = (flareG * lifeForce);
		currentFlareB = (flareB * lifeForce);

		sr.color = new Color(lifeColor.r + (currentFlareR), lifeColor.g +  + (currentFlareG), lifeColor.b +  + (currentFlareB));
		lt.color = lifeColor;
		lt.intensity = ((lifeColor.r + lifeColor.g + lifeColor.b)*currentFlare)-1.174f;
	}

	// Keep colours within bounds.
	Color checkColor(Color c) {
		Color dC = c;
		if (dC.r > 1f) {
			dC.r = 1f;
		}
		if (dC.g > 1f) {
			dC.g = 1f;
		}
		if (dC.b > 1f) {
			dC.b = 1f;
		}
		if (dC.r < 0f) {
			dC.r = 0f;
		}
		if (dC.g < 0f) {
			dC.g = 0f;
		}
		if (dC.b < 0f) {
			dC.b = 0f;
		}
		return dC; // Fixed color.
	}
	// While collider (presumably player) is within, handle the exchange of color.
	void OnTriggerStay2D(Collider2D c) {
		if (isReady) {
			if ((player != null) && (c.gameObject.Equals(player))) {
				if (!playerScript.isLit) {
					vortexScript.setAngle (new Vector3 (0, 0, (Mathf.Atan2 (player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x))));
					vortexScript.setColor (characterRGB);
					if (playerScript.r < characterRGB.r) {
						dR = characterRGB.r / 500f;
					} else {
						dR = -characterRGB.r / 800f;
					}
					if (playerScript.g < characterRGB.g) {
						dG = characterRGB.g / 500f;
					} else {
						dG = -characterRGB.g / 800f;
					}
					if (playerScript.b < characterRGB.b) {
						dB = characterRGB.b / 500f;
					} else {
						dB = -characterRGB.b / 800f;
					}

					shed (); // Shed character's color.


					// Give player new color + deltaColor.
					Color newColor = new Color (playerScript.r + dR, playerScript.g + dG, playerScript.b + dB);
					playerScript.setColor (newColor);
					vortexScript.isFading = true; // Color-absorption cone sprite helper bool.
				} else {
					if (!isDead) {

						isHealed = true;
					
						dialogue.state = dialogue.HEALED;
					
						vortexScript.setAngle (new Vector3 (0, 0, (Mathf.Atan2 (player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x))));
						vortexScript.setColor (characterRGB);
						if (lifeColor.r < characterRGB.r) {
							dR = (characterRGB.r - lifeColor.r) * 0.1f;
						}
						if (lifeColor.g < characterRGB.g) {
							dG = (characterRGB.g - lifeColor.g) * 0.1f;

						}
						if (lifeColor.b < characterRGB.b) {
							dB = (characterRGB.b - lifeColor.b) * 0.1f;

						}
						// Give player new color + deltaColor.
						Color newColor = new Color (lifeColor.r + dR, lifeColor.g + dG, lifeColor.b + dB);
						setColor (newColor);
						vortexScript.isFading = true; // Color-absorption cone sprite helper bool.
					} else {
						vortexScript.setAngle (new Vector3 (0, 0, (Mathf.Atan2 (player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x))));
						vortexScript.setColor (Color.gray);
						if (lifeColor.r < Color.gray.r) {
							dR = (Color.gray.r - lifeColor.r) * 0.1f;
						}
						if (lifeColor.g < Color.gray.g) {
							dG = (Color.gray.g - lifeColor.g) * 0.1f;

						}
						if (lifeColor.b < Color.gray.b) {
							dB = (Color.gray.b - lifeColor.b) * 0.1f;

						}
						// Give player new color + deltaColor.
						Color newColor = new Color (lifeColor.r + dR, lifeColor.g + dG, lifeColor.b + dB);
						setColor (newColor);
						vortexScript.isFading = true; // Color-absorption cone sprite helper bool.
					}
				}
			}
		}
	}

	// Get rid of absorption cone sprite.
	void OnTriggerExit2D (Collider2D c) {
		if (c.gameObject.Equals (player)) {
			vortexScript.isFading = true;
		}
	}
		

	// Color shedding for character color.
	void shed() {
		if (lifeColor.r > BLACK) {
			lifeColor.r -= shedSpeed;
		} else if (characterRGB.r < BLACK) {
			lifeColor.r += shedSpeed;
		}
		if (lifeColor.g > BLACK) {
			lifeColor.g -= shedSpeed;
		} else if (characterRGB.g < BLACK)  {
			lifeColor.g += shedSpeed;
		}
		if (lifeColor.b > BLACK) {
			lifeColor.b -= shedSpeed;
		} else if (characterRGB.b < BLACK)  {
			lifeColor.b += shedSpeed;
		}

		lifeForce = ((lifeColor.r + lifeColor.g + lifeColor.b) / (characterRGB.r + characterRGB.g + characterRGB.b));

		if (lifeForce < 0) {
			lifeForce = 0;
		}

		lifeForce = Mathf.Sqrt (lifeForce);

		currentFlare = (flare * lifeForce);
		currentFlareR = (flareR * lifeForce);
		currentFlareG = (flareG * lifeForce);
		currentFlareB = (flareB * lifeForce);

		sr.color = new Color(lifeColor.r + (currentFlareR), lifeColor.g +  + (currentFlareG), lifeColor.b +  + (currentFlareB));
		lt.color = lifeColor;
		lt.intensity = ((lifeColor.r + lifeColor.g + lifeColor.b)*currentFlare)-1.174f;



		if ((lifeForce <= 0.7f) && (lifeForce > 0.01f)) {
			Debug.Log ("O HURT");

			dialogue.state = dialogue.HURT;
		} else if (lifeForce <= 0.01f) {
			Debug.Log ("O DEAD");


			isDead = true;
			sr.sprite = deadSprite;
			dialogue.state = dialogue.DEAD;
		}
	}
}
