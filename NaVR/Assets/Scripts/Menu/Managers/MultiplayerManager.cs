namespace Menu.Managers
{
    using UnityEngine;
    using GooglePlayGames;
    using GooglePlayGames.BasicApi.Multiplayer;
    using System;
    using UnityEngine.SocialPlatforms;
    using System.Collections.Generic;
    using GooglePlayGames.BasicApi;

    [Serializable]
    public class Mensaje
    {
        public string jugador;
        public int tipo;
        public Casilla [] listaCasillas;
        public bool tocoTrampa = false;

        public Barco barcoRival;
		public int numeroBarco;
        public bool recepcionBarcosRival = false;
        public bool invitacionModoClasico = false;
        public bool invitacionModoContrarreloj = false;
        public int invitacionCantidadTurnos = 10;
        public bool sonConfiguraciones = false;
        public bool finalizoAnimacion = false;
        public bool finalizoPartida = false;
    }

    public class MultiplayerManager : RealTimeMultiplayerListener
    {

        const int QuickGameOpponents = 1;
        const int GameVariant = 0;
        const int MinOpponents = 1;
        const int MaxOpponents = 1;

        public static bool activarQuickGameFix = false;
        public static MultiplayerManager sInstance = null;

        public enum MultiplayerState
        {
            SettingUp,
            Playing,
            Finished,
            SetupFailed,
            Aborted
        };

        private MultiplayerState mMultiplayerState = MultiplayerState.SettingUp;
        private string mMyParticipantId = "";
        private float mRoomSetupProgress = 0.0f;
        const float FakeProgressSpeed = 1.0f;
        const float MaxFakeProgress = 30.0f;
        float mRoomSetupStartTime = 0.0f;
        private AdministradorPartida administrador;
        private BoardManager boardManager;
        private bool registrarPuntaje = true;

        private MultiplayerManager()
        {
            mRoomSetupStartTime = Time.time;
        }

        public MultiplayerState State
        {
            get
            {
                return mMultiplayerState;
            }
        }

        public String PID{

            get
            {
                return mMyParticipantId;
            }
        }

        public static MultiplayerManager Instance
        {
            get
            {
                return sInstance;
            }
        }

        public static bool mOnLeftRoom
        {
            get
            {
                return activarQuickGameFix;
            }
            set 
            {
                activarQuickGameFix = value;
            }
        }

        public int obtenerIdOrden(){

            List<Participant> participants = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
            if (participants[0].ParticipantId == PID){

                return 0;

            } else {

                return 1;

            }

        }

        public static void CreateQuickGame()
        {
            if (sInstance == null) 
            {
                sInstance = new MultiplayerManager();
            }
            if (GameObject.FindWithTag("Administrador") != null) 
            {
                sInstance.administrador = GameObject.FindWithTag("Administrador").GetComponent<AdministradorPartida>();
            }
            if( GameObject.Find("BoardManager") != null)
            {
                sInstance.boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
            }
            PlayGamesPlatform.Instance.RealTime.CreateQuickGame(QuickGameOpponents, QuickGameOpponents, GameVariant, sInstance);
        }

        public void OnParticipantLeft(Participant participant)
        {
            Debug.Log("El participante abandono/rechazo la invitacion: " + participant);
        }

        public void EnviarMensaje(string message)
        {
            Debug.Log("Enviando mensaje: " + message);
            byte[] mensaje = System.Text.Encoding.UTF8.GetBytes(message);
            bool reliable = true;
            PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliable, mensaje);
        }

        
        public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
        {
            Debug.Log("Mensaje recibido!!!");
            Debug.Log("Sender ID: " + senderId);
            string result = System.Text.Encoding.UTF8.GetString(data);
            Debug.Log("Mensaje: " + result);
            Mensaje msjRecibido = ParsearMensaje(result);
            if(msjRecibido.recepcionBarcosRival == true)
                 boardManager.ConfirmarBarcosRival(msjRecibido);
            else if(msjRecibido.sonConfiguraciones == true)
                boardManager.AplicarConfiguraciones(msjRecibido);
            else if(msjRecibido.finalizoAnimacion == true)
                administrador.FinalizoAnimacionMultiplayer();
            else if(msjRecibido.finalizoPartida == true)
                administrador.finalizarPartida();
            else
                administrador.EjecutarAccionEnemigo(msjRecibido);
        }

        /*
        public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
        {
            Debug.Log("Mensaje recibido!!!");
            Debug.Log("Sender ID: " + senderId);
            string result = System.Text.Encoding.UTF8.GetString(data);
            Debug.Log("Mensaje: " + result);
            Mensaje msjRecibido = ParsearMensaje(result);
            if(msjRecibido.FlagDistribucionBarco == true)
                 llamar a su metodo que necesiten;
            else
                 administrador.EjecutarAccionEnemigo(msjRecibido);
        }
         */

        private Mensaje ParsearMensaje(string msg){

            return JsonUtility.FromJson<Mensaje>(msg);

        }

        public void OnRoomSetupProgress(float percent)
        {
            Debug.Log("El porcentaje de setup del room es: " + percent);
        }

        public void OnRoomConnected(bool success)
        {
            if (success)
            {
                Debug.Log("OnRoomConnected: YA PUEDE COMENZAR LA COMUNICACION ENTRE JUGADORES");
                mMultiplayerState = MultiplayerState.Playing;
                mMyParticipantId = GetSelf().ParticipantId;
                //SetupTrack();
            }
            else
            {
                Debug.Log("On room connected fallo");
                mMultiplayerState = MultiplayerState.SetupFailed;
            }
        }

        public void OnLeftRoom()
        {
            Debug.Log("on left room");
            // Si la partida es la partida posta(reconectada) y el admin no es null registramos el score del participante que esta abandonando la partida
            Debug.Log("administrador: " + administrador);
            if (administrador != null)
            {
                Debug.Log("REGISTRANDO SCORE!!!!!!!!!!!!!!!!!!!!!");
                administrador.setFinDePartida(true);
                long total = administrador.RegistrarScore();
                administrador.RivalAbandono(total);
                //administrador.finalizarPartida();
            }

            if (mMultiplayerState != MultiplayerState.Finished)
            {
                Debug.Log("on left room: Se aborto la partida");
                mMultiplayerState = MultiplayerState.Aborted;
                activarQuickGameFix = true;
            }
        }

        public void OnPeersConnected(string[] peers)
        {
            Debug.Log("Se conecto un participante(peer) al room");
        }

        public void OnPeersDisconnected(string[] peers)
        {
            Debug.Log("On peers disconnected");
            // Si la partida es la partida posta(reconectada) y el admin no es null registramos el score del participante que quedo solo
            Debug.Log("administrador: " + administrador);
            if (administrador != null && registrarPuntaje) 
            {
                Debug.Log("REGISTRANDO SCORE!!!!!!!!!!!!!!!!!!!!!");
                administrador.setFinDePartida(true);
                long total = administrador.RegistrarScore();
                administrador.RivalAbandono(total);
                //administrador.finalizarPartida();
                registrarPuntaje = false;
            }

            // if, as a result, we are the only player in the race, it's over
            List<Participant> navers = GetConnectedParticipants();
            if (mMultiplayerState == MultiplayerState.Playing && (navers == null || navers.Count < 2))
            {
                mMultiplayerState = MultiplayerState.Aborted;
            }
        }

        private List<Participant> GetConnectedParticipants()
        {
            return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
        }

        public void CleanUp()
        {
            Debug.Log("Leave Room debido a CleanUp");
            PlayGamesPlatform.Instance.RealTime.LeaveRoom();
            //TearDownTrack();
            mMultiplayerState = MultiplayerState.Aborted;
            sInstance = null;
        }

        public void SettingUp()
        { 
            mMultiplayerState = MultiplayerState.SettingUp;
        }

        private Participant GetSelf()
        {
            return PlayGamesPlatform.Instance.RealTime.GetSelf();
        }

        private Participant GetParticipant(string participantId)
        {
            return PlayGamesPlatform.Instance.RealTime.GetParticipant(participantId);
        }

        public static void AcceptInvitation(string invitationId)
        {
            Debug.Log("Aceptando invitacion desde MM");
            if (sInstance == null)
            {
                sInstance = new MultiplayerManager();
            }
            PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitationId, sInstance);
        }

        public static void AcceptFromInbox()
        {
            if (sInstance == null)
            {
                sInstance = new MultiplayerManager();
            }
            PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(sInstance);
        }

        public static void CreateWithInvitationScreen()
        {
            if (sInstance == null)
            {
                sInstance = new MultiplayerManager();
            }
            PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(MinOpponents, MaxOpponents,
                GameVariant, sInstance);
        }

        public void PostearPuntaje(long puntaje)
        {
            Debug.Log("Enviando informacion de score...");
            Debug.Log("PLAYER: " + GetSelf().Player.id);
            Debug.Log("PUNTAJE PUBLICADO: " + puntaje);
            Social.ReportScore(puntaje, "CgkInv_L-tIZEAIQAQ", (bool success) =>
            {
                if (success) 
                {
                    Debug.Log("Envio correcto de puntaje!");
                } else 
                {
                    Debug.Log("Fallo el envio de puntaje!");
                }
            });
        }



        public void CargarScoreJugador()
        {
           PlayGamesPlatform.Instance.LoadScores(
             GPGSIds.leaderboard_leaderboard,
             LeaderboardStart.PlayerCentered,
             1,
             LeaderboardCollection.Public,
             LeaderboardTimeSpan.AllTime,
                (LeaderboardScoreData data) =>
                {
                    Debug.Log("-----SCORE DEL JUGADOR------");
                    Debug.Log("Valid = " + data.Valid);
                    Debug.Log("Id = " + data.Id);
                    Debug.Log("PlayerScore = " + data.PlayerScore);
                    Debug.Log("userID = " + data.PlayerScore.userID);
                    Debug.Log("formattedValue = " + data.PlayerScore.formattedValue);
                    Debug.Log("value = " + data.PlayerScore.value);
                    Debug.Log("-----FIN SCORE DEL JUGADOR------");
                    administrador.SetearScoreJugador(data.PlayerScore.value);
                });
        }

    }
}