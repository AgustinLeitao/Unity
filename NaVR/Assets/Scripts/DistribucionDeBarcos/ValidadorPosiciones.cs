using UnityEngine;

public class ValidadorPosiciones
{
	private int[,] casillas;
	private Casilla[] posicionesOcupadas;
	private Barco barco;	

	public void InicializarTablero () {
		this.casillas = new int[10, 10];
		for (int i=0; i<10; i++) {
			for (int j = 0; j < 10; j++) {
				this.casillas [i, j] = 0;
			}
		}
	}

	public void MostrarContenidoTablero () { //Para pruebas
		/*int filaLength = 10;
		for (int i = 0; i < filaLength; i++) {
			Debug.Log (this.casillas [i, 0] + " " + this.casillas [i, 1] + " " + this.casillas [i, 2] +
				" " + this.casillas [i, 3] + " " + this.casillas [i, 4] + " " +
				this.casillas [i, 5] + " " + this.casillas [i, 6] + " " +
				this.casillas [i, 7] + " " + this.casillas [i, 8] + " " + this.casillas [i, 9]);
		}*/
	}

	public Barco ValidarSiEsPosibleUbicarBarco (RaycastHit hit, int tipoBarco, int orientacion, int tipoHabilidades) {
		int filaInicial = (int)hit.point.x;
		int columnaInicial = (int)hit.point.z;
		posicionesOcupadas = new Casilla[tipoBarco];
		 
		if (orientacion == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
			int columnaCambiante = columnaInicial;
			for (int i=0; i < tipoBarco; i++) {
				if (columnaCambiante > 9 || columnaCambiante < 0 || filaInicial > 9 || filaInicial < 0 || this.casillas[filaInicial, columnaCambiante] != 0) {
					return null;
				}             
				posicionesOcupadas [i] = new Casilla(filaInicial, columnaCambiante, 0);
				columnaCambiante += 1;
			}
		}

		if (orientacion == ConstantesDeBarco.ORIENTACION_VERTICAL) {
			int filaCambiante = filaInicial;
			for (int i=0; i < tipoBarco; i++) {
				if (filaCambiante > 9 || filaCambiante < 0 || columnaInicial > 9 || columnaInicial < 0 || this.casillas [filaCambiante, columnaInicial] != 0) {
					return null;
				}
				posicionesOcupadas [i] = new Casilla(filaCambiante, columnaInicial, 0);
				filaCambiante += 1;
			}
		}
		barco = new Barco(posicionesOcupadas, tipoBarco, tipoHabilidades, orientacion);
		return barco;
	}

	public void ActualizarTablero (RaycastHit hit, int tipoBarco, int orientacion) {
		int filaInicial = (int)hit.point.x;
		int columnaInicial = (int)hit.point.z;
		int extremo = 0;
		if (orientacion == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
			int columnaCambiante = columnaInicial;
			for (int i=0; i < tipoBarco; i++) {
				this.casillas [filaInicial, columnaCambiante] = 1;
				extremo = EsExtremo (filaInicial, columnaCambiante, orientacion, i, tipoBarco);
				if (extremo != 1) {
					if (i == 0) {
						AgregarIniciales (filaInicial, columnaCambiante, orientacion);	
					} else {
						if (i == (tipoBarco - 1)) {
							AgregarFinales (filaInicial, columnaCambiante, orientacion);
						} else {
							AgregarIntermedios (filaInicial, columnaCambiante, orientacion);
						}
					}
				}
				columnaCambiante += 1;
			}
		}
		if (orientacion == ConstantesDeBarco.ORIENTACION_VERTICAL) {
			int filaCambiante = filaInicial;
			for (int i=0; i < tipoBarco; i++) {
				this.casillas [filaCambiante, columnaInicial] = 1;
				extremo = EsExtremo (filaCambiante, columnaInicial, orientacion, i, tipoBarco);
				if (extremo != 1) {
					if (i == 0) {
						AgregarIniciales (filaCambiante, columnaInicial, orientacion);	
					} else {
						if (i == (tipoBarco - 1)) {
							AgregarFinales (filaCambiante, columnaInicial, orientacion);
						} else {
							AgregarIntermedios (filaCambiante, columnaInicial, orientacion);
						}
					}
				}
				filaCambiante += 1;
			}
		}
	}

