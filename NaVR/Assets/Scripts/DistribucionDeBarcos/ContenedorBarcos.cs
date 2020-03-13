using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContenedorBarcos : MonoBehaviour {

	public Barco[] barcos;
	public Barco[] barcosRival;

	void Awake () {
		DontDestroyOnLoad (this);
	}

	void Start () {
		barcos = new Barco[8];
		barcosRival = new Barco[8];
	}

	public void ReiniciarArray () {
		barcos = new Barco[8];
	}

}
