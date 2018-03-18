using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	SpriteRenderer sr, srR, srG, srB, srM;

	GameObject camObject;
	Camera cam;

	GameObject vortex;

	ParticleSystem ps;

	public Color rgb;
	public float r, g, b;

	Collider2D col;
	GameObject activeCircle;
	Character activeScript;
	Rigidbody2D rb;

	GameObject glowObject;
	public Object bubblePrefab;

	Light lt;

	public float brightness;

	Image blackout, whiteout;
	bool justStarted;
	float fadeTime;

	public bool freezeShedding;
	public bool isDreaming;

	public AudioClip[] clips;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		srR = transform.Find ("RedDot").GetComponent<SpriteRenderer> ();
		srG = transform.Find ("GreenDot").GetComponent<SpriteRenderer> ();
		srB = transform.Find ("BlueDot").GetComponent<SpriteRenderer> ();

		source = GetComponent<AudioSource> ();

		rb = GetComponent<Rigidbody2D> ();
		ps = GetComponent<ParticleSystem> ();
	
		vortex = GameObject.Find ("Vortex");

		rgb = sr.color;

		r = rgb.r;
		g = rgb.g;
		b = rgb.b;
		if (!isLit) {
			srR.color = new Color (r, 0, 0);
			srG.color = new Color (0, g, 0);
			srB.color = new Color (0, 0, b);
		} else {
			srR.color = new Color (1f, 1f, 1f);
			srG.color = new Color (1f, 1f, 1f);
			srB.color = new Color (1f, 1f, 1f);
		}
		updateProperties ();
		camObject = GameObject.Find ("Main Camera");
		cam = camObject.GetComponent<Camera> ();
		lt = transform.Find ("Glow").GetComponent<Light> ();
		lt.color = rgb + Color.gray;


		subtitle = GameObject.Find("Canvas/Subtitle").GetComponent<Text>();

		blackout = GameObject.Find ("Canvas/Blackout").GetComponent<Image> ();
		whiteout = GameObject.Find ("Canvas/Whiteout").GetComponent<Image> ();
		fadeTime = 3.0f;

		justStarted = true;
	}

	bool hasPropelled;
	void FixedUpdate() {
		if (!hasWon) {
			mousePos = Input.mousePosition;
			mousePos.z = 10.0f;
			mousePos = Camera.main.ScreenToWorldPoint (mousePos);
			vortex.transform.position = new Vector3 (transform.position.x, transform.position.y, 1f);
			lerpFocus ();
			reTarget ();
			if (Input.GetMouseButton (0)) {
				if (!hasPropelled) {
					//propel (strength); // Propel in direction of orientation.
					hasPropelled = true;
				}
				propel (strength);
			} else {
				hasPropelled = false;
			}
			if (Input.GetMouseButton (1) == true) {
				if (!freezeShedding) {
					shed ();
				}
			}

			if (justStarted) {
				fadeIn ();
			} else {
				playSubtitle ();
			}
			if ((isLit) || (isDreaming)) {
				fadeOut ();
			}
		}
		if (bubbleTimer > 0) {
			bubbleTimer -= Time.deltaTime;
		}
}


	// Distributes color changes through helper variables and the renderer/light.
	public void setColor(Color c) {
		if (!isLit) {
			rgb = checkColor (c);
			sr.color = rgb;
			r = rgb.r;
			g = rgb.g;
			b = rgb.b;
			srR.color = new Color (r, 0, 0);
			srG.color = new Color (0, g, 0);
			srB.color = new Color (0, 0, b);
			updateProperties ();
			lt.color = rgb + Color.gray;
		} else {
			rgb = new Color (1f, 1f, 1f);
				sr.color = rgb;
				r = rgb.r;
				g = rgb.g;
				b = rgb.b;
				srR.color = new Color (1f, 1f, 1f);
				srG.color = new Color (1f, 1f, 1f);
				srB.color = new Color (1f, 1f, 1f);
				updateProperties ();
				lt.color = rgb;
		}

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

	Vector3 mousePos;
	float strength, buoyancy, weight;
	float targetAngle;


	// Secondary mechanic of color-channel-based physical properties.
	void updateProperties() {
		strength = (r * 1000f) + 200f;
		buoyancy = (g * 30f) + 1f;
		weight = (b * 30f) + 1f;
		rb.gravityScale = weight - buoyancy + 1;
	}

	// Rotate player.
	void reTarget() {
		targetAngle = Mathf.Rad2Deg * (Mathf.PI + Mathf.Atan2(-(transform.position.x - mousePos.x), -(transform.position.y - mousePos.y))); // Get angle based on mouse position.
		transform.localEulerAngles = new Vector3 (0, 0, -targetAngle); // Euler angle assignment.

	}
	float bubbleTimer;
	public bool isAbove;
	void propel (float s) {
		if (!isAbove) {
			if (bubbleTimer <= 0) {
				bubbleTimer = 0.15f;

				rb.AddRelativeForce (new Vector2 (0, s)); // Relative force (along rotation) applied to character.

				GameObject b = (GameObject)Instantiate (bubblePrefab, transform.position + new Vector3 (0, 0, 1f), transform.rotation);
				Bubble bScript = b.GetComponent<Bubble> ();
				bScript.updateProperties (buoyancy, weight, (-new Vector2 (0, s) * 0.05f), sr.color);
			}
		}
	}


	// Called on right click, this code turns the player grey again. (50, 50, 50)
	readonly float GREY = 0; // 50 / 255
	float shedSpeed = 0.015f; // Rate of shedding.
	// Shed color on RMB.
	void shed() {
		if (!isLit) {
			if (rgb.r > GREY) {
				rgb.r -= shedSpeed;
			} else if (rgb.r < GREY) {
				rgb.r += shedSpeed;
			}
			if (rgb.g > GREY) {
				rgb.g -= shedSpeed;
			} else if (rgb.g < GREY) {
				rgb.g += shedSpeed;
			}
			if (rgb.b > GREY) {
				rgb.b -= shedSpeed;
			} else if (rgb.b < GREY) {
				rgb.b += shedSpeed;
			}
			ParticleSystem.MainModule psMain = ps.main; // Reference to particle system's main attributes.
			psMain.startColor = new Color (rgb.r, 0, 0); // Change color of particles.
			ps.Emit (1); // Emit red particle.
			psMain.startColor = new Color (0, rgb.g, 0); // Change color of particles.
			ps.Emit (1); // Emit red particle.
			psMain.startColor = new Color (0, 0, rgb.b); // Change color of particles.
			ps.Emit (1); // Emit red particle.
			// Change color based on result.
			setColor (rgb);
		} else {
			ParticleSystem.MainModule psMain = ps.main; // Reference to particle system's main attributes.
			psMain.startColor = new Color (1f, 1f, 1f);
			ps.Emit (3); // Emit particle.
			setColor (rgb);

		}
	}

	// Called when player successfully breaches a gate.
	public void revokeColor() {
		source.clip = clips [1];
		float randomPitch = Random.Range (-0.2f, 0.2f);
		source.volume = 1f;
		source.pitch = 1 + randomPitch;
		source.Play ();

		ParticleSystem.MainModule psMain = ps.main; // Reference to particle system's main attributes.
		psMain.startColor = new Color(rgb.r, 0, 0); // Change color of particles.
		ps.Emit (30); // Emit red particle.
		psMain.startColor = new Color(0, rgb.g, 0); // Change color of particles.
		ps.Emit (30); // Emit red particle.
		psMain.startColor = new Color(0, 0, rgb.b); // Change color of particles.
		ps.Emit (30); // Emit red particle.
		setColor(new Color (GREY, GREY, GREY)); // Return player to grey.
	}

	Focus currentFocus; // Reference to current camera angle, if any.
	// Focus-point triggers access this to set themselves as the focus.
	public void newFocus(Focus f) {
		currentFocus = f;
	}

	// Get rid of focal point of camera.
	public void resetFocus() {
		currentFocus = null;
	}

	// Interpolate focus point of camera.
	void lerpFocus() {
		Vector3 targetPos;
		float targetScale;
		if (currentFocus == null) { // If player is not in a focus point trigger.
			// Just make the target position around the player.
			targetPos = transform.position + new Vector3(rb.velocity.x * 1.3f, rb.velocity.y * 1.3f, 0);
			targetScale = 10;

		} else {
			// Set focal point to be the radius and location of the focus trigger.
			targetPos = currentFocus.transform.position;
			targetScale = currentFocus.scale;

		}
		// Interpolation to target location.
		if ((camObject.transform.position - targetPos).magnitude > 1f) { // If difference is bigger than 1.
			float smoothX = camObject.transform.position.x;
			smoothX -= (camObject.transform.position.x - targetPos.x) / 50f;

			float smoothY = camObject.transform.position.y;
			smoothY -= (camObject.transform.position.y - targetPos.y) / 50f;

			camObject.transform.position = new Vector3 (smoothX, smoothY, -10);
		}
		// Interpolate orthographic scale.
		if ((Mathf.Abs(cam.orthographicSize - targetScale)) > 1f) { // If difference is bigger than 1.
			cam.orthographicSize -= (cam.orthographicSize - targetScale) / 200f;
		}
	}

	// Experimental code. Never called in this iteration.
	void handleEnvironmentBrightness() {
		brightness = (transform.position.y + 80)/255f;
		RenderSettings.ambientLight = new Color (brightness / 1.2f, brightness / 1.1f, brightness / 1f);
		cam.backgroundColor = new Color ((brightness*brightness)/4f, 
			(brightness*brightness)/2f, 
			(brightness*brightness)/1f);


	}

	// PLOT RELATED
	public bool isLit;

	readonly int NONE = 0;
	readonly int PURPLE_START = 1; // Wake up and find me.
	readonly int PURPLE_INTRO = 2; // Take this.

	readonly int RED_INTRO = 3;
	readonly int RED_HURT = 4;
	readonly int RED_DEAD = 5;

	Text subtitle;

	Subtitle currentSubScript;
	string currentSub;
	Color currentSubColor;
	int currentLine;
	public float subOpacity;
	public bool subFadeIn;
	public float subTime;
	bool isReadyForNew;

	void resetSubtitle() {
		currentSubScript.isReadyForNew = true;

		currentSubScript = null;
		currentSub = "";
		currentSubColor = new Color (0, 0, 0, 0);
	}

	public void getSubtitle(Subtitle s, string sub, Color subColor) {
		if (currentSubScript != null) { // Is not the first frame.
			if (!currentSubScript.Equals (s)) { // Is replacement.
				Debug.Log("Replace");
				//Debug.Log ("New replacement: " + sub);
				resetSubtitle ();

				currentSubScript = s;

				currentSub = sub;
				currentSubColor = subColor;
				isReadyForNew = false;
			} else {
				//Debug.Log ("Still within: " + sub);
				if (s.currentLine != currentLine) {
					if (s.currentLine < s.lines.Length) {
						currentLine = s.currentLine;
						currentSub = currentSubScript.lines [currentLine];
					} else {
						currentSubScript.deactivated = true;

					}
				}

			}

		} else { // Is first frame.
			//Debug.Log ("New first-frame: " + sub);

			currentSubScript = s;
			currentSub = sub;
			currentSubColor = subColor;
			isReadyForNew = false;
		}
	}

	public void playSubtitle() {
		//if (currentSubScript != null) {

			subtitle.text = currentSub;
			subtitle.color = new Color (currentSubColor.r, currentSubColor.g, currentSubColor.b, subOpacity);

			if (subFadeIn) { // Fade-in + hold.
				if (subOpacity < 1.0f) { // Fade in.
					subOpacity += Time.deltaTime;
				} else { // Keep faded in.
					subOpacity = 1.0f; // ^^
					if (subTime > 0) { // If timer.
						subTime -= Time.deltaTime; // Time down.
					} else { // If typer is complete.
						subFadeIn = false; // Begin fading out.
					}
				}
			} else {
				if (subOpacity > 0) { // Fade down
					subOpacity -= Time.deltaTime;
				} else { // Finished.
				
				if (currentSubScript != null) {
					resetSubtitle ();

				}
					isReadyForNew = true;
					subOpacity = 0;
				}
			}

		//}
	}

	void fadeIn() {
		fadeTime -= Time.deltaTime;
		blackout.color = new Color (1, 1, 1, fadeTime / 3f);
		if (fadeTime <= 0) {
			fadeTime = 0;
			justStarted = false;
		}
	}
	void fadeOut() {
		if (transform.position.y > 10f) {
			whiteout.color = new Color (1f, 1f, 1f, (transform.position.y - 10f)/40f);

		} else {
			whiteout.color = new Color (1f, 1f, 1f, 0);
		}

	}

	public bool hasWon;
	public void win() {
		hasWon = true;
		Application.LoadLevel (Application.loadedLevel + 1);
	}

	AudioSource source;
	void OnCollisionEnter2D(Collision2D c) {
		if (c.relativeVelocity.magnitude > 2f) {
			source.clip = clips [0];
			float randomPitch = Random.Range (-0.2f, 0.2f);
			source.volume = 1 - (1 / (c.relativeVelocity.magnitude * 1f));
			source.pitch = 1 + randomPitch;
			Debug.Log (source.volume);
			source.Play ();
		}
	}
}
