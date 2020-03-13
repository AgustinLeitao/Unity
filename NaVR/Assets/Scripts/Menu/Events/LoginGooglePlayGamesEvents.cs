namespace Menu.Events
{
    using Menu.Utils;
    using Menu.Managers;
    using UnityEngine;
    using GooglePlayGames;
    using GooglePlayGames.BasicApi;

    public class LoginGooglePlayGamesEvents : MonoBehaviour
    {

        private System.Action<bool> mAuthCallback;
        private bool mAuthOnStart = true;
        private bool mSigningIn = false;

        void Awake()
        {
            Screen.orientation = ScreenOrientation.Landscape;
            mAuthCallback = (bool success) =>
            {

                Debug.Log("In Auth callback, success = " + success);

                mSigningIn = false;
                if (success)
                {
                    NavigationUtil.ShowMainMenu();
                }
                else
                {
                    NavigationUtil.ShowLogin();
                    Debug.Log("Auth failed!!");
                }
            };

            var config = new PlayGamesClientConfiguration.Builder()
            .WithInvitationDelegate(InvitationManager.Instance.OnInvitationReceived)
            .Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();

            if (mAuthOnStart)
            {
                Authorize(true);
            }

        }

        void Start()
        {
        }

        public void OnLoginClicked()
        {
            Authorize(false);
        }

        public void OnOmitirClicked()
        {
            NavigationUtil.ShowMainMenu();
        }

        void Authorize(bool silent)
        {
            if (!mSigningIn)
            {
                Debug.Log("Starting sign-in...");
                PlayGamesPlatform.Instance.Authenticate(mAuthCallback, silent);
            }
            else
            {
                Debug.Log("Already started signing in");
            }
        }
    }
}