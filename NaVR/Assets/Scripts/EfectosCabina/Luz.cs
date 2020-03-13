using UnityEngine;
using System.Collections;

public class Luz : MonoBehaviour {

	public Light RedLight;
	public Light BlueLight;
	public int Number = 1;
	public int activarLuz = 0;
	// Use this for initialization
	void Start () 
	{
		//activarLuz = 0;
		Number = 1;
		RedLight = GetComponent<Light>();
	}

	// Update is called once per frame
	void Update () 
	{
		if (activarLuz == 1) {
			if (Number == 1) {
				RedLight.color = Color.red;
				StartCoroutine (waitforred ());
			}
			if (Number == 2) {
				RedLight.color = Color.white;
				StartCoroutine (waitforblue ());
			}
		}
	}

	public void Apagar () {
		RedLight.color = Color.white;
	}
		
	IEnumerator waitforred()
	{
		yield return new WaitForSeconds (0.4f);
		Number = 2;
	}
	IEnumerator waitforblue()
	{
		yield return new WaitForSeconds (0.4f);
		Number = 1;
	}
}
