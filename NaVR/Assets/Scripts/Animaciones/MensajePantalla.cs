using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;	

public class MensajePantalla : MonoBehaviour {

	private float currentAlpha;
	private float targetAlpha;
	private int currentWidth;
	private int targetWidth;
	private bool active;
	private string myText;
	private float updatesLeft;
	private Text txt;
	private Image img;
	private RectTransform rect;

	// Use this for initialization
	void Start () {
		myText = "";
		updatesLeft = 0;
		active = false;
		currentAlpha = 0;
		targetAlpha = 0;
		currentWidth = 0;
		targetWidth = 0;
		img = gameObject.GetComponent<Image> ();
		rect = gameObject.GetComponent<RectTransform> ();
		txt = this.transform.Find ("Text").gameObject.GetComponent<Text> ();
		img.color = new Color(gameObject.GetComponent<Image> ().color.r,gameObject.GetComponent<Image> ().color.g,gameObject.GetComponent<Image> ().color.b,0);
	}
	
	// Update is called once per frame
	void Update () {
		if (active && updatesLeft < 0) {
			active = false;
			targetAlpha = 0;
			targetWidth = 0;
		}
        if(active)
            updatesLeft -= Time.deltaTime;

		if (currentAlpha > targetAlpha && currentAlpha > 0 && active == false) {
			currentAlpha = currentAlpha - 0.8f * Time.deltaTime;
		} else if (currentAlpha < targetAlpha && currentAlpha < 1 && active == true) {
			currentAlpha = currentAlpha + 0.8f * Time.deltaTime;
		}

		if (currentWidth > targetWidth && currentWidth > 0 && active == false) {
			currentWidth = (int) (currentWidth - 300 * Time.deltaTime);
		} else if (currentWidth < targetWidth && currentWidth < 1000 && active == true) {
			currentWidth = (int) (currentWidth + 800 * Time.deltaTime);
		}

		txt.text = myText;
		txt.color = new Color(txt.color.r,txt.color.g,txt.color.b,currentAlpha*3);
		img.color = new Color(img.color.r,img.color.g,img.color.b,currentAlpha);
		rect.sizeDelta = new Vector2 (currentWidth, rect.sizeDelta.y);
	}

	public void DisplayMessage(string msg, float time, int width){
		updatesLeft = time;
		active = true;
		myText = msg;
		targetAlpha = 0.33f;
		targetWidth = width;
		if (currentWidth < (width/4)*2){
			currentWidth = (width/4)*2;
		}
	}
}
