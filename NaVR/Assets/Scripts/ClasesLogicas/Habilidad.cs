using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constantes {
	public const int NO_ATACADO = 0;
	public const int ATACADO_AGUA = -1;
	public const int ATACADO_BARCO = -2;
	public const int REVELADO = -3;
	public const int REVELADO_CONTRAMEDIDAS = -4;
	public const int REVELADO_REFORZADA = -5;

	public const int POSICION_AGUA = 0;
	public const int POSICION_BARCO = 1;
	public const int POSICION_BARCOFANTASMA = 2;

	public const int ATAQUE_NORMAL_ID = 0;
	public const int ARTILLERIA_RAPIDA_ID = 1;
	public const int PROYECTIL_HE_ID = 2;
	public const int TORPEDO_ID = 3;
	public const int DISPARO_DOBLE_ID = 4;
	public const int REFORZAR_ARMADURA_ID = 5;
	public const int TORMENTA_MISILES_ID = 6;
	public const int PULSO_ELECTROMAGNETICO_ID = 7;
	public const int CONTRAMEDIDAS_ID = 8; //NO ENTRA PARA LA EXPO
	public const int RADAR_PASIVO_ID = 9; //NO ENTRA PARA LA EXPO
	public const int SABOTAJE_ID = 10;

	public const int IMPACTO_AGUA = 0;
	public const int IMPACTO_BARCO = 1;
	//public const int IMPACTO_TRAMPA_SABOTAJE = 0;
	//public const int IMPACTO_TRAMPA_RADARPASIVO = 0;
	public const int IMPACTO_REFORZADA = 2;
	public const int IMPACTO_CONTRAMEDIDAS = 3;


	//public const int POSICION_REFORZADA = 2;
	//public const int POSICION_ATACADA_AGUA = 3;
	//public const int POSICION_ATACADA_BARCO = 4;
	//public const int POSICION_DESTRUIDA = 5;
	//public const int POSICION_REFORZADA_ATACADA = 6;
	//public const int POSICION_CONTRAMEDIDAS = 7;
	//public const int POSICION_REFORZADA_CONTRAMEDIDAS = 8;
	//public const int POSICION_REFORZADA_ATACADA_CONTRAMEDIDAS = 9;
	//public const int POSICION_TRAMPA_RADARPASIVO = 10;
	//public const int POSICION_TRAMPA_SABOTAJE = 11;

}

// Esta clase esta aca nomas para poder hacer un array con varios tipos de habilidad
[System.Serializable]
public class Habilidad {

    protected string name;
    protected int maxCooldown; 

    [System.NonSerialized]
	protected AdministradorPartida Administrador;

	protected int id;
	protected bool esTrampa = false;

	public Casilla [] listaCasillas;

	//public abstract void activar ();
	//public abstract void ejecutarAnimacion ();

	public string Name {
		get {
			return this.name;
		}
	}

	public int MaxCooldown {
		get {
			return this.maxCooldown;
		}
	}

	public int Id {
		get {
			return this.id;
		}
	}

	public bool EsTrampa {
		get {
			return this.esTrampa;
		}
	}

}

/*
public interface IHabilidad
{
	void activar(Tablero tableroAtaque, Tablero tableroPosicion, Casilla casilla);
	void activar(Tablero tableroAtaque, Tablero tableroPosicion, Casilla casilla1, Casilla casilla2);
	void activar(Tablero tableroAtaque, Tablero tableroPosicion, Casilla casilla1, Casilla casilla2, Casilla casilla3);
	void ejecutarAnimacion ();

}
*/

[System.Serializable]
public class Habilidad_Sabotaje : Habilidad {

	public Habilidad_Sabotaje(AdministradorPartida admin){
		maxCooldown = 5;
		Administrador = admin;
		id = Constantes.SABOTAJE_ID;
		name = "Sabotaje";
		esTrampa = true;
	}

	// Para darle mas utilidad a esta habilidad, al utilizarla no se pierde el turno (solo se puede atacar)
	public bool activar(Jugador jugador, Casilla casilla1, Casilla casilla2, Casilla casilla3, bool esJugador2 = true){
		Administrador.añadirEnfriamiento (this);
		Tablero tableroPosicion = jugador.TableroPosicion;
		Administrador.LimpiarCasillasTrampa(jugador, false);
		tableroPosicion.GetCasilla (casilla1.Fila, casilla1.Columna).Trampa = 2;
		tableroPosicion.GetCasilla (casilla2.Fila, casilla2.Columna).Trampa = 2;
		tableroPosicion.GetCasilla (casilla3.Fila, casilla3.Columna).Trampa = 2;
		Administrador.RefrescarCasilla (jugador.TableroPosicion, casilla1.Fila, casilla1.Columna);
		Administrador.RefrescarCasilla (jugador.TableroPosicion, casilla2.Fila, casilla2.Columna);
		Administrador.RefrescarCasilla (jugador.TableroPosicion, casilla3.Fila, casilla3.Columna);
		Casilla[] casillas = new Casilla[3];
		casillas[0] = casilla1;
		casillas[1] = casilla2;
		casillas[2] = casilla3;
		jugador.CasillasTrampa = casillas;
		jugador.TipoTrampa = 2;

		// ENVIO DE MENSAJE
		if (!esJugador2) {			
			Casilla [] lista = new Casilla [3];
			lista[0] = new Casilla(casilla1.Fila, casilla1.Columna);
			lista[1] = new Casilla(casilla2.Fila, casilla2.Columna);
			lista[2] = new Casilla(casilla3.Fila, casilla3.Columna);
			Administrador.EnviarCasillasAtacadas(lista,Constantes.SABOTAJE_ID);
		}

		Administrador.DesactivarHabilidades (!esJugador2);
		ejecutarAnimacion ();
		return true;
	}

