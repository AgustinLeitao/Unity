using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Menu.Managers;
using Menu.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames.BasicApi.Multiplayer;

public class MenuPrincipal : MonoBehaviour
{
    public Animator CameraObject;
    public OpcionesDelJuego opcionesDeJuego;
    public GameObject panelJuego, panelCreditos, panelAyuda, panelSiNo, partidaAleatoriaBtn, invitarAmigoBtn, modoClasicoText, modoContraRelojText, autoGuardadoText, btnAceptar, btnRechazar, statusInvitation;
    public GameObject newGameBtn, loadGameBtn, gameImage, ayudaImage, creditsImage, reglasImage, habilidadesImage, modosDeJuegoImage, facilImage, mediaImage, dificilImage;
    public GameObject panelHabilidades, panelModosDeJuego, panelReglas, canvasRanking, canvasOpciones, inputCantidadTurnos, loadGameManager, inputCantidadTurnosInvitacion;
    public GameObject btnJugarContraPc, btnJugarContraJugador, btnRanking, btnOpciones, btnSalir, btnSign, btnConfigurarPartida, btnModoContraRelojInvitacion, btnModoClasicoInvitacion;
    public GameObject volumenMusicaSlider, volumenSFXSlider, hoverSonido, clickSonido, opcionesDelJuego,canvasInvitacionRecibida,  canvasCargaEscena, canvasPrincipal, canvasCargarPartida, canvasCuartoDeEspera, canvasConfiguracionPartidaInvitacion, canvasConfiguracionPartida, canvasEstadisticas;

    private CargaEscenaAsync cargaEscenaAsync;
    private SalaDeEspera salaDeEspera;


    // GPGS     
    private Action<bool> mAuthCallback;
    private bool mAuthOnStart = true;
    private bool mSigningIn = false;
    public Text signInButtonText;
    public Text authStatus;

    void Start()
    {        
        volumenMusicaSlider.GetComponent<Slider>().value = 0.15f;
        volumenSFXSlider.GetComponent<Slider>().value = 0.15f;
        GetComponent<AudioSource>().Play();
        opcionesDeJuego = opcionesDelJuego.GetComponent<OpcionesDelJuego>();
        cargaEscenaAsync = GameObject.Find("CargaEscenaAsync").GetComponent<CargaEscenaAsync>();
        salaDeEspera = GameObject.Find("SalaDeEspera").GetComponent<SalaDeEspera>();
    }

    /* GPGS */
    void Awake()
    {
        Screen.orientation = ScreenOrientation.Landscape;

        var config = new PlayGamesClientConfiguration.Builder()
        .WithInvitationDelegate(InvitationManager.Instance.OnInvitationReceived)
        .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Instance.Authenticate(SignInCallback, true);
        if (PlayGamesPlatform.Instance.localUser.authenticated)
            ChangeSignText(true);
        else
            ChangeSignText(false);
    }

    void Update()
    {
        //Debug.Log("ESTOYOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        UpdateInvitation();
    }

