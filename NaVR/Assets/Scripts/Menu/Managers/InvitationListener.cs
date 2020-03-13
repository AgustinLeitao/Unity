using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using Menu.Managers;
using UnityEngine;

public class InvitationListener : MonoBehaviour
{
    private MenuPrincipal menu;

    // Use this for initialization
    void Start()
    {
        menu = GameObject.Find("Camera").GetComponent<MenuPrincipal>();
    }


    // Update is called once per frame
    void Update()
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
                    menu.irAEscena("DistribucionDeBarcos");
                }

                //NavigationUtil.ShowPlayingPanel();
            }
            else
            {
                // show the "incoming invitation" screen
                Debug.Log("INVITACION RECIBIDA");
                menu.ActivarCanvasInvitacionRecibida();
                //NavigationUtil.ShowInvitationPanel();
            }
        }
    }
}