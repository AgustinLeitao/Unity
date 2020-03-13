using UnityEngine;

public class PantallaClick1 : MonoBehaviour {

	public int marker;
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
	}

	public void cabinaMoveTo () {

		//player.GetComponent<PlayerMovement> ().iniciarCambio (marker, this.gameObject);
		//player.GetComponent<PlayerMovement>().cambiarCabina = true;
		//player.GetComponent<PlayerMovement>().nuevaCabina = marker;
	}
}