	public void ejecutarAnimacion (){
		// Reproduce sonido
		// Informa por pantalla que se coloco la trampa
	}

}

[System.Serializable]
public class Habilidad_RadarPasivo : Habilidad  {

	public Habilidad_RadarPasivo(AdministradorPartida admin){
		maxCooldown = 5;
		Administrador = admin;
		id = Constantes.RADAR_PASIVO_ID;
		name = "Radar Pasivo";
		esTrampa = true;
	}

	// Para darle mas utilidad a esta habilidad, al utilizarla no se pierde el turno (solo se puede atacar)
	public bool activar(Jugador jugador, Casilla casilla1, Casilla casilla2, Casilla casilla3){
		Administrador.añadirEnfriamiento (this);
		Tablero tableroPosicion = jugador.TableroPosicion;
		//Administrador.ColocarTrampa (0, casilla1, casilla2, casilla3);
		Administrador.LimpiarCasillasTrampa(jugador, false);
		tableroPosicion.GetCasilla (casilla1.Fila, casilla1.Columna).Trampa = 1;
		tableroPosicion.GetCasilla (casilla2.Fila, casilla2.Columna).Trampa = 1;
		tableroPosicion.GetCasilla (casilla3.Fila, casilla3.Columna).Trampa = 1;
		Administrador.RefrescarCasilla (jugador.TableroPosicion, casilla1.Fila, casilla1.Columna);
		Administrador.RefrescarCasilla (jugador.TableroPosicion, casilla2.Fila, casilla2.Columna);
		Administrador.RefrescarCasilla (jugador.TableroPosicion, casilla3.Fila, casilla3.Columna);
		Casilla[] casillas = new Casilla[3];
		casillas[0] = casilla1;
		casillas[1] = casilla2;
		casillas[2] = casilla3;
		jugador.CasillasTrampa = casillas;
		jugador.TipoTrampa = 1;

		Administrador.DesactivarHabilidades (true);
		ejecutarAnimacion ();
		return true;
	}

	public void ejecutarAnimacion (){
		// Reproduce sonido
		// Informa por pantalla que se coloco la trampa
	}

}

// Esto deberia ser para el barco que la lanza, pero como por ahora las habilidades son globales, se puede seleccionar el barco que la activa.
// De ultima podriamos dejarlo asi, que se pueda lanzar sobre cualquier barco.
[System.Serializable]
public class Habilidad_Contramedidas : Habilidad  {

	private int duracion;

	public Habilidad_Contramedidas(AdministradorPartida admin){
		maxCooldown = 5;
		duracion = 4; // Son 3 turnos reales, +1 porque cambia en el turno del enemigo
		Administrador = admin;
		id = Constantes.CONTRAMEDIDAS_ID;
		name = "Contramedidas";
	}

	// Para darle mas utilidad a esta habilidad, al utilizarla no se pierde el turno (solo se puede atacar)
	public bool activar(Tablero tableroPosicion, Casilla casilla1, Jugador jugador){
		if (tableroPosicion.GetHayBarco (casilla1.Fila, casilla1.Columna) && tableroPosicion.GetEstadoCasilla(casilla1.Fila, casilla1.Columna) == Constantes.POSICION_BARCO) {
			Barco barco = casilla1.Barco;
			//Administrador.activarContramedidas (barco, duracion);
			Casilla[] casillas = barco.GetPosicionesOcupadas ();
			for (int i = 0; i < casillas.Length; i++) {
				if (casillas [i].Estado == Constantes.POSICION_BARCO) {
					jugador.TableroPosicion.GetCasilla (casillas [i].Fila, casillas [i].Columna).Contramedidas = true;
					//jugador.ControllerPosicion.placeMark (casillas [i].Fila, casillas [i].Columna, new Color (1, 0.5f, 0));
					Administrador.RefrescarCasilla (jugador.TableroPosicion, casillas [i].Fila, casillas [i].Columna);
				}
			}
			jugador.TurnosContramedidas = duracion;
			jugador.BarcoContramedidas = barco;

			Administrador.añadirEnfriamiento (this);
			Administrador.DesactivarHabilidades (true);
			ejecutarAnimacion ();
			return true;
		}
		return false;
	}

