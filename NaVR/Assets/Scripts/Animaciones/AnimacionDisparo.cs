using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionDisparo : MonoBehaviour {

	public GameObject explosion;
    public GameObject particulaExplosion1, particulaExplosion2;
    public GameObject ascensoMisil, caidaMisilFallo, caidaMisilAcierto;
	public GameObject player;
	private Vector3 posicionSplash;	

	
	void Start () {
		
	}
	
	void Update () {
		
	}

	private void SetearPosicionSplash(Casilla objetivo)
    {
        System.Random random = new System.Random();

        if (objetivo.Columna >= 0 && objetivo.Columna <= 1)
        {
			this.posicionSplash = new Vector3(random.Next(-360, -216), -70, random.Next(610, 850));
        }
        else if (objetivo.Columna >= 2 && objetivo.Columna <= 3)
        {
			this.posicionSplash = new Vector3(random.Next(-216, -72), -70, random.Next(610, 850));
        }
        else if (objetivo.Columna >= 4 && objetivo.Columna <= 5)
        {
			this.posicionSplash = new Vector3(random.Next(-72, 72), -70, random.Next(610, 850));
        }
        else if (objetivo.Columna >= 6 && objetivo.Columna <= 7)
        {
			this.posicionSplash = new Vector3(random.Next(72, 216), -70, random.Next(610, 850));
        }
        else 
        {
			this.posicionSplash = new Vector3(random.Next(216, 360), -70, random.Next(610, 850));
        }
    }



	private IEnumerator IniciarAnimacionAcierto(GameObject barcoEnemigo)
	{
		yield return new WaitForSeconds(3.0f);
        explosion.transform.position = barcoEnemigo.transform.position;
		explosion.transform.LookAt (player.transform.position);
		explosion.transform.Rotate (new Vector3 (45, 0, 0));
        caidaMisilAcierto.SetActive(false);
        caidaMisilAcierto.SetActive(true);
		yield return new WaitForSeconds(2.0f);
		barcoEnemigo.SetActive (false);
		particulaExplosion1.SetActive(false);
        particulaExplosion2.SetActive(false);
        particulaExplosion1.SetActive(true);
        particulaExplosion2.SetActive(true);
	}

	private IEnumerator IniciarAnimacionFallo(GameObject splash, Vector3 posTarget)
	{
		yield return new WaitForSeconds(3.0f);

		caidaMisilFallo.transform.position = new Vector3(this.posicionSplash.x, 30, this.posicionSplash.z);
        caidaMisilFallo.transform.LookAt (player.transform.position);
        caidaMisilFallo.transform.Rotate (new Vector3 (45, 0, 0));
        caidaMisilFallo.SetActive(false);
        caidaMisilFallo.SetActive(true);

        yield return new WaitForSeconds(1.5f);
		splash.transform.position = posTarget;
        splash.GetComponent<activateSplash>().SetActivateEffect(true);
    }

	public void IniciarAnimacionDisparoAcierto(GameObject barcoEnemigo)
	{
		ascensoMisil.SetActive(false);
		ascensoMisil.SetActive(true);
		StartCoroutine(IniciarAnimacionAcierto(barcoEnemigo));
	}

	public void IniciarAnimacionDisparoFallo(GameObject splashEnemigo, Vector3 posTarget)
	{
		ascensoMisil.SetActive(false);
		ascensoMisil.SetActive(true);
		StartCoroutine(IniciarAnimacionFallo(splashEnemigo, posTarget));
	}
}
