using System;

[Serializable]
public class DatosPartida
{
    public Jugador jugador, Ia;
    public int turno;
    public int barcoActual;
    public int dificultad, cantidadDeTurnosMax, cantidadDeTurnosJugados;
    public DateTime fechaGuardado;
    public bool habilidadesActivadas, modoContraReloj, modoClasico;

    //private int tiempoTurnoActual;

    public DatosPartida(Jugador jugador, Jugador Ia, int turno, int dificultad, DateTime fechaGuardado, int barcoActual, bool habilidadesActivadas, int cantidadDeTurnosMax, bool modoContraReloj, bool modoClasico, int cantidadDeTurnosJugados)
    {
        this.jugador = jugador;
        this.Ia = Ia;
        this.turno = turno;
        this.barcoActual = barcoActual;
        this.dificultad = dificultad;
        this.fechaGuardado = fechaGuardado;
        this.habilidadesActivadas = habilidadesActivadas;
        this.modoClasico = modoClasico;
        this.modoContraReloj = modoContraReloj;
        this.cantidadDeTurnosMax = cantidadDeTurnosMax;
        this.cantidadDeTurnosJugados = cantidadDeTurnosJugados;
    }
}