	public void ejecutarAnimacion (){

	}

}

[System.Serializable]
public class Habilidad_PulsoElectromagnetico : Habilidad  {

	private int duracion;

	public Habilidad_PulsoElectromagnetico(AdministradorPartida admin){
		maxCooldown = 5;
		duracion = 3;
		Administrador = admin;
		id = Constantes.PULSO_ELECTROMAGNETICO_ID;
		name = "Pulso EM";
	}

	// No consume turno
	public bool activar(Jugador jugador, bool esJugador2 = true){
		// Administrador.Silenciar (duracion);
		jugador.TurnosSilenciado = jugador.TurnosSilenciado + duracion;
		Administrador.añadirEnfriamiento (this);
		Administrador.DesactivarHabilidades (!esJugador2);
		// ENVIO DE MENSAJE
		if (!esJugador2) {			
			Administrador.EnviarCasillasAtacadas(null,Constantes.PULSO_ELECTROMAGNETICO_ID);
		}
		ejecutarAnimacion ();
		return true;
	}

	public void ejecutarAnimacion (){
		// Reproduce sonido
		// Hace una onda expansiva o alguna boludes asi
	}
}

[System.Serializable]
public class Habilidad_TormentaDeMisiles : Habilidad  {

	public Habilidad_TormentaDeMisiles(AdministradorPartida admin){
		maxCooldown = -1;
		Administrador = admin;
		id = Constantes.TORMENTA_MISILES_ID;
		name = "Tormenta de Misiles";
	}

	// Habilidad de un solo uso. Para aumentar la utilidad, la habilidad golpea 10 posiciones que no hayan sido atacadas todavia.
	public bool activar(Tablero tableroAtaque, Tablero tableroPosicion, int barco = -1, bool esJugador2 = true){
		//Casilla[] listaCasillas = new Casilla[10]();
		//int numCasilla = 0;
		int[] area = new int[100];
		int random1; 
		int random2; 
		//Casilla[] listaCasillas = new Casilla[10];
		listaCasillas = new Casilla[10];
		int aux = 0;
		for (int i = 0; i < 10; i++) {
			random1 = Random.Range (0, 10);
			random2 = Random.Range (0, 10);

			// Compruebo que no se repitan las casillas y que no caiga en agua o atacado
			while ((area [(random1)+((random2)*10)] == 1) || tableroAtaque.GetEstadoCasilla(random1,random2) == Constantes.ATACADO_AGUA || tableroAtaque.GetEstadoCasilla(random1,random2) == Constantes.ATACADO_BARCO) {
				random1 = Random.Range (0, 10);
				random2 = Random.Range (0, 10);
			}
			area [(random1)+((random2)*10)] = 1;

			// Compruebo que ninguna de las casillas sea una trampa de sabotaje
			if (tableroPosicion.GetCasilla (random1, random2).Trampa == 2) {
				// CODIGO SABOTAJE
				if (esJugador2){
					Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",null, false);
				} else {
					Administrador.ImpactoSabotaje("¡Has impactado en la trampa sabotaje enemiga!",null, true, Constantes.TORMENTA_MISILES_ID);
				}
				return true;
			}
			// Guardo la posicion en un vector para atacarla luego
			listaCasillas [aux] = new Casilla (random1, random2, 0);
			aux++;
		}

		// Ataco las posiciones
		for (int i = 0; i < aux; i++) {
			if(esJugador2)
				Administrador.AtacarUnaPosicion(tableroAtaque, tableroPosicion, listaCasillas [i], true, false);
			else
				Administrador.AtacarUnaPosicion(tableroAtaque, tableroPosicion, listaCasillas [i], true, true);
		}

		if (!esJugador2){

			Administrador.EnviarCasillasAtacadas(listaCasillas, Constantes.TORMENTA_MISILES_ID);

		}

		Administrador.DeboCambiarTurno = true;
		ejecutarAnimacion (barco, listaCasillas);

		return true;
	}

	public bool activar(Tablero tableroAtaque, Tablero tableroPosicion, int barco, bool esJugador2, Casilla [] casillas) {
		if (casillas == null) {
			// CODIGO SABOTAJE
			Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",null, false);
			return true;
		}

		// Ataco las posiciones
		for (int i = 0; i < 10; i++) {
			Administrador.AtacarUnaPosicion (tableroAtaque, tableroPosicion, casillas [i], true, false);
		}

		Administrador.DeboCambiarTurno = true;
		ejecutarAnimacion (barco, casillas);

		return true;
	}

