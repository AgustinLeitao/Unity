using UnityEngine;
using UnityEngine.UI;

public class ReglasDelJuegoIngame : MonoBehaviour
{
    public GameObject reticle, panelAyuda, btnReglasDelJuego;
    private bool estaMostradosePanelAyuda;

    private void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(reticle.transform.position, reticle.transform.forward);

        if (Input.GetButtonDown("Fire1") && Physics.Raycast(ray, out hit, 100F, LayerMask.GetMask("BotonesDelJuego")))
        {
            switch (hit.transform.name)
            {            
                case "BtnReglasDelJuego": 
                    if(estaMostradosePanelAyuda)
                    {
                        panelAyuda.SetActive(false);
                        estaMostradosePanelAyuda = false;
                        btnReglasDelJuego.GetComponent<Text>().text = "Mostrar Ayuda";
                    }
                    else
                    {
                        panelAyuda.SetActive(true);
                        estaMostradosePanelAyuda = true;
                        btnReglasDelJuego.GetComponent<Text>().text = "Ocultar Ayuda";
                    }
                    break;               
            }
        }
    }
}
