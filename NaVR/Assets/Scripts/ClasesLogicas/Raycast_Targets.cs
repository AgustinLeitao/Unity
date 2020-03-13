using UnityEngine;

public class Raycast_Targets : MonoBehaviour
{
    public GameObject reticle;
    public AdministradorPartida adminPartida;   

    private void Update ()
    {
        RaycastHit hit;
        Ray ray = new Ray(reticle.transform.position, reticle.transform.forward);

        if (Input.GetButtonDown("Fire1") && Physics.Raycast(ray, out hit, 100F, LayerMask.GetMask("BotonesDelJuego")))
        {
            switch(hit.transform.name)
            {
                case "Torpedo": adminPartida.modoTorpedo(); break;
                case "ReforzarArmadura": adminPartida.modoReforzarArmadura(); break;
                case "DisparoDoble": adminPartida.modoAtaqueDoble(); break;
                case "Sabotaje": adminPartida.modoSabotaje(); break;
                case "ProyectilHE": adminPartida.modoProyectilAltamenteExplosivo(); break;
                case "ArtilleriaRapida": adminPartida.modoArtilleriaRapida(); break;
                case "PulsoElectromagnetico": adminPartida.modoPulsoElectromagnetico(); break;
                case "TormentaDeMisiles": adminPartida.modoTormentaDeMisiles(); break;
                case "BtnReiniciarPartida": adminPartida.ReiniciarPartida(); break;
                case "BtnGuardarPartida": adminPartida.GuardarPartida(); break;
                case "BtnAbandonarPartida": adminPartida.AbandonarPartida(); break;
                case "BtnRegresarMenuPrincipal": adminPartida.AbandonarPartida(); break;                                          
            }
        }
    }
}