	public void ejecutarAnimacion (int barco, Casilla[] listaCasilla){
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[0], "TormentaMisiles",-0.5f,0.2f);
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[1], "TormentaMisiles",-0.4f,0.2f);
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[2], "TormentaMisiles",-0.3f,0.2f);
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[3], "TormentaMisiles",-0.2f,0.2f);
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[4], "TormentaMisiles",-0.1f,0.2f);
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[5], "TormentaMisiles",0,-0.2f);
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[6], "TormentaMisiles",0.1f,-0.2f);
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[7], "TormentaMisiles",0.2f,-0.2f);
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[8], "TormentaMisiles",0.3f,-0.2f);
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[9], "TormentaMisiles",0.4f,-0.2f);
	}
}

[System.Serializable]
public class Habilidad_ReforzarArmadura : Habilidad  {

	public Habilidad_ReforzarArmadura(AdministradorPartida admin){
		maxCooldown = 5;
		Administrador = admin;
		id = Constantes.REFORZAR_ARMADURA_ID;
		name = "Reforzar Armadura";
	}

	// Para darle mas utilidad a esta habilidad, al utilizarla no se pierde el turno (solo se puede atacar)
	public bool activar(Tablero tableroPosicion, Casilla casilla1, Jugador jugador, bool esJugador2 = true){
		if (tableroPosicion.GetHayBarco (casilla1.Fila, casilla1.Columna) && tableroPosicion.GetCasilla(casilla1.Fila, casilla1.Columna).Reforzada == 0 && tableroPosicion.GetCasilla(casilla1.Fila, casilla1.Columna).Contramedidas == false && tableroPosicion.GetCasilla(casilla1.Fila, casilla1.Columna).Atacada == false) {
			//Administrador.reforzarCasilla (tableroPosicion, casilla1);
			tableroPosicion.GetCasilla (casilla1.Fila, casilla1.Columna).Reforzada = 1;
			//jugador.ControllerPosicion.placeMark (casilla1.Fila, casilla1.Columna,new Color(1,1,0));
			Administrador.RefrescarCasilla (jugador.TableroPosicion, casilla1.Fila, casilla1.Columna);
			Administrador.añadirEnfriamiento (this);
			Administrador.DesactivarHabilidades (!esJugador2);
			// ENVIO DE MENSAJE
			if (!esJugador2) {			
				Casilla [] lista = new Casilla [1];
				lista[0] = new Casilla(casilla1.Fila, casilla1.Columna);
				Administrador.EnviarCasillasAtacadas(lista,Constantes.REFORZAR_ARMADURA_ID);
			}
			ejecutarAnimacion ();
			return true;
		}
		return false;
	}

	public void ejecutarAnimacion (){

	}

}

[System.Serializable]
public class Habilidad_ReconocimientoSatelital : Habilidad  {

	public Habilidad_ReconocimientoSatelital(AdministradorPartida admin){
		maxCooldown = 5;
		Administrador = admin;
		name = "Reconocimiento Satelital";
	}

	// Para darle mas utilidad a esta habilidad, al utilizarla no se pierde el turno (solo se puede atacar)
	public bool activar(Tablero tableroAtaque, Tablero tableroPosicion, Casilla casilla1, Casilla casilla2, Casilla casilla3){
		if (tableroPosicion.GetHayBarco (casilla1.Fila, casilla1.Columna) || tableroPosicion.GetHayBarco (casilla2.Fila, casilla2.Columna) || tableroPosicion.GetHayBarco (casilla3.Fila, casilla3.Columna)) {
			Administrador.marcarPosibleBarco (tableroAtaque, casilla1);
			Administrador.marcarPosibleBarco (tableroAtaque, casilla1);
			Administrador.marcarPosibleBarco (tableroAtaque, casilla1);
		} else {
			Administrador.AtacarUnaPosicion (tableroAtaque, tableroPosicion, casilla1, true);
			Administrador.AtacarUnaPosicion (tableroAtaque, tableroPosicion, casilla2, true);
			Administrador.AtacarUnaPosicion (tableroAtaque, tableroPosicion, casilla3, true);
		}
		Administrador.añadirEnfriamiento (this);
		Administrador.DesactivarHabilidades (true);
		ejecutarAnimacion ();
		return true;
	}

	public void ejecutarAnimacion (){

	}

}

[System.Serializable]
public class Habilidad_DisparoDoble : Habilidad  {

	public Habilidad_DisparoDoble(AdministradorPartida admin){
		maxCooldown = 5;
		Administrador = admin;
		id = Constantes.DISPARO_DOBLE_ID;
		name = "Disparo Doble";
    }

