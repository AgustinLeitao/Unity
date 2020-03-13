using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FakeBoardManager : MonoBehaviour {

	private const float TILE_SIZE = 1.0F;
	private const float TILE_OFFSET = 0.5F;

	private Vector3 widthLine;
	private Vector3 heightLine;

	public GameObject reticle;
	public Material lineMaterial;

	public shipEnum[,] matrizDeBarcos { get; set; }

	public GameObject buttonManager;
	private FakeButtonManager buttonManagerObject;
    private FakeBackToBoat boat;

    void Start()
	{
		widthLine = Vector3.right * 10;
		heightLine = Vector3.forward * 10;
		matrizDeBarcos = new shipEnum[10, 10];
		buttonManagerObject = (FakeButtonManager)buttonManager.GetComponent("FakeButtonManager");
        boat = new FakeBackToBoat();

        DrawBoard();
	}

	void Update()
	{
		RaycastHit hit;
		if(Physics.Raycast(new Ray(reticle.transform.position, reticle.transform.forward), out hit, 100F, LayerMask.GetMask("Clickable")) && Input.GetButtonDown("Fire1"))
        {
            boat.BackToBoat();
        }
		LoadScript ();

	}

	private void DrawBoard()
	{
		for (int i = 0; i <= 10; i++)
		{
			Vector3 start = Vector3.right * i;
			DrawLine(start, start + heightLine);
		}

		for (int j = 0; j <= 10; j++)
		{
			Vector3 start = Vector3.forward * j;
			DrawLine(start, start + widthLine);
		}
	}

	private void DrawLine(Vector3 start, Vector3 end)
	{
		GameObject myLine = new GameObject("Lineas del tablero");
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.material = lineMaterial;      
		lr.startWidth  = 0.1f;     
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);      
	}

	public void LoadScript()
	{
		var shipManager = GameObject.Find ("ShipManager");
		if (SceneManager.GetActiveScene ().name == "TableroPosicion") {
			if (!shipManager.GetComponent<FakeBarco2Posiciones> ()) {
				FakeBarco2Posiciones barco = shipManager.AddComponent<FakeBarco2Posiciones> ();
				barco.reticle = GameObject.Find ("GvrReticlePointer");
				barco.prefabBarco = (GameObject)Resources.Load ("Prefabs/Barco_Horizontal", typeof(GameObject));
				barco.boardManager = this;
				barco.buttonManager = (FakeButtonManager)GameObject.Find ("FakeButtonManager").GetComponent ("FakeButtonManager");
			}
		} else if (SceneManager.GetActiveScene ().name == "TableroAtaque") {
			if (!shipManager.GetComponent<FakePosicionesAtacadas> ()) {
				FakePosicionesAtacadas barco = shipManager.AddComponent<FakePosicionesAtacadas> ();
				barco.reticle = GameObject.Find ("GvrReticlePointer");
				barco.prefabBarco = (GameObject)Resources.Load ("Prefabs/Cube", typeof(GameObject));
				barco.boardManager = this;
				barco.buttonManager = (FakeButtonManager)GameObject.Find ("FakeButtonManager").GetComponent ("FakeButtonManager");
			}
		}
	}
}
