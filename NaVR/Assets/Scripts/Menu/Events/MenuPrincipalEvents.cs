namespace Menu.Events
{
    using UnityEngine;
    using UnityEngine.UI;
    using GooglePlayGames;
    using UnityEngine.VR;
    using UnityEngine.SceneManagement;
    using System.Collections;
    using Menu.Utils;
    using Menu.Managers;
    using UnityEngine.EventSystems;
    using GooglePlayGames.BasicApi.Multiplayer;

    public class MenuPrincipalEvents : MonoBehaviour
    {
        public float fadeSpeed = 1.5f;
        public Image mFader;
        bool toBlack = false;
        bool toClear = true;
        public Text signInButtonText;
        public Text authStatus;
        private float mRoomSetupProgress = 0.0f;


        void Awake()
        {
            //mFader.color = Color.black;
            //mFader.gameObject.SetActive(true);
            //toClear = true;
        }

        void Update()
        {

            //HandleStatusUpdate();

            UpdateInvitation();

            if (MultiplayerManager.Instance == null)
            {
                return;
            }
            /*
            switch (MultiplayerManager.Instance.State)
            {
                case MultiplayerManager.MultiplayerState.SettingUp:
                    if (statusText != null)
                    {
                        //reset the timer, we can stay here for a long time.
                        mStatusMsg = null;
                        ShowStatus("Waiting for opponents...", false);
                    }
                    break;
                case MultiplayerManager.MultiplayerState.SetupFailed:
                    ShowStatus("Game setup failed", true);
                    MultiplayerManager.Instance.CleanUp();
                    processed = false;
                    break;
                case MultiplayerManager.MultiplayerState.Aborted:
                    ShowStatus("Race Aborted.", true);
                    MultiplayerManager.Instance.CleanUp();
                    processed = false;
                    break;
                case MultiplayerManager.MultiplayerState.Finished:
            // really should not see this on the main menu page,
            // so go to playing panel to display the final outcome of the race.
                    NavigationUtil.ShowPlayingPanel();
                    processed = false;
                    break;
                case MultiplayerManager.MultiplayerState.Playing:
                    NavigationUtil.ShowPlayingPanel();
                    processed = false;
                    break;
                default:
                    Debug.Log("MultiplayerManager.Instance.State = " + MultiplayerManager.Instance.State);
                    break;
            }  */
        }
	public void UpdateInvitation()
	{

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
				InvitationManager.Instance.Clear();
				//MultiplayerManager.AcceptInvitation(inv.InvitationId);
                Debug.Log("INVITACION ACEPTADA POR SHOULDAUTOACCEPT");
				//NavigationUtil.ShowPlayingPanel();
			}
			else
			{
				// show the "incoming invitation" screen
                Debug.Log("INVITACION ACEPTADA");
				NavigationUtil.ShowInvitationPanel();
			}
		}
	}
        public void Start()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            GameObject startButton = GameObject.Find("startButton");
            EventSystem.current.firstSelectedGameObject = startButton;

            PlayGamesPlatform.DebugLogEnabled = true;

            signInButtonText = GameObject.Find("BtnSignIn").GetComponentInChildren<Text>();
            bool success = PlayGamesPlatform.Instance.localUser.authenticated;
            Debug.Log("EStado actual: " + success);
            ChangeSignButtonText(success);
        }

        private void ChangeSignButtonText(bool success)
        {
            if (success)
            {
                Debug.Log("(NaVR) Signed in!");
                Debug.Log("Logueado como: " + Social.localUser.userName);
                signInButtonText.text = "Sign out";
               
                authStatus.text = "Logueado como: " + Social.localUser.userName;
            }
            else
            {
                Debug.Log("(NaVR) Sign-in failed...");
                Debug.Log("No se encuentra logueado con Google Play Games Service");
                signInButtonText.text = "Sign in";
                authStatus.text = "No se encuentra logueado con Google Play Games Service";
            }
        }

        public void OnSignInClicked()
        {
            if (!PlayGamesPlatform.Instance.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.Authenticate(SignInCallback, false);
            }
            else
            {
                PlayGamesPlatform.Instance.SignOut();
                ChangeSignButtonText(false);
            }
        }

        public void SignInCallback(bool success)
        {
            ChangeSignButtonText(success);
        }

       /* void LateUpdate()
        {
            if (toBlack)
            {
                FadeToBlack();
            }
            else if (toClear)
            {
                FadeToClear();
            }
        } */

        public void OnJugarPartidaVsIaClicked()
        {
            Debug.Log("JugarPartidaVsIA!!");
            NavigationUtil.ShowSetUpPartidaVsIA();
        }

        IEnumerator LoadDevice(string newDevice)
        {
            VRSettings.LoadDeviceByName(newDevice);
            yield return null;
            VRSettings.enabled = true;
        }

        public void OnJugarPartidaVsJugadorClicked()
        {
            Debug.Log("JugarPartidaVsIA!!");
            NavigationUtil.ShowSetUpPartidaVsJugador();

            /* 
            toBlack = true;
            mFader.gameObject.SetActive(true);
            FadeToBlack();

            FadeController fader = gameObject.GetComponentInChildren<FadeController>();
            if (fader != null)
            {
                fader.FadeToLevel(() => SceneManager.LoadScene("Environment"));
            }
            else
            {
                SceneManager.LoadScene("Environment");
                StartCoroutine(LoadDevice("cardboard"));
            }
        */
        }



        public void OnUnirseAPartidaClicked()
        {
            MultiplayerManager.CreateQuickGame();
            NavigationUtil.ShowWaitingRoom();
        }

        public void OnPartidasGuardadasClicked()
        {
        }

        public void OnRankingClicked()
        {
            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.ShowLeaderboardUI();
            }
            else
            {
                Debug.Log("Cannot show leaderboard: not authenticated");
            }
        }

        public void OnEstadisticasClicked()
        {

        }

        public void OnAyudaClicked()
        {

        }

        public void OnPreferenciasClicked()
        {

        }


        public void OnSalirClicked()
        {
            Application.Quit();
        }

        void FadeToClear()
        {
            mFader.color = Color.Lerp(mFader.color, Color.clear, fadeSpeed * Time.deltaTime);
            if (mFader.color.a <= 0.05f)
            {
                toClear = false;
                mFader.gameObject.SetActive(false);
            }
        }

        void FadeToBlack()
        {
            mFader.color = Color.Lerp(mFader.color, Color.black, fadeSpeed * Time.deltaTime);
            if (mFader.color.a >= 0.95f)
            {
                toBlack = false;
            }
        }

        public void OnRoomSetupProgress(float percent)
        {
            mRoomSetupProgress = percent;
        }

    }
}