	// Para darle mas utilidad a esta habilidad, si alguno de los dos disparos acierta en un barco, no se pierde el turno (solo se puede atacar)
	public bool activar(Tablero tableroAtaque, Tablero tableroPosicion, Casilla casilla1, Casilla casilla2, int barco = -1, bool esJugador2 = true){
		if ((casilla1 == null || casilla2 == null) && esJugador2){
			// CODIGO SABOTAJE ENEMIGO
			Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",this, false);
			Administrador.RefrescarCasilla (tableroAtaque, casilla1.Fila, casilla1.Columna);
			Administrador.RefrescarCasilla (tableroAtaque, casilla2.Fila, casilla2.Columna);
			return true;
		}

		// Compruebo que ninguna de las dos posiciones atacadas sea una trampa sabotaje
		if (tableroPosicion.GetCasilla (casilla1.Fila, casilla1.Columna).Trampa == 2 || tableroPosicion.GetCasilla (casilla2.Fila, casilla2.Columna).Trampa == 2) {
			// CODIGO SABOTAJE
			if (esJugador2){
				Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",this, false);
			} else {
				Administrador.ImpactoSabotaje("¡Has impactado en la trampa sabotaje enemiga!",this, true, Constantes.DISPARO_DOBLE_ID);
			}
			Administrador.RefrescarCasilla (tableroAtaque, casilla1.Fila, casilla1.Columna);
			Administrador.RefrescarCasilla (tableroAtaque, casilla2.Fila, casilla2.Columna);
			return true;
		}
        int aux1, aux2;
        if (esJugador2 == true) 
		{
            aux1 = Administrador.AtacarUnaPosicion(tableroAtaque, tableroPosicion, casilla1, false, false);
            aux2 = Administrador.AtacarUnaPosicion(tableroAtaque, tableroPosicion, casilla2, false, false);
		} else 
		{
            aux1 = Administrador.AtacarUnaPosicion(tableroAtaque, tableroPosicion, casilla1, true, true);
            aux2 = Administrador.AtacarUnaPosicion(tableroAtaque, tableroPosicion, casilla2, true, true);
		}


        // FALTA LA VERSION DE LA IA
		/*
		if (!esIA) {
			if (barco != -1) {
				ejecutarAnimacion (barco, casilla1, casilla2, esIA);
				Administrador.DeboCambiarTurno = true;
			} 
		}else{
			Administrador.CambiarTurno();
		}
		*/



		if ((aux1 == Constantes.IMPACTO_BARCO || aux2 == Constantes.IMPACTO_BARCO) && aux1 != Constantes.IMPACTO_REFORZADA && aux2 != Constantes.IMPACTO_REFORZADA) {
			Administrador.añadirEnfriamiento (this);
			Administrador.DesactivarHabilidades (false);
		} else {
			Administrador.añadirEnfriamiento (this);
			Administrador.DeboCambiarTurno = true;
			//Administrador.CambiarTurno ();
		}

		// ENVIO DE MENSAJE
		if (!esJugador2) {			
			Casilla [] lista = new Casilla [2];
			lista[0] = new Casilla(casilla1.Fila, casilla1.Columna);
			lista[1] = new Casilla(casilla2.Fila, casilla2.Columna);
			Administrador.EnviarCasillasAtacadas(lista,Constantes.DISPARO_DOBLE_ID);
		}

		ejecutarAnimacion (barco, casilla1, casilla2);

		return true;
	}

	public void ejecutarAnimacion (int barco, Casilla casilla1, Casilla casilla2){
		Administrador.IniciarAnimacionDisparo (barco, casilla1, "CannonDoble", -0.4f, 0f);
		Administrador.IniciarAnimacionDisparo (barco, casilla2, "CannonDoble", 0.3f, 0f);
	}

}

[System.Serializable]
public class Habilidad_Torpedo : Habilidad  {

	public Habilidad_Torpedo(AdministradorPartida admin){
		maxCooldown = 5;
		Administrador = admin;
		id = Constantes.TORPEDO_ID;
		name = "Torpedo";
	}

