using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTableroController : MonoBehaviour {

	public GameObject box;
	public GameObject son;
	public bool editable;
	//private ArrayList boxList;
	private GameObject[,] boxList;
	private Color[,] estadoActual;
	private Tablero tablero;
	private AdministradorPartida Administrador;

	private GameObject reticle;

	// Use this for initialization
	void Awake () {
		//this.boxList = new ArrayList();
		boxList = new GameObject[10,10];
		estadoActual = new Color[10,10];

		// El color negro es NULL, no usar casilleros negros en el juego.
		for (int i = 0; i <= 9; i++) {
			for (int j = 0; j <= 9; j++) {
				estadoActual [j, i] = new Color (0, 0, 0);
			}
		}
		Administrador = GameObject.FindWithTag ("Administrador").GetComponent<AdministradorPartida>();

		reticle = GameObject.Find ("Player/GvrEditorEmulator/Main Camera/GvrReticlePointer");
	}

	public void mostrarTablero(Tablero tab){
		GameObject test;
		for (int i = 0; i <= 9; i++) {
			for (int j = 0; j <= 9; j++) {
				// Se invierte el orden porque las coordenadas en la batalla naval son diferentes (x=columna, y=fila)
				if (tab.GetHayBarco (j, i)) {
					placeMark (j, i, new Color (1, 1, 1));
					//test = GameObject.Instantiate (box);
					//test.transform.parent = this.gameObject.transform;
					//test.transform.localPosition = new Vector3 ((i+1)-0.5f, -(j+1)+0.5f, 0);
					//test.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
				}
				//Debug.Log(tab.GetCasilla(i,j).GetEstado());
			}
		}
	}

	public void setTablero(Tablero tab){
		this.tablero = tab;
	}

    // Se toma segun el orden de los audio source que tiene el gameObject
    private void ReproducirSonidoDelJuego(string sonido)
    {
        var sonidosDelJuego = GameObject.Find("SonidosDelJuego").GetComponents<AudioSource>();
        switch(sonido)
        {
            case "ColocarBarco": sonidosDelJuego[0].Play(); break;
        }       
    }

	// Update is called once per frame
	private void Update () {
		if (editable && (Input.GetMouseButtonDown (0) || Input.GetButtonDown("Fire1"))) {
			RaycastHit hit;
			//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Ray ray = new Ray(reticle.transform.position, reticle.transform.forward);
			if (!Administrador.EsFinDePartida() && Physics.Raycast (ray, out hit, 1000F, LayerMask.GetMask ("ShipBoard"))) {              
                if (hit.transform == son.transform) {
					int[] casillas = coordToGrid(hit);
					if (!casillaValida(casillas[0],casillas[1])) return;
                    ReproducirSonidoDelJuego("ColocarBarco");
                    Administrador.EjecutarAccion (casillas [0], casillas [1], this);                 
                }
			}          
        }
		//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
	}

	private int[] coordToGrid(RaycastHit hit){
		Vector3 localHit = this.gameObject.transform.InverseTransformPoint (hit.point);
		int[] casillas = new int[2];
		// Se invierte el orden porque las coordenadas en la batalla naval son diferentes (x=columna, y=fila)
		casillas[1] = (int) Mathf.Floor (localHit.x);
		casillas[0] = (int) -(Mathf.Floor (localHit.y) + 1);
		return casillas;
	}

	public bool placeMark(int casillax, int casillay, Color col){
		GameObject test = GameObject.Instantiate (box);
		test.transform.parent = this.gameObject.transform;
		// Se invierte el orden porque las coordenadas en la batalla naval son diferentes (x=columna, y=fila)
		test.transform.localPosition = new Vector3 ((casillay + 1) - 0.5f, -(casillax+1)+ 0.5f, -0.01f);
		//test.transform.localScale = new Vector3 (0.15f, 0.15f, 0.15f);
		test.transform.localRotation = Quaternion.identity;
		test.transform.Rotate (new Vector3 (0,-90,90));
		test.GetComponent<Renderer> ().material.color = col;
		Color auxcol = new Color(col.r*0.5f,col.g*0.5f,col.b*0.5f);
		test.GetComponent<Renderer> ().material.SetColor("_EmissionColor",auxcol);
		//boxList.Add (test);
		if (boxList [casillax, casillay] != null) {
			GameObject.Destroy (boxList [casillax, casillay]);
			boxList [casillax, casillay] = null;
		}
		boxList[casillax,casillay] = test;
		estadoActual [casillax, casillay] = col;
		return true;
	}

	public bool clearMark(int casillax, int casillay){
		if (boxList [casillax, casillay] != null) {
			GameObject.Destroy (boxList [casillax, casillay]);
			boxList [casillax, casillay] = null;
			estadoActual [casillax, casillay] = new Color (0, 0, 0);
			return true;
		}
		return false;
	}

	public void clearMarks(){
		if (boxList == null)
			return;
		for (int i = 0; i <= 9; i++) {
			for (int j = 0; j <= 9; j++) {
				GameObject.Destroy (boxList [i, j]);
				boxList [i, j] = null;
			}
		}
	}

	public void TransferirEstado(TestTableroController nuevo){
		nuevo.EstadoActual = this.estadoActual;
		for (int i = 0; i <= 9; i++) {
			for (int j = 0; j <= 9; j++) {
				// Se invierte el orden porque las coordenadas en la batalla naval son diferentes (x=columna, y=fila)
				if (nuevo.EstadoActual [j, i].Equals(new Color (0, 0, 0))) {
					nuevo.clearMark (j, i);
				} else {
					nuevo.placeMark (j, i, nuevo.EstadoActual [j, i]);
				}
			}
		}
	}

	private bool casillaValida(int x, int y){
		if (x < 0 || x > 9 || y < 0 || y > 9)
			return false;
		else
			return true;
	}

	public Color[,] EstadoActual {
		get {
			return this.estadoActual;
		}
		set {
			estadoActual = value;
		}
	}
							
}
