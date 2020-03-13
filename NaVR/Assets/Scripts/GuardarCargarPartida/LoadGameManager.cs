using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameManager : MonoBehaviour
{
    [HideInInspector]
    public DatosPartida datosPartida;

    [HideInInspector]
    public int contadorDePartidasGuardadas, numeroPartidaACargar;

    public GameObject listaPartidasGuardadas;
    public GameObject prefabBtnPartidaGuardada;

    private GameObject partidaSeleccionada;
    private CargaEscenaAsync cargaEscenaAsync;

    private void Start()
    {
        contadorDePartidasGuardadas = 0;
        cargaEscenaAsync = GameObject.Find("CargaEscenaAsync").GetComponent<CargaEscenaAsync>();

        MostrarPartidasGuardadas();
    }

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject); // Necesario para mantener el gameObject en la escena del juego y poder cargar la partida seleccionada en dicha escena.     
    }            	

    void MostrarPartidasGuardadas()
    {
        var registroIndexado = PlayerPrefs.GetString(string.Format("PartidaIndexada{0}", contadorDePartidasGuardadas), string.Empty);
        while (registroIndexado != string.Empty)
        {
            string registro;
            if (!registroIndexado.Equals("Borrado"))
            {
                registro = PlayerPrefs.GetString(string.Format("Partida{0}", contadorDePartidasGuardadas), string.Empty);
                datosPartida = Serializador.Deserializar<DatosPartida>(registro);

                var btnPartida = Instantiate(prefabBtnPartidaGuardada, listaPartidasGuardadas.transform, false);
                int numeroPartida = contadorDePartidasGuardadas;                          

                btnPartida.name = string.Format("Partida {0}", contadorDePartidasGuardadas);
                btnPartida.GetComponent<Button>().onClick.AddListener(delegate { SeleccionarPartida(numeroPartida, btnPartida); });          
                btnPartida.GetComponentInChildren<Text>().text = datosPartida.fechaGuardado.ToString();                 
            }

            contadorDePartidasGuardadas++;
            registroIndexado = PlayerPrefs.GetString(string.Format("PartidaIndexada{0}", contadorDePartidasGuardadas), string.Empty);
        }
    }

    void SeleccionarPartida(int numeroPartida, GameObject btnPartida)
    {
        if (partidaSeleccionada != null)
            partidaSeleccionada.transform.GetChild(1).gameObject.SetActive(false);

        numeroPartidaACargar = numeroPartida;
        partidaSeleccionada = btnPartida;
        partidaSeleccionada.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void CargarPartida()
    {
        if (partidaSeleccionada != null)
        {
            datosPartida = Serializador.Deserializar<DatosPartida>(PlayerPrefs.GetString(string.Format("Partida{0}", numeroPartidaACargar), string.Empty));

            GameObject.Find("Opciones").GetComponent<OpcionesDelJuego>().esModoIA = true;
            cargaEscenaAsync.CargarEscenaAsync("DemoExpo");          
        }
    }

    public void BorrarPartida()
    {
        if (partidaSeleccionada != null)
        {
            Destroy(partidaSeleccionada.gameObject);
            PlayerPrefs.DeleteKey(string.Format("Partida{0}", numeroPartidaACargar)); // Borrado fisico
            PlayerPrefs.SetString(string.Format("PartidaIndexada{0}", numeroPartidaACargar), "Borrado"); // Borrado logico para mantener contiguedad de partidas
        }     
        partidaSeleccionada = null;
    }
}
