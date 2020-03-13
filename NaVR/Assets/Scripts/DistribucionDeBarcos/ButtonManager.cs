using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

    public Button btnBarcoDos, btnBarcoTres, btnBarcoCuatro, btnBarcoCinco, btnContinue, btnRotarBarco, btnLimpiarTablero;
    public Text textContinue, textValidarContinue, textErrores;
    public const int CANTIDAD_DE_BARCOS_DE_DOS_POSICIONES = 3, CANTIDAD_DE_BARCOS_DE_TRES_POSICIONES = 2, CANTIDAD_DE_BARCOS_DE_CUATRO_POSICIONES = 2, CANTIDAD_DE_BARCOS_DE_CINCO_POSICIONES = 1 ;

    [HideInInspector]
    public int contadorDos = CANTIDAD_DE_BARCOS_DE_DOS_POSICIONES, contadorTres = CANTIDAD_DE_BARCOS_DE_TRES_POSICIONES, contadorCuatro = CANTIDAD_DE_BARCOS_DE_CUATRO_POSICIONES, contadorCinco = CANTIDAD_DE_BARCOS_DE_CINCO_POSICIONES;

    public Color btnContinueInitialColor;
    public int btnContinueInitialFontSize;
    public string btnContinueInitialText;

    // Use this for initialization
    void Start () {
        btnContinueInitialColor = textContinue.color;
        btnContinueInitialFontSize = textContinue.fontSize;
        btnContinueInitialText = textValidarContinue.text;
    }

    // Update is called once per frame
    void Update()
    {
        Color color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * 1f, 1));
        if (contadorDos == 0)
            btnBarcoDos.interactable = false;
        if (contadorTres == 0)
            btnBarcoTres.interactable = false;
        if (contadorCuatro == 0)
            btnBarcoCuatro.interactable = false;
        if (contadorCinco == 0)
            btnBarcoCinco.interactable = false;      
        if (contadorDos == 0 && contadorTres == 0 && contadorCuatro == 0 && contadorCinco == 0) {
            textValidarContinue.text = string.Format("Pulse este boton para\ncontinuar al juego");
            textContinue.fontSize = 45;       
            textContinue.color = color;
            btnContinue.interactable = true;
        }
        textErrores.color = color;
    }
}
