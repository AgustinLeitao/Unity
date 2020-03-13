using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibracionCamara : MonoBehaviour {

	public int number;
	public int vibrarCamara;

	public Vector3 originalPos;

	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 1.5f;
	public float decreaseFactor = 1.0f;

	void Start() {
	}

	void OnEnable()
	{
		originalPos = Camera.main.transform.localPosition;
	}

	void Update () {
		//Debug.Log (shakeDuration);
		if (shakeDuration > 0) {
			Camera.main.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else {
			shakeDuration = 0f;
			Camera.main.transform.localPosition = originalPos;
		}
	}

	public void VibrarCamara () {
		if (shakeDuration == 0) {
			shakeDuration = 4;
		} else {
			shakeDuration = 0;
		}
	}
}
