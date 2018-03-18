using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreWin : MonoBehaviour {

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

	}

	// Decision tree for various ending phrases.
	void fetchStats() {
		int dead = 0;
		int healed = 0;
		for (int i = 0; i < characters.Length; i++) {
			GameObject c = characters [i];
			Character s = c.GetComponent<Character> ();
			if (s.isDead) {
				dead++;
			}
			if (s.isHealed) {
				healed++;
			}
		}

		ss.dead = dead;
		ss.healed = healed;

	}


	void OnTriggerStay2D (Collider2D c) {
		if (c.gameObject.Equals (player)) {
			playerScript.win ();
			if (!hasWon) {
				hasWon = true;
				fetchStats ();
			}
		}
	}
}
