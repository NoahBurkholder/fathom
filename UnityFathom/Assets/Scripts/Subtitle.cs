using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subtitle : MonoBehaviour {

	GameObject player;
	Player playerScript;
	CircleCollider2D col;

	public readonly int ALIVE = 0;
	public readonly int HURT = 1;
	public readonly int DEAD = 2;
	public readonly int HEALED = 3;

	public int state;

	public Color color;
	public string[] lines;
	public int beginHurt;
	public int beginDead;
	public int beginHealed;

	public bool isReadyForNew;
	public int currentLine;
	public bool activated, deactivated;
	public GameObject activatedCharacter;

	// Mostly for reference objects.
	void Start () {
		currentLine = -1; // incremented by characters.
		isReadyForNew = true;
		player = GameObject.Find ("Player");
		playerScript = player.GetComponent<Player> ();
		col = GetComponent<CircleCollider2D> ();
	}

	// Keep focus on self if player is within this focal trigger.
	void OnTriggerStay2D (Collider2D c) {
		if (c.gameObject.Equals (player)) {
			if (!deactivated) {
				if (activated) {
					if (currentLine < lines.Length) { // If within array.
				

						if (activatedCharacter != null) {
							if (state == ALIVE) {
								// Assumedly is now 0.

								if (currentLine < beginHurt - 1) { // If within alive lines.
									if (isReadyForNew) { // Prompt new dialogue.
										Debug.Log ("ALIVE");

										currentLine++; // Increment dialogue.
										isReadyForNew = false; // In process.
										playerScript.subFadeIn = true;
										playerScript.getSubtitle (this, lines [currentLine], color);
										playerScript.subTime = ((float)lines [currentLine].Length + 1) * 0.05f;
										if (currentLine == beginHurt - 1) {
											characterReady ();
										}

									} /*else {
									if (c.gameObject.Equals (player)) {
										playerScript.getSubtitle (this, lines [currentLine], color);

									}
								}*/
								
									//deactivated = true;
								}
							} else if (state == HURT) {

								if (currentLine < beginHurt - 1) { // Make sure within hurt lines.
									Debug.Log ("MADE HURT");
									currentLine = beginHurt;
								}
								if (currentLine < beginDead - 1) {
									if (isReadyForNew) { // Prompt new dialogue.
										Debug.Log ("HURT");

										currentLine++; // Increment dialogue.
										isReadyForNew = false; // In process.
										playerScript.subFadeIn = true;
										playerScript.getSubtitle (this, lines [currentLine], color);
										playerScript.subTime = ((float)lines [currentLine].Length + 1) * 0.05f;
										if (currentLine == beginHurt - 1) {
											characterReady ();
										}

									}
								}
							} else if (state == DEAD) {

								if (currentLine < beginDead - 1) {
									currentLine = beginDead - 1;
								}
								if (currentLine < beginHealed - 1) {
									if (isReadyForNew) { // Prompt new dialogue.
										Debug.Log ("DEAD");

										currentLine++; // Increment dialogue.
										isReadyForNew = false; // In process.
										playerScript.subFadeIn = true;
										playerScript.getSubtitle (this, lines [currentLine], color);
										playerScript.subTime = ((float)lines [currentLine].Length + 1) * 0.05f;
										if (currentLine == beginHurt - 1) {
											characterReady ();
										}

									}
								}
							} else if (state == HEALED) {

								if (currentLine < beginHealed - 1) {
									currentLine = beginHealed - 1;
								}
								if (currentLine < lines.Length - 1) {
									if (isReadyForNew) { // Prompt new dialogue.

										currentLine++; // Increment dialogue.
										isReadyForNew = false; // In process.
										playerScript.subFadeIn = true;
										playerScript.getSubtitle (this, lines [currentLine], color);
										playerScript.subTime = ((float)lines [currentLine].Length + 1) * 0.05f;
										if (currentLine == beginHurt - 1) {
											characterReady ();
										}

									}
								}
							}
						} else { // No trigger events.
							if (isReadyForNew) { // Prompt new dialogue.
								if (currentLine < lines.Length - 1) {
									Debug.Log ("BASIC");

									currentLine++; // Increment dialogue.
									isReadyForNew = false; // In process.
									playerScript.subFadeIn = true;
									playerScript.getSubtitle (this, lines [currentLine], color);
									playerScript.subTime = ((float)lines [currentLine].Length + 1) * 0.05f;
								}
							}
						}
					}
				}
			}
		}
	}

	void characterReady() {
		if (activatedCharacter != null) {
			activatedCharacter.GetComponent<Character>().isReady = true;
		}
	}
}
