namespace Menu.Managers
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class PanelManager : MonoBehaviour
    {

        public GameObject loginPanel;
        public GameObject mainMenu;
        public GameObject invitationPanel;
        public GameObject setUpPartidaVsJugadorPanel;
        public GameObject watingRoomPanel;
        public GameObject setUpPartidaVsIAPanel;
        public GameObject loadingPanel;

        private GameObject currentPanel;
        private GameObject prevSelected;

        void Start()
        {
            OpenPanel(loginPanel);
        }

        public void OpenLoginPanel()
        {
            OpenPanel(loginPanel);
        }

        public void OpenMainMenu()
        {
            OpenPanel(mainMenu);
        }

        public void OpenWaitingRoomPanel() 
        {
            OpenPanel(watingRoomPanel);
        }

        public void OpenInvitationPanel()
        {
            OpenPanel(invitationPanel);
        }

        public void OpenSetUpPartidaVsJugadorPanel() 
        {
            OpenPanel(setUpPartidaVsJugadorPanel);
        }

        public void OpenSetUpPartidaVsIAPanel() 
        {
            OpenPanel(setUpPartidaVsIAPanel);
        }

        public void OpenLoadingPanel() 
        {
            OpenPanel(loadingPanel);
        }

        void OpenPanel(GameObject panel)
        {

            if (currentPanel == panel)
            {
                return;
            }

            if (currentPanel != null)
            {
                ClosePanel(currentPanel);
            }

            panel.gameObject.SetActive(true);
            prevSelected = EventSystem.current.currentSelectedGameObject;
            currentPanel = panel;

            Selectable[] items = panel.GetComponentsInChildren<Selectable>(true);


            foreach (Selectable s in items)
            {
                if (s.IsActive() && s.IsInteractable())
                {
                    EventSystem.current.SetSelectedGameObject(s.gameObject);
                    break;
                }
            }
        }

        void Update()
        {

            StandaloneInputModule mod = EventSystem.current.currentInputModule as StandaloneInputModule;

            //handle quirk where the input is mouse mode at the start, so fire the submit event manually
            if (Input.GetKey(KeyCode.JoystickButton0))
            {

                if (mod != null && mod.inputMode == StandaloneInputModule.InputMode.Mouse)
                {
                    Button b = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                    if (b)
                    {
                        ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject,
                            null, ExecuteEvents.submitHandler);
                    }
                }
            }

        }

        void ClosePanel(GameObject panel)
        {
            panel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(prevSelected);
        }
    }
}
