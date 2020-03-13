using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.VR;
using GooglePlayGames;
using Menu.Managers;
using System;

public class BoardManager : MonoBehaviour
{
    [HideInInspector]
    public AudioSource sonidoClipBarco;

    public GameObject reticle;  
    public GameObject buttonManager;
    public ContenedorBarcos contenedorBarcos; //Codigo agregado
    public int numeroBarco;
    public int orientacionBarco;
    public Text textOrientacionBarco;
    private ValidadorPosiciones validadorPosiciones;	
    private ButtonManager btnManager;
    private CargaEscenaAsync cargaEscenaAsync;

    private bool esModoIA;
    private bool reconectado;

    private bool confirmacionBarcosRival;
	private bool confirmacionBarcos;

    private OpcionesDelJuego opciones;
    private bool envioDeConfiguracion = true;

    public IEnumerator LoadDevice(string newDevice)
    {
        VRSettings.LoadDeviceByName(newDevice);
        yield return null;
        VRSettings.enabled = true;
    }

    void Awake()
    {
        var OpcionesDelJuego = GameObject.Find("Opciones");
        Debug.Log("Opciones del juego: " + OpcionesDelJuego);
        if (OpcionesDelJuego == null)
            esModoIA = true;
        else
        {       
            esModoIA = OpcionesDelJuego.GetComponent<OpcionesDelJuego>().esModoIA;
        }
        if (OpcionesDelJuego != null)
            opciones = OpcionesDelJuego.GetComponent<OpcionesDelJuego>();

        
        Debug.Log("esMODOIA - BOARD MANAGERRRRRRRRRRRRR: " + esModoIA);

        reconectado = false;
        confirmacionBarcosRival = false;
		confirmacionBarcos = false;
        StartCoroutine(LoadDevice("cardboard"));
    }

    void Start ()
    {
        cargaEscenaAsync = GameObject.Find("CargaEscenaAsync").GetComponent<CargaEscenaAsync>();    
		orientacionBarco = 0;
		validadorPosiciones = new ValidadorPosiciones();
		validadorPosiciones.InicializarTablero ();
		numeroBarco = 0;
        btnManager = buttonManager.GetComponent<ButtonManager>();
        sonidoClipBarco = GameObject.Find("SonidosDelJuego").GetComponent<AudioSource>();
    }

