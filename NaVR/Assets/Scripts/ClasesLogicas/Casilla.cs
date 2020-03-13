using System;
using UnityEngine;

[System.Serializable]
public class Casilla {

    [SerializeField] private int fila;
	[SerializeField] private int columna;
	private int estado;
    [System.NonSerialized] private Barco barco;
    private bool hayBarco;
	//private bool hayBarcoFantasma;
	private bool atacada;
	private bool revelada;
	private int reforzada; // 0 - No reforzada; 1 - Reforzada; 2 - Reforzada y atacada
	private bool contramedidas;
	private int trampa; // 0 - No trampa; 1 - Radar Pasivo; 2 - Sabotaje

	public Casilla(int fila, int columna, int estado){
		this.fila = fila;
		this.columna = columna;
		this.estado = estado;
		this.barco = null;
		this.hayBarco = false;
		this.reforzada = 0;
		this.atacada = false;
		this.revelada = false;
		this.contramedidas = false;
		this.trampa = 0;
	}

	public Casilla(int fila, int columna){
		this.fila = fila;
		this.columna = columna;
		this.estado = -1;
		this.barco = null;
		this.hayBarco = false;
		this.reforzada = 0;
		this.atacada = false;
		this.revelada = false;
		this.contramedidas = false;
		this.trampa = 0;
	}

	/*
	// Use this for initialization
	public void Init (int fila, int columna, int estado) {
		this.fila = fila;
		this.columna = columna;
		this.estado = estado;
	}

	public static Casilla CreateInstance(int fila, int columna, int estado)
	{
		var data = ScriptableObject.CreateInstance<Casilla>();
		data.Init(fila, columna, estado);
		return data;
	}

	public int GetFila () {
		return this.fila;
	}

	public int GetColumna () {
		return this.columna;
	}

	public int GetEstado () {
		return this.estado;
	}

	public Barco GetBarco () {
		return this.barco;
	}

	public bool GetHayBarco () {
		return this.hayBarco;
	}

	public void SetFila (int fila) {
		this.fila = fila;	
	}

	public void SetColumna (int columna) {
		this.columna = columna;
	}

	public void SetEstado (int estado) {
		this.estado = estado;
		if (estado == Constantes.POSICION_ATACADA_BARCO)
			barco.estoyVivo();
	}

	public void SetHayBarco (bool hayBarco) {
		this.hayBarco = hayBarco;
	}

	public void SetBarco (Barco barco) {
		this.barco = barco;
	}

	*/

	public void setPosicion(int fila, int columna) 
	{
		this.fila = fila;
		this.columna = columna;
	}

	public int Fila {
		get {
			return this.fila;
		}
		set {
			fila = value;
		}
	}

	public int Columna {
		get {
			return this.columna;
		}
		set {
			columna = value;
		}
	}

	public int Estado {
		get {
			return this.estado;
		}
		set {
			estado = value;
		}
	}

	public Barco Barco {
		get {
			return this.barco;
		}
		set {
			barco = value;
			if (value != null)
				hayBarco = true;
			else
				hayBarco = false;
		}
	}

	public bool HayBarco {
		get {
			return this.hayBarco;
		}
		set {
			hayBarco = value;
		}
	}

	public int Reforzada {
		get {
			return this.reforzada;
		}
		set {
			reforzada = value;
		}
	}

	public bool Atacada {
		get {
			return this.atacada;
		}
		set {
			atacada = value;
			if (hayBarco)
				barco.estoyVivo();
		}
	}

	public bool Revelada {
		get {
			return this.revelada;
		}
		set {
			revelada = value;
		}
	}

	public bool Contramedidas {
		get {
			return this.contramedidas;
		}
		set {
			contramedidas = value;
		}
	}

	public int Trampa {
		get {
			return this.trampa;
		}
		set {
			trampa = value;
		}
	}
		
}
