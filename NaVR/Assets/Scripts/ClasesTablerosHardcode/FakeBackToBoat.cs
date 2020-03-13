using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FakeBackToBoat : MonoBehaviour {

	// Use this for initialization
	void Start () {
	//Comentario	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BackToBoat() {
        SceneManager.LoadScene("Environment");
    }
}
