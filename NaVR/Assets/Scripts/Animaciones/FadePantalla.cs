using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;	

public class FadePantalla : MonoBehaviour {

	private float currentAlpha;
	private bool active;
	private bool forceactive;
	private float speed;
	private Image img;
	private RectTransform rect;
	private CallBackDelegate callbackfunc;

	// Use this for initialization
	void Start () {
		active = false;
		forceactive = false;
		currentAlpha = 0;
		speed = 1f;
		img = gameObject.GetComponent<Image> ();
		rect = gameObject.GetComponent<RectTransform> ();
		img.color = new Color(gameObject.GetComponent<Image> ().color.r,gameObject.GetComponent<Image> ().color.g,gameObject.GetComponent<Image> ().color.b,0);
		callbackfunc = null;
	}
	
	// Update is called once per frame
	void Update () {

		if (active && currentAlpha >= 1f){

			callbackfunc();
			active = false;

		}

		if (currentAlpha > 0f && active == false) {
			currentAlpha = currentAlpha - speed * Time.deltaTime;
		} else if (currentAlpha < 1f && active == true) {
			currentAlpha = currentAlpha + speed * Time.deltaTime;
		}

		if (forceactive){
			currentAlpha = 1f;
		}

		img.color = new Color(img.color.r,img.color.g,img.color.b,currentAlpha);
	}

	public void IniciarFade(CallBackDelegate callback, float speedp = 1f){

		active = true;
		speed = speedp;
		callbackfunc = callback;

	}

	// Esto es instantaneo
	public void IniciarFadeForzado(){

		forceactive = true;

	}

	public void TerminarFadeForzado(){

		forceactive = false;

	}
}
