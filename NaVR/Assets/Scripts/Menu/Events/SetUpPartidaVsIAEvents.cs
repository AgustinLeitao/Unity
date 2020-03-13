namespace Menu.Events 
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Menu.Utils;
    using UnityEngine.SceneManagement;
    using UnityEngine.VR;

    public class SetUpPartidaVsIAEvents : MonoBehaviour {

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void OnVolverClicked() 
		{
			Debug.Log("Boton volver clickeado");
			NavigationUtil.ShowMainMenu();
		}

		public void OnConfigurarPartidaClicked() 
		{
			Debug.Log("Boton configurar partida clickeado");
		}

		public void OnPartidaRapidaClicked() 
		{
			Debug.Log("Boton Partida rapida clickeada");
			NavigationUtil.ShowLoadingScreen();
           // SceneManager.LoadScene("DistribucionDeBarcos");
           // StartCoroutine(LoadDevice("cardboard"));
		}

		public void actualizarConfiguracionActual()
		{
			Debug.Log("Configuracion actual actualizada");
		}

	/*	IEnumerator LoadDevice(string newDevice)
        {
            VRSettings.LoadDeviceByName(newDevice);
            yield return null;
            VRSettings.enabled = true;
        } */
	}
}
