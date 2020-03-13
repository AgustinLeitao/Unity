using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextoPremio : MonoBehaviour {

	private bool showing;
	private int text;
	private Color alphaColor;
	private Color oldColor;
	private bool direction;
	private float timeToFade;
	private float curAlpha;

	private GameObject TextBarco3Pos;
	private GameObject TextBarco4Pos;
	private GameObject TextBarco5Pos;

	private Color baseColor;

	// Use this for initialization
	void Start () {
		showing = false;
		text = 0;
		alphaColor = GetComponent<MeshRenderer>().material.color;
		alphaColor.a = 0;
		direction = false;
		GetComponent<MeshRenderer> ().material.color = alphaColor;
		curAlpha = 0f;
		TextBarco3Pos = GameObject.Find("TextBarco3Pos");
		TextBarco4Pos = GameObject.Find("TextBarco4Pos1");
		TextBarco5Pos = GameObject.Find("TextBarco5Pos");
		baseColor = new Color (255, 255, 255, 0);
		baseColor.a = 0;
		TextBarco3Pos.GetComponent<MeshRenderer> ().material.color = baseColor;
		TextBarco4Pos.GetComponent<MeshRenderer> ().material.color = baseColor;
		TextBarco5Pos.GetComponent<MeshRenderer> ().material.color = baseColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (showing == true && direction == false) {
			curAlpha -= 0.02f;

		} else if (showing == true && direction == true) {
			curAlpha += 0.02f;
			if (curAlpha >= 3) {
				direction = false;
				timeToFade = 0f;
			}

		}
		alphaColor = GetComponent<MeshRenderer>().material.color;
		alphaColor.a = curAlpha;
		GetComponent<MeshRenderer> ().material.color = alphaColor;
		if (text == 1) {
			TextBarco3Pos.GetComponent<MeshRenderer> ().material.color = alphaColor;
			TextBarco4Pos.GetComponent<MeshRenderer> ().material.color = baseColor;
			TextBarco5Pos.GetComponent<MeshRenderer> ().material.color = baseColor;
		} else if (text == 2) {
			TextBarco4Pos.GetComponent<MeshRenderer> ().material.color = alphaColor;
			TextBarco3Pos.GetComponent<MeshRenderer> ().material.color = baseColor;
			TextBarco5Pos.GetComponent<MeshRenderer> ().material.color = baseColor;
		} else if (text == 3) {
			TextBarco5Pos.GetComponent<MeshRenderer> ().material.color = alphaColor;
			TextBarco3Pos.GetComponent<MeshRenderer> ().material.color = baseColor;
			TextBarco4Pos.GetComponent<MeshRenderer> ().material.color = baseColor;
		}
	}

	public void startShowing(int text){
		showing = true;
		this.text = text;
		direction = true;
	}
}
