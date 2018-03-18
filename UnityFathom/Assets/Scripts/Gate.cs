using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

	GameObject rayObject;
	SpriteRenderer raySprite;


	Color rgb;

	void Start () {
		
		rayObject = transform.Find ("Ray").gameObject;
		raySprite = rayObject.GetComponent<SpriteRenderer> ();
		getRayColor (); // Unused.
	}

	void getRayColor() {
		rgb = raySprite.color;
	}

	public void shatter() {


			
			GameObject.Destroy (gameObject); // Destroy self.

	}
}
