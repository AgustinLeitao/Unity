using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CargaEscenaAsync : MonoBehaviour
{
    public GameObject imagenCarga, porcentajeCarga, canvasCargaEscena;
    public GameObject[] canvasADeshabilitar;

    private bool estaCargandoEscena;
    private AsyncOperation asyncScene;  
   
    void Update ()
    {
        if (estaCargandoEscena)
        {
            StartCoroutine(CargarEscenaAsyncCorutine());
        }
    }

    public void CargarEscenaAsync(string scene)
    {
        foreach (var canvas in canvasADeshabilitar)
            canvas.SetActive(false);

        estaCargandoEscena = true;
        canvasCargaEscena.SetActive(true);
        asyncScene = SceneManager.LoadSceneAsync(scene);
        asyncScene.allowSceneActivation = false;
    }

    private IEnumerator CargarEscenaAsyncCorutine()
    {
        float porcentaje = 0;
        while (!asyncScene.isDone)
        {
            if (asyncScene.progress < 0.9f)
            {
                imagenCarga.GetComponent<Image>().fillAmount = asyncScene.progress / 0.9f;
                porcentaje = asyncScene.progress * 100;
                porcentajeCarga.GetComponent<Text>().text = (int)porcentaje + "%";
            }
            else
            {
                imagenCarga.GetComponent<Image>().fillAmount = asyncScene.progress / 0.9f;
                porcentaje = (asyncScene.progress / 0.9f) * 100;
                porcentajeCarga.GetComponent<Text>().text = (int)porcentaje + "%";
                asyncScene.allowSceneActivation = true;
            }
            yield return null;
        }
        yield return asyncScene;
    }
}