    void Update () {

        if (esModoIA == false)
        {
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

            if (opciones.esPartidaRapida)
            {
                opciones.modoClasico = false;
                opciones.modoContraReloj = false;
                opciones.cantidadTurnosMax = 10;
            }
        }
        
        RaycastHit hit;
        if (Physics.Raycast(new Ray(reticle.transform.position, reticle.transform.forward), out hit, 100F, LayerMask.GetMask("BtnDos")) && Input.GetButtonDown("Fire1") && btnManager.contadorDos > 0) {
            CrearBarco(2, 2); //Primer parametro es tamaño barco, segundo parametro es tipo de habilidades de barco
        }
        else {
            if (Physics.Raycast(new Ray(reticle.transform.position, reticle.transform.forward), out hit, 100F, LayerMask.GetMask("BtnTres")) && Input.GetButtonDown("Fire1") && btnManager.contadorTres > 0) {
				CrearBarco(3, 3); //Primer parametro es tamaño barco, segundo parametro es tipo de habilidades de barco
            }
        	else {
				if (Physics.Raycast (new Ray (reticle.transform.position, reticle.transform.forward), out hit, 100F, LayerMask.GetMask ("BtnCuatro")) && Input.GetButtonDown ("Fire1") && btnManager.contadorCuatro > 0) {
					CrearBarco (4, 4); //Primer parametro es tamaño barco, segundo parametro es tipo de habilidades de barco
				} 
				else {
					if (Physics.Raycast (new Ray (reticle.transform.position, reticle.transform.forward), out hit, 100F, LayerMask.GetMask ("BtnCinco")) && Input.GetButtonDown ("Fire1") && btnManager.contadorCinco > 0) {
						CrearBarco (5, 5); //Primer parametro es tamaño barco, segundo parametro es tipo de habilidades de barco
					} 
					else {
						if (Physics.Raycast (new Ray (reticle.transform.position, reticle.transform.forward), out hit, 100F, LayerMask.GetMask ("BtnLimpiarTablero")) && Input.GetButtonDown ("Fire1")) {                           
                            contenedorBarcos.ReiniciarArray ();
                            LimpiarTableroYReiniciarContadores();
						} else {
							if (Physics.Raycast(new Ray(reticle.transform.position, reticle.transform.forward), out hit, 100F, LayerMask.GetMask("BtnContinuar")) && Input.GetButtonDown("Fire1") && btnManager.btnContinue.interactable == true) {
                                reticle.GetComponent<MeshRenderer>().enabled = false;
                                //enviar msj con nuestro array de barcos al otro jugador...
                                //agregar un if con una bandera antes de la carga de escena para asegurarnos de que los dos jugadores ya colocaron sus barcos
                                if (esModoIA == true){
                                    cargaEscenaAsync.CargarEscenaAsync("DemoExpo");	
                                }
                                else {
									confirmacionBarcos = true;
									if (MultiplayerManager.MultiplayerState.Playing == MultiplayerManager.Instance.State && envioDeConfiguracion && opciones.yoEnvieInvitacion && ! opciones.esPartidaRapida)
									{
										Mensaje msgConfiguracion = new Mensaje();
										msgConfiguracion.sonConfiguraciones = true;
										msgConfiguracion.invitacionCantidadTurnos = opciones.invitacionCantidadTurnosMax;
										msgConfiguracion.invitacionModoClasico = opciones.invitacionModoClasico;
										msgConfiguracion.invitacionModoContrarreloj = opciones.invitacionModoContraReloj;
										MultiplayerManager.Instance.EnviarMensaje(JsonUtility.ToJson(msgConfiguracion));
										envioDeConfiguracion = false;
									}
                                    Mensaje msg = new Mensaje();
									for (int i = 0; i < contenedorBarcos.barcos.Length; i++) {
										msg.recepcionBarcosRival = true;
										msg.barcoRival = contenedorBarcos.barcos[i];
										msg.numeroBarco = i;
										MultiplayerManager.Instance.EnviarMensaje(JsonUtility.ToJson(msg));
									}
                                }
                            
							}
						}
					}
				}
            }
        }

        if(confirmacionBarcosRival == true && confirmacionBarcos == true)
        {
			confirmacionBarcosRival = false;
			confirmacionBarcos = false;
			MultiplayerManager.activarQuickGameFix = true;
            cargaEscenaAsync.CargarEscenaAsync("DemoExpo");	
        }
    }

    public void ConfirmarBarcosRival(Mensaje msg){
		contenedorBarcos.barcosRival[msg.numeroBarco] = msg.barcoRival;
		if (msg.numeroBarco == 7) {
			confirmacionBarcosRival = true;
		}
    }

	public void AplicarConfiguraciones(Mensaje msg)
	{
		Debug.Log("ESTOY EN APLICAR CONFIGURACIONES: " + msg);
		Debug.Log("opciones: " + opciones);
		Debug.Log("opciones.invitacionCantidadTurnosMax: " + opciones.invitacionCantidadTurnosMax);
		Debug.Log("opciones.invitacionModoClasico: " + opciones.invitacionModoClasico);
		Debug.Log(" opciones.invitacionModoContraReloj: " + opciones.invitacionModoContraReloj);

		opciones.invitacionCantidadTurnosMax = msg.invitacionCantidadTurnos;
		opciones.invitacionModoClasico = msg.invitacionModoClasico;
		opciones.invitacionModoContraReloj = msg.invitacionModoContrarreloj;
	}

