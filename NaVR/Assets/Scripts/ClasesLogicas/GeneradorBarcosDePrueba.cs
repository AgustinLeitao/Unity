using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorBarcosDePrueba {

	private System.Random randomNumber;
	private Barco[] barcos;
	Casilla[] posicionesBarco;
	// Use this for initialization
	void Start () {
		
	}

	public Barco [] CrearBarcosParaPruebas (int numero = 0) { //Para pruebas
		barcos = new Barco[8];
		this.randomNumber = new System.Random ();


		if (numero == 0) {
			numero = randomNumber.Next (1, 6);	
		}

		switch (numero) {
		case 1:
			CrearArmado1 ();
			break;
		case 2:
			CrearArmado2 ();
			break;
		case 3:
			CrearArmado3 ();
			break;
		case 4:
			CrearArmado4 ();
			break;
		case 5:
			CrearArmado5 ();
			break;
		}
		return barcos;
	}

	private void CrearArmado1 () {
		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(0, 5, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(0, 6, Constantes.POSICION_BARCO); //Para pruebas
		barcos[0] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(0, 0, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(0, 1, Constantes.POSICION_BARCO); //Para pruebas
		barcos[1] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(3, 0, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(4, 0, Constantes.POSICION_BARCO); //Para pruebas
		barcos[2] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_VERTICAL);

		posicionesBarco = new Casilla[3];
		posicionesBarco[0] = new Casilla(7, 7, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(7, 8, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(7, 9, Constantes.POSICION_BARCO); //Para pruebas
		barcos[3] = new Barco (posicionesBarco, 3, 3, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[3];
		posicionesBarco[0] = new Casilla(9, 0, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(9, 1, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(9, 2, Constantes.POSICION_BARCO); //Para pruebas
		barcos[4] = new Barco (posicionesBarco, 3, 3, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[4];
		posicionesBarco[0] = new Casilla(4, 4, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(5, 4, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(6, 4, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(7, 4, Constantes.POSICION_BARCO); //Para pruebas
		barcos[5] = new Barco (posicionesBarco, 4, 4, ConstantesDeBarco.ORIENTACION_VERTICAL);

		posicionesBarco = new Casilla[4];
		posicionesBarco[0] = new Casilla(9, 6, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(9, 7, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(9, 8, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(9, 9, Constantes.POSICION_BARCO); //Para pruebas
		barcos[6] = new Barco (posicionesBarco, 4, 4, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[5];
		posicionesBarco[0] = new Casilla(0, 9, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(1, 9, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(2, 9, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(3, 9, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[4] = new Casilla(4, 9, Constantes.POSICION_BARCO); //Para pruebas
		barcos[7] = new Barco (posicionesBarco, 5, 5, ConstantesDeBarco.ORIENTACION_VERTICAL);
	}

	private void CrearArmado2 () {
		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(6, 1, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(6, 2, Constantes.POSICION_BARCO); //Para pruebas
		barcos[0] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(5, 4, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(6, 4, Constantes.POSICION_BARCO); //Para pruebas
		barcos[1] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_VERTICAL);

		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(0, 0, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(0, 1, Constantes.POSICION_BARCO); //Para pruebas
		barcos[2] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[3];
		posicionesBarco[0] = new Casilla(3, 7, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(3, 8, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(3, 9, Constantes.POSICION_BARCO); //Para pruebas
		barcos[3] = new Barco (posicionesBarco, 3, 3, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[3];
		posicionesBarco[0] = new Casilla(2, 1, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(2, 2, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(2, 3, Constantes.POSICION_BARCO); //Para pruebas
		barcos[4] = new Barco (posicionesBarco, 3, 3, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[4];
		posicionesBarco[0] = new Casilla(5, 6, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(6, 6, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(7, 6, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(8, 6, Constantes.POSICION_BARCO); //Para pruebas
		barcos[5] = new Barco (posicionesBarco, 4, 4, ConstantesDeBarco.ORIENTACION_VERTICAL);

		posicionesBarco = new Casilla[4];
		posicionesBarco[0] = new Casilla(5, 8, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(6, 8, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(7, 8, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(8, 8, Constantes.POSICION_BARCO); //Para pruebas
		barcos[6] = new Barco (posicionesBarco, 4, 4, ConstantesDeBarco.ORIENTACION_VERTICAL);

		posicionesBarco = new Casilla[5];
		posicionesBarco[0] = new Casilla(9, 0, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(9, 1, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(9, 2, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(9, 3, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[4] = new Casilla(9, 4, Constantes.POSICION_BARCO); //Para pruebas
		barcos[7] = new Barco (posicionesBarco, 5, 5, ConstantesDeBarco.ORIENTACION_HORIZONTAL);
	}

	private void CrearArmado3 () {
		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(0, 2, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(0, 3, Constantes.POSICION_BARCO); //Para pruebas
		barcos[0] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(0, 5, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(0, 6, Constantes.POSICION_BARCO); //Para pruebas
		barcos[1] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(6, 0, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(6, 1, Constantes.POSICION_BARCO); //Para pruebas
		barcos[2] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[3];
		posicionesBarco[0] = new Casilla(0, 0, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(1, 0, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(2, 0, Constantes.POSICION_BARCO); //Para pruebas
		barcos[3] = new Barco (posicionesBarco, 3, 3, ConstantesDeBarco.ORIENTACION_VERTICAL);

		posicionesBarco = new Casilla[3];
		posicionesBarco[0] = new Casilla(2, 7, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(2, 8, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(2, 9, Constantes.POSICION_BARCO); //Para pruebas
		barcos[4] = new Barco (posicionesBarco, 3, 3, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[4];
		posicionesBarco[0] = new Casilla(4, 3, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(4, 4, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(4, 5, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(4, 6, Constantes.POSICION_BARCO); //Para pruebas
		barcos[5] = new Barco (posicionesBarco, 4, 4, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[4];
		posicionesBarco[0] = new Casilla(6, 9, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(7, 9, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(8, 9, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(9, 9, Constantes.POSICION_BARCO); //Para pruebas
		barcos[6] = new Barco (posicionesBarco, 4, 4, ConstantesDeBarco.ORIENTACION_VERTICAL);

		posicionesBarco = new Casilla[5];
		posicionesBarco[0] = new Casilla(8, 2, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(8, 3, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(8, 4, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(8, 5, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[4] = new Casilla(8, 6, Constantes.POSICION_BARCO); //Para pruebas
		barcos[7] = new Barco (posicionesBarco, 5, 5, ConstantesDeBarco.ORIENTACION_HORIZONTAL);
	}

	private void CrearArmado4 () {
		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(7, 4, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(7, 5, Constantes.POSICION_BARCO); //Para pruebas
		barcos[0] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(9, 5, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(9, 6, Constantes.POSICION_BARCO); //Para pruebas
		barcos[1] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(0, 0, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(1, 0, Constantes.POSICION_BARCO); //Para pruebas
		barcos[2] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_VERTICAL);

		posicionesBarco = new Casilla[3];
		posicionesBarco[0] = new Casilla(3, 0, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(4, 0, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(5, 0, Constantes.POSICION_BARCO); //Para pruebas
		barcos[3] = new Barco (posicionesBarco, 3, 3, ConstantesDeBarco.ORIENTACION_VERTICAL);

		posicionesBarco = new Casilla[3];
		posicionesBarco[0] = new Casilla(7, 0, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(8, 0, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(9, 0, Constantes.POSICION_BARCO); //Para pruebas
		barcos[4] = new Barco (posicionesBarco, 3, 3, ConstantesDeBarco.ORIENTACION_VERTICAL);

		posicionesBarco = new Casilla[4];
		posicionesBarco[0] = new Casilla(3, 4, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(3, 5, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(3, 6, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(3, 7, Constantes.POSICION_BARCO); //Para pruebas
		barcos[5] = new Barco (posicionesBarco, 4, 4, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[4];
		posicionesBarco[0] = new Casilla(5, 3, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(5, 4, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(5, 5, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(5, 6, Constantes.POSICION_BARCO); //Para pruebas
		barcos[6] = new Barco (posicionesBarco, 4, 4, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[5];
		posicionesBarco[0] = new Casilla(0, 5, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(0, 6, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(0, 7, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(0, 8, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[4] = new Casilla(0, 9, Constantes.POSICION_BARCO); //Para pruebas
		barcos[7] = new Barco (posicionesBarco, 5, 5, ConstantesDeBarco.ORIENTACION_HORIZONTAL);
	}	

	private void CrearArmado5 () {
		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(7, 5, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(7, 6, Constantes.POSICION_BARCO); //Para pruebas
		barcos[0] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(9, 4, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(9, 5, Constantes.POSICION_BARCO); //Para pruebas
		barcos[1] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[2];
		posicionesBarco[0] = new Casilla(0, 1, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(1, 1, Constantes.POSICION_BARCO); //Para pruebas
		barcos[2] = new Barco (posicionesBarco, 2, 2, ConstantesDeBarco.ORIENTACION_VERTICAL);

		posicionesBarco = new Casilla[3];
		posicionesBarco[0] = new Casilla(3, 1, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(4, 1, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(5, 1, Constantes.POSICION_BARCO); //Para pruebas
		barcos[3] = new Barco (posicionesBarco, 3, 3, ConstantesDeBarco.ORIENTACION_VERTICAL);

		posicionesBarco = new Casilla[3];
		posicionesBarco[0] = new Casilla(7, 1, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(8, 1, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(9, 1, Constantes.POSICION_BARCO); //Para pruebas
		barcos[4] = new Barco (posicionesBarco, 3, 3, ConstantesDeBarco.ORIENTACION_VERTICAL);

		posicionesBarco = new Casilla[4];
		posicionesBarco[0] = new Casilla(3, 5, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(3, 6, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(3, 7, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(3, 8, Constantes.POSICION_BARCO); //Para pruebas
		barcos[5] = new Barco (posicionesBarco, 4, 4, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[4];
		posicionesBarco[0] = new Casilla(5, 4, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(5, 5, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(5, 6, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(5, 7, Constantes.POSICION_BARCO); //Para pruebas
		barcos[6] = new Barco (posicionesBarco, 4, 4, ConstantesDeBarco.ORIENTACION_HORIZONTAL);

		posicionesBarco = new Casilla[5];
		posicionesBarco[0] = new Casilla(1, 5, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[1] = new Casilla(1, 6, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[2] = new Casilla(1, 7, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[3] = new Casilla(1, 8, Constantes.POSICION_BARCO); //Para pruebas
		posicionesBarco[4] = new Casilla(1, 9, Constantes.POSICION_BARCO); //Para pruebas
		barcos[7] = new Barco (posicionesBarco, 5, 5, ConstantesDeBarco.ORIENTACION_HORIZONTAL);
	}	
}