	public int EsExtremo (int fila, int columna, int orientacion, int posicion, int barco) {
		if (fila == 0 && orientacion == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
			if (posicion == 0 && columna != 0) {
				this.casillas [fila, columna - 1] = 2;
				this.casillas [fila + 1, columna - 1] = 2;
			}
			if ((posicion == (barco-1)) && columna != 9) {
				this.casillas [fila, columna + 1] = 2;
				this.casillas [fila + 1, columna + 1] = 2;
			}
			this.casillas [fila + 1, columna] = 2;
			return 1;
		}
		if (fila == 9 && orientacion == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
			if (posicion == 0 && columna != 0) {
				this.casillas [fila, columna - 1] = 2;
				this.casillas [fila - 1, columna - 1] = 2;
			}
			if ((posicion == (barco-1)) && columna != 9) {
				this.casillas [fila, columna + 1] = 2;
				this.casillas [fila - 1, columna + 1] = 2;
			}
			this.casillas [fila - 1, columna] = 2;
			return 1;
		}
		if (columna == 0 && orientacion == ConstantesDeBarco.ORIENTACION_VERTICAL) {
			if (posicion == 0 && fila != 0) {
				this.casillas [fila - 1, columna] = 2;
				this.casillas [fila - 1, columna + 1] = 2;
			}
			if ((posicion == (barco-1)) && fila != 9) {
				this.casillas [fila + 1, columna] = 2;
				this.casillas [fila + 1, columna + 1] = 2;
			}
			this.casillas [fila, columna + 1] = 2;
			return 1;
		}
		if (columna == 9 && orientacion == ConstantesDeBarco.ORIENTACION_VERTICAL) {
			if (posicion == 0 && fila != 0) {
				this.casillas [fila - 1, columna] = 2;
				this.casillas [fila - 1, columna - 1] = 2;
			}
			if ((posicion == (barco-1)) && fila != 9) {
				this.casillas [fila + 1, columna] = 2;
				this.casillas [fila + 1, columna - 1] = 2;
			}
			this.casillas [fila, columna - 1] = 2;
			return 1;
		}
		return 0;
	}

	public void AgregarIniciales (int fila, int columna, int orientacion) {
		if (orientacion == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
			if (columna != 0) {
				this.casillas [fila - 1, columna - 1] = 2;
				this.casillas [fila, columna - 1] = 2;
				this.casillas [fila + 1, columna - 1] = 2;
			}
			this.casillas [fila - 1, columna] = 2;
			this.casillas [fila + 1, columna] = 2;
		}
		if (orientacion == ConstantesDeBarco.ORIENTACION_VERTICAL) {
			if (fila != 0) {
				this.casillas [fila - 1, columna - 1] = 2;
				this.casillas [fila - 1, columna] = 2;
				this.casillas [fila - 1, columna + 1] = 2;
			}
			this.casillas [fila, columna - 1] = 2;
			this.casillas [fila, columna + 1] = 2;
		}
	}

	public void AgregarFinales (int fila, int columna, int orientacion) {
		if (orientacion == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
			if (columna != 9) {
				this.casillas [fila - 1, columna + 1] = 2;
				this.casillas [fila, columna + 1] = 2;
				this.casillas [fila + 1, columna + 1] = 2;
			}
			this.casillas [fila - 1, columna] = 2;
			this.casillas [fila + 1, columna] = 2;
		}
		if (orientacion == ConstantesDeBarco.ORIENTACION_VERTICAL) {
			if (fila != 9) {
				this.casillas [fila + 1, columna - 1] = 2;
				this.casillas [fila + 1, columna] = 2;
				this.casillas [fila + 1, columna + 1] = 2;
			}
			this.casillas [fila, columna - 1] = 2;
			this.casillas [fila, columna + 1] = 2;
		}
	}

	public void AgregarIntermedios (int fila, int columna, int orientacion) {
		if (orientacion == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
			this.casillas [fila - 1, columna] = 2;
			this.casillas [fila + 1, columna] = 2;
		}
		if (orientacion == ConstantesDeBarco.ORIENTACION_VERTICAL) {
			this.casillas [fila, columna - 1] = 2;
			this.casillas [fila, columna + 1] = 2;
		}
	}

	public void AgregarInicialesExtremo (int fila, int columna, int orientacion) {
		if (fila == 0 && orientacion == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
			this.casillas [fila, columna + 1] = 2;
			this.casillas [fila + 1, columna + 1] = 2;
		}
		if (fila == 9 && orientacion == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
			this.casillas [fila - 1, columna + 1] = 2;
			this.casillas [fila, columna + 1] = 2;
		}
		if (columna == 0 && orientacion == ConstantesDeBarco.ORIENTACION_VERTICAL) {
			this.casillas [fila + 1, columna] = 2;
			this.casillas [fila + 1, columna + 1] = 2;
		}
		if (columna == 9 && orientacion == ConstantesDeBarco.ORIENTACION_VERTICAL) {
			this.casillas [fila + 1, columna - 1] = 2;
			this.casillas [fila + 1, columna] = 2;
		}	
	}
}
