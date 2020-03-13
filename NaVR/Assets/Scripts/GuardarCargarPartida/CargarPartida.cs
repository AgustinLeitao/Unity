using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargarPartida {

	private string key;
	private string horaDeGuardado;
	private Tablero tableroPosicionJugador1;
    private Tablero tableroAtaqueJugador1;
    private Tablero tableroPosicionJugador2;
    private Tablero tableroAtaqueJugador2;
    private int dificultad;
    private string turnoActual;
    private int tiempoTurnoActual;
	private string keyUltimaPartidaGuardada;

	public CargarPartida(string key) {
		this.key = key;
	}

	public void Cargar(AdministradorPartida admin) {
		this.dificultad = PlayerPrefs.GetInt(this.key + "Dificultad");
        this.turnoActual = PlayerPrefs.GetString(this.key + "TurnoActual");
        this.tiempoTurnoActual = PlayerPrefs.GetInt(this.key + "TiempoTurnoActual");
        this.horaDeGuardado = PlayerPrefs.GetString(this.key + "HoraDeGuardado");
        this.tableroPosicionJugador1 = Serializador.Deserializar<Tablero>(PlayerPrefs.GetString(this.key + "TableroPosicionJugador1"));
        this.tableroAtaqueJugador1 = Serializador.Deserializar<Tablero>(PlayerPrefs.GetString(this.key + "TableroAtaqueJugador1"));
        this.tableroPosicionJugador2 = Serializador.Deserializar<Tablero>(PlayerPrefs.GetString(this.key + "TableroPosicionJugador2"));
        this.tableroAtaqueJugador2 = Serializador.Deserializar<Tablero>(PlayerPrefs.GetString(this.key + "TableroAtaqueJugador2"));

		//Setear los valores e instanciar lo necesario en el administrador...	
	}
}
