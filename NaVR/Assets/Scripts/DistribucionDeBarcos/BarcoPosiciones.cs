using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BarcoPosiciones : MonoBehaviour {

	public Text textoInformativo, textoContador, textErrores;
	private float speed = 1;
	private int selectionXOrigen, selectionYOrigen;
	private int esTemporal;
	public GameObject reticle;
	public GameObject prefabBarco;
	public GameObject prefabBarcoTemp;
	public BoardManager boardManager;
	private GameObject refBarco;
	private GameObject refBarcoTemp;
	public ButtonManager buttonManager;
	public ValidadorPosiciones validadorPosiciones;
	public Barco barco;
	public int orientacionBarco;

    [HideInInspector]
    public int tipoBarco;
	public int tipoHabilidades;

    // Use this for initialization
    void Start () {
		//barco = new Barco ();
	}

	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if (Physics.Raycast (new Ray (reticle.transform.position, reticle.transform.forward), out hit, 100F, LayerMask.GetMask ("BtnRotarBarco")) && Input.GetButtonDown ("Fire1")) {
			if (this.orientacionBarco == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
				this.orientacionBarco = ConstantesDeBarco.ORIENTACION_VERTICAL;
				boardManager.orientacionBarco = ConstantesDeBarco.ORIENTACION_VERTICAL;
				boardManager.textOrientacionBarco.text = "Vertical";
			} else if (this.orientacionBarco == ConstantesDeBarco.ORIENTACION_VERTICAL) {
				this.orientacionBarco = ConstantesDeBarco.ORIENTACION_HORIZONTAL;
				boardManager.orientacionBarco = ConstantesDeBarco.ORIENTACION_HORIZONTAL;
				boardManager.textOrientacionBarco.text = "Horizontal";
			}
		}
		textoInformativo.text = "Coloque el barco en el tablero.";
		Color color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * speed, 1));
		textoInformativo.color = color;
		UpdateSelection ();   
    }

	private void OnDestroy() {
		if(textoInformativo != null)
			textoInformativo.text = string.Empty;
		if (textErrores != null)
			textErrores.text = string.Empty;
        if (refBarco != null)
            Destroy(refBarco);
	}

	private IEnumerator MostrarMensaje(string message, float delay) {    
		textErrores.text = message;     
		yield return new WaitForSeconds(delay);
		textErrores.text = string.Empty;
	}

	private void UpdateSelection()	{
		if (!Camera.main) {
			return;
		}

		RaycastHit hit;
		if (Physics.Raycast (new Ray (reticle.transform.position, reticle.transform.forward), out hit, 25f, LayerMask.GetMask ("ShipBoard"))) {           
			if (Input.GetButtonDown ("Fire1")) {
				barco = validadorPosiciones.ValidarSiEsPosibleUbicarBarco (hit, this.tipoBarco, this.orientacionBarco, this.tipoHabilidades);
				if (barco != null) {
					esTemporal = 0;
					DibujarBarco (hit, this.tipoBarco, this.orientacionBarco, esTemporal);
					//boardManager.barcos [boardManager.numeroBarco] = barco;
					boardManager.contenedorBarcos.barcos [boardManager.numeroBarco] = barco; //Codigo agregado
					boardManager.numeroBarco++;
					//El metodo de abajo lo uso solamente para ver como queda el tablero.
					//validadorPosiciones.MostrarContenidoTablero ();
				} else {
					StopAllCoroutines ();
					StartCoroutine (MostrarMensaje (ErrorsMessages.ERROR_BARCO, 5f));
				}
			} else {
				barco = validadorPosiciones.ValidarSiEsPosibleUbicarBarco (hit, this.tipoBarco, this.orientacionBarco, this.tipoHabilidades);
				if (barco != null) {
					esTemporal = 1;
					DibujarBarco (hit, this.tipoBarco, this.orientacionBarco, esTemporal);
				} else {
					DibujarBarcoNoDisponible (hit, this.tipoBarco, this.orientacionBarco);
				}
			}
		} else {
			if (esTemporal == 1 && refBarco != null) {
				Destroy (refBarco);
			}
			if (refBarcoTemp != null) {
				Destroy (refBarcoTemp);
			}
		}
	}

	private void DibujarBarcoNoDisponible (RaycastHit hit, int tipoBarco, int orientacionBarco) {
		selectionXOrigen = (int)hit.point.x;
		selectionYOrigen = (int)hit.point.z;
		if (refBarcoTemp != null) {
			Destroy (refBarcoTemp);
		}
		if (esTemporal == 1 && refBarco != null) {
			Destroy (refBarco);
		}
		switch(tipoBarco) {
		case 2:
			if (orientacionBarco == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
				refBarcoTemp = Instantiate (prefabBarcoTemp, new Vector3 (selectionXOrigen + 0.25f, 0, selectionYOrigen + 0.65f), Quaternion.Euler (new Vector3 (0, 90, 0)));	
			} else {
				if (orientacionBarco == ConstantesDeBarco.ORIENTACION_VERTICAL) {
					refBarcoTemp = Instantiate (prefabBarcoTemp, new Vector3 (selectionXOrigen + 1.5f, 0, selectionYOrigen + 0.25f), Quaternion.identity);	
				}
			}
			break;
		case 3:
			if (orientacionBarco == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
				refBarcoTemp = Instantiate (prefabBarcoTemp, new Vector3 (selectionXOrigen + 0.25f, 0, selectionYOrigen + 1.1f), Quaternion.Euler (new Vector3(0,90,0)));	
			} else {
				if (orientacionBarco == ConstantesDeBarco.ORIENTACION_VERTICAL) {
					refBarcoTemp = Instantiate (prefabBarcoTemp, new Vector3 (selectionXOrigen + 2f, 0, selectionYOrigen + 0.25f), Quaternion.identity);
				}
			}
			break;
		case 4:
			if (orientacionBarco == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
				refBarcoTemp = Instantiate (prefabBarcoTemp, new Vector3 (selectionXOrigen + 0.25f, 0, selectionYOrigen + 1.25f), Quaternion.Euler (new Vector3(0,90,0)));	
			} else {
				if (orientacionBarco == ConstantesDeBarco.ORIENTACION_VERTICAL) {
					refBarcoTemp = Instantiate (prefabBarcoTemp, new Vector3 (selectionXOrigen + 3f, 0, selectionYOrigen + 0.25f), Quaternion.identity);
				}
			}
			break;
		case 5:
			if (orientacionBarco == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
				refBarcoTemp = Instantiate (prefabBarcoTemp, new Vector3 (selectionXOrigen + 0.25f, 0, selectionYOrigen + 1.45f), Quaternion.Euler (new Vector3(0,90,0)));
			} else {
				if (orientacionBarco == ConstantesDeBarco.ORIENTACION_VERTICAL) {
					refBarcoTemp = Instantiate (prefabBarcoTemp, new Vector3 (selectionXOrigen + 3.5f, 0, selectionYOrigen + 0.25f), Quaternion.identity);
				}
			}
			break;
		}
	}

	private void DibujarBarco (RaycastHit hit, int tipoBarco, int orientacionBarco, int barcoTemporal) {
		selectionXOrigen = (int)hit.point.x;
		selectionYOrigen = (int)hit.point.z;
		if (refBarcoTemp != null) {
			Destroy (refBarcoTemp);
		}
		if (barcoTemporal == 1 && refBarco != null) {
			Destroy (refBarco);
		}
		switch(tipoBarco) {
		case 2:
			if (orientacionBarco == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
				refBarco = Instantiate (prefabBarco, new Vector3 (selectionXOrigen + 0.25f, 0, selectionYOrigen + 0.65f), Quaternion.Euler (new Vector3 (0, 90, 0)));
                refBarco.tag = "BarcoPosiciones";    
			} else {
				if (orientacionBarco == ConstantesDeBarco.ORIENTACION_VERTICAL) {
					    refBarco = Instantiate (prefabBarco, new Vector3 (selectionXOrigen + 1.5f, 0, selectionYOrigen + 0.25f), Quaternion.identity);
                        refBarco.tag = "BarcoPosiciones";
                    }
			}
			if (barcoTemporal == 0) {
				//boardManager.refBarcos [boardManager.numeroBarco] = refBarco;
				//Debug.Log (boardManager.refBarcos [boardManager.numeroBarco]);
				validadorPosiciones.ActualizarTablero (hit, this.tipoBarco, orientacionBarco);
				buttonManager.contadorDos--;
                boardManager.sonidoClipBarco.Play();
				textoContador.text = "Remanentes = " + buttonManager.contadorDos;
				if (buttonManager.contadorDos == 0) {
					Destroy (this);
				}
			}
			break;
		case 3:
			if (orientacionBarco == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
				    refBarco = Instantiate (prefabBarco, new Vector3 (selectionXOrigen + 0.25f, 0, selectionYOrigen + 1.1f), Quaternion.Euler (new Vector3 (0, 90, 0)));
                    refBarco.tag = "BarcoPosiciones";
                } else {
				if (orientacionBarco == ConstantesDeBarco.ORIENTACION_VERTICAL) {
					refBarco = Instantiate (prefabBarco, new Vector3 (selectionXOrigen + 2f, 0, selectionYOrigen + 0.25f), Quaternion.identity);
                    refBarco.tag = "BarcoPosiciones";
                }
			}
			if (barcoTemporal == 0) {
				validadorPosiciones.ActualizarTablero (hit, this.tipoBarco, orientacionBarco);
				buttonManager.contadorTres--;
                boardManager.sonidoClipBarco.Play();
                textoContador.text = "Remanentes = " + buttonManager.contadorTres;
				if (buttonManager.contadorTres == 0) {
					Destroy (this);
				}
			}
			break;
		case 4:
			if (orientacionBarco == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
				    refBarco = Instantiate (prefabBarco, new Vector3 (selectionXOrigen + 0.25f, 0, selectionYOrigen + 1.25f), Quaternion.Euler (new Vector3 (0, 90, 0)));
                    refBarco.tag = "BarcoPosiciones";
                } else {
				if (orientacionBarco == ConstantesDeBarco.ORIENTACION_VERTICAL) {
					refBarco = Instantiate (prefabBarco, new Vector3 (selectionXOrigen + 3f, 0, selectionYOrigen + 0.25f), Quaternion.identity);
                    refBarco.tag = "BarcoPosiciones";
                }
			}
			if (barcoTemporal == 0) {
				validadorPosiciones.ActualizarTablero (hit, this.tipoBarco, orientacionBarco);
				buttonManager.contadorCuatro--;
                boardManager.sonidoClipBarco.Play();
                textoContador.text = "Remanentes = " + buttonManager.contadorCuatro;
				if (buttonManager.contadorCuatro == 0) {
					Destroy (this);
				}
			}
			break;
		case 5:
			if (orientacionBarco == ConstantesDeBarco.ORIENTACION_HORIZONTAL) {
				    refBarco = Instantiate (prefabBarco, new Vector3 (selectionXOrigen + 0.25f, 0, selectionYOrigen + 1.45f), Quaternion.Euler (new Vector3 (0, 90, 0)));
                    refBarco.tag = "BarcoPosiciones";
                } else {
				if (orientacionBarco == ConstantesDeBarco.ORIENTACION_VERTICAL) {
					refBarco = Instantiate (prefabBarco, new Vector3 (selectionXOrigen + 3.5f, 0, selectionYOrigen + 0.25f), Quaternion.identity);
                    refBarco.tag = "BarcoPosiciones";
                }
			}
			if (barcoTemporal == 0) {
				validadorPosiciones.ActualizarTablero (hit, this.tipoBarco, orientacionBarco);
				buttonManager.contadorCinco--;
                boardManager.sonidoClipBarco.Play();
                textoContador.text = "Remanentes = " + buttonManager.contadorCinco;
				if (buttonManager.contadorCinco == 0) {
					Destroy (this);
				}
			}
			break;
		}
	}
}

