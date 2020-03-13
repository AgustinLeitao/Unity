

namespace Menu.Events
{
    using Menu.Managers;
	using Menu.Utils;
    using UnityEngine;
    using UnityEngine.UI;
    using GooglePlayGames.BasicApi.Multiplayer;


    // Handles the prompting of accepting or declining an invitation to play.
    // This should be attached to the InvitationPanel object in the main scene.
    public class InvitacionEvents : MonoBehaviour
    {

        // associated Text component to display the message.
        public Text message;

        // the invitation object being processed.
        private Invitation inv;

        private bool processed = false;
        private string inviterName = null;
        private MenuPrincipal menuPrincipal;

        void start()
        { 
            menuPrincipal = GameObject.Find("Camera").GetComponent<MenuPrincipal>();
        }

        // Update is called once per frame
        void Update()
        {

            inv = (inv != null) ? inv : InvitationManager.Instance.Invitation;
            
            if (inv == null && !processed)
            {
                //Debug.Log("No Invite -- back to main");
                //NavigationUtil.ShowMainMenu();
                return;
            }
            Debug.Log("INV: " + inv);
            Debug.Log("MultiplayerManager.Instance: " + MultiplayerManager.Instance);
            /*if (inviterName == null)
            {
                inviterName = (inv.Inviter == null || inv.Inviter.DisplayName == null) ? "Someone" :
          inv.Inviter.DisplayName;
                message.text = inviterName + " is challenging you to a batalla naval!";
            }*/

            if (MultiplayerManager.Instance != null)
            {
                Debug.Log("MultiplayerManager.Instance entro al IFFFFFFFFFFFFFFFFFFFFF: " + MultiplayerManager.Instance);
                Debug.Log("MultiplayerManager.Instance.State: " + MultiplayerManager.Instance.State);
                switch (MultiplayerManager.Instance.State)
                {
                    case MultiplayerManager.MultiplayerState.Aborted:
                        Debug.Log("Aborted -- back to main");
                        //NavigationUtil.ShowMainMenu();
                        break;
                    case MultiplayerManager.MultiplayerState.Finished:
                        Debug.Log("Finished-- back to main");
                        //NavigationUtil.ShowMainMenu();
                        break;
                    case MultiplayerManager.MultiplayerState.Playing:
						Debug.Log("IR AL JUEGO!!");
                        menuPrincipal.irAEscena("DistribucionDeBarcos");
                        //NavigationUtil.ShowPlayingPanel();
                        break;
                    case MultiplayerManager.MultiplayerState.SettingUp:
                        message.text = "Setting up Race...";
                        break;
                    case MultiplayerManager.MultiplayerState.SetupFailed:
                        Debug.Log("Failed -- back to main");
                        //NavigationUtil.ShowMainMenu();
                        break;
                }
            }
        }

        // Handler script for the Accept button.  This method should be added
        // to the On Click list for the accept button.
        public void OnAccept()
        {
            Debug.Log("Aceptando invitacion desde IE");
            if (processed)
            {
                return;
            }

            processed = true;
            InvitationManager.Instance.Clear();

            MultiplayerManager.AcceptInvitation(inv.InvitationId);
            Debug.Log("Accepted! MultiplayerManager state is now " + MultiplayerManager.Instance.State);

        }

        // Handler script for the decline button.
        public void OnDecline()
        {
            Debug.Log("Rechazando invitacion desde IE");
            if (processed)
            {
                return;
            }

            processed = true;
            InvitationManager.Instance.DeclineInvitation();

            NavigationUtil.ShowMainMenu();
        }
    }
}
