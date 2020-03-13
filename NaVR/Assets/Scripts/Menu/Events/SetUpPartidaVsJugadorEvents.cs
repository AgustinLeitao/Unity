using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menu.Managers;
using GooglePlayGames.BasicApi.Multiplayer;
using Menu.Utils;

public class SetUpPartidaVsJugadorEvents : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnInvite()
	{
		MultiplayerManager.CreateWithInvitationScreen();
	}
	

}