	public bool activar(Tablero tableroAtaque, Tablero tableroPosicion, Casilla casilla, int barco = -1, bool esJugador2 = true){
		if (casilla == null && esJugador2){
			// CODIGO SABOTAJE ENEMIGO
			Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",this, false);
			return true;
		}

		Casilla casillaAtaque = new Casilla(0, 0, 1);
		int posicionFila = 0;
		//Casilla[] listaCasillas = new Casilla[10];
		listaCasillas = new Casilla[10];
		int aux = 0;
		casillaAtaque.Fila = posicionFila;
		casillaAtaque.Columna = casilla.Columna;



		// Compruebo que ninguna de las casillas sea una trampa de sabotaje
		if (tableroPosicion.GetCasilla (casillaAtaque.Fila, casillaAtaque.Columna).Trampa == 2) {
			// CODIGO SABOTAJE
			if (esJugador2){
				Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",this, false);
			} else {
				Administrador.ImpactoSabotaje("¡Has impactado en la trampa sabotaje enemiga!",this, true, Constantes.TORPEDO_ID);
			}
			return true;
		}
		listaCasillas [aux] = new Casilla (casillaAtaque.Fila, casillaAtaque.Columna, 0);
		aux++;

		int aux2 = (tableroPosicion.GetCasilla (casillaAtaque.Fila, casillaAtaque.Columna).Estado);
		while (posicionFila < 9 && (aux2 == Constantes.NO_ATACADO || aux2 == Constantes.ATACADO_AGUA )) {
			posicionFila++;
			casillaAtaque.Fila = posicionFila;

			// Compruebo que ninguna de las casillas sea una trampa de sabotaje
			if (tableroPosicion.GetCasilla (casillaAtaque.Fila, casillaAtaque.Columna).Trampa == 2) {
				// CODIGO SABOTAJE
				if (esJugador2){
					Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",this, false);
				} else {
					Administrador.ImpactoSabotaje("¡Has impactado en la trampa sabotaje enemiga!",this, true, Constantes.TORPEDO_ID);
				}
				return true;
			}
			listaCasillas [aux] = new Casilla (casillaAtaque.Fila, casillaAtaque.Columna, 0);
			aux++;

			aux2 = (tableroPosicion.GetCasilla (casillaAtaque.Fila, casillaAtaque.Columna).Estado);
		}
		Casilla casillaHit = new Casilla (casillaAtaque.Fila, casillaAtaque.Columna);

		posicionFila++;
		if (posicionFila < 10) {
			casillaAtaque.Fila = posicionFila;

			// Compruebo que ninguna de las casillas sea una trampa de sabotaje
			if (tableroPosicion.GetCasilla (casillaAtaque.Fila, casillaAtaque.Columna).Trampa == 2) {
				// CODIGO SABOTAJE
				if (esJugador2){
					Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",this, false);
				} else {
					Administrador.ImpactoSabotaje("¡Has impactado en la trampa sabotaje enemiga!",this, true, Constantes.TORPEDO_ID);
				}
				return true;
			}
			listaCasillas [aux] = new Casilla (casillaAtaque.Fila, casillaAtaque.Columna, 0);
			aux++;
		}

		// Ataco las posiciones
		for (int i = 0; i < aux; i++) {
			if(esJugador2 == true)
				Administrador.AtacarUnaPosicion(tableroAtaque, tableroPosicion, listaCasillas [i], false, false);
			else
				Administrador.AtacarUnaPosicion(tableroAtaque, tableroPosicion, listaCasillas[i], true, true);
		}

		// ENVIO DE MENSAJE
		if (!esJugador2) {			
			Casilla [] lista = new Casilla [1];
			lista[0] = new Casilla(casilla.Fila, casilla.Columna);
			Administrador.EnviarCasillasAtacadas(lista,Constantes.TORPEDO_ID);
		}

		/*
		if (!esIA) {
			if (barco != -1) {
				ejecutarAnimacion (barco,casillaHit,esIA);
				Administrador.DeboCambiarTurno = true;
			} 
		}else{
			Administrador.CambiarTurno();
		}
		*/

		Administrador.DeboCambiarTurno = true;

		Administrador.añadirEnfriamiento (this);

		ejecutarAnimacion (barco,casillaHit);
		return true;
	}

	public void ejecutarAnimacion (int barco, Casilla casilla){
		Administrador.IniciarAnimacionDisparo (barco, casilla, "CannonTorpedo");
	}

}

[System.Serializable]
public class Habilidad_ProyectilHE : Habilidad  {

	public Habilidad_ProyectilHE(AdministradorPartida admin){
		maxCooldown = 5;
		Administrador = admin;
		id = Constantes.PROYECTIL_HE_ID;
		name = "Proyectil HE"; // High Explosive, para acortar
	}