    public void SignIn()
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.Authenticate(SignInCallback, false);
        }
        else
        {
            PlayGamesPlatform.Instance.SignOut();
            ChangeSignText(false);
        }
    }

    public void SignInCallback(bool success)
    {
        Debug.Log("SignCallback: " + success);
        ChangeSignText(success);
    }

    private void ChangeSignText(bool success)
    {
        if (success)
        {
            Debug.Log("(NaVR) Signed in!");
            Debug.Log("Logueado como: " + Social.localUser.userName);
            ActivarCaracteristicasGPGS();
            signInButtonText.text = "SIGN OUT";
            authStatus.text = "Google Play Games Service: Logueado como " + Social.localUser.userName + ".";
        }
        else
        {
            Debug.Log("(NaVR) Sign-in failed...");
            Debug.Log("No se encuentra logueado con Google Play Games Service");
            DesactivarCaracteristicasGPGS();
            signInButtonText.text = "SIGN IN";
            authStatus.text = "Google Play Games Service: No se encuentra logueado.";
        }
    }
    /* FIN GPGS */

    // Handle detecting incoming invitations.
    // Update is called once per frame
    void UpdateInvitation()
    {
        // Handle detecting incoming invitations.
        //Debug.Log("INVI-VIVOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        if (InvitationManager.Instance == null)
        {
            return;
        }

        // if an invitation arrived, switch to the "invitation incoming" GUI
        // or directly to the game, if the invitation came from the notification
        Invitation inv = InvitationManager.Instance.Invitation;
        if (inv != null)
        {
            if (InvitationManager.Instance.ShouldAutoAccept)
            {
                // jump straight into the game, since the user already indicated
                // they want to accept the invitation!
                Debug.Log("INVITACION ACEPTADA POR SHOULDAUTOACCEPT");
                if (PlayGamesPlatform.Instance.localUser.authenticated) 
                {
                InvitationManager.Instance.Clear();
                MultiplayerManager.AcceptInvitation(inv.InvitationId);
                SalaDeEspera();
                }

                //irAEscena("DistribucionDeBarcos");


                //NavigationUtil.ShowPlayingPanel();
            }
            else
            {
                // show the "incoming invitation" screen
                Debug.Log("INVITACION RECIBIDA");
                ActivarCanvasInvitacionRecibida();
                //NavigationUtil.ShowInvitationPanel();
            }
        }
    }

    public void ActualizarVolumenMusica()
    {
        GetComponent<AudioSource>().volume = volumenMusicaSlider.GetComponent<Slider>().value;
    }

    public void ActualizarVolumenEfectosDeSonido()
    {
        hoverSonido.GetComponent<AudioSource>().volume = volumenSFXSlider.GetComponent<Slider>().value;
        clickSonido.GetComponent<AudioSource>().volume = volumenSFXSlider.GetComponent<Slider>().value;       
    }

    private void DesactivarPanelesMenuAyuda()
    {
        reglasImage.SetActive(false);
        habilidadesImage.SetActive(false);
        modosDeJuegoImage.SetActive(false);
        panelHabilidades.SetActive(false);
        panelModosDeJuego.SetActive(false);
        panelReglas.SetActive(false);
    }

    private void DesactivarFondosDificultades()
    {
        facilImage.SetActive(false);
        mediaImage.SetActive(false);
        dificilImage.SetActive(false);
    }

    private void DesactivarPanelesPrincipales()
    {
        gameImage.SetActive(false);
        creditsImage.SetActive(false);
        ayudaImage.SetActive(false);
        panelJuego.SetActive(false);
        panelCreditos.SetActive(false);
        panelAyuda.SetActive(false);
    }

    public void DesactivarSubMenuesPrincipales()
    {
        canvasInvitacionRecibida.SetActive(false);
        canvasCargarPartida.SetActive(false);
        canvasOpciones.SetActive(false);
        canvasRanking.SetActive(false);
        canvasCuartoDeEspera.SetActive(false);
        canvasConfiguracionPartida.SetActive(false);
        canvasConfiguracionPartidaInvitacion.SetActive(false);
        canvasEstadisticas.SetActive(false);
        panelSiNo.SetActive(false);
        newGameBtn.SetActive(false);
        loadGameBtn.SetActive(false);
        btnConfigurarPartida.SetActive(false);
        partidaAleatoriaBtn.SetActive(false);
        invitarAmigoBtn.SetActive(false);
        btnAceptar.SetActive(false);
        btnRechazar.SetActive(false);
    }

    public void ActivarCanvasInvitacionRecibida()
    {
        DesactivarSubMenuesPrincipales();
        canvasPrincipal.SetActive(false);
        canvasInvitacionRecibida.SetActive(true);
        btnAceptar.SetActive(true);
        btnRechazar.SetActive(true);
        statusInvitation.GetComponent<Text>().text = "InvitacionRecibida";
    }

    public void DificultadFacil()
    {
        DesactivarFondosDificultades();
        facilImage.SetActive(true);
    }

    public void DificultadMedia()
    {
        DesactivarFondosDificultades();
        mediaImage.SetActive(true);
    }

    public void DificultadDificil()
    {
        DesactivarFondosDificultades();
        dificilImage.SetActive(true);
    }

    public void JugarContraPc()
    {
        DesactivarSubMenuesPrincipales();
        newGameBtn.SetActive(true);
        loadGameBtn.SetActive(true);
        btnConfigurarPartida.SetActive(true);
    }

    public void JugarContraJugador()
    {
        DesactivarSubMenuesPrincipales();
        partidaAleatoriaBtn.SetActive(true);
        invitarAmigoBtn.SetActive(true);
    }

    public void AutoGuardado()
    {
        bool estaActivado = autoGuardadoText.GetComponent<Text>().text.Equals("Desactivado") ? false : true;
        if (estaActivado)
        {
            autoGuardadoText.GetComponent<Text>().text = "Desactivado";
        }
        else
        {
            autoGuardadoText.GetComponent<Text>().text = "Activado";
        }
    }

    public void ModoClasico()
    {
        bool estaActivado = modoClasicoText.GetComponent<Text>().text.Equals("Desactivado") ? false : true;
        if (estaActivado)
        {
            modoClasicoText.GetComponent<Text>().text = "Desactivado";
        }
        else
        {
            modoClasicoText.GetComponent<Text>().text = "Activado";
        }
    }

    public void ModoContraReloj()
    {
        bool estaActivado = modoContraRelojText.GetComponent<Text>().text.Equals("Desactivado") ? false : true;
        if (estaActivado)
        {
            modoContraRelojText.GetComponent<Text>().text = "Desactivado";
            inputCantidadTurnos.SetActive(false);
        }
        else
        {
            modoContraRelojText.GetComponent<Text>().text = "Activado";
            inputCantidadTurnos.SetActive(true);
        }
    }

    public void ModoClasicoInvitacion()
    {
        bool estaActivado = btnModoClasicoInvitacion.GetComponent<Text>().text.Equals("Desactivado") ? false : true;
        if (estaActivado)
        {
            btnModoClasicoInvitacion.GetComponent<Text>().text = "Desactivado";
        }
        else
        {
            btnModoClasicoInvitacion.GetComponent<Text>().text = "Activado";
        }
    }

    public void ModoContraRelojInvitacion()
    {
        bool estaActivado = btnModoContraRelojInvitacion.GetComponent<Text>().text.Equals("Desactivado") ? false : true;
        if (estaActivado)
        {
            btnModoContraRelojInvitacion.GetComponent<Text>().text = "Desactivado";
            inputCantidadTurnosInvitacion.SetActive(false);
        }
        else
        {
            btnModoContraRelojInvitacion.GetComponent<Text>().text = "Activado";
            inputCantidadTurnosInvitacion.SetActive(true);
        }
    }

    public void HabilitarBotonesPrincipales()
    {
        btnJugarContraJugador.SetActive(true);
        btnJugarContraPc.SetActive(true);
        btnOpciones.SetActive(true);
        btnSalir.SetActive(true);
        btnSign.SetActive(true);
        btnRanking.SetActive(true); 
    }

    public void ActivarCaracteristicasGPGS()
    {
        btnJugarContraJugador.GetComponent<Button>().interactable = true;
        btnJugarContraJugador.GetComponent<EventTrigger>().enabled = true;
        btnRanking.GetComponent<Button>().interactable = true;
        btnRanking.GetComponent<EventTrigger>().enabled = true;
    }

    public void DesactivarCaracteristicasGPGS()
    {
        HabilitarBotonesPrincipales();
        btnJugarContraJugador.GetComponent<Button>().interactable = false;
        btnJugarContraJugador.GetComponent<EventTrigger>().enabled = false;
        btnRanking.GetComponent<Button>().interactable = false;
        btnRanking.GetComponent<EventTrigger>().enabled = false;
    }

    public void CargarPartida()
    {
        DesactivarSubMenuesPrincipales();
        canvasCargarPartida.SetActive(true);     
        loadGameManager.SetActive(true);

        Position2();
    }

    public void ConfigurarPartida()
    {
        DesactivarSubMenuesPrincipales();
        canvasConfiguracionPartida.SetActive(true);       

        Position2();
    }

    public void ConfigurarPartidaInvitacion()
    {
        DesactivarSubMenuesPrincipales();
        canvasConfiguracionPartidaInvitacion.SetActive(true);

        Position2();
    }

    public void EstadisticasDelJugador()
    {
        DesactivarSubMenuesPrincipales();
        canvasEstadisticas.SetActive(true);    

        Position2();
    }

    public void NuevaPartida()
    {
        DesactivarSubMenuesPrincipales();
        opcionesDeJuego.esModoIA = true;
        irAEscena("DistribucionDeBarcos");
    }

    public void irAEscena(string escena)
    {
        //SceneManager.LoadScene(escena);
        cargaEscenaAsync.CargarEscenaAsync(escena);
    }

    public void Opciones()
    {
        DesactivarSubMenuesPrincipales();
        canvasOpciones.SetActive(true);
        Position2();
    }

    public void PanelDeJuego()
    {
        DesactivarPanelesPrincipales();
        gameImage.SetActive(true);
        panelJuego.SetActive(true);
    }

    public void PanelDeCreditos()
    {
        DesactivarPanelesPrincipales();
        creditsImage.SetActive(true);
        panelCreditos.SetActive(true);
    }

    public void PanelDeAyuda()
    {
        DesactivarPanelesPrincipales();
        ayudaImage.SetActive(true);
        panelAyuda.SetActive(true);
    }

    public void PanelDeReglas()
    {
        DesactivarPanelesMenuAyuda();
        reglasImage.SetActive(true);
        panelReglas.SetActive(true);
    }

    public void PanelDeHabilidades()
    {
        DesactivarPanelesMenuAyuda();
        habilidadesImage.SetActive(true);
        panelHabilidades.SetActive(true);
    }

    public void PanelDeModos()
    {
        DesactivarPanelesMenuAyuda();
        modosDeJuegoImage.SetActive(true);
        panelModosDeJuego.SetActive(true);
    }

    public void PlayHover()
    {
        hoverSonido.GetComponent<AudioSource>().Play();
    }   

    public void PlayClick()
    {
        clickSonido.GetComponent<AudioSource>().Play();
    }

    public void Position2()
    {
        CameraObject.SetFloat("Animate", 1);
    }

    public void Position1()
    {      
        if (loadGameManager.activeSelf == true)
        {           
            loadGameManager.transform.SetParent(transform.root);
            loadGameManager.transform.SetParent(null);
            loadGameManager.SetActive(false);        
        }

        CameraObject.SetFloat("Animate", 0);
    }

    public void VolverDesdeOpciones()
    {
        if (opcionesDeJuego.cantidadTurnosMax < 10)
        {
            opcionesDeJuego.cantidadTurnosMax = 10;
            opcionesDeJuego.cantidadTurnosInput.GetComponent<InputField>().text = "10";
        }
        if(opcionesDeJuego.invitacionCantidadTurnosMax < 10)
        {
            opcionesDeJuego.invitacionCantidadTurnosMax = 10;
            inputCantidadTurnosInvitacion.GetComponent<InputField>().text = "10";
        }

        Position1();
    }

    public void Ranking()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }
        else
        {
            Debug.Log("Cannot show leaderboard: not authenticated");
        }
        //DesactivarSubMenuesPrincipales();
        //canvasRanking.SetActive(true);
        //Position2();
    }

    public void UnirseAPartida()
    {
        opcionesDeJuego.esPartidaRapida = true;
        DesactivarSubMenuesPrincipales();
        canvasCuartoDeEspera.SetActive(true);
        Position2();
        this.salaDeEspera.IniciarSalaDeEspera();
    }

    public void SalaDeEspera()
    {
        DesactivarSubMenuesPrincipales();
        canvasCuartoDeEspera.SetActive(true);
        Position2();
        this.salaDeEspera.ActivarSalaDeEspera();
    }

    public void InvitarAmigo()
    {
        if (opcionesDeJuego.invitacionCantidadTurnosMax < 10)
        {
            opcionesDeJuego.invitacionCantidadTurnosMax = 10;
            inputCantidadTurnosInvitacion.GetComponent<InputField>().text = "10";
        }
        opcionesDeJuego.yoEnvieInvitacion = true;
        MultiplayerManager.CreateWithInvitationScreen();
        SalaDeEspera();
    }

    public void VolverDesdeCuartoDeEspera()
    {
        opcionesDeJuego.esPartidaRapida = false;
        MultiplayerManager.Instance.CleanUp();
    }

    public void Salir()
    {
        DesactivarSubMenuesPrincipales();
        panelSiNo.SetActive(true);
    }

    public void No()
    {
        panelSiNo.SetActive(false);
    }

    public void Si()
    {
        Application.Quit();
    }    
}