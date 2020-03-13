using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slot {

	private Habilidad habilidad;
	private int enfriamiento;
	private bool usado;

	public Slot(){
		this.habilidad = null;
		this.enfriamiento = -1;
		this.usado = false;
	}

	public Habilidad Habilidad {
		get {
			return this.habilidad;
		}
		set {
			habilidad = value;
		}
	}

	public int Enfriamiento {
		get {
			return this.enfriamiento;
		}
		set {
			enfriamiento = value;
		}
	}

	public bool Usado {
		get {
			return this.usado;
		}
		set {
			usado = value;
		}
	}

}

[System.Serializable]
public class Jugador {

	private Tablero tableroAtaque;
	private Tablero tableroPosicion;

    [System.NonSerialized]
	private TestTableroController controllerAtaque;
    [System.NonSerialized]
    private TestTableroController controllerPosicion;

	private bool usoTormentaMisiles;
	private int posicionesDisponibles;
	private int turnosSilenciado;
	private int turnosContramedidas;
	private Barco barcoContramedidas;

	private int tipoTrampa;
	private Casilla[] casillasTrampa;

	private Barco[] listaBarcos;
	private Slot[] slots;

	private string nombre;

	public Jugador (string nombre)
	{
		this.tableroAtaque = null;
		this.tableroPosicion = null;
		this.usoTormentaMisiles = false;
		this.posicionesDisponibles = 100;
		this.turnosSilenciado = -1;
		this.turnosContramedidas = -1;
		this.tipoTrampa = 0;
		this.casillasTrampa = null;
		this.slots = new Slot[3];
		// inicializacion
		this.slots[0] = new Slot();
		this.slots[1] = new Slot();
		this.slots[2] = new Slot();
		this.nombre = nombre;
	}

	public Tablero TableroAtaque {
		get {
			return this.tableroAtaque;
		}
		set {
			tableroAtaque = value;
		}
	}

	public Tablero TableroPosicion {
		get {
			return this.tableroPosicion;
		}
		set {
			tableroPosicion = value;
		}
	}

	public TestTableroController ControllerAtaque {
		get {
			return this.controllerAtaque;
		}
		set {
			controllerAtaque = value;
		}
	}

    public TestTableroController ControllerPosicion {
		get {
			return this.controllerPosicion;
		}
		set {
			controllerPosicion = value;
		}
	}

	public bool UsoTormentaMisiles {
		get {
			return this.usoTormentaMisiles;
		}
		set {
			usoTormentaMisiles = value;
		}
	}

	public int PosicionesDisponibles {
		get {
			return this.posicionesDisponibles;
		}
		set {
			posicionesDisponibles = value;
		}
	}

	public int TurnosSilenciado {
		get {
			return this.turnosSilenciado;
		}
		set {
			turnosSilenciado = value;
		}
	}

	public int TurnosContramedidas {
		get {
			return this.turnosContramedidas;
		}
		set {
			turnosContramedidas = value;
		}
	}

	public Barco BarcoContramedidas {
		get {
			return this.barcoContramedidas;
		}
		set {
			barcoContramedidas = value;
		}
	}

	public int TipoTrampa {
		get {
			return this.tipoTrampa;
		}
		set {
			tipoTrampa = value;
		}
	}

	public Casilla[] CasillasTrampa {
		get {
			return this.casillasTrampa;
		}
		set {
			casillasTrampa = value;
		}
	}

	public Barco[] ListaBarcos {
		get {
			return this.listaBarcos;
		}
		set {
			listaBarcos = value;
		}
	}

	public Slot[] Slots {
		get {
			return this.slots;
		}
	}

	public string Nombre {
		get {
			return this.nombre;
		}
	}

    public int puedeAñadirHabilidad(Habilidad hab){
		if (turnosSilenciado > 0)
			return -1;

		if (hab.EsTrampa == true && this.tipoTrampa != 0) {
			return -1;
		}
		for (int i = 0; i < 3; i++) {
            if (slots[i].Habilidad != null) { 
				if (slots [i].Habilidad.Id == hab.Id) {
					return -1;
			}
			}

		}
		for (int i = 0; i < 3; i++) {
			if (slots [i].Usado == false) {
				return i;
			}
		}
		return -1;
	}

	public void añadirHabilidad(Habilidad hab){
		int aux = puedeAñadirHabilidad (hab);
		if(aux == -1) return;
		this.slots [aux].Habilidad = hab;
		this.slots [aux].Usado = true;
		if (hab.EsTrampa == true) {
			this.slots [aux].Enfriamiento = 99;
		} else {
			this.slots [aux].Enfriamiento = hab.MaxCooldown;
		}
	}

	public void decrementarEnfriamientos(){
		for (int i = 0; i < 3; i++) {
			if (slots [i].Usado == true) {
				if (slots [i].Habilidad.EsTrampa == true){
					if (this.slots [i].Enfriamiento == 99) {
						if (this.tipoTrampa == 0) {
							this.slots [i].Enfriamiento = this.slots [i].Habilidad.MaxCooldown;
						}
					} else if (slots [i].Enfriamiento > 1) {
						this.slots [i].Enfriamiento--;
					} else {
						this.slots [i].Habilidad = null;
						this.slots [i].Enfriamiento = -1;
						this.slots [i].Usado = false;
					}
				}else if (slots [i].Enfriamiento > 1) {
					this.slots [i].Enfriamiento--;
				} else {
					this.slots [i].Habilidad = null;
					this.slots [i].Enfriamiento = -1;
					this.slots [i].Usado = false;
				}
			}
		}
	}

	public bool habilidadDisponible(Habilidad hab){
		for (int i = 0; i < 3; i++) {
			if (slots[i].Usado == true && slots[i].Habilidad.ToString() == hab.ToString()) {
				return false;
			}
			if (hab.EsTrampa == true && this.tipoTrampa != 0) {
				return false;
			}
		}
		return true;
	}

	public int GetIndiceBarcoVivo(){
		int[] indices = new int[listaBarcos.Length];
		int count = 0;
		for (int i = 0; i < listaBarcos.Length; i++) {
			if (listaBarcos[i].Vivo) {
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

	public bool haySlotLibre(){
		for (int i = 0; i < 3; i++) {
			if (slots [i].Usado == false) {
				return true;
			}
		}
		return false;
	}
}