    public void CrearBarco(int tipoBarco, int tipoHabilidades) {
        var shipManager = GameObject.Find("ShipManager");

        ValidaComponentesYDestruye(shipManager);

		BarcoPosiciones barcoPosiciones = shipManager.AddComponent<BarcoPosiciones> ();
        barcoPosiciones.tipoBarco = tipoBarco;
		barcoPosiciones.tipoHabilidades = tipoHabilidades;       
        ConfigurarElementosBarco(barcoPosiciones);

        switch (tipoBarco) {
		case 2:
			barcoPosiciones.textoInformativo = GameObject.Find ("MensajeInformativoBarcoDos").GetComponent<Text> ();
			barcoPosiciones.textoContador = GameObject.Find ("ContadorBarcoDos").GetComponent<Text> ();
			barcoPosiciones.prefabBarco = (GameObject)Resources.Load ("Prefabs/Barco2Posiciones", typeof(GameObject));
			barcoPosiciones.prefabBarcoTemp = (GameObject)Resources.Load ("Prefabs/Barco2PosicionesTemp", typeof(GameObject));
         	break;
        case 3:              
            barcoPosiciones.textoInformativo = GameObject.Find("MensajeInformativoBarcoTres").GetComponent<Text>();
            barcoPosiciones.textoContador = GameObject.Find("ContadorBarcoTres").GetComponent<Text>();
			barcoPosiciones.prefabBarco = (GameObject)Resources.Load ("Prefabs/Barco3Posiciones", typeof(GameObject));
			barcoPosiciones.prefabBarcoTemp = (GameObject)Resources.Load ("Prefabs/Barco3PosicionesTemp", typeof(GameObject));
            break;
		case 4:           
 			barcoPosiciones.textoInformativo = GameObject.Find("MensajeInformativoBarcoCuatro").GetComponent<Text>();
            barcoPosiciones.textoContador = GameObject.Find("ContadorBarcoCuatro").GetComponent<Text>();
			barcoPosiciones.prefabBarco = (GameObject)Resources.Load ("Prefabs/Barco4Posiciones", typeof(GameObject));
			barcoPosiciones.prefabBarcoTemp = (GameObject)Resources.Load ("Prefabs/Barco4PosicionesTemp", typeof(GameObject));
            break;
        case 5:                
            barcoPosiciones.textoInformativo = GameObject.Find("MensajeInformativoBarcoCinco").GetComponent<Text>();
            barcoPosiciones.textoContador = GameObject.Find("ContadorBarcoCinco").GetComponent<Text>();
			barcoPosiciones.prefabBarco = (GameObject)Resources.Load ("Prefabs/Barco5Posiciones", typeof(GameObject));
			barcoPosiciones.prefabBarcoTemp = (GameObject)Resources.Load ("Prefabs/Barco5PosicionesTemp", typeof(GameObject));
            break;
        }
    }

	private void ValidaComponentesYDestruye (GameObject shipManager) {
		foreach (var comp in shipManager.GetComponents<Component>()) {
			if (comp is BarcoPosiciones) {
				Destroy(comp);
			}
		}
	}

	private void ConfigurarElementosBarco (BarcoPosiciones barcoPosiciones) {
		barcoPosiciones.reticle = GameObject.Find("GvrReticlePointer");
		barcoPosiciones.boardManager = this;
		barcoPosiciones.buttonManager = (ButtonManager) GameObject.Find("ButtonManager").GetComponent("ButtonManager");
		barcoPosiciones.textErrores = GameObject.Find("TextoErrores").GetComponent<Text>();
		barcoPosiciones.validadorPosiciones = validadorPosiciones;
		barcoPosiciones.orientacionBarco = orientacionBarco;	
	}

    private void LimpiarTableroYReiniciarContadores()
    {
        foreach (var barco in GameObject.FindGameObjectsWithTag("BarcoPosiciones"))
            Destroy(barco);

        btnManager.contadorDos = 3;
        btnManager.contadorTres = 2;
        btnManager.contadorCuatro = 2;
        btnManager.contadorCinco = 1;
        numeroBarco = 0;
        validadorPosiciones.InicializarTablero();
        btnManager.btnContinue.interactable = false;
        btnManager.btnContinue.GetComponentsInChildren<Text>()[0].fontSize = btnManager.btnContinueInitialFontSize;
        btnManager.btnContinue.GetComponentsInChildren<Text>()[0].color = btnManager.btnContinueInitialColor;
        btnManager.btnContinue.GetComponentsInChildren<Text>()[1].text = btnManager.btnContinueInitialText;        

        btnManager.btnBarcoDos.interactable = true;
        btnManager.btnBarcoTres.interactable = true;
        btnManager.btnBarcoCuatro.interactable = true;
        btnManager.btnBarcoCinco.interactable = true;
        
        btnManager.btnBarcoDos.GetComponentsInChildren<Text>()[1].text = "Remanentes = " + btnManager.contadorDos;
        btnManager.btnBarcoTres.GetComponentsInChildren<Text>()[1].text = "Remanentes = " + btnManager.contadorTres;
        btnManager.btnBarcoCuatro.GetComponentsInChildren<Text>()[1].text = "Remanentes = " + btnManager.contadorCuatro;
        btnManager.btnBarcoCinco.GetComponentsInChildren<Text>()[1].text = "Remanentes = " + btnManager.contadorCinco;
    }
}
