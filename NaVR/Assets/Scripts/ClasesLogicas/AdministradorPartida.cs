using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using Menu.Managers;
using GooglePlayGames;
using UnityEngine.VR;

// Delegate usado para callbacks de fade
public delegate void CallBackDelegate();

public class AdministradorPartida : MonoBehaviour
{
    private Jugador jugador1;
    private Jugador jugador2;

    private int modoSeleccionado;
    private bool habilidadesActivadas;
    private Habilidad[] habilidades;
    private Habilidad_ArtilleriaRapida ArtilleriaRapida;
    private Habilidad_ProyectilHE ProyectilHE;
    private Habilidad_Torpedo Torpedo;
    private Habilidad_DisparoDoble DisparoDoble;
    private Habilidad_ReforzarArmadura ReforzarArmadura;
    private Habilidad_TormentaDeMisiles TormentaDeMisiles;
    private Habilidad_PulsoElectromagnetico PulsoElectromagnetico;
    private Habilidad_Contramedidas Contramedidas;
    private Habilidad_RadarPasivo RadarPasivo;
    private Habilidad_Sabotaje Sabotaje;

    private GameObject turno1Label;
    private GameObject turno2Label;

    private Luz luz;
    private VibracionCamara vibracion;
	private int activarVibracion = 0;

    private bool esModoIA;
    private InteligenciaArtificial IA;

    private int turno;

    // Para habilidades en donde se selecciona mas de una casilla
    private int numCasillasSeleccionadas;
    private Casilla[] casillasSeleccionadas;

    // Esto podria ir en una clase aparte
    // Son los casilleros que fueron revelados como "posibles barcos" al utilizar reconocimiento satelital
    private ArrayList posiblesBarcos1;
    private ArrayList posiblesBarcos2;

    private Barco[] barcos1;
    private Barco[] barcos2;

    private GameObject[] posicionamientoBarcos;
    private GameObject[] posicionamientoBarcosIA;

    // FIXME: Reemplazar esto por la referencia a un gameObject vacio
    private float ESQUINA_X;    //Esquina del Mapa. Cambiar a la que mejor nos parezca
    private float ESQUINA_Z;    //Esquina del Mapa. Cambiar a la que mejor nos parezca
    private const float MEDIDA_CASILLA = 13F;       //Escala de una casilla visual, Cambiar a la que mejor nos parezca

    private int barcoActual;

    // TESTING
    // Estas variables referencian otras variables, dependiendo el jugador a quien le corresponde el turno
    // Sirven para que todas las funciones "sepan" que variables deben leer o escribir
    private GameObject[] botones;
    private ArrayList posiblesBarcos;

    [HideInInspector]
    public Jugador jugadorSeleccionado;

    private Jugador jugadorEnemigo;

    private AudioSource sonidoAmbiente;
    private AudioSource sonidosDelJuego;

    private GameObject[] slotLabels;

    public InteligenciaArtificial Ia { get { return this.IA; } set { IA = value; } }

    private GameObject player;

    // Barcos para instanciar
    private GameObject barco2Pos;
    private GameObject barco3Pos;
    private GameObject barco4Pos;
    private GameObject barco5Pos;
    private GameObject barcoEnemigo;

    // Markers
    private GameObject markerBarcosEnemigos;
    private GameObject markerBarcosJugador;
    private GameObject markerEsquinaTablero;

    // Flags
    private bool animacionEnCurso;
    private bool EsMultijugador;

    // Para guardar las posiciones que se van a marcar en el tablero cuando la animacion termine
    private int numCasillasAtacadas;
    private Casilla[] casillasAtacadas;
    private Casilla casillaMarcadaAtaque;
    private bool deboCambiarTurno;
    private bool deboDesactivarHabilidades;

    private GameObject canvasUI;
    private GameObject lluvia;

    // Para IA
    private ArrayList ultimosHits;

    // Mensajes en pantalla
    private MensajePantalla mensajeTop;
    private MensajePantalla mensajeBot;
    private FakeMensajeCarga mensajeCarga;
    private FakeMensajeCarga mensajeEspera;
    private FadePantalla fadePantalla;

    private LoadGameManager loadGameManager;
    private int clavePartida;
    private int dificultad;
    private bool esInicioPartida = true, partidaEmpatada, turnosMaximosAlcanzados;
    private OpcionesDelJuego opciones;

    [HideInInspector]
    public bool esModoClasico, esModoContraReloj;
    [HideInInspector]
    public int cantidadDeTurnosMaxima, contadorDeTurnos;

    private float volumenMusica, volumenEfectos, tiempoAutoGuardado;

    private ContenedorBarcos contenedorBarcos;

    private CargaEscenaAsync cargaEscenaAsync;
    private GameObject reticle;

    // Variables de cambiar barco
    private int barcoSeleccionado;
    private bool esInicioDeJuego;

    private AudioManagerScript audioManager;

    private string idJugador = "";
    private int idOrden = -1;
    private long score = 0;
    private long scoreAcumulado = 0;
    private int cantidadDeHabilidadesUtilizadas = 0;
    private bool finalizoPartida = false;

    private bool huboImpactoReforzada = false;

    private bool reconectado = false;

    // Flags mugrosos
    private bool esMiAtaque = false;
    private bool recibiMensaje = false;
    private bool registroUnicoDeScore = true;

    public IEnumerator LoadDevice(string newDevice)
    {
        VRSettings.LoadDeviceByName(newDevice);
        yield return null;
        VRSettings.enabled = true;
    }

    private DatosEstadisticas estadisticasDelJugador;
    private EstadisticaHabilidad[] estadisticasHabilidadesEspeciales;

    private void Awake()
    {
        StartCoroutine(LoadDevice("cardboard"));

        var LoadGameManager = GameObject.Find("LoadGameManager");
        var OpcionesDelJuego = GameObject.Find("Opciones");
        var contenedorDeBarcos = GameObject.Find("ContenedorBarcos");

        if (LoadGameManager != null || OpcionesDelJuego == null)
            esModoIA = true;
        else
        {
            if (OpcionesDelJuego != null)
                esModoIA = OpcionesDelJuego.GetComponent<OpcionesDelJuego>().esModoIA;
        }

        if (esModoIA == false)
        {
            idOrden = -1;
        }
        else
        {           
            idOrden = 0;
        }

        //Inicializacion de estadisticas
        estadisticasDelJugador = new DatosEstadisticas();
        estadisticasHabilidadesEspeciales = new EstadisticaHabilidad[8];
        string[] habilidadesEspeciales = {"Torpedo", "ProyectilHE", "Artilleria Rapida", "Ataque Doble", "Sabotaje", "Pulso Electromagnetico", "Reforzar Armadura", "Tormenta De Misiles" };
        for(int index = 0; index < 8; index++)            
            estadisticasHabilidadesEspeciales[index] = new EstadisticaHabilidad(habilidadesEspeciales[index]);
       
        if (contenedorDeBarcos != null)
            contenedorBarcos = contenedorDeBarcos.GetComponent<ContenedorBarcos>();

        if (OpcionesDelJuego != null)
            opciones = OpcionesDelJuego.GetComponent<OpcionesDelJuego>();

        if (LoadGameManager != null)
        {
            loadGameManager = LoadGameManager.GetComponent<LoadGameManager>();
            clavePartida = loadGameManager.numeroPartidaACargar;
        }
        else
        {
            clavePartida = 0;
            while (PlayerPrefs.GetString(string.Format("Partida{0}", clavePartida), string.Empty) != string.Empty)
            {
                clavePartida++;
            }
        }
    }

    private void Start()
    {     
        if (loadGameManager != null)
            CargarPartidaDesdeArchivo();

        DeclararAtributosYComponentesDePartida();
        if (!esModoIA){
            mensajeCarga.DisplayMessage(true);
            fadePantalla.IniciarFadeForzado();
        }
        InicializarPartida();
    }

    private void CargarPartidaDesdeArchivo()
    {
        dificultad = loadGameManager.datosPartida.dificultad;
        jugador1 = loadGameManager.datosPartida.jugador;
        jugador2 = loadGameManager.datosPartida.Ia;
        turno = loadGameManager.datosPartida.turno;
        esModoClasico = loadGameManager.datosPartida.modoClasico;
        esModoContraReloj = loadGameManager.datosPartida.modoContraReloj;
        cantidadDeTurnosMaxima = loadGameManager.datosPartida.cantidadDeTurnosMax;
        contadorDeTurnos = loadGameManager.datosPartida.cantidadDeTurnosJugados;

        for (int i = 0; i < 8; i++)
        {
            foreach (var casilla in jugador1.TableroPosicion.Barcos[i].GetPosicionesOcupadas())
                casilla.Barco = jugador1.TableroPosicion.Barcos[i];

            foreach (var casilla in jugador1.ListaBarcos[i].GetPosicionesOcupadas())
                casilla.Barco = jugador1.ListaBarcos[i];

            foreach (var casilla in jugador2.TableroPosicion.Barcos[i].GetPosicionesOcupadas())
                casilla.Barco = jugador2.TableroPosicion.Barcos[i];

            foreach (var casilla in jugador2.ListaBarcos[i].GetPosicionesOcupadas())
                casilla.Barco = jugador2.ListaBarcos[i];
        }
    }

    private void DeclararAtributosYComponentesDePartida()
    {

        //Inicializar cada una de las habilidades y asociarlas con el array de habilidades
        InicializarHabilidades();

        // Barcos para instanciar
        // FIXME: Si movemos este bloque mas abajo, el juego se rompe
        // Hay que hacer que las referencias a los tableros fisicos iniciales se hagan despues de que aparezcan los barcos
        // Eligiendo algun barco al azar donde el jugador va a arrancar el juego.
        // Una forma facil seria ejecutar la funcion de teletransporte justo despues de que este todo cargado.
        barco2Pos = GameObject.Find("BarcosJugador/Barco2Pos_Base");
        barco2Pos.SetActive(false);
        barco3Pos = GameObject.Find("BarcosJugador/Barco3Pos_Base");
        barco3Pos.SetActive(false);
        barco4Pos = GameObject.Find("BarcosJugador/Barco4Pos_Base");
        barco4Pos.SetActive(false);
        barco5Pos = GameObject.Find("BarcosJugador/Barco5Pos_Base");
        barco5Pos.SetActive(false);
        barcoEnemigo = GameObject.Find("BarcosJugador/BarcoEnemigo");
        barcoEnemigo.SetActive(false);

        player = GameObject.Find("Player");

        markerBarcosEnemigos = GameObject.Find("Markers/BarcosEnemigos");
        markerBarcosJugador = GameObject.Find("Markers/BarcosJugador");
        markerEsquinaTablero = GameObject.Find("Markers/EsquinaTablero");

        markerEsquinaTablero = GameObject.Find("Markers/EsquinaTablero");

        ESQUINA_X = markerEsquinaTablero.transform.position.x;
        ESQUINA_Z = markerEsquinaTablero.transform.position.z;

        //Se usa para varias habilidades, podría estar en otro lado.
        casillasSeleccionadas = new Casilla[5];
        casillasAtacadas = new Casilla[10];
        numCasillasSeleccionadas = 0;
        numCasillasAtacadas = 0;

        //Inicializar Jugador 1 y Jugador 2, que en este caso es la IA. Mas adelante seguro tengamos que diferenciar con el Jugador 2 real.
        if (loadGameManager == null)
        {
            if (esModoIA)
            {
                jugador1 = new Jugador("Jugador 1");
                jugador2 = new Jugador("IA");
            }
            else
            {
                jugador1 = new Jugador("Jugador 1");
                jugador2 = new Jugador("Jugador 2");
            }
        }

        //Creacion de tableros y labels de turno para cada jugador. Podría mandarse jugador1 y jugador2 de parametro pero no es necesario.
        CrearTablerosYLabelsDeTurno();

        //Creacion de botones para habilidades especiales.
        CrearBotonesHabilidades();
        CrearSlotsHabilidades();

        // Esto no sirve por ahora
        posiblesBarcos1 = new ArrayList();
        posiblesBarcos2 = new ArrayList();
        posiblesBarcos = null;
        // ------

        deboCambiarTurno = false;
        deboDesactivarHabilidades = false;

        sonidoAmbiente = GameObject.Find("SonidoAmbiente").GetComponent<AudioSource>();
        sonidoAmbiente.Play();

        if (opciones != null)
        {
            sonidoAmbiente.volume = opciones.volumenMusica;
            foreach (var sonidoDeLjuego in GameObject.Find("SonidosDelJuego").GetComponents<AudioSource>())
                sonidoDeLjuego.volume = opciones.volumenEfectos;
            tiempoAutoGuardado = 120;
        }

        lluvia = GameObject.Find("Lluvia");


        mensajeTop = GameObject.Find("MensajeEnPantallaCanvas/MensajeTop").GetComponent<MensajePantalla>();
        mensajeBot = GameObject.Find("MensajeEnPantallaCanvas/MensajeBottom").GetComponent<MensajePantalla>();
        mensajeCarga = GameObject.Find("MensajeEnPantallaCanvas/MensajeCarga").GetComponent<FakeMensajeCarga>();
        mensajeEspera = GameObject.Find("MensajeEnPantallaCanvas/MensajeEspera").GetComponent<FakeMensajeCarga>();
        fadePantalla = GameObject.Find("MensajeEnPantallaCanvas/FadePantalla").GetComponent<FadePantalla>();

        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();

        ultimosHits = new ArrayList();

        cargaEscenaAsync = GameObject.Find("CargaEscenaAsync").GetComponent<CargaEscenaAsync>();
        reticle = GameObject.Find("GvrReticlePointer");

        if (loadGameManager == null && opciones != null)
        {
			Debug.Log ("Entre a opciones");
			if (esModoIA == true) {
				esModoClasico = opciones.modoClasico;
				esModoContraReloj = opciones.modoContraReloj;
				cantidadDeTurnosMaxima = opciones.cantidadTurnosMax;
				dificultad = opciones.dificultad;
			} else if (opciones.esPartidaRapida == false) {
				Debug.Log ("Por Invitacion");
				esModoClasico = opciones.invitacionModoClasico;
				esModoContraReloj = opciones.invitacionModoContraReloj;
				cantidadDeTurnosMaxima = opciones.invitacionCantidadTurnosMax;
			} else {
				esModoClasico = false;
				esModoContraReloj = false;
				cantidadDeTurnosMaxima = 10;
			}
			Debug.Log ("Configuracion: " + cantidadDeTurnosMaxima + " " + esModoContraReloj + " " + esModoClasico);
            volumenEfectos = opciones.volumenEfectos;
            volumenMusica = opciones.volumenMusica;
        }
    }

