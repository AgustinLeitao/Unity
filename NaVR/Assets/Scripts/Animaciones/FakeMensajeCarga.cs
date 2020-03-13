using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;	

public class FakeMensajeCarga : MonoBehaviour {

	private bool active;
	private Text txt;
	private Image img;
	private GameObject sprite;

	// Use this for initialization
	void Start () {
		active = false;
		txt = this.transform.Find ("Text").gameObject.GetComponent<Text> ();
		sprite = this.transform.Find ("Circulo").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		txt.enabled = active;
		sprite.GetComponent<Image> ().enabled = active;
		if (active){
			sprite.transform.Rotate (new Vector3(0,0,-100*Time.deltaTime));
		}
	}

	public void DisplayMessage(bool activate){
		active = activate;
	}

}
