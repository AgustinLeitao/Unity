using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour {

	public shipEnum tamano;
	public GameObject barcoReal;
	public ArrayList listaVec;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public Boat(shipEnum tamano, GameObject barcoReal){
		this.tamano = tamano;
		this.barcoReal = barcoReal;
		this.listaVec = new ArrayList();

	}

	public Boat(){
		this.tamano = shipEnum.posicionLibre;
	}

	public void addCoord(Vector2 vec){
		listaVec.Add (vec);
	}
}