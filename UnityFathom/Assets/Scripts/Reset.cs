using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("r")) {
			Application.LoadLevel (Application.loadedLevel);
		}
		if (Input.GetKeyDown ("escape")) {
			Application.LoadLevel (0);
		}

	}
}
