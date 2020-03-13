using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstantesDeBarco {
	public const int ORIENTACION_HORIZONTAL = 0;
	public const int ORIENTACION_VERTICAL = 1;



}

[System.Serializable]	
public class Barco {

	[SerializeField] private Casilla[] posicionesOcupadas;
	[SerializeField] private int tipoBarco;
	[SerializeField] private int tipoHabilidades;
	[SerializeField] private int orientacion; //0 es horizontal, 1 es vertical
	[SerializeField] private bool vivo;
    [System.NonSerialized]
	private GameObject barcoFisico;

	// Use this for initialization
	public Barco (Casilla [] posicionesOcupadas, int tipoBarco, int tipoHabilidades, int orientacion) {
		this.posicionesOcupadas = new Casilla[posicionesOcupadas.Length];
		for (int i=0; i< posicionesOcupadas.Length; i++) {
			this.posicionesOcupadas [i] = posicionesOcupadas [i];
		}
		this.tipoBarco = tipoBarco;
		this.tipoHabilidades = tipoHabilidades;
		this.orientacion = orientacion;
		this.vivo = true;
	}

	public Casilla[] GetPosicionesOcupadas () {
		return this.posicionesOcupadas;
	}

	public int GetTipoHabilidades () {
		return this.tipoHabilidades;
	}

	public int GetOrientacion () {
		return this.orientacion;
	}

	public int GetTipoBarco () {
		return this.tipoBarco;
	}

	public void estoyVivo(){
		bool aux = false;
		foreach (Casilla cas in posicionesOcupadas) {
			if (cas.Atacada == false)
				aux = true;
		}

		vivo = aux;
		if (aux == false)
			barcoFisico.GetComponent<BarcoFisico>().Hundido = true;
		//if (vivo == false)
		//	Debug.Log ("Mori");
	}

	public bool Vivo {
		get {
			return this.vivo;
		}
	}

	public GameObject BarcoFisico {
		get {
			return this.barcoFisico;
		}
		set {
			barcoFisico = value;
		}
	}

	public int GetIndicePosicionViva(){
		int[] indices = new int[posicionesOcupadas.Length];
		int count = 0;
		for (int i = 0; i < posicionesOcupadas.Length; i++) {
			if (!posicionesOcupadas[i].Atacada) {
				indices [count] = i;
				count++;
			}
		}

		if (count > 0) {
			int ran = Random.Range (0, count);
			return indices [ran];
		} else {
			return -1;
		}
	}
}
