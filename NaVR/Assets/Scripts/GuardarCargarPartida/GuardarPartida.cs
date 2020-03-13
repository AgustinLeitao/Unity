using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardarPartida {

    private Tablero tableroPosicionJugador1;
    private Tablero tableroAtaqueJugador1;
    private Tablero tableroPosicionJugador2;
    private Tablero tableroAtaqueJugador2;
    private int dificultad;
    private string turnoActual;
    private int tiempoTurnoActual;
	private string keyUltimaPartidaGuardada;
    
    public Tablero TableroPosicionJugador1 { get { return this.tableroPosicionJugador1; } set { tableroPosicionJugador1 = value; } }
    public Tablero TableroAtaqueJugador1 { get { return this.tableroAtaqueJugador1; } set { tableroAtaqueJugador1 = value; } }
    public Tablero TableroPosicionJugador2 { get { return this.tableroPosicionJugador2; } set { tableroPosicionJugador2 = value; } }
    public Tablero TableroAtaqueJugador2 { get { return this.tableroAtaqueJugador2; } set { tableroAtaqueJugador2 = value; } }
    public int Dificultad { get { return this.dificultad; } set { dificultad = value; } }
    public string TurnoActual { get { return this.turnoActual; } set { turnoActual = value; } }
    public int TiempoTurnoActual { get { return this.tiempoTurnoActual; } set { tiempoTurnoActual = value; } }
    public string KeyUltimaPartidaGuardada { get { return this.keyUltimaPartidaGuardada; } set { keyUltimaPartidaGuardada = value; } }

    public GuardarPartida(Tablero tableroPosicionJugador1, Tablero tableroAtaqueJugador1, Tablero tableroPosicionJugador2, Tablero tableroAtaqueJugador2, int dificultad, string turnoActual, int tiempoTurnoActual) {
        this.tableroPosicionJugador1 = tableroPosicionJugador1;
        this.tableroAtaqueJugador1 = tableroAtaqueJugador1;
        this.tableroPosicionJugador2 = tableroAtaqueJugador2;
        this.tableroAtaqueJugador2 = tableroAtaqueJugador2;
        this.dificultad = dificultad;
        this.turnoActual = turnoActual;
        this.tiempoTurnoActual = tiempoTurnoActual; 
    }

    public void Guardar()
    {
        string keyNuevaPartida = GenerarKeyDePartida();
        PlayerPrefs.SetString("UltimaPartidaGuardada", keyNuevaPartida);
		GuardarDatos(keyNuevaPartida);
    }

	public void SobreEscribirPartida(string keyDePartida) {
        GuardarDatos(keyDePartida);
	}

	private void GuardarDatos(string key) {
		PlayerPrefs.SetInt(key + "Dificultad", this.dificultad);
        PlayerPrefs.SetString(key + "TurnoActual", this.turnoActual);
        PlayerPrefs.SetInt(key + "TiempoTurnoActual", this.tiempoTurnoActual);
        PlayerPrefs.SetString(key + "HoraDeGuardado", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        PlayerPrefs.SetString(key + "TableroPosicionJugador1", Serializador.Serializar(this.tableroPosicionJugador1));
        PlayerPrefs.SetString(key + "TableroAtaqueJugador1", Serializador.Serializar(this.tableroAtaqueJugador1));
        PlayerPrefs.SetString(key + "TableroPosicionJugador2", Serializador.Serializar(this.tableroPosicionJugador2));
        PlayerPrefs.SetString(key + "TableroAtaqueJugador2", Serializador.Serializar(this.tableroAtaqueJugador2));

        PlayerPrefs.Save();
	}

	private string GenerarKeyDePartida() {
        string keyNuevaPartida = string.Empty;
        this.keyUltimaPartidaGuardada = PlayerPrefs.GetString("UltimaPartidaGuardada", string.Empty);
        if(this.keyUltimaPartidaGuardada.Equals(string.Empty)) {
             keyNuevaPartida = string.Format("{0}", 0);
        } else {
           keyNuevaPartida = ((int.Parse(this.keyUltimaPartidaGuardada)) + 1 ) + "";
        }
		return keyNuevaPartida;
	}
}
