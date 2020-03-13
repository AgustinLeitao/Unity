using System;
using System.Collections;
using UnityEngine;

public class BoatManager : MonoBehaviour {

	public GameObject reticle;
	public GameObject player;
	public GameObject board;
	public GameObject hundido, agua, circulo;
    public GameObject explosion;
    public GameObject particulaExplosion1;
    public GameObject particulaExplosion2;
    public GameObject rocketUp;
	public GameObject rocketDown;
    public GameObject splash;
    public GameObject rocketDownSplash; 

    private Boat[,] matrizBarcos;
	int[,] matrizAtaque;
	private int rowSelected, columnSelected;

	private GameObject[] listaCirculos;
	private Boat[] listaHundidos;
	private Vector2[] listaPos;
	private int circuloActual;
	private int seleccionados;
	private bool habilitado;
	private Vector3 posSplash;	
    
	void Start () {
		matrizBarcos = new Boat[10, 10];
		matrizAtaque = new int[10, 10];
		circuloActual = 0;
		seleccionados = 0;
		listaCirculos = new GameObject[3];
		listaHundidos = new Boat[3];
		listaPos = new Vector2[3];
		habilitado = true;
		InicializarMatrizDePrueba();	
	}

	private void InicializarMatrizDePrueba()
	{
		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
				matrizBarcos [i, j] = new Boat ();
			}
		}

		Boat aux = new Boat(shipEnum.barcoTresPosiciones, GameObject.Find("Barco3Pos"));
		aux.addCoord (new Vector2 (6, 7));
		aux.addCoord (new Vector2 (7, 7));
		aux.addCoord (new Vector2 (8, 7));
		matrizBarcos[6, 7] = aux;
		matrizBarcos[7, 7] = aux;
		matrizBarcos[8, 7] = aux;

		Boat aux2 = new Boat(shipEnum.barcoCuatroPosiciones, GameObject.Find("Barco4Pos"));
		aux2.addCoord (new Vector2 (4, 3));
		aux2.addCoord (new Vector2 (4, 4));
		aux2.addCoord (new Vector2 (4, 5));
		aux2.addCoord (new Vector2 (4, 6));
		matrizBarcos[4, 3] = aux2;
		matrizBarcos[4, 4] = aux2;
		matrizBarcos[4, 5] = aux2;
		matrizBarcos[4, 6] = aux2;

		Boat aux3 = new Boat(shipEnum.barcoCincoPosiciones, GameObject.Find("Barco5Pos01"));
		aux3.addCoord (new Vector2 (1, 3));
		aux3.addCoord (new Vector2 (1, 4));
		aux3.addCoord (new Vector2 (1, 5));
		aux3.addCoord (new Vector2 (1, 6));
		aux3.addCoord (new Vector2 (1, 7));
		matrizBarcos[1, 3] = aux3;
		matrizBarcos[1, 4] = aux3;
		matrizBarcos[1, 5] = aux3;
		matrizBarcos[1, 6] = aux3;
		matrizBarcos[1, 7] = aux3;

		Boat aux4 = new Boat(shipEnum.barcoCincoPosiciones, GameObject.Find("Barco5Pos02"));
		aux4.addCoord (new Vector2 (4, 9));
		aux4.addCoord (new Vector2 (5, 9));
		aux4.addCoord (new Vector2 (6, 9));
		aux4.addCoord (new Vector2 (7, 9));
		aux4.addCoord (new Vector2 (8, 9));
		matrizBarcos[4, 9] = aux4;
		matrizBarcos[5, 9] = aux4;
		matrizBarcos[6, 9] = aux4;
		matrizBarcos[7, 9] = aux4;
		matrizBarcos[8, 9] = aux4;

		Boat aux5 = new Boat(shipEnum.barcoCincoPosiciones, GameObject.Find("Barco5Pos03"));
		aux5.addCoord (new Vector2 (9, 1));
		aux5.addCoord (new Vector2 (9, 2));
		aux5.addCoord (new Vector2 (9, 3));
		aux5.addCoord (new Vector2 (9, 4));
		aux5.addCoord (new Vector2 (9, 5));
		matrizBarcos[9, 1] = aux5;
		matrizBarcos[9, 2] = aux5;
		matrizBarcos[9, 3] = aux5;
		matrizBarcos[9, 4] = aux5;
		matrizBarcos[9, 5] = aux5;

		Boat aux6 = new Boat(shipEnum.barcoCincoPosiciones, GameObject.Find("Barco5Pos04"));
		aux6.addCoord (new Vector2 (0, 1));
		aux6.addCoord (new Vector2 (1, 1));
		aux6.addCoord (new Vector2 (2, 1));
		aux6.addCoord (new Vector2 (3, 1));
		aux6.addCoord (new Vector2 (4, 1));
		matrizBarcos[0, 1] = aux6;
		matrizBarcos[1, 1] = aux6;
		matrizBarcos[2, 1] = aux6;
		matrizBarcos[3, 1] = aux6;
		matrizBarcos[4, 1] = aux6;
    }

	public void EnableHundimientoAnimation(GameObject barcoReal)
	{
		GameObject.Find ("CannonCabina").GetComponent<ApuntarObjetivo>().apuntar(barcoReal,0,posSplash);
    }

	public void BoomBarco(GameObject barcoEnemigo){
		rocketUp.SetActive(false);
		rocketUp.SetActive(true);
		StartCoroutine(IniciarAnimacionTrayectoria(barcoEnemigo));
	}

	public void BoomSplash(GameObject splashEnemigo, Vector3 posTarget){
		rocketUp.SetActive(false);
		rocketUp.SetActive(true);
		StartCoroutine(IniciarAnimacionTrayectoriaSplash(splashEnemigo, posTarget));
	}

	public void EnableSplashAnimation(GameObject splash)
	{
		GameObject.Find ("CannonCabina").GetComponent<ApuntarObjetivo>().apuntar(this.splash,1,posSplash);
    }

	public void EnableExplosionAnimation()
	{
		explosion.SetActive(false);
		explosion.SetActive(true);
    }

	public IEnumerator IniciarAnimacionTrayectoria(GameObject barcoEnemigo)
	{
		yield return new WaitForSeconds(3.0f);
        explosion.transform.position = barcoEnemigo.transform.position;
		explosion.transform.LookAt (player.transform.position);
		explosion.transform.Rotate (new Vector3 (45, 0, 0));
        rocketDown.SetActive(false);
        rocketDown.SetActive(true);
		yield return new WaitForSeconds(2.0f);
		barcoEnemigo.SetActive (false);
		particulaExplosion1.SetActive(false);
        particulaExplosion2.SetActive(false);
        particulaExplosion1.SetActive(true);
        particulaExplosion2.SetActive(true);
        //rocketDown.transform.position = new Vector3(0, 30, 0);
	}

	public IEnumerator IniciarAnimacionTrayectoriaSplash(GameObject splash, Vector3 posTarget)
	{
		yield return new WaitForSeconds(3.0f);

		rocketDownSplash.transform.position = new Vector3(posSplash.x, 30, posSplash.z);
        rocketDownSplash.transform.LookAt (player.transform.position);
        rocketDownSplash.transform.Rotate (new Vector3 (45, 0, 0));
        rocketDownSplash.SetActive(false);
        rocketDownSplash.SetActive(true);

        yield return new WaitForSeconds(1.5f);
		splash.transform.position = posTarget;
        splash.GetComponent<activateSplash>().SetActivateEffect(true);
        //rocketDownSplash.transform.position = new Vector3(0, 30, 0);
    }

    public void MoveSplashRandomly(int row, int col)
    {
        System.Random random = new System.Random();
        if (col >= 0 && col <= 1)
        {
            //splash.transform.position = new Vector3(random.Next(-360, -216), -70, random.Next(610, 850));
			posSplash = new Vector3(random.Next(-360, -216), -70, random.Next(610, 850));
        }
        else if (col >= 2 && col <= 3)
        {
			posSplash = new Vector3(random.Next(-216, -72), -70, random.Next(610, 850));
        }
        else if (col >= 4 && col <= 5)
        {
			posSplash = new Vector3(random.Next(-72, 72), -70, random.Next(610, 850));
        }
        else if (col >= 6 && col <= 7)
        {
			posSplash = new Vector3(random.Next(72, 216), -70, random.Next(610, 850));
        }
        else 
        {
			posSplash = new Vector3(random.Next(216, 360), -70, random.Next(610, 850));
        }
 
    }

	public IEnumerator disparoSecuencial()
	{
		if (matrizBarcos [(int) listaPos [0].x,(int)  listaPos [0].y].tamano != shipEnum.posicionLibre) {
			EnableHundimientoAnimation (matrizBarcos [(int) listaPos [0].x, (int) listaPos [0].y].barcoReal);
		} else {
			MoveSplashRandomly ((int)listaPos [0].x, (int)listaPos [0].y);
			EnableSplashAnimation(splash);
		}
		yield return new WaitForSeconds(6f);

		if (matrizBarcos [(int) listaPos [1].x,(int)  listaPos [1].y].tamano != shipEnum.posicionLibre) {
			EnableHundimientoAnimation (matrizBarcos [(int) listaPos [1].x, (int) listaPos [1].y].barcoReal);
			//listaHundidos[1].barcoReal.SetActive(false);
		} else {
			MoveSplashRandomly ((int)listaPos [1].x, (int)listaPos [1].y);
			EnableSplashAnimation(splash);
		}
		yield return new WaitForSeconds(6f);

		if (matrizBarcos [(int) listaPos [2].x, (int) listaPos [2].y].tamano != shipEnum.posicionLibre) {
			EnableHundimientoAnimation (matrizBarcos [(int) listaPos [2].x, (int) listaPos [2].y].barcoReal);
			//listaHundidos[2].barcoReal.SetActive(false);
		} else {
			MoveSplashRandomly ((int)listaPos [2].x, (int)listaPos [2].y);
			EnableSplashAnimation(splash);
		}



		if (matrizBarcos [(int) listaPos [0].x,(int)  listaPos [0].y].tamano != shipEnum.posicionLibre) {
			foreach (var item in listaHundidos[0].listaVec)
			{
				Vector2 vec = (Vector2) item;
				GameObject obj = GameObject.Find ("Pos" + vec.x + vec.y);
				GameObject aux = Instantiate(hundido, new Vector3(obj.transform.position.x+0.005f, obj.transform.position.y, obj.transform.position.z), Quaternion.identity);
				aux.transform.Rotate (new Vector3 (90, 180, 96));
				aux.transform.localScale = new Vector3 (0.0035f,0.0035f,0.0035f);
			}
		} else {
			GameObject obj = GameObject.Find ("Pos" + (int)listaPos [0].x + (int)listaPos [0].y);
			GameObject aux = Instantiate(agua, new Vector3(obj.transform.position.x+0.005f, obj.transform.position.y, obj.transform.position.z), Quaternion.identity);
			aux.transform.Rotate (new Vector3 (90, 180, 96));
			aux.transform.localScale = new Vector3 (0.0035f,0.0035f,0.0035f);
		}

		if (matrizBarcos [(int) listaPos [1].x,(int)  listaPos [1].y].tamano != shipEnum.posicionLibre) {
			foreach (var item in listaHundidos[1].listaVec)
			{
				Vector2 vec = (Vector2) item;
				GameObject obj = GameObject.Find ("Pos" + vec.x + vec.y);
				GameObject aux = Instantiate(hundido, new Vector3(obj.transform.position.x+0.005f, obj.transform.position.y, obj.transform.position.z), Quaternion.identity);
				aux.transform.Rotate (new Vector3 (90, 180, 96));
				aux.transform.localScale = new Vector3 (0.0035f,0.0035f,0.0035f);
			}
		} else {
			GameObject obj = GameObject.Find ("Pos" + (int)listaPos [1].x + (int)listaPos [1].y);
			GameObject aux = Instantiate(agua, new Vector3(obj.transform.position.x+0.005f, obj.transform.position.y, obj.transform.position.z), Quaternion.identity);
			aux.transform.Rotate (new Vector3 (90, 180, 96));
			aux.transform.localScale = new Vector3 (0.0035f,0.0035f,0.0035f);
		}

		if (matrizBarcos [(int) listaPos [2].x, (int) listaPos [2].y].tamano != shipEnum.posicionLibre) {
			foreach (var item in listaHundidos[2].listaVec)
			{
				Vector2 vec = (Vector2) item;
				GameObject obj = GameObject.Find ("Pos" + vec.x + vec.y);
				GameObject aux = Instantiate(hundido, new Vector3(obj.transform.position.x+0.005f, obj.transform.position.y, obj.transform.position.z), Quaternion.identity);
				aux.transform.Rotate (new Vector3 (90, 180, 96));
				aux.transform.localScale = new Vector3 (0.0035f,0.0035f,0.0035f);
			}
		} else {
			GameObject obj = GameObject.Find ("Pos" + (int)listaPos [2].x + (int)listaPos [2].y);
			GameObject aux = Instantiate(agua, new Vector3(obj.transform.position.x+0.005f, obj.transform.position.y, obj.transform.position.z), Quaternion.identity);
			aux.transform.Rotate (new Vector3 (90, 180, 96));
			aux.transform.localScale = new Vector3 (0.0035f,0.0035f,0.0035f);
		}



		GameObject.Destroy (listaCirculos [0]);
		GameObject.Destroy (listaCirculos [1]);
		GameObject.Destroy (listaCirculos [2]);
		matrizAtaque[(int) listaPos [0].x, (int) listaPos [0].y] = 2;
		matrizAtaque[(int) listaPos [1].x, (int) listaPos [1].y] = 2;
		matrizAtaque[(int) listaPos [2].x, (int) listaPos [2].y] = 2;


		seleccionados = 0;
		circuloActual = 0;
		listaCirculos[0] = null;
		listaCirculos[1] = null;
		listaCirculos[2] = null;
		listaHundidos[0] = null;
		listaHundidos[1] = null;
		listaHundidos[2] = null;
		listaPos[0] = Vector2.zero;
		listaPos[1] = Vector2.zero;
		listaPos[2] = Vector2.zero;

		habilitado = true;

	}

	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray ray = new Ray(reticle.transform.position, reticle.transform.forward);
		bool isButtonFire1Pressed = Input.GetButtonDown("Fire1");
           
        if (Physics.Raycast(ray, out hit, 100F, LayerMask.GetMask("ShipBoard")) && isButtonFire1Pressed && habilitado == true)
		{
			rowSelected = (int)char.GetNumericValue(hit.transform.gameObject.name.Substring(3).ToCharArray()[0]);
			columnSelected = (int)char.GetNumericValue(hit.transform.gameObject.name.Substring(3).ToCharArray()[1]);

			if (matrizAtaque[rowSelected, columnSelected]==0)
			{
				if (matrizBarcos[rowSelected, columnSelected].tamano != shipEnum.posicionLibre)
				{
					//GameObject aux = Instantiate(hundido, new Vector3(hit.transform.position.x+0.005f, hit.transform.position.y, hit.transform.position.z), Quaternion.identity);
					GameObject aux = Instantiate(circulo, new Vector3(hit.transform.position.x+0.005f, hit.transform.position.y, hit.transform.position.z), Quaternion.identity);
					aux.transform.Rotate (new Vector3 (90, 180, 96));
					aux.transform.localScale = new Vector3 (0.0035f,0.0035f,0.0035f);
					//EnableHundimientoAnimation(matrizBarcos[rowSelected, columnSelected].barcoReal);
					if (listaCirculos [circuloActual] != null) {
						GameObject.Destroy(listaCirculos [circuloActual]);
					}
					listaHundidos[circuloActual] = matrizBarcos[rowSelected, columnSelected];
					listaCirculos[circuloActual] = aux;
					matrizAtaque[(int) listaPos [circuloActual].x, (int) listaPos [circuloActual].y] = 0;
					listaPos [circuloActual] = new Vector2 (rowSelected, columnSelected);
					circuloActual++;
					seleccionados++;
					if (circuloActual > 2) {
						circuloActual = 0;
					}
				}
				else
				{ 
					//GameObject aux = Instantiate(agua, new Vector3(hit.transform.position.x+0.005f, hit.transform.position.y, hit.transform.position.z), Quaternion.identity);
					GameObject aux = Instantiate(circulo, new Vector3(hit.transform.position.x+0.005f, hit.transform.position.y, hit.transform.position.z), Quaternion.identity);
					aux.transform.Rotate (new Vector3 (90, 180, 96));
					aux.transform.localScale = new Vector3 (0.0035f,0.0035f,0.0035f);
					//EnableSplashAnimation(splash);
					if (listaCirculos [circuloActual] != null) {
						GameObject.Destroy(listaCirculos [circuloActual]);
					}
					listaCirculos[circuloActual] = aux;
					listaHundidos [circuloActual] = null;
					matrizAtaque[(int) listaPos [circuloActual].x, (int) listaPos [circuloActual].y] = 0;
					listaPos [circuloActual] = new Vector2 (rowSelected, columnSelected);
					circuloActual++;
					seleccionados++;
					if (circuloActual > 2) {
						circuloActual = 0;
					}
				}
				matrizAtaque[rowSelected, columnSelected] = 1;
			}
		}else if (Physics.Raycast(ray, out hit, 100F, LayerMask.GetMask("Clickable")) && isButtonFire1Pressed && seleccionados >= 3
			&& habilitado == true)
		{
			habilitado = false;
			Debug.Log ("Entro");
			StartCoroutine(disparoSecuencial());
			//TextBackground1.GetComponent<TextoPremio> ().startShowing (1);
		}

		/*
		if (Physics.Raycast(ray, out hit, 100F, LayerMask.GetMask("Barco1")) && isButtonFire1Pressed)
		{
			player.GetComponent<PlayerMovement>().iniciarCambio(5, GameObject.Find("TableroBarco1"));
		}
		else
		{
			if (Physics.Raycast(ray, out hit, 100F, LayerMask.GetMask("Barco2")) && isButtonFire1Pressed)
			{
				player.GetComponent<PlayerMovement>().iniciarCambio(0, GameObject.Find("TableroBarco2"));
			}
			else
			{
				if (Physics.Raycast(ray, out hit, 100F, LayerMask.GetMask("Barco3")) && isButtonFire1Pressed)
				{
					player.GetComponent<PlayerMovement>().iniciarCambio(4, GameObject.Find("TableroBarco3"));
				}
				else
				{
					if (Physics.Raycast(ray, out hit, 100F, LayerMask.GetMask("Barco4")) && isButtonFire1Pressed)
					{
						player.GetComponent<PlayerMovement>().iniciarCambio(2, GameObject.Find("TableroBarco4"));
					}
					else
					{
						if (Physics.Raycast(ray, out hit, 100F, LayerMask.GetMask("Barco5")) && isButtonFire1Pressed)
						{
							player.GetComponent<PlayerMovement>().iniciarCambio(1, GameObject.Find("TableroBarco5"));
						}
						else
						{
							if (Physics.Raycast(ray, out hit, 100F, LayerMask.GetMask("Barco6")) && isButtonFire1Pressed)
							{
								player.GetComponent<PlayerMovement>().iniciarCambio(3, GameObject.Find("TableroBarco6"));
							}
						}
					}
				}
			}
		}
		*/
	 } 
}