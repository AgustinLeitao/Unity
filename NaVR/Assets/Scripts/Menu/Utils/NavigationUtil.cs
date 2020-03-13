namespace Menu.Utils
{
    
    using UnityEngine;
    using UnityEngine.EventSystems;
    using Menu.Managers;

    // Utility class to navigate between the various panels in the main scene.
    public static class NavigationUtil
    {

        private static PanelManager theMgr;

        public static PanelManager PanelManager
        {
            get
            {
                if (theMgr == null)
                {
                    theMgr = EventSystem.current.GetComponent<PanelManager>();
                }
                return theMgr;
            }
        }

        public static void ShowMainMenu()
        {
            PanelManager mgr = NavigationUtil.PanelManager;
            if (mgr != null)
            {
                Debug.Log("Showing MainMenu!");
                mgr.OpenMainMenu();
            }
            else
            {
                Debug.LogWarning("PanelManager script missing!");
            }
        }

        public static void ShowLogin()
        {
            PanelManager mgr = NavigationUtil.PanelManager;
            if (mgr != null)
            {
                Debug.Log("Showing Game Menu!");
                mgr.OpenLoginPanel();
            }
            else
            {
                Debug.Log("PanelManager script missing!");
            }
        }

        public static void ShowWaitingRoom()
        {
            PanelManager mgr = NavigationUtil.PanelManager;
            if (mgr != null)
            {
                Debug.Log("Showing Waiting Room!");
                mgr.OpenWaitingRoomPanel();
            }
            else
            {
                Debug.Log("PanelManager script missing!");
            }
        }

        public static void ShowSetUpPartidaVsJugador()
        {
            PanelManager mgr = NavigationUtil.PanelManager;
            if (mgr != null)
            {
                Debug.Log("Showing Set Up Partiva Vs Jugador!");
                mgr.OpenSetUpPartidaVsJugadorPanel();
            }
            else
            {
                Debug.Log("PanelManager script missing!");
            }
        }

        public static void ShowSetUpPartidaVsIA()
        {
            PanelManager mgr = NavigationUtil.PanelManager;
            if (mgr != null)
            {
                Debug.Log("Showing Set Up Partiva Vs Jugador!");
                mgr.OpenSetUpPartidaVsIAPanel();
            }
            else
            {
                Debug.Log("PanelManager script missing!");
            }
        }

        public static void ShowInvitationPanel()
        {
            PanelManager mgr = NavigationUtil.PanelManager;
            if (mgr != null)
            {
                Debug.Log("Showing Invitation Panel!");
                mgr.OpenInvitationPanel();
            }
            else
            {
                Debug.Log("PanelManager script Missing");
            }
        }

        public static void ShowLoadingScreen()
        {
            PanelManager mgr = NavigationUtil.PanelManager;
            if (mgr != null)
            {
                Debug.Log("Showing Invitation Panel!");
                mgr.OpenLoadingPanel();
            }
            else
            {
                Debug.Log("PanelManager script Missing");
            }
        }
    }
}