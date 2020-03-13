namespace Menu.Events
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Menu.Managers;
    using Menu.Utils;
    using GooglePlayGames;

    public class SalaDeEspera : MonoBehaviour
    {
        public GameObject statusText;
        private string mStatusMsg = null;
        private float mStatusCountdown = 0f;
        // seconds for showing an error message;
        private const float ERROR_STATUS_TIMEOUT = 10.0f;
        // seconds for showing an info message;
        private const float INFO_STATUS_TIMEOUT = 2.0f;

        private bool iniciarSalaDeEspera = false;
        private MenuPrincipal menuPrincipal;

        // Use this for initialization
        void Start()
        {
            menuPrincipal = GameObject.Find("Camera").GetComponent<MenuPrincipal>();
        }

        // Update is called once per frame
        void Update()
        {
            if (iniciarSalaDeEspera) 
            {
                if (MultiplayerManager.Instance == null)
                {
                    return;
                }

                switch (MultiplayerManager.Instance.State)
                {
                    case MultiplayerManager.MultiplayerState.SettingUp:
                        if (statusText != null)
                        {
                            mStatusMsg = null;
                            ShowStatus("ESPERANDO OPONENTE...", false);
                        }
                        break;
                    case MultiplayerManager.MultiplayerState.SetupFailed:
                        ShowStatus("FALLO LA BUSQUEDA", true);
                        MultiplayerManager.Instance.CleanUp();
                        //processed = false;
                        break;
                    case MultiplayerManager.MultiplayerState.Playing:
                        // NavigationUtil.ShowPlayingPanel();
                        Debug.Log("Jugando!!!");
                        ShowStatus("CONECTADO - A JUGAR!", false);
                        iniciarSalaDeEspera = false;
                        //menuPrincipal.irAEscena("DemoExpo");
                        menuPrincipal.irAEscena("DistribucionDeBarcos");
                        //NavigationUtil.ShowLoadingScreen(); TODO: MOSTRAR PANTALLA DE CARGA E INICIAR EL JUEGO
                        //processed = false;
                        break;
                    case MultiplayerManager.MultiplayerState.Aborted:
                        ShowStatus("SE ABORTO LA PARTIDA", true);
                        MultiplayerManager.Instance.CleanUp();
                        //processed = false;
                        break;
                    default:
                        Debug.Log("MultiplayerManager.Instance.State = " + MultiplayerManager.Instance.State);
                        break;
                }
                //HandleStatusUpdate();

                //UpdateInvitation();
            }
        }

        public void IniciarSalaDeEspera() 
        {
            MultiplayerManager.CreateQuickGame();
            MultiplayerManager.Instance.SettingUp();
            ShowStatus("ESPERANDO OPONENTE...", false);
            this.iniciarSalaDeEspera = true;
        }

        public void ActivarSalaDeEspera()
        {
            this.iniciarSalaDeEspera = true;
        }

        //Shows a status message.  Errors are displayed differently.
        void ShowStatus(string msg, bool error)
        {
            Debug.Log("show status!!!" + msg + error);
            if (msg != mStatusMsg)
            {
                mStatusMsg = msg;
                statusText.SetActive(true);
                Text txt = statusText.GetComponent<Text>();
                txt.text = msg;
                /*if (error)
                {
                    Color c = statusText.GetComponent<Text>().color;
                    c.a = 1.0f;
                    statusText.GetComponent<Text>().color = c;
                    mStatusCountdown = ERROR_STATUS_TIMEOUT;
                }
                else
                {
                    Color c = statusText.GetComponent<Text>().color;
                    c.a = 0.0f;
                    statusText.GetComponent<Text>().color = c;
                    mStatusCountdown = INFO_STATUS_TIMEOUT;
                } */
            }
        }
    }
}