    private void InicializarHabilidades()
    {
        ArtilleriaRapida = new Habilidad_ArtilleriaRapida(this);
        ProyectilHE = new Habilidad_ProyectilHE(this);
        Torpedo = new Habilidad_Torpedo(this);
        DisparoDoble = new Habilidad_DisparoDoble(this);
        ReforzarArmadura = new Habilidad_ReforzarArmadura(this);
        TormentaDeMisiles = new Habilidad_TormentaDeMisiles(this);
        PulsoElectromagnetico = new Habilidad_PulsoElectromagnetico(this);
        Contramedidas = new Habilidad_Contramedidas(this);
        RadarPasivo = new Habilidad_RadarPasivo(this);
        Sabotaje = new Habilidad_Sabotaje(this);
        habilidades = new Habilidad[11];
        habilidades[Torpedo.Id] = Torpedo;
        habilidades[ProyectilHE.Id] = ProyectilHE;
        habilidades[ArtilleriaRapida.Id] = ArtilleriaRapida;
        habilidades[DisparoDoble.Id] = DisparoDoble;
        habilidades[ReforzarArmadura.Id] = ReforzarArmadura;
        habilidades[TormentaDeMisiles.Id] = TormentaDeMisiles;
        habilidades[PulsoElectromagnetico.Id] = PulsoElectromagnetico;
        habilidades[Contramedidas.Id] = Contramedidas;
        habilidades[RadarPasivo.Id] = RadarPasivo;
        habilidades[Sabotaje.Id] = Sabotaje;
    }

    private void CrearTablerosYLabelsDeTurno()
    {
        turno1Label = GameObject.Find("TurnoJugador1Label");
        turno2Label = GameObject.Find("TurnoJugador2Label");
        jugador2.ControllerPosicion = GameObject.Find("TableroPrueba1").GetComponent<TestTableroController>(); // Jugador 2 - Tablero de posicion
        jugador1.ControllerAtaque = GameObject.Find("TableroPrueba2").GetComponent<TestTableroController>(); // Jugador 1 - Tablero de ataques
        jugador1.ControllerPosicion = GameObject.Find("TableroPrueba3").GetComponent<TestTableroController>(); // Jugador 1 - Tablero de posicion
        jugador2.ControllerAtaque = GameObject.Find("TableroPrueba4").GetComponent<TestTableroController>(); // Jugador 2 - Tablero de ataques

        if (loadGameManager == null)
        {
            jugador1.TableroAtaque = new Tablero(1, null);
            jugador2.TableroAtaque = new Tablero(1, null);
        }
    }

    private void CrearBotonesHabilidades()
    {
        //Creacion de Botones de habilidades
        botones = new GameObject[11];
        botones[Constantes.ATAQUE_NORMAL_ID] = GameObject.Find("Canvas/BotonAtaqueNormal");
        botones[Torpedo.Id] = GameObject.Find("Canvas/BotonTorpedo");
        botones[ProyectilHE.Id] = GameObject.Find("Canvas/BotonProyectilHE");
        botones[ArtilleriaRapida.Id] = GameObject.Find("Canvas/BotonArtilleriaRapida");
        botones[DisparoDoble.Id] = GameObject.Find("Canvas/BotonAtaqueDoble");
        botones[ReforzarArmadura.Id] = GameObject.Find("Canvas/BotonReforzarArmadura");
        botones[TormentaDeMisiles.Id] = GameObject.Find("Canvas/BotonTormentaDeMisiles");
        botones[PulsoElectromagnetico.Id] = GameObject.Find("Canvas/BotonPulsoElectromagnetico");
        botones[Contramedidas.Id] = GameObject.Find("Canvas/BotonContramedidas");
        botones[RadarPasivo.Id] = GameObject.Find("Canvas/BotonRadarPasivo");
        botones[Sabotaje.Id] = GameObject.Find("Canvas/BotonSabotaje");
    }

    private void CrearSlotsHabilidades()
    {
        //Creacion de Slots de habilidades
        slotLabels = new GameObject[2];
        slotLabels[0] = GameObject.Find("Canvas/SlotsJugador1Label/Text");
        slotLabels[1] = GameObject.Find("Canvas/SlotsJugador2Label/Text");
    }

    public void setFinDePartida(bool value)
    {
        this.finalizoPartida = value;
    }
    public void InicializarPartida()
    {

        //HABILITAR CUANDO SE REALICE LA INTEGRACION:
        //Se levanta el contenedor de barcos de la escena Distribucion de barcos y se los carga en la variable barcos1.
        //Esto va a funcionar cuando se usen las escenas integradas, por lo tanto lo voy a dejar comentado.
        //datosDistribucionBarcos = GameObject.FindGameObjectsWithTag ("BoardManager") [0] as GameObject;
        //contenedorBarcos = datosDistribucionBarcos.GetComponent<ContenedorBarcos> ();
        //barcos1 = contenedorBarcos.barcos; 

        //Esta clase y las lineas que le siguen tienen que eliminar y reemplazarse por carga de barcos reales.
        GeneradorBarcosDePrueba generadorBarcos = new GeneradorBarcosDePrueba();
        //Pueden mandarle parametro del 1 al 5, si no mandan nada hace posicionamiento random.

        idJugador = "";

        if (contenedorBarcos != null)
            barcos1 = contenedorBarcos.barcos;
        else
            barcos1 = generadorBarcos.CrearBarcosParaPruebas(1); // Esto es a fines de testing nomas, para cargar la escena de la expo directamente y que no tire error porque contenedorBarcos es null.

		if (esModoIA == false) {
			barcos2 = contenedorBarcos.barcosRival;
		} else {
			barcos2 = generadorBarcos.CrearBarcosParaPruebas();
		}

        if (loadGameManager == null)
        {
            //Dibujar en la pantalla los barcos del jugador 1
            DibujarBarcosJugador(barcos1);

            //Dibujar en la pantalla los barcos del jugador 2
            DibujarBarcosEnemigos(barcos2);
        }
        else
        {
            //Dibujar en la pantalla los barcos del jugador 1
            DibujarBarcosJugador(jugador1.TableroPosicion.Barcos);

            //Dibujar en la pantalla los barcos del jugador 2
            DibujarBarcosEnemigos(jugador2.TableroPosicion.Barcos);
        }

        //Para las siguientes lineas tenemos que recibir los arrays de barcos de distribucion de barcos o generarlos para la IA.
        if (loadGameManager == null)
        {
            jugador1.TableroPosicion = new Tablero(1, barcos1);
            jugador2.TableroPosicion = new Tablero(2, barcos2);

            // Guardo lista de barcos en los jugadores
            jugador1.ListaBarcos = barcos1;
            jugador2.ListaBarcos = barcos2;
        }

        SetearModoYValoresParaHabilidades();

        // Guardo referencia al tablero visual en cada tablero logico
        jugador1.TableroAtaque.Controller = jugador1.ControllerAtaque;
        jugador1.TableroPosicion.Controller = jugador1.ControllerPosicion;
        jugador2.TableroAtaque.Controller = jugador2.ControllerAtaque;
        jugador2.TableroPosicion.Controller = jugador2.ControllerPosicion;

        //Trasladar a jugador a barco inicial
        if (loadGameManager == null)
            barcoActual = 0;
        else
            barcoActual = loadGameManager.datosPartida.barcoActual;

        CambiarDeBarco(barcoActual, true);

        // Inicializar a la IA
        if (esModoIA == true)
        {
            IA = new InteligenciaArtificial(jugador2, jugador1, dificultad == 0 ? 1 : dificultad, esModoClasico ? 0 : 1);
        }

        /* ELIMINAR - SOLO ESTA PARA PROBAR GPGPS */
        if (MultiplayerManager.Instance == null)
        {
            Debug.Log("El multiplayer manager es null");
        }
        else
        {
            Debug.Log("El multiplayer manager NO es null");
        }
        /* ELIMINAR - SOLO ESTA PARA PROBAR GPGPS */

        //Configuro variables de turno y modo de ataque para el inicio de la partida
        if (esModoIA == true)
        {
            SetearEstadoInicialDePartida();
        }
    }

    //Utilizar los atributos de barco para dibujar.
    //Primera casilla ocupada, orientacion, y tipo de barco. No hace falta mas q eso.
    private void DibujarBarcosJugador(Barco[] barcos)
    {
        GameObject objBarco = null;
        posicionamientoBarcos = new GameObject[barcos.Length];
        int i = 0;
        int tipo;
        foreach (Barco barco in barcos)
        {
            tipo = barco.GetTipoBarco();
            switch (tipo)
            {
                case 2:
                    //objBarco = (GameObject)Resources.Load ("Prefabs/Barcos/Barco2Pos_Base", typeof(GameObject));
                    objBarco = barco2Pos;
                    break;
                case 3:
                    //objBarco = (GameObject)Resources.Load ("Prefabs/Barcos/Barco3Pos_Base", typeof(GameObject));
                    objBarco = barco3Pos;
                    break;
                case 4:
                    //objBarco = (GameObject)Resources.Load ("Prefabs/Barcos/Barco4Pos_Base", typeof(GameObject));
                    objBarco = barco4Pos;
                    break;
                case 5:
                    //objBarco = (GameObject)Resources.Load ("Prefabs/Barcos/Barco5Pos_Base", typeof(GameObject));
                    objBarco = barco5Pos;
                    break;
            }
            if (barco.GetOrientacion() == ConstantesDeBarco.ORIENTACION_HORIZONTAL)
            {
                posicionamientoBarcos[i] = Instantiate(objBarco, CasillaAPosicion(barco.GetPosicionesOcupadas()[0], objBarco.transform.position.y, (tipo / 2), 0),
                 Quaternion.Euler(new Vector3(0, 90, 0)));
                if (barco.GetPosicionesOcupadas()[0].Columna + tipo / 2 > 5)
                    posicionamientoBarcos[i].transform.Rotate(new Vector3(0, 180, 0));
                posicionamientoBarcos[i].SetActive(true);

            }
            else
            {
                posicionamientoBarcos[i] = Instantiate(objBarco, CasillaAPosicion(barco.GetPosicionesOcupadas()[0], objBarco.transform.position.y, 0, (tipo / 2)),
                Quaternion.Euler(new Vector3(0, 0, 0)));
                posicionamientoBarcos[i].SetActive(true);
            }

            barco.BarcoFisico = posicionamientoBarcos[i];

            i++;
        }
        //GameObject player = GameObject.Find("Player");
        //player.transform.position = GameObject.Find("Barco2Pos_Base(Clone)/Cabina").transform.position;
        //TODO: cargar los datos de los tableros en cada cabina...

    }

    private Vector3 CasillaAPosicion(Casilla casilla, float posy, float offsetx, float offsetz)
    {
        return new Vector3((ESQUINA_X + (MEDIDA_CASILLA / 2)) + ((casilla.Columna + offsetx) * MEDIDA_CASILLA), posy,
            ESQUINA_Z - (MEDIDA_CASILLA / 2) - ((casilla.Fila + offsetz) * MEDIDA_CASILLA));
    }

    //Utilizar los atributos de barco para dibujar.
    //Primera casilla ocupada, orientacion, y tipo de barco. No hace falta mas q eso.
    private void DibujarBarcosEnemigos(Barco[] barcos)
    {
        GameObject objBarco = barcoEnemigo;
        posicionamientoBarcosIA = new GameObject[barcos.Length];
        int i = 0;

        foreach (Barco barco in barcos)
        {

            posicionamientoBarcosIA[i] = Instantiate(objBarco, new Vector3(markerBarcosEnemigos.transform.position.x - 200 + ((400 / barcos.Length) * i) + UnityEngine.Random.Range(-20, 20), markerBarcosEnemigos.transform.position.y, markerBarcosEnemigos.transform.position.z),
                Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(-40, 40), 0)));
            posicionamientoBarcosIA[i].SetActive(true);

            posicionamientoBarcosIA[i].transform.localScale = new Vector3(posicionamientoBarcosIA[i].transform.localScale.x * ((float)barco.GetTipoBarco() / 5), posicionamientoBarcosIA[i].transform.localScale.y, posicionamientoBarcosIA[i].transform.localScale.z);

            barco.BarcoFisico = posicionamientoBarcosIA[i];

