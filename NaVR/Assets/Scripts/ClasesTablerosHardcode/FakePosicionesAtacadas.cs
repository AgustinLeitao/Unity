using UnityEngine;
using UnityEngine.UI;

public class FakePosicionesAtacadas : MonoBehaviour {

	public Text textoInformativo, textoContador, textErrores;
	public GameObject reticle;
	public GameObject prefabBarco;
	public FakeBoardManager boardManager;
	private GameObject refBarco;
	public FakeButtonManager buttonManager;

	// Use this for initialization
	void Start ()
	{ 
		//Barco 3 posiciones
		Instantiate(prefabBarco, new Vector3(5 + 0.5f, 0, 6 + 0.5f), Quaternion.identity);
		Instantiate(prefabBarco, new Vector3(5 + 0.5f, 0, 7 + 0.5f), Quaternion.identity);
		Instantiate(prefabBarco, new Vector3(5 + 0.5f, 0, 8 + 0.5f), Quaternion.identity);
		//Barco 3 posiciones
		Instantiate(prefabBarco, new Vector3(2 + 0.5f, 0, 2 + 0.5f), Quaternion.identity);
		Instantiate(prefabBarco, new Vector3(3 + 0.5f, 0, 2 + 0.5f), Quaternion.identity);
		Instantiate(prefabBarco, new Vector3(4 + 0.5f, 0, 2 + 0.5f), Quaternion.identity);
		//Barco 2 posiciones
		Instantiate(prefabBarco, new Vector3(0 + 0.5f, 0, 8 + 0.5f), Quaternion.identity);
		Instantiate(prefabBarco, new Vector3(0 + 0.5f, 0, 9 + 0.5f), Quaternion.identity);
		//Barco 2 posiciones
		Instantiate(prefabBarco, new Vector3(0 + 0.5f, 0, 1 + 0.5f), Quaternion.identity);
		Instantiate(prefabBarco, new Vector3(0 + 0.5f, 0, 2 + 0.5f), Quaternion.identity);
		//Barco 4 posiciones
		Instantiate(prefabBarco, new Vector3(8 + 0.5f, 0, 1 + 0.5f), Quaternion.identity);
		Instantiate(prefabBarco, new Vector3(8 + 0.5f, 0, 2 + 0.5f), Quaternion.identity);
		Instantiate(prefabBarco, new Vector3(8 + 0.5f, 0, 3 + 0.5f), Quaternion.identity);
		Instantiate(prefabBarco, new Vector3(8 + 0.5f, 0, 4 + 0.5f), Quaternion.identity);
		//Barco 4 posiciones
		Instantiate(prefabBarco, new Vector3(9 + 0.5f, 0, 6 + 0.5f), Quaternion.identity);
		Instantiate(prefabBarco, new Vector3(9 + 0.5f, 0, 7 + 0.5f), Quaternion.identity);
		Instantiate(prefabBarco, new Vector3(9 + 0.5f, 0, 8 + 0.5f), Quaternion.identity);
		Instantiate(prefabBarco, new Vector3(9 + 0.5f, 0, 9 + 0.5f), Quaternion.identity);
	}

	// Update is called once per frame
	void Update ()
	{       
		if (!Camera.main)
			return;
	}
}