using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApuntarObjetivo : MonoBehaviour {

	private bool apuntarOn = false;
	private GameObject target;
	public Vector3 targetPoint;
	public Quaternion targetRotation;
    public GameObject rocketUp;
    private GameObject posicionSalidaRocketUp;
    private GameObject boatManager;
	private int tipoTarget;
	private Vector3 posTarget2;

    // Use this for initialization
    void Start () {
        posicionSalidaRocketUp = GameObject.Find("posicionSalidaRocketUp");
        boatManager = GameObject.Find("BoatManager"); ;

    }

	// Update is called once per frame
	void Update () {
		if (apuntarOn) {
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, targetRotation, Time.deltaTime * 3f);
			if (this.transform.rotation == targetRotation) {
                apuntarOn = false;
				rocketUp.transform.rotation = targetRotation;
				rocketUp.transform.Rotate (new Vector3(-40,180,0));
                rocketUp.transform.position = posicionSalidaRocketUp.transform.position;
				if(tipoTarget == 0)
					boatManager.GetComponent<BoatManager>().BoomBarco(target);
				else
					boatManager.GetComponent<BoatManager>().BoomSplash(target,posTarget2);	
			}
		}
	}

	public void apuntar(GameObject objetivo, int tipoTarget, Vector3 posTarget){
		target = objetivo;
		this.tipoTarget = tipoTarget;
		posTarget2 = posTarget;
		if (tipoTarget == 0) {
			targetPoint = this.transform.position - target.transform.position;
			targetRotation = Quaternion.LookRotation (targetPoint);
		} else {
			targetPoint = this.transform.position - posTarget;
			targetRotation = Quaternion.LookRotation (targetPoint);
		}
		apuntarOn = true;
	}
		
}