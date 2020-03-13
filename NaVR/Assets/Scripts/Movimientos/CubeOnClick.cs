using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeOnClick : MonoBehaviour {

	public void changeSceneToTableroPosicion () {
        Debug.Log("ir a tablero posicion");
        SceneManager.LoadScene("TableroPosicion");
    }

	public void changeSceneToTableroAtaque () {
		Debug.Log("ir a tablero ataque");
		SceneManager.LoadScene("TableroAtaque");
	}
}


