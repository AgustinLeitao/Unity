using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayAudioPoint(string clipName, Vector3 position, float maxDist){

		//  Create a temporary audio source object
		GameObject tempAudioSource = new GameObject("TempAudio");
		tempAudioSource.transform.position = position;

		//  Add an audio source
		AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();

		//  Add the clip to the audio source
		//audioSource.clip = clip;
		audioSource.clip = Resources.Load(clipName) as AudioClip;
		if (audioSource.clip == null){
			Destroy(tempAudioSource);
			return;
		}

		//  Set the volume
		//audioSource.volume = this.sfxVolume;

		//  Set properties so it's 2D sound
		audioSource.spatialBlend = 1.0f;

		audioSource.dopplerLevel = 0.0f;
		//audioSource.rolloffMode = Linear;
		audioSource.minDistance = 1.0f;
		audioSource.maxDistance = maxDist;
		audioSource.rolloffMode = AudioRolloffMode.Linear;

		//  Play the audio
		audioSource.Play();

		//  Set it to self destroy
		Destroy(tempAudioSource, audioSource.clip.length);

	}
}
