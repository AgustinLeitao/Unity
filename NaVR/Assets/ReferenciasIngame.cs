using UnityEngine;
using UnityEngine.UI;

public class ReferenciasIngame : MonoBehaviour
{
    public GameObject reticle, panelAyuda, btnReglasDelJuego;
    private bool estaMostradoseReferencias;

    private void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(reticle.transform.position, reticle.transform.forward);

        if (Input.GetButtonDown("Fire1") && Physics.Raycast(ray, out hit, 100F, LayerMask.GetMask("BotonesDelJuego")))
        {
            if(hit.transform.name == "BtnMostrarReferencias")
            {            
                if(estaMostradoseReferencias)
                {
                    panelAyuda.SetActive(false);
                    estaMostradoseReferencias = false;
                    btnReglasDelJuego.GetComponent<Text>().text = "Mostrar Referencias";
                }
                else
                {
                    panelAyuda.SetActive(true);
                    estaMostradoseReferencias = true;
                    btnReglasDelJuego.GetComponent<Text>().text = "Ocultar Referencias";
                }          
            }
        }
    }
}
