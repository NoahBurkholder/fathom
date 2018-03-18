using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginGame : MonoBehaviour {

	GameObject player;
	Player playerScript;

	void OnTriggerStay2D (Collider2D c) {
			Application.LoadLevel (2);


	}
}
