using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Win : MonoBehaviour {

	GameObject player;
	Player playerScript;
	Text escText;
	Text messageText;
	string message;
	public bool hasWon;
	Color colorEsc;
	Color colorMsg;

	StatStore ss;

	public GameObject[] characters;

	// Mostly for reference objects.
	void Start () {

		player = GameObject.Find ("Player");
		playerScript = player.GetComponent<Player> ();
		ss = GameObject.Find ("StatObject").GetComponent<StatStore>();

		escText = GameObject.Find ("Canvas/Esc").GetComponent<Text> ();
		messageText = GameObject.Find ("Canvas/Message").GetComponent<Text> ();
		timerEsc = 0;
		timerMsg = 0;



	}
	float timerEsc, timerMsg;

	// Handles win-screen states. Pretty basic stuff.
	void winScreen() {
		Debug.Log (timerEsc);
		if (timerMsg < 1f) {
			timerMsg += 1f / 360f;
			colorMsg = new Color (timerMsg * 0.5f, timerMsg * 0.5f, timerMsg * 0.5f, timerMsg);
			messageText.color = colorMsg;
		} else {

			timerMsg = 1f;

			if (timerEsc < 1f) {
				timerEsc += 1f / 360f;

				colorEsc = new Color (timerEsc * 0.5f, timerEsc * 0.5f, timerEsc * 0.5f, timerEsc);
				escText.color = colorEsc;


			} else {
				timerEsc = 1f;
			}
		}

	}


	// Decision tree for various ending phrases.
	void fetchStats() {
		int dead = 0;
		int healed = 0;

		dead = ss.dead;
		healed = ss.healed;

		if (dead == 0) {
			message = "... and so the pebble left, humbly and quietly. \n";
		} else if (dead == 1) {
			message = "... and so the pebble left, with a nagging regret. \n";
		} else if (dead == 2) {
			message = "... and so the pebble left, with a heavy weight to carry. \n";
		} else if (dead > 2) {
			message = "... and so the pebble left, greedy and reckless. \n";
		}

		if (healed == 0) {
			if (dead == 0) {
				message = message + "the others soon forgot ...";
			} else if (dead == 1) {
				message = message + "the others did not hold grudges ...";
			} else if (dead == 2) {
				message = message + "the others recovered, alone ...";
			} else if (dead > 2) {
				message = message + "an outcast ...";
			}

		} else if (healed == 1) {
			message = message + "but the others saw hope still for pebble ...";
		} else if (healed == 2) {
			message = message + "but the others knew pebble had a good heart ...";
		} else if (healed > 2) {
			message = message + "the others never forgot pebble ...";
		}
		messageText.text = message;
		InvokeRepeating ("winScreen", 0, 1f / 60f); // This is called once, but gives a function repetion every 1/60 of a second.
	}


	void OnTriggerStay2D (Collider2D c) {
		if (c.gameObject.Equals (player)) {
			if (!hasWon) {
				hasWon = true;
				fetchStats ();
			}
		}
	}
}