            i++;
        }
        //GameObject player = GameObject.Find("Player");
        //player.transform.position = GameObject.Find("Barco2Pos_Base(Clone)/Cabina").transform.position;
        //TODO: cargar los datos de los tableros en cada cabina...

    }

    private void SetearModoYValoresParaHabilidades()
    {
        modoSeleccionado = Constantes.ATAQUE_NORMAL_ID; // Ataque normal
        if (loadGameManager == null)
        {
            jugador1.UsoTormentaMisiles = false;
            jugador2.UsoTormentaMisiles = false;
            jugador1.PosicionesDisponibles = 100;
            jugador2.PosicionesDisponibles = 100;
            habilidadesActivadas = true;
        }

        LimpiarCasillasSeleccionadas();

        jugador2.ControllerPosicion.setTablero(jugador2.TableroPosicion);
        jugador1.ControllerAtaque.setTablero(jugador1.TableroAtaque);
        jugador1.ControllerPosicion.setTablero(jugador1.TableroPosicion);
        jugador2.ControllerAtaque.setTablero(jugador2.TableroAtaque);
        jugador2.ControllerPosicion.clearMarks();
        jugador1.ControllerAtaque.clearMarks();
        jugador1.ControllerPosicion.clearMarks();
        jugador2.ControllerAtaque.clearMarks();
        jugador1.ControllerPosicion.mostrarTablero(jugador1.TableroPosicion);
        jugador2.ControllerPosicion.mostrarTablero(jugador2.TableroPosicion);
    }

    private void SetearEstadoInicialDePartida()
    {
        if (loadGameManager == null)
        {
            if (idOrden == 0)
            {
                turno = 1;
                jugadorSeleccionado = jugador2;
                jugadorEnemigo = jugador1;
            }
            else
            {
                turno = 0;
                jugadorSeleccionado = jugador1;
                jugadorEnemigo = jugador2;
            }
        }
        else
        {
            turno = loadGameManager.datosPartida.turno;
            if (turno == 0)
            {
                turno = 1;
                jugadorSeleccionado = jugador2;
                jugadorEnemigo = jugador1;
            }
            else
            {
                turno = 0;
                jugadorSeleccionado = jugador1;
                jugadorEnemigo = jugador2;
            }
        }

        CambiarTurno();
        modoAtaqueNormal();
        turno1Label.GetComponent<Image>().color = new Color(0.5f, 0.7f, 1);
        Debug.Log("Es el turno de : " + turno);
        if (!esModoIA){
            mensajeCarga.DisplayMessage(false);
            fadePantalla.TerminarFadeForzado();
        }
        //testController4.editable = false;
    }

    public string ObtenerGanador()
    {
        string empate = "Empate";
        int barcosVivosJugador1 = jugador1.TableroPosicion.CantidadDeBarcosVivos();
        int barcosVivosJugador2 = jugador2.TableroPosicion.CantidadDeBarcosVivos();

        Debug.Log("barcosVivosJugador1= " + barcosVivosJugador1);
        Debug.Log("barcosVivosJugador2= " + barcosVivosJugador2);



        if(barcosVivosJugador1 > barcosVivosJugador2)
            return jugador1.Nombre;
        else
        {
            if (barcosVivosJugador1 < barcosVivosJugador2)
            { 
                return jugador2.Nombre;
            }
            else
            { 
                if(barcosVivosJugador1 == barcosVivosJugador2)
                {
                    int barcosAveriadosJugador1 = jugador2.TableroAtaque.CantidadDeBarcosAveriadosDelEnemigo();
                    int barcosAveriadosJugador2 = jugador1.TableroAtaque.CantidadDeBarcosAveriadosDelEnemigo();
                    Debug.Log("barcosAveriadosJugador1= " + barcosAveriadosJugador1);
                    Debug.Log("barcosAveriadosJugador2= " + barcosAveriadosJugador2);

                    if (barcosAveriadosJugador1 > barcosAveriadosJugador2)
                    {
                        return jugador2.Nombre;
                    }
                    else
                    {
                        if (barcosAveriadosJugador1 < barcosAveriadosJugador2)
                        {
                            return jugador1.Nombre;
                        }
                        else
                        {
                            return empate;
                        }
                    }
                }
                return string.Empty;
            }
        }
    }

    public void finalizarPartida()
    {
        setFinDePartida(true);
        long total = 0;
        if (!esModoIA)
            total = RegistrarScore();

        if (esModoContraReloj)
        {
            if (partidaEmpatada)
                estadisticasDelJugador.empatesModoContraReloj++;
            else
            {
                if (jugadorSeleccionado.Nombre.Equals("IA") || jugadorSeleccionado.Nombre.Equals("Jugador 2"))
                {
                    estadisticasDelJugador.derrotasModoContraReloj++;
                }
                else
                    estadisticasDelJugador.victoriasModoContraReloj++;
            }
        }
        else
        {
            if (esModoClasico)
            {
                if (jugadorSeleccionado.Nombre.Equals("IA") || jugadorSeleccionado.Nombre.Equals("Jugador 2"))
                {
                    estadisticasDelJugador.derrotasModoClasico++;
                }
                else
                    estadisticasDelJugador.victoriasModoClasico++;
            }
            else
            {
                if (jugadorSeleccionado.Nombre.Equals("IA") || jugadorSeleccionado.Nombre.Equals("Jugador 2"))
                {
                    estadisticasDelJugador.derrotasModoNormal++;
                }
                else
                    estadisticasDelJugador.victoriasModoNormal++;
            }
        }
        GuardarEstadisticasDelJugador();

        posicionamientoBarcos[barcoActual].transform.Find("Cabina/CanvasIngameOptions").gameObject.SetActive(false);

        foreach (var habilidadEspecial in GameObject.FindGameObjectsWithTag("HabilidadEspecial"))
            habilidadEspecial.SetActive(false);

        if (canvasUI != null)
            canvasUI.SetActive(true);
            
        string ganador = ObtenerGanador();
        Debug.Log("ganador= " + ganador);

        if (esModoIA)
        {
            if (ganador.Equals("Empate"))
            {
                mensajeTop.DisplayMessage("PARTIDA EMPATADA!", 30f, 500);
            }
            else
            {
                if (ganador.Equals("Jugador 2"))
                {
                    mensajeTop.DisplayMessage("EL JUGADOR ENEMIGO HA VENCIDO. ¡HAS PERDIDO LA PARTIDA!", 30f, 500);
                    IrAlMenuPrincipal();
                }
                else
                {
                    mensajeTop.DisplayMessage("EL JUGADOR ENEMIGO HA SIDO DERROTADO. ¡HAS GANADO LA PARTIDA!", 30f, 500);
                }
            }
        } else
        {
            Debug.Log("SoyElGanador??= " + SoyElGanador(ganador));

            if (ganador.Equals("Empate"))
            {
                mensajeTop.DisplayMessage("¡PARTIDA EMPATADA!", 10f, 500);
                mensajeBot.DisplayMessage("¡Sumaste " + total + " Puntos en esta partida!", 10f, 600);
            }
            else if (SoyElGanador(ganador))
            {
                mensajeTop.DisplayMessage("EL JUGADOR ENEMIGO HA SIDO DERROTADO. ¡HAS GANADO LA PARTIDA!", 10f, 500);
                mensajeBot.DisplayMessage("¡Sumaste " + total + " Puntos en esta partida!", 10f, 600);
            }
            else {
                mensajeTop.DisplayMessage("EL JUGADOR ENEMIGO HA VENCIDO. ¡HAS PERDIDO LA PARTIDA!", 10f, 500);
                mensajeBot.DisplayMessage("¡Sumaste " + total + " Puntos en esta partida!", 10f, 600);
                IrAlMenuPrincipal();
            }
        }
    }

    IEnumerator IrAlMenuPrincipal()
    {
        yield return new WaitForSeconds(10);
        AbandonarPartida();
    }

    private bool SoyElGanador(string ganador)
    {
        if (ganador.Equals("Jugador 1"))
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void Update()
    {
     
        if (esModoIA == false)
        {
            var btnReiniciarPartida = GameObject.Find("BtnReiniciarPartida");
            if (btnReiniciarPartida != null)
                btnReiniciarPartida.SetActive(false);

            var btnGuardarPartida = GameObject.Find("BtnGuardarPartida");
            if (btnGuardarPartida != null)
                btnGuardarPartida.SetActive(false);

            if (reconectado == false && MultiplayerManager.Instance.State != MultiplayerManager.MultiplayerState.Playing)
            {

                reconectado = true;
            }
            if (MultiplayerManager.activarQuickGameFix == true)
            {
                Debug.Log("CREANDO QUICK GAME");
                MultiplayerManager.CreateQuickGame();
                MultiplayerManager.activarQuickGameFix = false;
            }
            if (MultiplayerManager.Instance.State == MultiplayerManager.MultiplayerState.Playing && idJugador == "" && reconectado)
            {
                idJugador = MultiplayerManager.Instance.PID;
                idOrden = MultiplayerManager.Instance.obtenerIdOrden();
                MultiplayerManager.Instance.CargarScoreJugador();
                SetearEstadoInicialDePartida();
            }
            if (MultiplayerManager.Instance.State != MultiplayerManager.MultiplayerState.Playing && reconectado)
            {
                idJugador = "";
                idOrden = -1;
            }
        }

        if (esModoClasico)
            foreach (var habilidadEspecial in GameObject.FindGameObjectsWithTag("HabilidadEspecial"))
                habilidadEspecial.SetActive(false);

        var canvasUIRef = GameObject.FindGameObjectsWithTag("OpcionesFinPartida");
        if (canvasUIRef.Length > 0)
            canvasUI = canvasUIRef[0];

        if (idOrden != -1 && !EsFinDePartida())
        {
            if (canvasUI != null)
            {
                canvasUI.SetActive(false);
            }
        }

        if (opciones != null && opciones.autoGuardado)
        {
            tiempoAutoGuardado -= Time.deltaTime;
            if (tiempoAutoGuardado < 0)
            {
                tiempoAutoGuardado = 120;
                GuardarPartida();
            }
        }
    }

    public void CambiarTurno()
    {
        if (loadGameManager == null)
            habilidadesActivadas = true;
        else
        {
            if (esInicioPartida)
                habilidadesActivadas = loadGameManager.datosPartida.habilidadesActivadas;
            else
                habilidadesActivadas = true;
        }

        TurnosMaximosAlcanzados();

        if (EsFinDePartida())
        {
            finalizarPartida();
            return;
        }

        if (turno == 0)
        { // Cambio a turno de Jugador 2
            mensajeTop.DisplayMessage("Turno del enemigo", 2f, 250);
            turno = 1;
            turno1Label.GetComponent<Image>().color = new Color(1, 1, 1);
            turno2Label.GetComponent<Image>().color = new Color(0.5f, 0.7f, 1);
            posiblesBarcos = posiblesBarcos1;
            jugadorSeleccionado = jugador2;
            jugadorEnemigo = jugador1;

            if (loadGameManager == null)
                jugador2.decrementarEnfriamientos();
            else
            {
                if (!esInicioPartida)
                    jugador2.decrementarEnfriamientos();
                else
                    esInicioPartida = false;
            }

            if (loadGameManager != null)
            {
                RefrescarTablero(jugador1.TableroAtaque);
                RefrescarTablero(jugador1.TableroPosicion);
            }

            if (jugador2.TurnosContramedidas > 0)
            {
                //DesactivarHabilidad (8);
                jugador2.TurnosContramedidas--;
            }
            if (jugador2.TurnosContramedidas == 0)
            {
                RestaurarContramedidas();
                jugador2.TurnosContramedidas--;
            }
            if (jugador2.UsoTormentaMisiles == true)
            {
                //DesactivarHabilidad (6);
            }
            if (jugador2.TurnosSilenciado == 0)
            {
                mensajeBot.DisplayMessage("Tu pulso electromagnetico ha expirado", 2.5f, 500);
                jugador2.TurnosSilenciado--;
            }
            if (jugador2.TurnosSilenciado > 0)
            {
                ColorearBoton();
                DesactivarHabilidades();
                jugador2.TurnosSilenciado--;
            }

            ActualizarSlotLabels();
            turno = 1;

            if (esModoIA == true)
            {
                StartCoroutine(EsperarIA());
            }

        }
        else
        { // Cambio a turno de Jugador 1
            mensajeTop.DisplayMessage("Tu turno", 2f, 200);
            turno = 0;
            turno2Label.GetComponent<Image>().color = new Color(1, 1, 1);
            turno1Label.GetComponent<Image>().color = new Color(0.5f, 0.7f, 1);
            posiblesBarcos = posiblesBarcos2;
            jugadorSeleccionado = jugador1;
            jugadorEnemigo = jugador2;

            if (esModoContraReloj)
                if (!(loadGameManager != null && esInicioPartida))
                    contadorDeTurnos++;

            if (loadGameManager == null)
                jugador1.decrementarEnfriamientos();
            else
            {
                if (!esInicioPartida)
                    jugador1.decrementarEnfriamientos();
                else
                    esInicioPartida = false;
            }

            if (jugador1.TurnosContramedidas > 0)
            {
                //DesactivarHabilidad (8);
                jugador1.TurnosContramedidas--;
            }
            if (jugador1.UsoTormentaMisiles == true)
            {
                //DesactivarHabilidad (6);
            }
            if (jugador1.TurnosContramedidas == 0)
            {
                RestaurarContramedidas();
                jugador1.TurnosContramedidas--;
            }
            if (jugador1.TurnosSilenciado == 0)
            {
                jugador1.TurnosSilenciado--;
                mensajeBot.DisplayMessage("El pulso electromagnetico enemigo ha expirado", 2.5f, 500);
            }
            if (jugador1.TurnosSilenciado > 0)
            {
                ColorearBoton();
                DesactivarHabilidades();
                jugador1.TurnosSilenciado--;
            }
            RefrescarTablero(jugador1.TableroAtaque);
            RefrescarTablero(jugador1.TableroPosicion);
        }
        ActualizarSlotLabels();
        modoAtaqueNormal();
        ActualizarTurnoLabel();
        //ColorearBoton ();
    }

    IEnumerator EsperarIA()
    {
        yield return new WaitForSeconds(1);
        IA.Atacar();
    }

    public void ActivarVibracion(Casilla[] casillasAtacadas, int habilidadUtilizada)
    {
        int barcoAtacado;// = jugador1.TableroPosicion.ObtenerIdBarcoConCasilla (casillaAtacada.Fila, casillaAtacada.Columna);
                         //Debug.Log ("Barco Atacado" + barcoAtacado);
       
        if (casillasAtacadas != null)
        {
            foreach (Casilla casilla in casillasAtacadas)
            {
                barcoAtacado = jugador1.TableroPosicion.ObtenerIdBarcoConCasilla(casilla.Fila, casilla.Columna);
                if (barcoAtacado == barcoActual || habilidadUtilizada == Constantes.TORMENTA_MISILES_ID)
                {
					activarVibracion = 1;
                }
            }
        }
        //gameObject.GetComponent<Luz> ();
    }

    public void SetearScoreJugador(long score)
    {
        this.score = score;
    }

    private void VerificarEstadoBarcoActual()
    {
		
		if (turno == 1 && !jugadorEnemigo.ListaBarcos[barcoActual].Vivo)
        {
            for (int i = 0; i < this.jugadorEnemigo.ListaBarcos.Length; i++)
            {
                if (this.jugadorEnemigo.ListaBarcos[i].Vivo)
                    CambiarDeBarco(i);
            }
        }
    }

    public void DesactivarHabilidades(bool mostrarMensaje = false)
    {   
        if (mostrarMensaje)
            MensajeBot.DisplayMessage("Ahora realizá un ataque normal", 2.5f, 400);
        habilidadesActivadas = false;
        modoAtaqueNormal();
    }

    public void DesactivarBotonesEnfriamiento()
    {
        for (int i = 1; i < habilidades.Length; i++)
        {
            if (jugadorSeleccionado.habilidadDisponible(habilidades[i]) == false)
            {
                botones[i].GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f);
            }
        }
    }

    public void ActualizarSlotLabels()
    {
        //Slot[] slots = jugador1.Slots;
        if (jugador1.TurnosSilenciado >= 0){
            GameObject aux = posicionamientoBarcos[barcoSeleccionado].transform.Find("Cabina/CanvasSlots/Label").gameObject;
            GameObject aux2 = posicionamientoBarcos[barcoSeleccionado].transform.Find("Cabina/CanvasSlots/Label/Static").gameObject;
            slotLabels[0].GetComponent<Text>().enabled = false;
            aux.GetComponent<Image>().enabled = false;
            aux2.GetComponent<MeshRenderer>().enabled = true;
            aux2.GetComponent<AnimatedTiledTexture>().enabled = true;
        } else {
            GameObject aux = posicionamientoBarcos[barcoSeleccionado].transform.Find("Cabina/CanvasSlots/Label").gameObject;
            GameObject aux2 = posicionamientoBarcos[barcoSeleccionado].transform.Find("Cabina/CanvasSlots/Label/Static").gameObject; 
            slotLabels[0].GetComponent<Text>().enabled = true;
            aux.GetComponent<Image>().enabled = true;
            aux2.GetComponent<MeshRenderer>().enabled = false;
            aux2.GetComponent<AnimatedTiledTexture>().enabled = false;
        }
        ActualizarSlotDeJugador(jugador1.Slots, 0);
        //ActualizarSlotDeJugador(jugador2.Slots, 1);
    }

    public void ActualizarTurnoLabel()
    {
        //Slot[] slots = jugador1.Slots;
        if (turno == 0){
            GameObject aux = posicionamientoBarcos[barcoSeleccionado].transform.Find("Cabina/LabelTurno/CanvasTurno/TextoTurno").gameObject;
            aux.GetComponent<Text>().text = "Tu turno";
        } else {
            GameObject aux = posicionamientoBarcos[barcoSeleccionado].transform.Find("Cabina/LabelTurno/CanvasTurno/TextoTurno").gameObject;
            aux.GetComponent<Text>().text = "Turno enemigo";
        }
    }

    private void ActualizarSlotDeJugador(Slot[] slots, int label)
    {
        slotLabels[label].GetComponent<Text>().text = " Enfriamientos:";
        if (slots[0].Usado == true)
        {
            slotLabels[label].GetComponent<Text>().text = slotLabels[label].GetComponent<Text>().text + "\n " + slots[0].Enfriamiento + " - " + slots[0].Habilidad.Name;
        }
        else
        {
            slotLabels[label].GetComponent<Text>().text = slotLabels[label].GetComponent<Text>().text + "\n 0 -";
        }
        if (slots[1].Usado == true)
        {
            slotLabels[label].GetComponent<Text>().text = slotLabels[label].GetComponent<Text>().text + "\n " + slots[1].Enfriamiento + " - " + slots[1].Habilidad.Name;
        }
        else
        {
            slotLabels[label].GetComponent<Text>().text = slotLabels[label].GetComponent<Text>().text + "\n 0 -";
        }
        if (slots[2].Usado == true)
        {
            slotLabels[label].GetComponent<Text>().text = slotLabels[label].GetComponent<Text>().text + "\n " + slots[2].Enfriamiento + " - " + slots[2].Habilidad.Name;
        }
        else
        {
            slotLabels[label].GetComponent<Text>().text = slotLabels[label].GetComponent<Text>().text + "\n 0 -";
        }
    }

    public void DesactivarHabilidad(int habilidad)
    {
        botones[habilidad].GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f);
    }

    public void RestaurarContramedidas()
    {
        Casilla[] casillas = jugadorSeleccionado.BarcoContramedidas.GetPosicionesOcupadas();
        for (int i = 0; i < casillas.Length; i++)
        {
            if (jugadorSeleccionado.TableroPosicion.GetCasilla(casillas[i].Fila, casillas[i].Columna).Contramedidas == true)
            {
                jugadorSeleccionado.TableroPosicion.GetCasilla(casillas[i].Fila, casillas[i].Columna).Contramedidas = false;
                if (jugadorSeleccionado.TableroPosicion.GetCasilla(casillas[i].Fila, casillas[i].Columna).Reforzada == 1)
                {
                    if (jugadorEnemigo.TableroAtaque.GetEstadoCasilla(casillas[i].Fila, casillas[i].Columna) == Constantes.REVELADO_CONTRAMEDIDAS)
                    {
                        jugadorEnemigo.TableroAtaque.SetEstadoCasilla(casillas[i].Fila, casillas[i].Columna, Constantes.REVELADO_REFORZADA);
                    }
                }
                else if (jugadorSeleccionado.TableroPosicion.GetCasilla(casillas[i].Fila, casillas[i].Columna).Reforzada == 2)
                {
                    if (jugadorEnemigo.TableroAtaque.GetEstadoCasilla(casillas[i].Fila, casillas[i].Columna) == Constantes.REVELADO_CONTRAMEDIDAS)
                    {
                        jugadorEnemigo.TableroAtaque.SetEstadoCasilla(casillas[i].Fila, casillas[i].Columna, Constantes.REVELADO);
                    }
                }
                else
                {
                    if (jugadorEnemigo.TableroAtaque.GetEstadoCasilla(casillas[i].Fila, casillas[i].Columna) == Constantes.REVELADO_CONTRAMEDIDAS)
                    {
                        jugadorEnemigo.TableroAtaque.SetEstadoCasilla(casillas[i].Fila, casillas[i].Columna, Constantes.REVELADO);
                    }
                }
                RefrescarCasilla(jugadorSeleccionado.TableroPosicion, casillas[i].Fila, casillas[i].Columna);
                RefrescarCasilla(jugadorEnemigo.TableroAtaque, casillas[i].Fila, casillas[i].Columna);
            }
        }
    }

    public void RefrescarCasilla(Tablero tablero, int fila, int columna)
    {
        Casilla aux = tablero.GetCasilla(fila, columna);
        Casilla aux2 = jugador2.TableroPosicion.GetCasilla(fila, columna);
        if (aux.Estado == Constantes.POSICION_BARCO)
        {
            if (aux.Contramedidas == true)
            {
                tablero.Controller.placeMark(fila, columna, new Color(1, 0.5f, 0));
            }
            else if (aux.Reforzada == 1)
            {
                tablero.Controller.placeMark(fila, columna, new Color(1, 0.5f, 0)); // La misma que contramedidas
            }
            else if (aux.Reforzada == 2)
            {
                tablero.Controller.placeMark(fila, columna, new Color(0.5f, 0.5f, 0.5f));
            }
            else if (aux.Atacada == true && aux.Barco.Vivo)
            {
                tablero.Controller.placeMark(fila, columna, new Color(1, 0, 0));
            }
            else if (aux.Atacada == true && !aux.Barco.Vivo)
            {
                tablero.Controller.placeMark(fila, columna, new Color(0.3f, 0, 0));
            }
            else if (barcoActual == jugador1.TableroPosicion.ObtenerIdBarcoConCasilla(fila, columna))
            {
                tablero.Controller.placeMark(fila, columna, new Color(0.6f, 1, 0.6f));
            }
            else
            {
                tablero.Controller.placeMark(fila, columna, new Color(1, 1, 1));
            }
        }
        else if (aux.Estado == Constantes.POSICION_AGUA)
        {
            if (aux.Trampa != 0)
            {
                tablero.Controller.placeMark(fila, columna, new Color(1, 0, 1));
            }
            else if (aux.Atacada == true)
            {
                tablero.Controller.placeMark(fila, columna, new Color(0.3f, 0.3f, 1));
            }
            else
            {
                tablero.Controller.clearMark(fila, columna);
            }
        }
        else if (aux.Estado == Constantes.NO_ATACADO)
        {
            tablero.Controller.clearMark(fila, columna);
        }
        else if (aux.Estado == Constantes.ATACADO_AGUA)
        {
            tablero.Controller.placeMark(fila, columna, new Color(0.3f, 0.3f, 1));
        }
        else if (aux.Estado == Constantes.ATACADO_BARCO && aux2.Barco.Vivo)
        {
            tablero.Controller.placeMark(fila, columna, new Color(1, 0, 0));
        }
        else if (aux.Estado == Constantes.ATACADO_BARCO && !aux2.Barco.Vivo)
        {
            tablero.Controller.placeMark(fila, columna, new Color(0.3f, 0, 0));
        }
        else if (aux.Estado == Constantes.REVELADO)
        {
            tablero.Controller.placeMark(fila, columna, new Color(1, 1, 1));
        }
        else if (aux.Estado == Constantes.REVELADO_CONTRAMEDIDAS)
        {
            tablero.Controller.placeMark(fila, columna, new Color(1, 0.5f, 0));
        }
        else if (aux.Estado == Constantes.REVELADO_REFORZADA)
        {
            tablero.Controller.placeMark(fila, columna, new Color(1, 1, 0));
        }
    }

    private void AcumularScore(long score)
    {
        this.scoreAcumulado += score;
    }

    // Refresca las posiciones seleccionadas en la habilidad que se esta lanzando
    public void LimpiarCasillasSeleccionadas()
    {
        for (int i = 0; i < numCasillasSeleccionadas; i++)
        {
            if (casillasSeleccionadas[i] != null)
            {
                RefrescarCasilla(jugadorSeleccionado.TableroAtaque, casillasSeleccionadas[i].Fila, casillasSeleccionadas[i].Columna);
                RefrescarCasilla(jugadorSeleccionado.TableroPosicion, casillasSeleccionadas[i].Fila, casillasSeleccionadas[i].Columna);
                casillasSeleccionadas[i] = null;
            }
        }
        numCasillasSeleccionadas = 0;
    }

    public void LimpiarCasillasTrampa(Jugador jugador = null, bool activado = false)
    {
        // La variable activado no se usa, por las dudas la dejo ahi por ahora
        if (jugador == null)
            jugador = jugadorEnemigo;
        if (jugador.CasillasTrampa == null)
            return;
        foreach (Casilla cas in jugador.CasillasTrampa)
        {
            if (cas != null && (jugador.TableroPosicion.GetCasilla(cas.Fila, cas.Columna).Trampa == 1 || jugador.TableroPosicion.GetCasilla(cas.Fila, cas.Columna).Trampa == 2))
            {
                jugador.TableroPosicion.GetCasilla(cas.Fila, cas.Columna).Trampa = 0;
                jugador.TableroPosicion.GetCasilla(cas.Fila, cas.Columna).Estado = Constantes.POSICION_AGUA;
                jugador.ControllerPosicion.clearMark(cas.Fila, cas.Columna);
            }
        }
        jugador.TipoTrampa = 0;
        jugador.CasillasTrampa = null;
    }

    // Marco en el tablero alguna las casillas seleccionadas, o retorno 1 si se seleccionaron suficientes casillas para ejecutar una habilidad
    // Tambien sirve para marcar la posicion en donde estoy lanzando un ataque o habilidad, antes de actualizar el tablero
    private bool seleccionarCasilla(int x, int y, int maxCount, TestTableroController tab)
    {
        for (int i = 0; i < numCasillasSeleccionadas; i++)
        {
            if (casillasSeleccionadas[i].Fila == x && casillasSeleccionadas[i].Columna == y)
            {
                return false;
            }
        }

        tab.placeMark(x, y, new Color(1, 1, 0));
        if (numCasillasSeleccionadas < maxCount - 1)
        {
            Casilla aux = new Casilla(x, y, Constantes.POSICION_AGUA);
            casillasSeleccionadas[numCasillasSeleccionadas] = aux;
            numCasillasSeleccionadas++;
            return false;
        }
        else
        {
            numCasillasSeleccionadas = 0;
            return true;
        }
    }

    // Guarda las casillas afectadas por el ataque o habilidad lanzado
    // Al terminar la animacion, se marcan en el tablero
    public bool ApilarCasillaAtacada(int x, int y)
    {
        for (int i = 0; i < numCasillasAtacadas; i++)
        {
            if (casillasSeleccionadas[i].Fila == x && casillasSeleccionadas[i].Columna == y)
            {
                return false;
            }
        }

        if (numCasillasAtacadas < 10)
        {
            Casilla aux = new Casilla(x, y, Constantes.POSICION_AGUA);
            casillasAtacadas[numCasillasAtacadas] = aux;
            numCasillasAtacadas++;
            return true;
        }
        return false;
    }

    // Refresca las casillas afectadas por el ataque o habilidad lanzado en este turno
    public void RefrescarCasillasAtacadas()
    {
        for (int i = 0; i < numCasillasAtacadas; i++)
        {
            if (casillasAtacadas[i] != null)
            {
                RefrescarCasilla(jugadorSeleccionado.TableroAtaque, casillasAtacadas[i].Fila, casillasAtacadas[i].Columna);
                RefrescarCasilla(jugadorSeleccionado.TableroPosicion, casillasAtacadas[i].Fila, casillasAtacadas[i].Columna);
                casillasAtacadas[i] = null;
            }
        }
        numCasillasAtacadas = 0;
    }

    public void ColorearBoton()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            if (i == modoSeleccionado)
            {
                botones[i].GetComponent<Image>().color = new Color(0.5f, 0.7f, 1);
            }
            else
            {
                if (habilidadesActivadas == true && (i != 6 || (i == 6 && turno == 0 && jugador1.UsoTormentaMisiles == false) || (i == 6 && turno == 1 && jugador2.UsoTormentaMisiles == false)) && (i != 8 || (i == 8 && turno == 0 && jugador1.TurnosContramedidas <= 0) || (i == 8 && turno == 1 && jugador2.TurnosContramedidas <= 0)))
                {
                    botones[i].GetComponent<Image>().color = new Color(1, 1, 1);
                }
                else
                {
                    botones[i].GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f);
                }
            }
        }
        DesactivarBotonesEnfriamiento();
    }

    //TODO para mario: verificar si en los llamados a "activar" de cada habilidad estoy pasando los parametros correctos...
    public int EjecutarAccionIA(Casilla casillaAtacable1, Casilla casillaAtacable2, Casilla casillaAtacable3, int modo)
    {
        Casilla atacar1;
        Casilla atacar2;
        Casilla atacar3;
        int barcoRandom = jugador2.GetIndiceBarcoVivo();

        switch (modo)
        {

            case Constantes.ATAQUE_NORMAL_ID:
                atacar1 = jugadorEnemigo.TableroPosicion.GetCasilla(casillaAtacable1.Fila, casillaAtacable1.Columna);
                int r = AtacarUnaPosicion(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, atacar1);
                if (jugador1.TableroPosicion.GetCasilla (casillaAtacable1.Fila, casillaAtacable1.Columna).Trampa != 2)
                {
                    // Entro aca si NO le pegue a una trampa
                    IniciarAnimacionDisparo(barcoRandom, casillaAtacable1, "CannonSimple");
                } else {
                    MensajeBot.DisplayMessage("¡El enemigo impacto en tu trampa sabotaje!", 2.5f, 500);
                    CambiarTurno();
                    return 0;
                }
                if (r == Constantes.IMPACTO_AGUA || r == Constantes.IMPACTO_REFORZADA)
                {
                    //CambiarTurno ();
                    deboCambiarTurno = true;
                    return 0;
                }
                break;

            case Constantes.ARTILLERIA_RAPIDA_ID:
                atacar1 = jugadorEnemigo.TableroPosicion.GetCasilla(casillaAtacable1.Fila, casillaAtacable1.Columna);
                mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Artilleria Rápida", 2.5f, 500);
                ArtilleriaRapida.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, atacar1, barcoRandom, true);
                break;

            case Constantes.PROYECTIL_HE_ID:
                atacar1 = jugadorEnemigo.TableroPosicion.GetCasilla(casillaAtacable1.Fila, casillaAtacable1.Columna);
                mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Proj. Altamente Explosivo", 2.5f, 500);
                ProyectilHE.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, atacar1, barcoRandom, true);
                break;

            case Constantes.TORPEDO_ID:
                atacar1 = jugadorEnemigo.TableroPosicion.GetCasilla(casillaAtacable1.Fila, casillaAtacable1.Columna);
                mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Torpedo", 2.5f, 500);
                Torpedo.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, atacar1, barcoRandom, true);
                break;

            case Constantes.DISPARO_DOBLE_ID:
                atacar1 = jugadorEnemigo.TableroPosicion.GetCasilla(casillaAtacable1.Fila, casillaAtacable1.Columna);
                atacar2 = jugadorEnemigo.TableroPosicion.GetCasilla(casillaAtacable2.Fila, casillaAtacable2.Columna);
                mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Disparo Doble", 2.5f, 500);
                DisparoDoble.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, atacar1, atacar2, barcoRandom, true);
                break;

            case Constantes.REFORZAR_ARMADURA_ID:
                atacar1 = jugadorSeleccionado.TableroPosicion.GetCasilla(casillaAtacable1.Fila, casillaAtacable1.Columna);
                mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Reforzar Armadura", 2.5f, 500);
                ReforzarArmadura.activar(jugadorSeleccionado.TableroPosicion, atacar1, jugadorSeleccionado); //Se pasan estos parametros???
                break;

            case Constantes.TORMENTA_MISILES_ID:
                // Esto deberia checkearse en algun lado
                /*
                if (habilidadesActivadas == false)
                    return;

                // No puedo ejecutar si ya la active anteriormente
                if ((turno == 0 && jugador1.UsoTormentaMisiles == true) || (turno == 1 && jugador2.UsoTormentaMisiles == true))
                    return;

                // No puedo ejecutar si hay menos de 20 posiciones
                if ((turno == 0 && jugador1.PosicionesDisponibles < 20) || (turno == 1 && jugador2.PosicionesDisponibles < 20))
                    return;
                */

                jugadorSeleccionado.UsoTormentaMisiles = true;

                // Esto creo que no aplica para Tormenta de misiles
                // Esa habilidad no lleva enfriamiento
                mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Tormenta de Misiles", 2.5f, 500);
                TormentaDeMisiles.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, barcoRandom, true);
                break;

		case Constantes.PULSO_ELECTROMAGNETICO_ID:

                // Esto deberia checkearse en algun lado
                /*
                if (habilidadesActivadas == false)
                    return;
                if (jugadorSeleccionado.habilidadDisponible (PulsoElectromagnetico) == false) { return; }
                if (jugadorSeleccionado.haySlotLibre () == false) { return; }
                */

			if (PulsoElectromagnetico.activar (jugadorEnemigo) == true) {
				mensajeBot.DisplayMessage ("El enemigo utilizo la habilidad Pulso Electromagnetico", 2.5f, 600);
				AudioManager.PlayAudioPoint("Sonidos/Power Failure", player.transform.position, 500);
			}

                break;

            case Constantes.CONTRAMEDIDAS_ID: //NO ENTRA EN LA EXPO
                break;

            case Constantes.RADAR_PASIVO_ID: //NO ENTRA EN LA EXPO
                break;

            case Constantes.SABOTAJE_ID:
                atacar1 = jugadorSeleccionado.TableroPosicion.GetCasilla(casillaAtacable1.Fila, casillaAtacable1.Columna);
                atacar2 = jugadorSeleccionado.TableroPosicion.GetCasilla(casillaAtacable2.Fila, casillaAtacable2.Columna);
                atacar3 = jugadorSeleccionado.TableroPosicion.GetCasilla(casillaAtacable3.Fila, casillaAtacable3.Columna);
                mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Sabotaje", 2.5f, 600);
                Sabotaje.activar(jugadorSeleccionado, atacar1, atacar2, atacar3);
                break;
        }
        return 1;
    }

    public void ImpactoSabotaje(string mensaje, Habilidad hab, bool enviarMsg = false, int habId = 0)
    {

        MensajeBot.DisplayMessage(mensaje, 2.5f, 500);
		AudioManager.PlayAudioPoint("Sonidos/Fuse Burning", player.transform.position, 500);
        LimpiarCasillasTrampa();
        if (hab != null)
            añadirEnfriamiento(hab);
        if (enviarMsg)
            EnviarCasillasAtacadas(null, habId);
        CambiarTurno();
        // REPRODUCIR SONIDO

    }

    public bool EsFinDePartida()
    {
        if (turnosMaximosAlcanzados)
            return true;

        foreach (Barco barco in this.jugadorEnemigo.ListaBarcos)
        {
            if (barco.Vivo)
                return false;
        }
        return true;
    }

    public void TurnosMaximosAlcanzados()
    {
        if (esModoContraReloj && jugadorSeleccionado == jugador2 && contadorDeTurnos == cantidadDeTurnosMaxima)
        {
            turnosMaximosAlcanzados = true;
            if (!esModoIA)
            { 
                Mensaje msj = new Mensaje();
                msj.finalizoPartida = true;
                MultiplayerManager.Instance.EnviarMensaje(JsonUtility.ToJson(msj));
            }
        }
    }

    // SOLO PARA ATAQUES Y HABILIDADES DEL JUGADOR, NO IA
    public void IniciarAnimacionDisparo(int barcoDispara, Casilla casilla, string cañon, float offsetx = 0, float offsety = 0)
    {
        ActualizarSlotLabels();
        animacionEnCurso = true;
        int barcoImpactado = jugadorEnemigo.TableroPosicion.ObtenerIdBarcoConCasilla(casilla.Fila, casilla.Columna);
        Barco barcoObj = jugadorSeleccionado.TableroPosicion.Barcos[barcoDispara];
        AnimacionCanon script;

        script = barcoObj.BarcoFisico.transform.Find(cañon + "/base/chasis").gameObject.GetComponent<AnimacionCanon>();
        if (!script)
            return;

        if (barcoImpactado != -1 && barcoObj.Vivo)
        {
            Barco targetObj = jugadorEnemigo.TableroPosicion.Barcos[barcoImpactado];
            script.Disparar(targetObj.BarcoFisico, offsetx, offsety);
        }
        else
        {
            script.Disparar(CasillaAPosicion(casilla, markerBarcosJugador.transform.position.y, 0f, 0f), offsetx, offsety);
        }
    }

    public void FinalizoAnimacionMultiplayer(){

        esMiAtaque = false;
        //FinalizoAnimacion();
        animacionEnCurso = false;

        if (huboImpactoReforzada){

            huboImpactoReforzada = false;
            MensajeBot.DisplayMessage("¡Impacto en posicion reforzada!", 2.5f, 400);

        }

        mensajeEspera.DisplayMessage(false);

        RefrescarTablero(jugador1.TableroAtaque);
        RefrescarTablero(jugador1.TableroPosicion);
        if (deboCambiarTurno)
        {
            CambiarTurno();
            deboCambiarTurno = false;
        }
        else
        {
            if (turno == 1 && esModoIA)
            {
                IA.Atacar();
            }
        }

    }

    public void FinalizoAnimacion()
    {
        
        /*
		Debug.Log("FINALIZO_ANIMACION: jugador1.TableroAtaque=" + jugador1.TableroAtaque);
		Debug.Log("FINALIZO_ANIMACION: jugador1.TableroPosicion=" + jugador1.TableroPosicion);
		Debug.Log("FINALIZO_ANIMACION: deboCambiarTurno=" + deboCambiarTurno);
		Debug.Log("FINALIZO_ANIMACION: turno=" + turno);
		*/
		if (activarVibracion == 1) {
			sonidosDelJuego = GameObject.Find("SonidosDelJuego").GetComponents<AudioSource>()[1];
			sonidosDelJuego.Play();
			vibracion = GameObject.Find("VibracionCamara").GetComponent<VibracionCamara>();
			vibracion.shakeDuration = 1;
			activarVibracion = 0;
		}
		ActivarLuz ();
        VerificarEstadoBarcoActual ();

        if (!esModoIA && esMiAtaque){
            mensajeEspera.DisplayMessage(true);
            return;
        }

        if (!esModoIA && recibiMensaje){
            Mensaje msj = new Mensaje();
            msj.finalizoAnimacion = true;
            MultiplayerManager.Instance.EnviarMensaje(JsonUtility.ToJson(msj));
            recibiMensaje = false;
        } 

        animacionEnCurso = false;

        if (huboImpactoReforzada){

            huboImpactoReforzada = false;
            MensajeBot.DisplayMessage("¡Impacto en posicion reforzada!", 2.5f, 400);

        }

        RefrescarTablero(jugador1.TableroAtaque);
        RefrescarTablero(jugador1.TableroPosicion);
        if (deboCambiarTurno)
        {
            CambiarTurno();
            deboCambiarTurno = false;
        }
        else
        {
            if (turno == 1 && esModoIA)
            {
                IA.Atacar();
            }
        }
    }

    public void EnviarCasillasAtacadas(Casilla[] casillas, int tipo)
    {
        //Debug.Log("EnviarCasillasAtacadas - msg.listaCasillas: " + casillas);
        if (esModoIA) return;
        Mensaje msg = new Mensaje();
        msg.jugador = idJugador;
        msg.tipo = tipo;
        msg.listaCasillas = casillas;
        msg.tocoTrampa = (casillas == null ? true : false);
        if (tipo != Constantes.SABOTAJE_ID && tipo != Constantes.PULSO_ELECTROMAGNETICO_ID && tipo != Constantes.REFORZAR_ARMADURA_ID)
            esMiAtaque = true;
        MultiplayerManager.Instance.EnviarMensaje(JsonUtility.ToJson(msg));

    }

    public void EjecutarAccionEnemigo(Mensaje msg)
    {

        Debug.Log("EjecutarAccionEnemigo - msg.listaCasillas: " + msg.listaCasillas);
        string jugador = msg.jugador;
        int tipo = msg.tipo;
        Casilla casilla;
        Casilla casilla2;
        Casilla casilla3;
        int barcoRandom = jugador2.GetIndiceBarcoVivo();

        switch (msg.tipo)
        {
            case Constantes.ATAQUE_NORMAL_ID:

                //Casilla casilla = new Casilla(msg.posx,msg.posy,0);

                casilla = jugadorEnemigo.TableroPosicion.GetCasilla(msg.listaCasillas[0].Fila, msg.listaCasillas[0].Columna);
                int r = AtacarUnaPosicion(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, casilla);
                if (jugador1.TableroPosicion.GetCasilla (msg.listaCasillas[0].Fila, msg.listaCasillas[0].Columna).Trampa != 2)
                {
                    // Entro aca si NO le pegue a una trampa
                    IniciarAnimacionDisparo(barcoRandom, casilla, "CannonSimple");
                    recibiMensaje = true;
                } else {
                    MensajeBot.DisplayMessage("¡El enemigo impacto en tu trampa sabotaje!", 2.5f, 500);
                    CambiarTurno();
                    break;
                }
                if (r == Constantes.IMPACTO_AGUA || r == Constantes.IMPACTO_REFORZADA)
                {
                    //CambiarTurno ();
                    deboCambiarTurno = true;
                    //return 0;
                }

                break;

            case Constantes.ARTILLERIA_RAPIDA_ID:
                // FIXME: Controlar que los mensajes de ataque enemigo los proceso SOLO CUANDO NO ES MI TURNO
                // Si localmente sigo en mi turno, debo retrasar el procesamiento del ataque hasta que cambie
                if (!msg.tocoTrampa)
                {
                    mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Artilleria Rápida", 2.5f, 500);
                    //casilla = jugadorEnemigo.TableroPosicion.GetCasilla (msg.listaCasillas[0].Fila,msg.listaCasillas[0].Columna);
                    ArtilleriaRapida.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, barcoRandom, true, msg.listaCasillas);
                    recibiMensaje = true;
                }
                else
                {
                    ArtilleriaRapida.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, barcoRandom, true, null);
                }
                break;


            case Constantes.PROYECTIL_HE_ID:

                if (!msg.tocoTrampa)
                {
                    casilla = jugadorEnemigo.TableroPosicion.GetCasilla(msg.listaCasillas[0].Fila, msg.listaCasillas[0].Columna);
                    mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Proj. Altamente Explosivo", 2.5f, 500);
                    recibiMensaje = true;
                    ProyectilHE.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, msg.listaCasillas[0], barcoRandom, true);
                }
                else
                {
                    ProyectilHE.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, null, barcoRandom, true);
                }
                break;


            case Constantes.TORPEDO_ID:
                if (!msg.tocoTrampa)
                {
                    casilla = jugadorEnemigo.TableroPosicion.GetCasilla(msg.listaCasillas[0].Fila, msg.listaCasillas[0].Columna);
                    mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Torpedo", 2.5f, 500);
                    Torpedo.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, msg.listaCasillas[0], barcoRandom, true);
                    recibiMensaje = true;
                }
                else
                {
                    Torpedo.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, null, barcoRandom, true);
                }
                break;


            case Constantes.DISPARO_DOBLE_ID:
                if (!msg.tocoTrampa)
                {
                    casilla = jugadorEnemigo.TableroPosicion.GetCasilla(msg.listaCasillas[0].Fila, msg.listaCasillas[0].Columna);
                    casilla2 = jugadorEnemigo.TableroPosicion.GetCasilla(msg.listaCasillas[1].Fila, msg.listaCasillas[1].Columna);
                    mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Disparo Doble", 2.5f, 500);
                    DisparoDoble.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, casilla, casilla2, barcoRandom, true);
                    recibiMensaje = true;
                }
                else
                {
                    DisparoDoble.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, null, null, barcoRandom, true);
                }
                break;

            case Constantes.REFORZAR_ARMADURA_ID:
                casilla = jugadorSeleccionado.TableroPosicion.GetCasilla(msg.listaCasillas[0].Fila, msg.listaCasillas[0].Columna);
                mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Reforzar Armadura", 2.5f, 500);
                ReforzarArmadura.activar(jugadorSeleccionado.TableroPosicion, casilla, jugadorSeleccionado, true); //Se pasan estos parametros???
                break;

            case Constantes.TORMENTA_MISILES_ID:
                jugadorSeleccionado.UsoTormentaMisiles = true;

                if (!msg.tocoTrampa)
                {
                    mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Tormenta de Misiles", 2.5f, 500);
                    //casilla = jugadorEnemigo.TableroPosicion.GetCasilla (msg.listaCasillas[0].Fila,msg.listaCasillas[0].Columna);
                    TormentaDeMisiles.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, barcoRandom, true, msg.listaCasillas);
                    recibiMensaje = true;
                }
                else
                {
                    TormentaDeMisiles.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, barcoRandom, true, null);
                }

                break;

            case Constantes.PULSO_ELECTROMAGNETICO_ID:

                // Esto deberia checkearse en algun lado
                //if (habilidadesActivadas == false)
                //	return;
                //if (jugadorSeleccionado.habilidadDisponible (PulsoElectromagnetico) == false) { return; }
                //if (jugadorSeleccionado.haySlotLibre () == false) { return; }

                if (PulsoElectromagnetico.activar(jugadorEnemigo) == true)
                {
				mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Pulso Electromagnetico", 2.5f, 600);
				AudioManager.PlayAudioPoint("Sonidos/Power Failure", player.transform.position, 500);
                    ActualizarSlotLabels();
                }

                break;

            case Constantes.CONTRAMEDIDAS_ID: //NO ENTRA EN LA EXPO
                break;

            case Constantes.RADAR_PASIVO_ID: //NO ENTRA EN LA EXPO
                break;

            case Constantes.SABOTAJE_ID:
                casilla = jugadorSeleccionado.TableroPosicion.GetCasilla(msg.listaCasillas[0].Fila, msg.listaCasillas[0].Columna);
                casilla2 = jugadorSeleccionado.TableroPosicion.GetCasilla(msg.listaCasillas[1].Fila, msg.listaCasillas[1].Columna);
                casilla3 = jugadorSeleccionado.TableroPosicion.GetCasilla(msg.listaCasillas[2].Fila, msg.listaCasillas[2].Columna);
                mensajeBot.DisplayMessage("El enemigo utilizo la habilidad Sabotaje", 2.5f, 600);
                Sabotaje.activar(jugadorSeleccionado, casilla, casilla2, casilla3);
                break;
        }

		ActivarVibracion (msg.listaCasillas, msg.tipo);

    }

    public long RegistrarScore()
    {
        if (this.finalizoPartida && registroUnicoDeScore)
        {
            if (this.cantidadDeHabilidadesUtilizadas <= 3)
            {
                this.scoreAcumulado += 200;
                Debug.Log("PUNTOS ACUMULADOS CON HABILIDADES: " + 200 + "// CANTIDAD DE HABILIDADES UTILIZADAS: " + this.cantidadDeHabilidadesUtilizadas);
                //mensajeBot.DisplayMessage("Utilizaste " + cantidadDeHabilidadesUtilizadas + " habilidades: ¡Sumaste 200 Puntos!", 2.5f, 600);
            }
            else if (this.cantidadDeHabilidadesUtilizadas <= 5)
            {
                this.scoreAcumulado += 150;
                Debug.Log("PUNTOS ACUMULADOS CON HABILIDADES: " + 150 + "// CANTIDAD DE HABILIDADES UTILIZADAS: " + this.cantidadDeHabilidadesUtilizadas);
                //mensajeBot.DisplayMessage("Utilizaste " + cantidadDeHabilidadesUtilizadas + " habilidades: ¡Sumaste 150 Puntos!", 2.5f, 600);
            }
            else if (this.scoreAcumulado <= 7)
            {
                this.scoreAcumulado += 100;
                Debug.Log("PUNTOS ACUMULADOS CON HABILIDADES: " + 100 + "// CANTIDAD DE HABILIDADES UTILIZADAS: " + this.cantidadDeHabilidadesUtilizadas);
                //mensajeBot.DisplayMessage("Utilizaste " + cantidadDeHabilidadesUtilizadas + " habilidades: ¡Sumaste 100 Puntos!", 2.5f, 600);
            }

            Debug.Log("SCORE ACUMULADO EN ESTA PARTIDA: " + this.scoreAcumulado);
            MultiplayerManager.Instance.PostearPuntaje(this.scoreAcumulado + this.score);
            registroUnicoDeScore = false;
        }
        return this.scoreAcumulado;
    }

    public void RivalAbandono(long total)
    { 
        mensajeTop.DisplayMessage("¡EL RIVAL ABANDONO LA PARTIDA!", 10f, 600);
        mensajeBot.DisplayMessage("¡Sumaste " + total + " Puntos en esta partida!", 10f, 600);
    }

    //Ejecuta las habilidades especiales
    public void EjecutarAccion(int x, int y, TestTableroController tablero)
    {
        Casilla casilla;
/* 
        Debug.Log("x: " + x);
        Debug.Log("y:" + y);
        Debug.Log("tablerocontroller: " + tablero);
        Debug.Log("jugador1.ControllerPosicion" + jugador1.ControllerPosicion);
        Debug.Log("modoSeleccionado: " + modoSeleccionado);
        Debug.Log("jugador1.TableroPosicion: " + jugador1.TableroPosicion);
        Debug.Log("jugador1.TableroPosicion.GetCasilla (x, y): " + jugador1.TableroPosicion.GetCasilla(x, y));
        Debug.Log("barcoActual: " + barcoActual);
*/

        //FIXME: Debemos contemplar que hay mas habilidades que utilizan tablero posicion.
        //Por el momento solo consideramos reforzar armadura.
        if (tablero == jugador1.ControllerPosicion && modoSeleccionado == Constantes.ATAQUE_NORMAL_ID)
        {
            casilla = jugador1.TableroPosicion.GetCasilla(x, y);
            int numeroBarcoSeleccionado = jugador1.TableroPosicion.ObtenerIdBarcoConCasilla(casilla.Fila, casilla.Columna);
            if (numeroBarcoSeleccionado != -1)
            {
                if (numeroBarcoSeleccionado == barcoActual)
                {
                    //TODO: Mostrar un canvas o algo...
                    mensajeBot.DisplayMessage("Ya te encuentras en ese barco", 3f, 200);
                    Debug.Log("Ya te encontras en el barco seleccionado");
                }
                else if (jugador1.TableroPosicion.GetCasilla(casilla.Fila, casilla.Columna).Barco.Vivo)
                {
                    // Se asociaron las referencias de los tableros para que el barco nuevo este actualizado.
                    // Necesito transferir el estado actual del tablero fisico. Redibujar todas las casillas trae problemas.
                    CambiarDeBarco(numeroBarcoSeleccionado);
                }
                else
                {
                    //TODO: Mostrar un canvas o algo...
                    mensajeBot.DisplayMessage("Ese barco ya ha sido destruido", 3f, 200);
                    Debug.Log("No es posible trasladarse a este barco porque ha sido hundido");
                }
            }
        }

        // Si hay una animacion no hago nada
        if (animacionEnCurso) return;

        switch (modoSeleccionado)
        {
            case Constantes.ATAQUE_NORMAL_ID: //AtaqueNormal.Id
                casilla = jugadorEnemigo.TableroPosicion.GetCasilla(x, y);
                if (tablero != jugadorSeleccionado.ControllerAtaque)
                    return;

                if (!jugadorSeleccionado.TableroAtaque.GetCasilla(x, y).Atacada)
                {
                    int r = AtacarUnaPosicion(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, casilla);
                    //MultiplayerManager.Instance.EnviarMensaje(idJugador+"|"+Constantes.ATAQUE_NORMAL_ID+"|"+x+"|"+y);
                    if (esModoIA == false)
                    {
                        Mensaje msg = new Mensaje();
                        msg.jugador = idJugador;
                        msg.tipo = Constantes.ATAQUE_NORMAL_ID;
                        msg.listaCasillas = new Casilla[1];
                        //Debug.Log("VERRRRR: msg.listaCasillas[0]:" + msg.listaCasillas[0]);
                        msg.listaCasillas[0] = new Casilla(x, y);
                        //msg.listaCasillas[0].Fila =	x;
                        //msg.listaCasillas[0].Columna =	y;
                        esMiAtaque = true;
                        MultiplayerManager.Instance.EnviarMensaje(JsonUtility.ToJson(msg));
                    }

                    if (jugadorEnemigo.TableroPosicion.GetCasilla (x, y).Trampa != 2)
                    {
                        // Entro aca si NO le pegue a una trampa
                        IniciarAnimacionDisparo(barcoActual, casilla, "CannonSimple", 0f, 0f);
                        seleccionarCasilla(x, y, 1, jugadorSeleccionado.ControllerAtaque);
                    } else {
                        MensajeBot.DisplayMessage("¡Has impactado en la trampa sabotaje enemiga!", 2.5f, 500);
                        CambiarTurno();
                        break;
                    }

                    if (r == Constantes.IMPACTO_AGUA || r == Constantes.IMPACTO_REFORZADA)
                    {
                        //CambiarTurno ();
                        deboCambiarTurno = true;
                    } else if(r == Constantes.IMPACTO_BARCO)
                    {
                        if (!esModoIA) 
                        {
                            AcumularScore(10);
                            Debug.Log("SUMASTE +10");
                            Debug.Log("PUNTAJE ACUMULADO: " + scoreAcumulado);
                        }

                    }
                }
                break;

            case Constantes.ARTILLERIA_RAPIDA_ID: //ArtilleriaRapida.Id
                this.cantidadDeHabilidadesUtilizadas++;
                casilla = jugadorEnemigo.TableroPosicion.GetCasilla(x, y);
                if (tablero != jugadorSeleccionado.ControllerAtaque)
                    return;
                if (jugadorSeleccionado.puedeAñadirHabilidad(ArtilleriaRapida) == -1)
                    return;
                ArtilleriaRapida.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, casilla, barcoActual, false);              

                seleccionarCasilla(x, y, 1, jugadorSeleccionado.ControllerAtaque);

                /*
                if (esModoIA == false) {			
                    Mensaje msg = new Mensaje();
                    msg.jugador = idJugador;
                    msg.tipo = Constantes.ARTILLERIA_RAPIDA_ID;
                    msg.listaCasillas = ArtilleriaRapida.listaCasillas;
                    Debug.Log("VERRRRR: msg.listaCasillas[0]:" + msg.listaCasillas[0]);
                    //msg.listaCasillas[0] = new Casilla(x, y);
                    //msg.listaCasillas[0].Fila =	x;
                    //msg.listaCasillas[0].Columna =	y;
                    MultiplayerManager.Instance.EnviarMensaje(JsonUtility.ToJson(msg));
                }
                */
                break;


            case Constantes.PROYECTIL_HE_ID: //ProyectilHE.Id
                this.cantidadDeHabilidadesUtilizadas++;
                casilla = jugadorEnemigo.TableroPosicion.GetCasilla(x, y);
                if (tablero != jugadorSeleccionado.ControllerAtaque)
                    return;
                if (jugadorSeleccionado.puedeAñadirHabilidad(ProyectilHE) == -1)
                    return;
                ProyectilHE.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, casilla, barcoActual, false);
                seleccionarCasilla(x, y, 1, jugadorSeleccionado.ControllerAtaque);

                break;

            case Constantes.TORPEDO_ID: //Torpedo.Id
                this.cantidadDeHabilidadesUtilizadas++;
                casilla = jugadorEnemigo.TableroPosicion.GetCasilla(x, y);
                if (tablero != jugadorSeleccionado.ControllerAtaque)
                    return;
                if (jugadorSeleccionado.puedeAñadirHabilidad(Torpedo) == -1)
                    return;
                Torpedo.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, casilla, barcoActual, false);
                seleccionarCasilla(x, y, 1, jugadorSeleccionado.ControllerAtaque);               

                break;

            case Constantes.DISPARO_DOBLE_ID: //DisparoDoble.Id
                this.cantidadDeHabilidadesUtilizadas++;
                casilla = jugadorEnemigo.TableroPosicion.GetCasilla(x, y);
                if (tablero != jugadorSeleccionado.ControllerAtaque)
                    return;
                if (jugadorSeleccionado.TableroAtaque.GetEstadoCasilla(x, y) != Constantes.NO_ATACADO && jugadorSeleccionado.TableroAtaque.GetEstadoCasilla(x, y) != Constantes.REVELADO && jugadorSeleccionado.TableroAtaque.GetEstadoCasilla(x, y) != Constantes.REVELADO_REFORZADA)
                    return;
                if (jugadorSeleccionado.puedeAñadirHabilidad(DisparoDoble) == -1)
                    return;

                if (seleccionarCasilla(x, y, 2, jugadorSeleccionado.ControllerAtaque))
                {
                    DisparoDoble.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, casillasSeleccionadas[0], casilla, barcoActual, false);
                }               

                break;

            case Constantes.REFORZAR_ARMADURA_ID: //ReforzarArmadura.Id
                this.cantidadDeHabilidadesUtilizadas++;
                casilla = jugadorSeleccionado.TableroPosicion.GetCasilla(x, y);
                if (tablero != jugadorSeleccionado.ControllerPosicion)
                    return;
                if (jugadorSeleccionado.puedeAñadirHabilidad(ReforzarArmadura) == -1)
                    return;
                ReforzarArmadura.activar(jugadorSeleccionado.TableroPosicion, casilla, jugadorSeleccionado, false);             

                break;

            case Constantes.PULSO_ELECTROMAGNETICO_ID: //PulsoElectromagnetico.Id                     
                break;

            case Constantes.CONTRAMEDIDAS_ID: //Contramedidas.Id
                this.cantidadDeHabilidadesUtilizadas++;
                casilla = jugadorSeleccionado.TableroPosicion.GetCasilla(x, y);
                if (tablero != jugadorSeleccionado.ControllerPosicion)
                    return;
                if (jugadorSeleccionado.puedeAñadirHabilidad(Contramedidas) == -1)
                    return;
                Contramedidas.activar(jugadorSeleccionado.TableroPosicion, casilla, jugadorSeleccionado);             

                break;

            case Constantes.RADAR_PASIVO_ID: //RadarPasivo.Id
                this.cantidadDeHabilidadesUtilizadas++;
                casilla = jugadorSeleccionado.TableroPosicion.GetCasilla(x, y);
                if (tablero != jugadorSeleccionado.ControllerPosicion)
                    return;
                if (jugadorSeleccionado.TableroPosicion.GetEstadoCasilla(x, y) != Constantes.POSICION_AGUA || jugadorSeleccionado.TableroPosicion.GetCasilla(x, y).Atacada == true || jugadorSeleccionado.TableroPosicion.GetCasilla(x, y).Trampa != 0)
                    return;
                if (jugadorSeleccionado.puedeAñadirHabilidad(RadarPasivo) == -1)
                    return;

                if (seleccionarCasilla(x, y, 3, jugadorSeleccionado.ControllerPosicion))
                {
                    RadarPasivo.activar(jugadorSeleccionado, casillasSeleccionadas[0], casillasSeleccionadas[1], casilla);
                }

                break;

            case Constantes.SABOTAJE_ID: //Sabotaje.Id
                this.cantidadDeHabilidadesUtilizadas++;
                casilla = jugadorSeleccionado.TableroPosicion.GetCasilla(x, y);
                if (tablero != jugadorSeleccionado.ControllerPosicion)
                    return;
                if (jugadorSeleccionado.TableroPosicion.GetEstadoCasilla(x, y) != Constantes.POSICION_AGUA || jugadorSeleccionado.TableroPosicion.GetCasilla(x, y).Atacada == true || jugadorSeleccionado.TableroPosicion.GetCasilla(x, y).Trampa != 0)
                    return;
                if (jugadorSeleccionado.puedeAñadirHabilidad(Sabotaje) == -1)
                    return;

                if (seleccionarCasilla(x, y, 3, jugadorSeleccionado.ControllerPosicion))
                {
                    Sabotaje.activar(jugadorSeleccionado, casillasSeleccionadas[0], casillasSeleccionadas[1], casilla, false);
                }               

                break;
        }
    }

    public void CambiarDeBarco(int barcoSeleccionado, bool esInicioDeJuego = false)
    {
        this.barcoSeleccionado = barcoSeleccionado;
        this.esInicioDeJuego = esInicioDeJuego;

        if (!esInicioDeJuego)
        {
            fadePantalla.IniciarFade(CambiarDeBarcoCallBack, 1);
        }
        else
        {
            CambiarDeBarcoCallBack();
        }
    }

    public void CambiarDeBarcoCallBack()
    {
        //Activo el top y desactivo la cabina del barco que me estoy yendo
        if (canvasUI != null)
        {
            canvasUI.SetActive(true);
        }
        if (!esInicioDeJuego)
        {
            posicionamientoBarcos[barcoActual].transform.Find("Barco" + jugador1.ListaBarcos[barcoActual].GetTipoBarco() + "Pos_Top").gameObject.SetActive(true);
            posicionamientoBarcos[barcoActual].transform.Find("Cabina").gameObject.SetActive(false);
        }

        //Desactivo el top y activo la cabina del barco al que estoy yendo
        posicionamientoBarcos[barcoSeleccionado].transform.Find("Barco" + jugador1.ListaBarcos[barcoSeleccionado].GetTipoBarco() + "Pos_Top").gameObject.SetActive(false);
        posicionamientoBarcos[barcoSeleccionado].transform.Find("Cabina").gameObject.SetActive(true);

        TestTableroController aux = posicionamientoBarcos[barcoSeleccionado].transform.Find("Cabina/PizarraAtaque/TableroPrueba2").gameObject.GetComponent<TestTableroController>(); // Jugador 1 - Tablero de ataques;
        jugador1.ControllerAtaque.TransferirEstado(aux);
        jugador1.ControllerAtaque.clearMarks();
        jugador1.ControllerAtaque = aux;
        aux = posicionamientoBarcos[barcoSeleccionado].transform.Find("Cabina/PizarraAtaque/TableroPrueba3").gameObject.GetComponent<TestTableroController>(); // Jugador 1 - Tablero de posicion
        jugador1.ControllerPosicion.TransferirEstado(aux);
        jugador1.ControllerPosicion.clearMarks();
        jugador1.ControllerPosicion = aux;
        jugador1.TableroAtaque.Controller = jugador1.ControllerAtaque;
        jugador1.TableroPosicion.Controller = jugador1.ControllerPosicion;
        //RefrescarTablero (jugador1.TableroAtaque);
        //RefrescarTablero (jugador1.TableroPosicion);
        slotLabels[0] = posicionamientoBarcos[barcoSeleccionado].transform.Find("Cabina/CanvasSlots/Label/Text").gameObject;
        ActualizarSlotLabels();
        ActualizarTurnoLabel();

        //guardo referencia de nuevo barco actual
        barcoActual = barcoSeleccionado;

        ActivarLuz();
        RefrescarTablero(jugador1.TableroPosicion);

        //Realizo el cambio de barco fisico
        player.transform.position = posicionamientoBarcos[barcoSeleccionado].transform.Find("Cabina").transform.position;
		//FIXME: Comente la lluvia porque tiraba errores y ahora esta desactivada
        //lluvia.transform.position = posicionamientoBarcos[barcoActual].transform.position;
    }

    private void ActivarLuz()
    {
        luz = posicionamientoBarcos[barcoActual].transform.Find("Cabina/LuzCabina").gameObject.GetComponent<Luz>();
        int validaPosiciones = 0;
        foreach (Casilla casilla in jugador1.ListaBarcos[barcoActual].GetPosicionesOcupadas())
        {
            if (casilla.Atacada)
            {
                validaPosiciones++;
            }
        }
        if (validaPosiciones == (jugador1.ListaBarcos[barcoActual].GetTipoBarco() - 1))
        {
            luz.activarLuz = 1;
        }
        else
        {
            //luz.activarLuz = 0;
            //luz.Apagar ();
        }
    }

    private void RefrescarTablero(Tablero tableroJugador)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                RefrescarCasilla(tableroJugador, i, j);
            }
        }
    }

    // Esto esta aca nomas para que la habilidad pueda ordenarle al administrador que llene un slot con un enfriamiento
    public void añadirEnfriamiento(Habilidad hab)
    {
        jugadorSeleccionado.añadirHabilidad(hab);
    }

    //La idea de las habilidades de ATAQUE es que en el turno del jugador1 se envie como parametro
    //el jugadorSeleccionado.TableroAtaque del jugador1, y el tableroPosicion del jugador2
    //Para las habilidades de NO ATAQUE se envia por parametro el tableroPosicion del jugador1

    public void marcarPosibleBarco(Tablero tabAtaque, Casilla casilla)
    {
        jugadorSeleccionado.ControllerAtaque.placeMark(casilla.Fila, casilla.Columna, new Color(0, 1, 0));
        posiblesBarcos.Add(casilla);
    }

    public void chequearPosibleBarco(Tablero tabAtaque, Tablero tabPosicion, Casilla casilla)
    {
        if (posiblesBarcos.Contains(casilla))
        {
            // Compruebo que no haya otras casillas marcadas como "posible barco" que contengan realmente un barco
            foreach (Casilla cas in posiblesBarcos)
            {
                if (cas.HayBarco == true)
                {
                    return;
                }
            }
            // Si ya no hay barcos en las casillas marcadas como "posible barco", entonses las revelo a todas
            foreach (Casilla cas in posiblesBarcos)
            {
                AtacarUnaPosicion(tabAtaque, tabPosicion, casilla);
            }
        }
    }

    public int AtacarUnaPosicion(Tablero tabAtaque, Tablero tabPosicion, Casilla casilla, bool usoHabilidad = false, bool registraScore = false)
    {
        //casilla = new Casilla(3, 4, 1); //Hardcodeado para prueba, descomentar para probar este ataque
        int fila = casilla.Fila;
        int columna = casilla.Columna;
        if (tabPosicion.GetHayBarco(fila, columna) == true)
        {
            // Impacto en barco con contramedidas activado
            if (tabPosicion.GetCasilla(fila, columna).Contramedidas == true)
            {
                if (tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.NO_ATACADO || tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.REVELADO || tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.REVELADO_REFORZADA)
                {
                    tabAtaque.SetEstadoCasilla(fila, columna, Constantes.REVELADO_CONTRAMEDIDAS);
                    //RefrescarCasilla (tabAtaque, fila, columna);
                    tabPosicion.GetCasilla(fila, columna).Revelada = true;
                }
                return Constantes.IMPACTO_CONTRAMEDIDAS;
            }
            // Impacto en posicion reforzada
            else if (tabPosicion.GetCasilla(fila, columna).Atacada == false && tabPosicion.GetCasilla(fila, columna).Reforzada == 1)
            {
                if (tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.NO_ATACADO || tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.REVELADO || tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.REVELADO_REFORZADA)
                {
                    tabAtaque.SetEstadoCasilla(fila, columna, Constantes.REVELADO);
                    tabPosicion.GetCasilla(fila, columna).Reforzada = 2;
                    //RefrescarCasilla (tabAtaque, fila, columna);
                    //RefrescarCasilla (tabPosicion, fila, columna);
                }
                huboImpactoReforzada = true;
                return Constantes.IMPACTO_REFORZADA;
            }
            // Impacto en barco
            else if (tabPosicion.GetEstadoCasilla(fila, columna) == Constantes.POSICION_BARCO && tabPosicion.GetCasilla(fila, columna).Atacada == false && tabPosicion.GetCasilla(fila, columna).Reforzada != 1)
            {
                if (tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.NO_ATACADO || tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.REVELADO || tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.REVELADO_REFORZADA)
                {
                    tabAtaque.SetEstadoCasilla(fila, columna, Constantes.ATACADO_BARCO); //Ataque exitoso a un barco
                    tabAtaque.GetCasilla(fila, columna).Atacada = true;
                    tabPosicion.GetCasilla(fila, columna).Atacada = true;
                    tabPosicion.GetCasilla(fila, columna).Reforzada = 0;
                    //Suma 5 puntos por cada vez que le pegue a una posicion de un barco usando una habilidad
                    if (registraScore && !esModoIA) 
                    {
                        AcumularScore(5);
                        Debug.Log("SUMASTE +5");
                        Debug.Log("PUNTAJE ACUMULADO: " + scoreAcumulado);
                    }
                    // Si el turno es del jugador IA, añado esa casilla a una lista para luego buscar adyacentes
                    if (turno == 1 && usoHabilidad && esModoIA)
                    {   
                        Debug.Log("Añadiendo casilla de hit: ("+casilla.Fila+","+casilla.Columna+")");
                        ultimosHits.Add(casilla);
                    }
                    //RefrescarCasilla (tabAtaque, fila, columna);
                    //RefrescarCasilla (tabPosicion, fila, columna);
                    jugadorSeleccionado.PosicionesDisponibles--;
                }
                return Constantes.IMPACTO_BARCO;
            }
        }
        else
        {
            // Impacto en trampa Radar Pasivo
            if (tabPosicion.GetCasilla(fila, columna).Trampa == 1)
            {
                if (tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.NO_ATACADO || tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.REVELADO || tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.REVELADO_REFORZADA)
                {
                    tabAtaque.SetEstadoCasilla(fila, columna, Constantes.ATACADO_AGUA);
                    tabAtaque.GetCasilla(fila, columna).Atacada = true;
                    tabPosicion.GetCasilla(fila, columna).Atacada = true;
                    tabPosicion.GetCasilla(fila, columna).Trampa = 0;
                    //RefrescarCasilla (tabAtaque, fila, columna);
                    //RefrescarCasilla (tabPosicion, fila, columna);

                    // ESTE EFECTO HAY QUE CAMBIARLO. DEBERIA ROMPER UN CASILLERO DEL BARCO QUE ACABA DE ATACAR
                    // PERO COMO TODAVIA NO HAY LOGICA DE BARCOS Y LOS ATAQUES SON GLOBALES, TOCAR LA TRAMPA DAÑA UN BARCO CUALQUIERA
                    Casilla[] auxCasillas = new Casilla[100];
                    int auxNumCasillas = 0;
                    // Copio las referencia de todas las posibles casillas que puedo revelar
                    foreach (Barco b in jugadorSeleccionado.ListaBarcos)
                    {
                        if (b.Vivo == true)
                        {
                            foreach (Casilla c in b.GetPosicionesOcupadas())
                            {
                                if (c.Atacada == false && c.Revelada == false)
                                {
                                    auxCasillas[auxNumCasillas] = c;
                                    auxNumCasillas++;
                                }
                            }
                        }
                    }
                    if (auxNumCasillas != 0)
                    {
                        int random = UnityEngine.Random.Range(0, auxNumCasillas - 1);
                        // Revelo la casilla seleccionada
                        jugadorSeleccionado.TableroPosicion.GetCasilla(auxCasillas[random].Fila, auxCasillas[random].Columna).Revelada = true;
                        jugadorEnemigo.TableroAtaque.SetEstadoCasilla(auxCasillas[random].Fila, auxCasillas[random].Columna, Constantes.REVELADO);
                        //RefrescarCasilla (jugadorSeleccionado.TableroPosicion, auxCasillas [random].Fila, auxCasillas [random].Columna);
                        //RefrescarCasilla (jugadorEnemigo.TableroAtaque, auxCasillas [random].Fila, auxCasillas [random].Columna);
                    }
                    LimpiarCasillasTrampa(jugadorEnemigo, true);
                }
                return Constantes.IMPACTO_AGUA;
                // Impacto en trampa Sabotaje
            }
            else if (tabPosicion.GetCasilla(fila, columna).Trampa == 2)
            {
                LimpiarCasillasTrampa(jugadorEnemigo, false);
                //if (tabAtaque.GetEstadoCasilla (fila, columna) == Constantes.NO_ATACADO || tabAtaque.GetEstadoCasilla (fila, columna) == Constantes.REVELADO || tabAtaque.GetEstadoCasilla (fila, columna) == Constantes.REVELADO_REFORZADA) {
                //tabAtaque.SetEstadoCasilla (fila, columna, Constantes.ATACADO_AGUA); //Ataque fallido que impacto en agua

                //tabPosicion.GetCasilla (fila, columna).Atacada = true;
                //RefrescarCasilla (tabAtaque, fila, columna);
                //RefrescarCasilla (tabPosicion, fila, columna);
                //jugadorSeleccionado.PosicionesDisponibles--;
                //}
                return Constantes.IMPACTO_AGUA;

                // Impacto en agua
            }
            else if (tabPosicion.GetHayBarco(fila, columna) == false && tabAtaque.GetEstadoCasilla(fila, columna) == 0 && tabPosicion.GetEstadoCasilla(fila, columna) == Constantes.POSICION_AGUA)
            {
                if (tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.NO_ATACADO || tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.REVELADO || tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.REVELADO_REFORZADA)
                {
                    tabAtaque.SetEstadoCasilla(fila, columna, Constantes.ATACADO_AGUA); //Ataque fallido que impacto en agua

                    tabAtaque.GetCasilla(fila, columna).Atacada = true;
                    tabPosicion.GetCasilla(fila, columna).Atacada = true;
                    //RefrescarCasilla (tabAtaque, fila, columna);
                    //RefrescarCasilla (tabPosicion, fila, columna);
                    jugadorSeleccionado.PosicionesDisponibles--;
                }
                return Constantes.IMPACTO_AGUA;
            }
        }
        // Tocar una casilla atacada
        if (tabAtaque.GetEstadoCasilla(fila, columna) == Constantes.ATACADO_AGUA)
            return -1;
        else
            return -2;
    }

    public void modoAtaqueNormal()
    {
        modoSeleccionado = Constantes.ATAQUE_NORMAL_ID;
        ColorearBoton();
        LimpiarCasillasSeleccionadas();
    }

    public void modoProyectilAltamenteExplosivo()
    {
        if (modoSeleccionado == ProyectilHE.Id){
            modoAtaqueNormal();
            mensajeBot.DisplayMessage("Modo ataque normal", 2.5f, 400);
            return;
        }
        if (habilidadesActivadas == false || turno != 0 || animacionEnCurso)
        {
            mensajeBot.DisplayMessage("No puedes usar habilidades en este momento", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.habilidadDisponible(ProyectilHE) == false)
        {
            mensajeBot.DisplayMessage("Ésta habilidad no esta disponible", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.haySlotLibre() == false)
        {
            mensajeBot.DisplayMessage("No hay espacios de enfriamiento disponible", 2.5f, 500);
            return;
        }
        mensajeBot.DisplayMessage("Selecciona una posicion donde lanzar la habilidad", 2.5f, 500);
        modoSeleccionado = ProyectilHE.Id;
        ColorearBoton();
        LimpiarCasillasSeleccionadas();
        estadisticasHabilidadesEspeciales[1].contador++;
    }

    public void modoTorpedo()
    {
        if (modoSeleccionado == Torpedo.Id){
            modoAtaqueNormal();
            mensajeBot.DisplayMessage("Modo ataque normal", 2.5f, 400);
            return;
        }
        if (habilidadesActivadas == false || turno != 0 || animacionEnCurso)
        {
            mensajeBot.DisplayMessage("No puedes usar habilidades en este momento", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.habilidadDisponible(Torpedo) == false)
        {
            mensajeBot.DisplayMessage("Ésta habilidad no esta disponible", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.haySlotLibre() == false)
        {
            mensajeBot.DisplayMessage("No hay espacios de enfriamiento disponible", 2.5f, 500);
            return;
        }
        mensajeBot.DisplayMessage("Selecciona una posicion donde lanzar la habilidad", 3f, 500);
        modoSeleccionado = Torpedo.Id;
        ColorearBoton();
        LimpiarCasillasSeleccionadas();
        estadisticasHabilidadesEspeciales[0].contador++;
    }

    public void modoArtilleriaRapida()
    {
        if (modoSeleccionado == ArtilleriaRapida.Id){
            modoAtaqueNormal();
            mensajeBot.DisplayMessage("Modo ataque normal", 2.5f, 400);
            return;
        }
        if (habilidadesActivadas == false || turno != 0 || animacionEnCurso)
        {
            mensajeBot.DisplayMessage("No puedes usar habilidades en este momento", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.habilidadDisponible(ArtilleriaRapida) == false)
        {
            mensajeBot.DisplayMessage("Ésta habilidad no esta disponible", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.haySlotLibre() == false)
        {
            mensajeBot.DisplayMessage("No hay espacios de enfriamiento disponible", 2.5f, 500);
            return;
        }
        mensajeBot.DisplayMessage("Selecciona una posicion donde lanzar la habilidad", 3f, 500);
        modoSeleccionado = ArtilleriaRapida.Id;
        ColorearBoton();
        LimpiarCasillasSeleccionadas();
        estadisticasHabilidadesEspeciales[2].contador++;
    }

    public void modoAtaqueDoble()
    {
        if (modoSeleccionado == DisparoDoble.Id){
            modoAtaqueNormal();
            mensajeBot.DisplayMessage("Modo ataque normal", 2.5f, 400);
            return;
        }
        if (habilidadesActivadas == false || turno != 0 || animacionEnCurso)
        {
            mensajeBot.DisplayMessage("No puedes usar habilidades en este momento", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.habilidadDisponible(DisparoDoble) == false)
        {
            mensajeBot.DisplayMessage("Ésta habilidad no esta disponible", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.haySlotLibre() == false)
        {
            mensajeBot.DisplayMessage("No hay espacios de enfriamiento disponible", 2.5f, 500);
            return;
        }
        mensajeBot.DisplayMessage("Selecciona dos posiciones donde lanzar la habilidad", 3f, 500);
        modoSeleccionado = DisparoDoble.Id;
        ColorearBoton();
        LimpiarCasillasSeleccionadas();
        estadisticasHabilidadesEspeciales[3].contador++;
    }

    public void modoReforzarArmadura()
    {
        if (modoSeleccionado == ReforzarArmadura.Id){
            modoAtaqueNormal();
            mensajeBot.DisplayMessage("Modo ataque normal", 2.5f, 400);
            return;
        }
        if (habilidadesActivadas == false || turno != 0 || animacionEnCurso)
        {
            mensajeBot.DisplayMessage("No puedes usar habilidades en este momento", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.habilidadDisponible(ReforzarArmadura) == false)
        {
            mensajeBot.DisplayMessage("Ésta habilidad no esta disponible", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.haySlotLibre() == false)
        {
            mensajeBot.DisplayMessage("No hay espacios de enfriamiento disponible", 2.5f, 500);
            return;
        }
        mensajeBot.DisplayMessage("Selecciona una posición de alguno de tus barcos para reforzarla", 3f, 600);
        modoSeleccionado = ReforzarArmadura.Id;
        ColorearBoton();
        LimpiarCasillasSeleccionadas();
        estadisticasHabilidadesEspeciales[6].contador++;
    }

    public void modoTormentaDeMisiles()
    {
        if (habilidadesActivadas == false || turno != 0 || animacionEnCurso)
        {
            mensajeBot.DisplayMessage("No puedes usar habilidades en este momento", 2.5f, 500);
            return;
        }
        //if (jugadorSeleccionado.habilidadDisponible (TormentaDeMisiles.Id) == false) { return; }
        // No puedo ejecutar si ya la active anteriormente
        if ((turno == 0 && jugador1.UsoTormentaMisiles == true) || (turno == 1 && jugador2.UsoTormentaMisiles == true))
        {
            mensajeBot.DisplayMessage("Esta habilidad solo puede usarse una vez", 2.5f, 500);
            return;
        }
        // No puedo ejecutar si hay menos de 20 posiciones
        if ((turno == 0 && jugador1.PosicionesDisponibles < 20) || (turno == 1 && jugador2.PosicionesDisponibles < 20))
        {
            mensajeBot.DisplayMessage("No hay suficientes casilleros sin atacar", 2.5f, 500);
            return;
        }
        if (turno == 0)
            jugador1.UsoTormentaMisiles = true;
        else
            jugador2.UsoTormentaMisiles = true;

        mensajeBot.DisplayMessage("Tormenta de misiles activada", 4f, 200);

        estadisticasHabilidadesEspeciales[7].contador++;
        TormentaDeMisiles.activar(jugadorSeleccionado.TableroAtaque, jugadorEnemigo.TableroPosicion, barcoActual, false);
        ActualizarSlotLabels();
        LimpiarCasillasSeleccionadas();
    }

    public void modoPulsoElectromagnetico()
    {
        if (habilidadesActivadas == false || turno != 0 || animacionEnCurso)
        {
            mensajeBot.DisplayMessage("No puedes usar habilidades en este momento", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.habilidadDisponible(PulsoElectromagnetico) == false)
        {
            mensajeBot.DisplayMessage("Ésta habilidad no esta disponible", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.haySlotLibre() == false)
        {
            mensajeBot.DisplayMessage("No hay espacios de enfriamiento disponible", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.puedeAñadirHabilidad(PulsoElectromagnetico) == -1) return;
        if (PulsoElectromagnetico.activar(jugadorEnemigo, false) == true)
        {
			mensajeBot.DisplayMessage("Pulso electromagnetico activado", 2.5f, 400);
			AudioManager.PlayAudioPoint("Sonidos/Power Failure", player.transform.position, 500);

            estadisticasHabilidadesEspeciales[5].contador++;

            ActualizarSlotLabels();
            jugadorSeleccionado.añadirHabilidad(PulsoElectromagnetico);
        }
        LimpiarCasillasSeleccionadas();
    }

    public void modoContramedidas()
    {
        if (modoSeleccionado == Contramedidas.Id){
            modoAtaqueNormal();
            mensajeBot.DisplayMessage("Modo ataque normal", 2.5f, 400);
            return;
        }
        if (habilidadesActivadas == false || turno != 0 || animacionEnCurso)
        {
            mensajeBot.DisplayMessage("No puedes usar habilidades en este momento", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.habilidadDisponible(Contramedidas) == false || jugadorSeleccionado.TurnosContramedidas > 0)
        {
            mensajeBot.DisplayMessage("Ésta habilidad no esta disponible", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.haySlotLibre() == false)
        {
            mensajeBot.DisplayMessage("No hay espacios de enfriamiento disponible", 2.5f, 500);
            return;
        }
        mensajeBot.DisplayMessage("Selecciona uno de tus barcos para protegerlo", 3f, 500);
        modoSeleccionado = Contramedidas.Id;
        ColorearBoton();
        LimpiarCasillasSeleccionadas();
    }

    public void modoRadarPasivo()
    {
        if (modoSeleccionado == RadarPasivo.Id){
            modoAtaqueNormal();
            mensajeBot.DisplayMessage("Modo ataque normal", 2.5f, 400);
            return;
        }
        if (habilidadesActivadas == false || turno != 0 || animacionEnCurso)
        {
            mensajeBot.DisplayMessage("No puedes usar habilidades en este momento", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.habilidadDisponible(RadarPasivo) == false)
        {
            mensajeBot.DisplayMessage("Ésta habilidad no esta disponible", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.haySlotLibre() == false)
        {
            mensajeBot.DisplayMessage("No hay espacios de enfriamiento disponible", 2.5f, 500);
            return;
        }
        mensajeBot.DisplayMessage("Selecciona tres posiciones de tu tablero propio para colocar las trampas", 3f, 600);
        modoSeleccionado = RadarPasivo.Id;
        ColorearBoton();
        LimpiarCasillasSeleccionadas();
    }

    public void modoSabotaje()
    {
        if (modoSeleccionado == Sabotaje.Id){
            modoAtaqueNormal();
            mensajeBot.DisplayMessage("Modo ataque normal", 2.5f, 400);
            return;
        }
        if (habilidadesActivadas == false || turno != 0 || animacionEnCurso)
        {
            mensajeBot.DisplayMessage("No puedes usar habilidades en este momento", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.habilidadDisponible(Sabotaje) == false)
        {
            mensajeBot.DisplayMessage("Ésta habilidad no esta disponible", 2.5f, 500);
            return;
        }
        if (jugadorSeleccionado.haySlotLibre() == false)
        {
            mensajeBot.DisplayMessage("No hay espacios de enfriamiento disponible", 2.5f, 500);
            return;
        }
        mensajeBot.DisplayMessage("Selecciona tres posiciones de tu tablero propio para colocar las trampas", 3f, 600);
        modoSeleccionado = Sabotaje.Id;
        ColorearBoton();
        LimpiarCasillasSeleccionadas();
        estadisticasHabilidadesEspeciales[4].contador++;
    }

    public bool DeboCambiarTurno
    {
        get
        {
            return this.deboCambiarTurno;
        }
        set
        {
            deboCambiarTurno = value;
        }
    }

    public bool DeboDesactivarHabilidades
    {
        get
        {
            return this.deboDesactivarHabilidades;
        }
        set
        {
            deboDesactivarHabilidades = value;
        }
    }

    public void CrearBarcoFantasma()
    { //Deberia recibir de parametro las posiciones del barco fantasma
      /*
      Casilla [] posicionesBarcoFantasma = new Casilla[3];
      posicionesBarcoFantasma[0] = new Casilla(7, 7, 1); //Para pruebas
      posicionesBarcoFantasma[1] = new Casilla(8, 7, 1); //Para pruebas
      posicionesBarcoFantasma[2] = new Casilla(9, 7, 1); //Para pruebas
      int tipo = 1; //Ultra hardcodeadisimo
      Barco barcoFantasma = new Barco (posicionesBarcoFantasma, tipo);
      habilidad.CrearBarcoFantasma (this.tableroPosicion, barcoFantasma);
      */
    }
    public void ReiniciarPartida()
    {
        if (contenedorBarcos != null)
            Destroy(contenedorBarcos.gameObject);
        if (loadGameManager != null)
        {
            opciones.dificultad = loadGameManager.datosPartida.dificultad;
            opciones.modoClasico = loadGameManager.datosPartida.modoClasico;
            opciones.modoContraReloj = loadGameManager.datosPartida.modoContraReloj;            
            Destroy(loadGameManager.gameObject);
        }

        cargaEscenaAsync.CargarEscenaAsync("DistribucionDeBarcos");
    }

    private void GuardarEstadisticasDelJugador()
    {
        if (!PlayerPrefs.GetString("EstadisticasDelJugador", string.Empty).Equals(string.Empty))
        {
            var historicoEstadisticasDelJugador = Serializador.Deserializar<DatosEstadisticas>(PlayerPrefs.GetString("EstadisticasDelJugador"));

            for (int index = 0; index < 8; index++)
            {
                estadisticasHabilidadesEspeciales[index].contador += historicoEstadisticasDelJugador.estadisticaHabilidades[index].contador;
            }

            estadisticasDelJugador.derrotasModoClasico += historicoEstadisticasDelJugador.derrotasModoClasico;
            estadisticasDelJugador.derrotasModoContraReloj += historicoEstadisticasDelJugador.derrotasModoContraReloj;
            estadisticasDelJugador.derrotasModoNormal += historicoEstadisticasDelJugador.derrotasModoNormal;
            estadisticasDelJugador.victoriasModoClasico += historicoEstadisticasDelJugador.victoriasModoClasico;
            estadisticasDelJugador.victoriasModoContraReloj += historicoEstadisticasDelJugador.victoriasModoContraReloj;
            estadisticasDelJugador.victoriasModoNormal += historicoEstadisticasDelJugador.victoriasModoNormal;
            estadisticasDelJugador.empatesModoContraReloj += historicoEstadisticasDelJugador.empatesModoContraReloj;
        }

        estadisticasDelJugador.estadisticaHabilidades = estadisticasHabilidadesEspeciales;

        PlayerPrefs.SetString("EstadisticasDelJugador", Serializador.Serializar(estadisticasDelJugador));
    }

    public void GuardarPartida()
    {       
        try
        {
            var datosAGuardar = new DatosPartida(jugador1, jugador2, turno, dificultad, DateTime.Now, barcoActual, habilidadesActivadas, cantidadDeTurnosMaxima, esModoContraReloj, esModoClasico, contadorDeTurnos);
            PlayerPrefs.SetString(string.Format("Partida{0}", clavePartida), Serializador.Serializar(datosAGuardar)); // Un archivo por partida para guardar su estado
            PlayerPrefs.SetString(string.Format("PartidaIndexada{0}", clavePartida), "Activa"); // Archivo indice para guardar que partidas estan activas y cuales fueron borradas
            PlayerPrefs.Save();
            mensajeTop.DisplayMessage("Partida Guardada!", 3f, 250);
        }
        catch (Exception e)
        {
            mensajeTop.DisplayMessage("Error al guardar la partida!", 3f, 250);
        }
    }
    public void AbandonarPartida()
    {
        if (loadGameManager != null)
            Destroy(loadGameManager.gameObject);
        if (opciones != null)
            Destroy(opciones.gameObject);
        if (contenedorBarcos != null)
            Destroy(contenedorBarcos.gameObject);

        cargaEscenaAsync.CargarEscenaAsync("MenuPrincipal");
        reticle.SetActive(false);
    }

    public ArrayList UltimosHits
    {
        get
        {
            return this.ultimosHits;
        }
        set
        {
            ultimosHits = value;
        }
    }

    public bool HabilidadesActivadas
    {
        get
        {
            return this.habilidadesActivadas;
        }
    }

    public MensajePantalla MensajeTop
    {
        get
        {
            return this.mensajeTop;
        }
    }

    public MensajePantalla MensajeBot
    {
        get
        {
            return this.mensajeBot;
        }
    }

    public AudioManagerScript AudioManager
    {
        get
        {
            return audioManager;
        }

        set
        {
            audioManager = value;
        }
    }
}