	public bool activar(Tablero tableroAtaque, Tablero tableroPosicion, Casilla casilla, int barco = -1, bool esJugador2 = true){
		int offsetx = 0;
		int offsety = 0;

		if (casilla == null && esJugador2){
			// CODIGO SABOTAJE ENEMIGO
			Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",this, false);
			return true;
		}

		//Casilla[] listaCasillas = new Casilla[5];
		listaCasillas = new Casilla[5];
		int aux = 0;

		// Compruebo que la posicion no contenga una trampa Sabotaje
		if (tableroPosicion.GetCasilla (casilla.Fila+offsetx, casilla.Columna+offsety).Trampa == 2 ){
			// CODIGO SABOTAJE
			if (esJugador2){
				Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",this, false);
			} else {
				Administrador.ImpactoSabotaje("¡Has impactado en la trampa sabotaje enemiga!",this, true, Constantes.PROYECTIL_HE_ID);
			}
			return true;
		}
		listaCasillas [aux] = new Casilla (casilla.Fila+offsetx, casilla.Columna+offsety, 0);
		aux++;

		if (casilla.Fila != 0) {
			offsetx = -1;
			offsety = 0;
			// Compruebo que la posicion no contenga una trampa Sabotaje
			if (tableroPosicion.GetCasilla (casilla.Fila+offsetx, casilla.Columna+offsety).Trampa == 2 ){
				// CODIGO SABOTAJE
				if (esJugador2){
					Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",this, false);
				} else {
					Administrador.ImpactoSabotaje("¡Has impactado en la trampa sabotaje enemiga!",this, true, Constantes.PROYECTIL_HE_ID);
				}
				return true;
			}
			listaCasillas [aux] = new Casilla (casilla.Fila+offsetx, casilla.Columna+offsety, 0);
			aux++;
		}
		if (casilla.Fila != 9) {
			offsetx = 1;
			offsety = 0;
			// Compruebo que la posicion no contenga una trampa Sabotaje
			if (tableroPosicion.GetCasilla (casilla.Fila+offsetx, casilla.Columna+offsety).Trampa == 2 ){
				// CODIGO SABOTAJE
				if (esJugador2){
					Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",this, false);
				} else {
					Administrador.ImpactoSabotaje("¡Has impactado en la trampa sabotaje enemiga!",this, true, Constantes.PROYECTIL_HE_ID);
				}
				return true;
			}
			listaCasillas [aux] = new Casilla (casilla.Fila+offsetx, casilla.Columna+offsety, 0);
			aux++;
		}
		if (casilla.Columna != 0) {
			offsetx = 0;
			offsety = -1;	
			// Compruebo que la posicion no contenga una trampa Sabotaje
			if (tableroPosicion.GetCasilla (casilla.Fila+offsetx, casilla.Columna+offsety).Trampa == 2 ){
				// CODIGO SABOTAJE
				if (esJugador2){
					Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",this, false);
				} else {
					Administrador.ImpactoSabotaje("¡Has impactado en la trampa sabotaje enemiga!",this, true, Constantes.PROYECTIL_HE_ID);
				}
				return true;
			}
			listaCasillas [aux] = new Casilla (casilla.Fila+offsetx, casilla.Columna+offsety, 0);
			aux++;
		}
		if (casilla.Columna != 9) {
			offsetx = 0;
			offsety = 1;
			// Compruebo que la posicion no contenga una trampa Sabotaje
			if (tableroPosicion.GetCasilla (casilla.Fila+offsetx, casilla.Columna+offsety).Trampa == 2 ){
				// CODIGO SABOTAJE
				if (esJugador2){
					Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",this, false);
				} else {
					Administrador.ImpactoSabotaje("¡Has impactado en la trampa sabotaje enemiga!",this, true, Constantes.PROYECTIL_HE_ID);
				}
				return true;
			}
			listaCasillas [aux] = new Casilla (casilla.Fila+offsetx, casilla.Columna+offsety, 0);
			aux++;
		}

		int huboHit;
		Casilla casillaHit = new Casilla (casilla.Fila, casilla.Columna);

		// Ataco las posiciones
		for (int i = 0; i < aux; i++) {
			if(esJugador2 == true)
            	huboHit = Administrador.AtacarUnaPosicion (tableroAtaque, tableroPosicion, listaCasillas [i], false, false);
			else
                huboHit = Administrador.AtacarUnaPosicion(tableroAtaque, tableroPosicion, listaCasillas[i], true, true);
            if (huboHit == Constantes.IMPACTO_BARCO || huboHit == Constantes.IMPACTO_REFORZADA || huboHit == Constantes.IMPACTO_CONTRAMEDIDAS) {
				// Si le pegue a algo, el disparo va a impactar sobre un barco, para que se note que le hizo daño y no explote en el agua
				casillaHit = new Casilla(listaCasillas[i].Fila,listaCasillas[i].Columna);
			}
		}

		Administrador.añadirEnfriamiento (this);

		// ENVIO DE MENSAJE
		if (!esJugador2) {			
			Casilla [] lista = new Casilla [1];
			lista[0] = new Casilla(casilla.Fila, casilla.Columna);
			Administrador.EnviarCasillasAtacadas(lista,Constantes.PROYECTIL_HE_ID);
		}

		//FIXME: FALTA LA VERSION DE LA IA
		/*
		if (!esIA) {
			if (barco != -1) {
				ejecutarAnimacion (barco,casillaHit,esIA);
				Administrador.DeboCambiarTurno = true;
			} 
		}else{
			Administrador.CambiarTurno();
		}
		*/

		Administrador.DeboCambiarTurno = true;
		ejecutarAnimacion (barco,casillaHit);

		return true;
	}

	public void ejecutarAnimacion (int barco, Casilla casilla1){
		Administrador.IniciarAnimacionDisparo (barco, casilla1, "CannonHE");
	}
}

[System.Serializable]
public class Habilidad_ArtilleriaRapida : Habilidad  {

	public Habilidad_ArtilleriaRapida(AdministradorPartida admin){
		maxCooldown = 5;
		Administrador = admin;
		id = Constantes.ARTILLERIA_RAPIDA_ID;
		name = "Artilleria Rapida";
	}


