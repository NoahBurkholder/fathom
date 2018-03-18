using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatStore : MonoBehaviour {
	public int dead;
	public int healed;

	void Start() {
		DontDestroyOnLoad (gameObject);
	}
}
