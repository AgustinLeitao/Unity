using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;

public class OpcionesDelJuego : MonoBehaviour
{
    [HideInInspector]
    public int dificultad, cantidadTurnosMax, invitacionCantidadTurnosMax;

    [HideInInspector]
    public bool modoContraReloj, modoClasico, invitacionModoContraReloj, invitacionModoClasico, autoGuardado, esModoIA, esPartidaRapida = false, yoEnvieInvitacion = false;

    [HideInInspector]
    public float volumenMusica, volumenEfectos;

    public GameObject cantidadTurnosInput, sliderVolumenMusica, sliderVolumenEfectos;

	void Start ()
    {
        if(GameObject.Find("OpcionesDelJuego") == null)
            DontDestroyOnLoad(gameObject);

        cantidadTurnosMax = 10;
        invitacionCantidadTurnosMax = 10;

        cantidadTurnosInput.GetComponent<InputField>().text = "10";

        VRSettings.enabled = false;
        StartCoroutine(SwitchOutOfVr());

        dificultad = 1;
        volumenMusica = 0.15f;
        volumenEfectos = 0.15f;
    }

    IEnumerator SwitchOutOfVr()
    {
        VRSettings.LoadDeviceByName(""); // Empty string loads the "None" device.

        // Wait one frame!
        yield return null;

        // Not needed, loading the None (`""`) device automatically sets `VRSettings.enabled` to `false`.
        // VRSettings.enabled = false;

        // If you only have one camera in your scene, you can just call `Camera.main.ResetAspect()` instead.
        ResetCameras();
    }

    private void ResetCameras()
    {
        // Camera looping logic copied from GvrEditorEmulator.cs
        for (int i = 0; i < Camera.allCameras.Length; i++)
        {
            Camera cam = Camera.allCameras[i];
            if (cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None)
            {
                // Reset local rotation. (Only required if you change the local rotation while in non-VR mode.)
                cam.transform.localRotation = Quaternion.identity;
                // Reset local position. (Only required if you change the local position while in non-VR mode.)
                cam.transform.localPosition = Vector3.zero;
                // Reset aspect ratio based on normal (non-VR) screen size.
                cam.ResetAspect();
                // Don't need to reset camera `fieldOfView`, since it's restored to the original value automatically.
            }
        }
    }
    
    public void AutoGuardado()
    {      
        if (autoGuardado)
            autoGuardado = false;
        else
            autoGuardado = true;     
    }

    public void DificultadFacil()
    {
        dificultad = 1;
    }

    public void DificultadMedia()
    {
        dificultad = 2;
    }

    public void DificultadDificil()
    {
        dificultad = 3;
    }

    public void ModoContraReloj()
    {
        if (modoContraReloj)
            modoContraReloj = false;
        else
            modoContraReloj = true;
    }

    public void ModoClasico()
    {
        if (modoClasico)
            modoClasico = false;
        else
            modoClasico = true;
    }

    public void ModoClasicoInvitacion()
    {
        if (invitacionModoClasico)
            invitacionModoClasico = false;
        else
            invitacionModoClasico = true;
    }

    public void ModoContraRelojInvitacion()
    {
        if (invitacionModoContraReloj)
            invitacionModoContraReloj = false;
        else
            invitacionModoContraReloj = true;
    }

    public void CantidadDeTurnosMaximo(string turnos)
    {
        if (string.IsNullOrEmpty(turnos))
            cantidadTurnosMax = 0;
        else
        {
            if (!int.TryParse(turnos, out cantidadTurnosMax))
                cantidadTurnosMax = 0;
        }
    }   

    public void CantidadDeTurnosMaximoInvitacion(string turnos)
    {
        if (string.IsNullOrEmpty(turnos))
            invitacionCantidadTurnosMax = 0;
        else
        {
            if (!int.TryParse(turnos, out invitacionCantidadTurnosMax))
                invitacionCantidadTurnosMax = 0;
        }          
    }
    
    public void VolumenMusica()
    {
       volumenMusica = sliderVolumenMusica.GetComponent<Slider>().value;
    }

    public void VolumenEfectos()
    {
        volumenEfectos = sliderVolumenEfectos.GetComponent<Slider>().value;
    }
}