	public bool activar(Tablero tableroAtaque, Tablero tableroPosicion, Casilla casilla, int barco = -1, bool esJugador2 = true){
		// Ajusto las filas y columnas para que no queden en los extremos, ya que no tiene sentido atacar un lugar fuera del tablero
		// Guardo la fila y columna en una variable auxiliar para evitar mover la original
		int fila = Mathf.Clamp(casilla.Fila,2,7);
		int columna = Mathf.Clamp(casilla.Columna,2,7);
		int random1; 
		int random2; 
		int[] area = new int[25];
		//Casilla[] listaCasillas = new Casilla[10];
		listaCasillas = new Casilla[10];
		int aux = 0;
		for (int i = 0; i < 5; i++) {
			// Genero un offset
			random1 = Random.Range (-2, 3);
			random2 = Random.Range (-2, 3);

			/* - No compruebo que las posiciones esten libres. Deberia?
			while (tableroAtaque.GetEstadoCasilla(fila + random1, columna + random2) == Constantes.ATACADO_AGUA || tableroAtaque.GetEstadoCasilla(fila + random1, columna + random2) == Constantes.ATACADO_BARCO) {
				random1 = Random.Range (-2, 2);
				random2 = Random.Range (-2, 2);
			}
			*/

			// Compruebo que la posicion que no se repitan las posiciones seleccionadas
			// Ignoren el calculo, it just works (?
			while (area [(2+random1)+((2+random2)*5)] == 1) {
				random1 = Random.Range (-2, 3);
				random2 = Random.Range (-2, 3);
			}
			area [(2+random1)+((2+random2)*5)] = 1;

			// Compruebo que ninguna de las casillas sea una trampa de sabotaje
			if (tableroPosicion.GetCasilla (fila + random1, columna + random2).Trampa == 2) {
				// CODIGO SABOTAJE
				if (esJugador2){
					Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",this, false);
				} else {
					Administrador.ImpactoSabotaje("¡Has impactado en la trampa sabotaje enemiga!",this, true, Constantes.ARTILLERIA_RAPIDA_ID);
				}
				// REPRODUCIR SONIDO
				return true;
			}
			// Guardo la posicion en un vector para atacarla luego
			listaCasillas [aux] = new Casilla (fila + random1, columna + random2, 0);
			aux++;
		}
		// Ataco las posiciones
		for (int i = 0; i < aux; i++) {
            Debug.Log("ARTILLERIA RAPIDA - ATACO: " + listaCasillas[i].Fila + "," + listaCasillas[i].Columna);
			if(esJugador2)
            	Administrador.AtacarUnaPosicion (tableroAtaque, tableroPosicion, listaCasillas [i], true, false);
			else
            	Administrador.AtacarUnaPosicion (tableroAtaque, tableroPosicion, listaCasillas [i], true, true);
		}

		if (!esJugador2){

			Administrador.EnviarCasillasAtacadas(listaCasillas, Constantes.ARTILLERIA_RAPIDA_ID);

		}

		Administrador.añadirEnfriamiento (this);
		/*
		if (!esIA) {
			if (barco != -1) {
				ejecutarAnimacion (barco, listaCasillas, aux, esIA);
				Administrador.DeboCambiarTurno = true;
			} 
		}else{
				Administrador.CambiarTurno();
		}
		*/

		Administrador.DeboCambiarTurno = true;
		ejecutarAnimacion (barco, listaCasillas, esJugador2);
		return true;
	}

	public bool activar(Tablero tableroAtaque, Tablero tableroPosicion, int barco = -1, bool esIA = true, Casilla [] casillas = null){
		if (casillas == null) {
			// CODIGO SABOTAJE
			Administrador.ImpactoSabotaje("¡El enemigo impacto en tu trampa sabotaje!",this, false, 0);
			return true;
		}

		// Ataco las posiciones
		for (int i = 0; i < 5; i++) {
            Debug.Log("ARTILLERIA RAPIDA - ATACO: " + casillas[i].Fila + "," + casillas[i].Columna);
            Administrador.AtacarUnaPosicion (tableroAtaque, tableroPosicion, casillas [i], true, false);
		}
		Administrador.añadirEnfriamiento (this);
		Administrador.MensajeBot.DisplayMessage ("El enemigo utilizo la habilidad Artilleria Rápida", 2.5f, 500);

		Administrador.DeboCambiarTurno = true;
		ejecutarAnimacion (barco, casillas, esIA);
		return true;
	}

	public void ejecutarAnimacion (int barco, Casilla[] listaCasilla, bool esIA){
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[0], "CannonArtilleriaRapida",-0.3f,-0.4f);
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[1], "CannonArtilleriaRapida",-0.5f,-0.1f);
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[2], "CannonArtilleriaRapida",0,0.4f);
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[3], "CannonArtilleriaRapida",0.5f,-0.1f);
		Administrador.IniciarAnimacionDisparo (barco, listaCasilla[4], "CannonArtilleriaRapida",0.3f,-0.4f);
	}
}
