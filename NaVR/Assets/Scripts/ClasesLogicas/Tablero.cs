using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tablero {

	private int idJugador;
	private Casilla[,] casillas;
	private Barco[] barcos;
    [NonSerialized]
	private TestTableroController controller;

	// Use this for initialization
	public Tablero (int idJugador, Barco [] barcos) {
		this.idJugador = idJugador;
		this.InicializarTablero ();
		this.controller = null;
		if (barcos != null) {
			this.InicializarTableroPosiciones (barcos);
		}
	}

	public void InicializarTablero () {
		this.casillas = new Casilla[10, 10];
		for (int i=0; i<10; i++) {
			for (int j = 0; j < 10; j++) {
				this.casillas[i,j] = new Casilla(i, j, Constantes.NO_ATACADO);
			}
		}
	}

	public void InicializarTableroPosiciones (Barco [] barcos) {
		this.barcos = barcos;
		for(int i=0; i<barcos.Length; i++) {
			Casilla [] posicionesOcupadas = barcos [i].GetPosicionesOcupadas ();
			for (int j=0; j<posicionesOcupadas.Length; j++) {
				this.casillas [posicionesOcupadas[j].Fila, posicionesOcupadas[j].Columna].HayBarco = true;
				this.casillas [posicionesOcupadas[j].Fila, posicionesOcupadas[j].Columna].Estado = Constantes.POSICION_BARCO;
				this.casillas [posicionesOcupadas[j].Fila, posicionesOcupadas[j].Columna].Barco = barcos [i];
				posicionesOcupadas [j] = this.casillas [posicionesOcupadas [j].Fila, posicionesOcupadas [j].Columna];
			}
		}
	}

	public int GetLongitud () {
		return this.casillas.Length;
	}

	public Casilla GetCasilla (int fila, int columna) {
		return this.casillas [fila, columna];
	}

	public int GetEstadoCasilla (int fila, int columna) {
		return this.casillas [fila, columna].Estado;
	}

	public bool GetHayBarco (int fila, int columna) {
		return this.casillas [fila, columna].HayBarco;
	}

	public void SetEstadoCasilla (int fila, int columna, int estado) {
		this.casillas [fila, columna].Estado = estado;
	}

	public void SetHayBarco (int fila, int columna, bool hayBarco) {
		this.casillas [fila, columna].HayBarco = hayBarco;
	}

	public int ObtenerIdBarcoConCasilla (int fila, int columna) {
		for (int i = 0; i < barcos.Length; i++) {
			Casilla [] casillas = barcos [i].GetPosicionesOcupadas ();
			for (int j = 0; j < casillas.Length; j++) {
				if (casillas[j].Fila == fila && casillas[j].Columna == columna) {
					return i;
				}
			}
		}
		return -1;
	}

    public int CantidadDeBarcosAveriadosDelEnemigo()
    {
        int contador = 0;
        for (int fila = 0; fila < 10; fila++)
            for (int col = 0; col < 10; col++)
                if (casillas[fila, col].Estado == -2)
                    contador++;

        return contador;
    }

    public int CantidadDeBarcosVivos()
    {
        int cantidad = 0;
        foreach (Barco barco in barcos)
		{
			if(barco.Vivo)
                cantidad++;
        }
        return cantidad;
    }

    public TestTableroController Controller {
		get {
			return this.controller;
		}
		set {
			controller = value;
		}
	}

	public Barco[] Barcos {
		get {
			return this.barcos;
		}
		set {
			barcos = value;
		}
	}

}
